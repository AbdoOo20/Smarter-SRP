using System.Diagnostics;
using System.Net.Http.Json;
using Fly.Shared.Common;
using SmartErpWin.Core.Globals;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Leapay.Client.ClientModels.Configuration;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core;
using SmartErpWin.Core.Helpers;
using SmartErpWin.Core.Models;
using Svg;

namespace Leapay.Client.UdpDiscovery;

public static class BackendDiscovery
{
    private static readonly List<string> MutedMessages = [];

    public static EventHandler<ContentEventArgs>? ConnectionsUpdated;
    
    public static bool HasUserHadToSelectConnection { get; set; }
    
    public static bool IsUserAwareOfCurrentUnavailableConnection { get; set; }
    
    public static async Task StartRoutineDiscoveryAsync()
    {
        //TestUdpPort(SharedConstants.DiscoveryConfig.DiscoveryPort);
        while (true)
        {
            if (StringGlobals.ActiveConnection is null)
            {
                Information("Looking for backend servers...");
            }
            
            var (search, socketError) = await DiscoverAsync(TimeSpan.FromSeconds(5));

            if (socketError)
            { 
                Verbose("Skipping socket error...");
                continue;
            }
            
            var stored = PreferencesService.Get(s => s.Connections);
            var newStored = new List<ConnectionInfo>();

            var mainWindowViewModel = App.GetService<MainWindowViewModel>();
            if (!search.Any() && StringGlobals.ActiveConnection is null)
            {
                AllUnavailable();
                
                Information("No backend servers found. Retrying...");

                _ = mainWindowViewModel.OnSynchronisationRequestedAsync(CodeBook.NoData);
                
                continue;
            }

            foreach (var item in search)
            {
                if (stored.All(a => a.DeviceId != item.DeviceId))
                {
                    item.LastUpdated = DateTime.Now;
                    newStored.Add(item);
                }
            }
            
            foreach (var storedItem in stored)
            {
                newStored.Add(storedItem);
                var match = search.FirstOrDefault(s => s.DeviceId == storedItem.DeviceId);

                if (match is null)
                {
                    storedItem.IsCurrentlyAvailable = false;
                    continue;
                }

                storedItem.CurrentConnectionPort = match.CurrentConnectionPort;
                storedItem.BroadcastedMachineName = match.BroadcastedMachineName;
                storedItem.CurrentConnectionIp = match.CurrentConnectionIp;
                storedItem.IsCurrentlyAvailable = true;
                storedItem.LastUpdated = DateTime.Now;
            }

            FinalizeSave();
            _ = mainWindowViewModel.OnSynchronisationRequestedAsync();

            if (StringGlobals.ActiveConnection is null)
            {
                // This will be on startup. Try to auto select a connection.
                // If no available connection exists, let the user select.
                var selection = newStored.Where(s => s.IsCurrentlyAvailable)
                    .OrderByDescending(s => s.LastConnectedTo)
                    .FirstOrDefault();

                if (selection is null)
                {
                    if (!HasUserHadToSelectConnection)
                    {
                        Information("Most recent connection is not available. Will let user select.");

                        _ = mainWindowViewModel.OnSynchronisationRequestedAsync(CodeBook.Unavailable);

                        HasUserHadToSelectConnection = true;
                    }
                
                    continue;
                }

                selection.LastConnectedTo = DateTime.Now;
                newStored[newStored.IndexOf(newStored.FirstOrDefault(s 
                    => s.DeviceId == selection.DeviceId)!)] = selection;
                
                StringGlobals.ActiveConnection = selection;
                FinalizeSave();
            }
            else
            {
                // We have to update the details for the active connection, and if it is unavailable,
                // fire an event to let the user now.
                
                var match = newStored.FirstOrDefault(c 
                    => c.IsCurrentlyAvailable && c.DeviceId == StringGlobals.ActiveConnection.DeviceId);

                if (match is null)
                {
                    if (!IsUserAwareOfCurrentUnavailableConnection)
                    {
                        Information("Current connection went offline. User will be prompted to wait or log out.");
                        mainWindowViewModel.EnterBusyModeForServerDisconnected();

                        IsUserAwareOfCurrentUnavailableConnection = true;
                    }
                    
                    continue;
                }

                // Current connection is back online. Fire up an event in case the user is waiting for the connection
                // to come back.

                if (IsUserAwareOfCurrentUnavailableConnection)
                {
                    Information("Current connection is back online.");

                    mainWindowViewModel.ClearBusyMode();

                    IsUserAwareOfCurrentUnavailableConnection = false;
                }

                if (match is not null)
                {
                    StringGlobals.ActiveConnection = match;
                }
            }
            
            void FinalizeSave()
            {
                PreferencesService.Set(s => s.Connections = newStored);
            }
            
            void AllUnavailable()
            {
                foreach (var item in stored)
                {
                    item.IsCurrentlyAvailable = false; 
                    newStored.Add(item);
                }
                
                FinalizeSave();
            }
            
            await Task.Delay(3000);
        }
    }
    
    public static async Task<(IReadOnlyList<ConnectionInfo>, bool exception)> DiscoverAsync(
        TimeSpan timeout)
    {
        var results = new HashSet<ConnectionInfo>();

        using var udp = new UdpClient(SharedConstants.DiscoveryConfig.DiscoveryPort);
        udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        
        using var cts = new CancellationTokenSource(timeout);

        try
        {
            while (!cts.IsCancellationRequested)
            {
                UdpReceiveResult r;

                try
                {
                    r = await udp.ReceiveAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    break; // timeout reached
                }

                var msg = Encoding.UTF8.GetString(r.Buffer);

                var connection = TryVerify(msg);

                if (connection is null)
                    continue;
                connection.CurrentConnectionIp = r.RemoteEndPoint.Address.ToString();
                
                results.Add(connection);
            }
        }
        catch (Exception e)
        {
            Error(e.ToString());

            return ([], true);
        }

        return (results.ToList(), false);
    }
    
    private static void KillOtherInstances()
    {
        var currentProcess = Process.GetCurrentProcess();
        var processes = Process.GetProcessesByName(currentProcess.ProcessName);
    
        foreach (var process in processes)
        {
            if (process.Id != currentProcess.Id)
            {
                try
                {
                    Console.WriteLine($"Killing orphaned process {process.Id}");
                    process.Kill();
                    process.WaitForExit(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not kill process {process.Id}: {ex.Message}");
                }
            }
        }
    }
    
    public static void TestUdpPort(int port)
    {
        Debug($"Testing port {port}...");
        Debug($"Process ID: {Environment.ProcessId}");
        Debug($"Is Admin: {IsAdministrator()}");
    
        try
        {
            // Try to create the UdpClient
            Debug($"About to create UdpClient on port {port}");
            var udp = new UdpClient(port);
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.ExclusiveAddressUse = false;
            Debug($"SUCCESS! UdpClient created on port {port}");
            Debug($"Local endpoint: {udp.Client.LocalEndPoint}");
            udp.Close();
        }
        catch (SocketException ex)
        {
            Debug($"SocketException: {ex.Message}");
            Debug($"Error code: {ex.ErrorCode}");
            Debug($"Socket error code: {ex.SocketErrorCode}");
        }
        catch (Exception ex)
        {
            Debug($"Exception: {ex.GetType().Name}");
            Debug($"Message: {ex.Message}");
            Debug($"Stack: {ex.StackTrace}");
        }
    
        Debug("Test complete.");
    }

    private static bool IsAdministrator()
    {
        try
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }
    
    static ConnectionInfo? TryVerify(string msg)
    {
        try
        {
            ConnectionInfo info = new();

            var p = msg.Split('|');
            if (p.Length != 7) return null;
            if (p[0] != SharedConstants.DiscoveryConfig.AppId) return null;
            if (!int.TryParse(p[1], out var port)) return null;

            info.CurrentConnectionPort = port;
            
            var ts = long.Parse(p[2]);
            var unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var abs = Math.Abs(unixTimeSeconds - ts);
            if (abs > 5)
                return null;

            var machineName = p[3];
            var machineId = p[4];

            info.BroadcastedMachineName = machineName;
            info.DeviceId = machineId;
            var body = string.Join('|', p[..6]);
            using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
            var expected = Convert.ToHexString(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(body)));

            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.ASCII.GetBytes(expected),
                    Encoding.ASCII.GetBytes(p[6])))
                return null;

            return info;
        }
        catch (Exception x)
        {
            if (MutedMessages.Contains(ApplicationHost.Start(msg)))
                return null;
            
            Verbose("An error occurred while trying to discover a backend. It is most likely out of" +
                    "date or incompatible. Muting this error message for further incompatibilities from " +
                    "this backend. Error below:\n{0}", x.ToString());
            
            MutedMessages.Add(ApplicationHost.Start(msg));
        }

        return null;
    }
    
    public static async Task<string> AuthenticateAsync(HttpClient http)
    {
        var chal = await http.GetFromJsonAsync<Challenge>("udp-auth/challenge");

        using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
        var resp = Convert.ToHexString(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(chal!.nonce)));

        var r = await http.PostAsJsonAsync("udp-auth/verify",
            new { nonce = chal.nonce, response = resp });

        var token = await r.Content.ReadFromJsonAsync<TokenResp>();
        return token!.token;
    }

    record Challenge(string nonce);
    record TokenResp(string token);
}

