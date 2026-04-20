using Microsoft.Win32;
using Newtonsoft.Json;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Fly.Framework.Desktop.Toos;

public class PCInfo
{
    public string Name { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string InterfaceType { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string PNPDeviceID { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string SystemName { get; set; } = string.Empty;
    public string DriveSize { get; set; } = string.Empty;
    public string ProcessorId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string ToString()
    {
        var typeProperties = typeof(PCInfo).GetProperties();
        var str = new List<string>();
        foreach (var prop in typeProperties)
            str.Add(prop.Name + " : " + prop.GetValue(this).ToString());
        return string.Join("\n", str);
    }
}

public class BasicSubscriber
{

    private string hashFilePath = string.Empty;
    private string regestryKeyPath = "MMTech\\Aqwas";
    private string regestryKeyAppPath = "Salt";
    private Encoding encoding = Encoding.ASCII;

    public BasicSubscriber()
    {
        SetValues();
    }

    private void SetValues()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        hashFilePath = Path.Combine(documentsPath, "AqwasErp_mmtech.mtk");
    }

    public void RemoveAqwasLicense()
    {
        try
        {
            if (File.Exists(hashFilePath))
            {
                File.Delete(hashFilePath);
            }

            using var mmtechRegistryKey = Registry.CurrentUser.OpenSubKey(regestryKeyPath, true);
            if (mmtechRegistryKey != null)
            {
                mmtechRegistryKey.DeleteValue(regestryKeyAppPath);
                if (mmtechRegistryKey.SubKeyCount == 0)
                {
                    //mmtechRegistryKey.Delete(true);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void GenerateAqwasLicense()
    {
        try
        {

            var info = GetPCInfo();
            var rng = RandomNumberGenerator.GetBytes(512);
            var salt = rng;

            using var SHA512 = System.Security.Cryptography.SHA512.Create();
            var sctringFingerPrint = JsonConvert.SerializeObject(info);

            var infoBytes = encoding.GetBytes(sctringFingerPrint);
            byte[] saltedInfoBytes = new byte[infoBytes.Length + salt.Length];
            Buffer.BlockCopy(infoBytes, 0, saltedInfoBytes, 0, infoBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedInfoBytes, infoBytes.Length, salt.Length);
            byte[] hash = SHA512.ComputeHash(saltedInfoBytes);

            using var mmtechRegistryKey = Registry.CurrentUser.CreateSubKey(regestryKeyPath);

            File.WriteAllBytes(hashFilePath, salt);
            mmtechRegistryKey.SetValue(regestryKeyAppPath, hash);

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);

        }
    }

    public bool SyncSub()
    {
        try
        {
            if (!File.Exists(hashFilePath))
            {
                return false;
            }

            byte[] salt = File.ReadAllBytes(hashFilePath);

            using var registryKey = Registry.CurrentUser.OpenSubKey(regestryKeyPath);
            if (registryKey is null)
            {
                return false;
            }

            var licenseHash = registryKey.GetValue(regestryKeyAppPath) as byte[];
            if (licenseHash is null)
            {
                return false;
            }

            var info = GetPCInfo();
            var sctringFingerPrint = JsonConvert.SerializeObject(info);
            byte[] infoBytes = encoding.GetBytes(sctringFingerPrint);
            byte[] saltedInfoBytes = new byte[infoBytes.Length + salt.Length];
            Buffer.BlockCopy(infoBytes, 0, saltedInfoBytes, 0, infoBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedInfoBytes, infoBytes.Length, salt.Length);

            using var SHA512 = System.Security.Cryptography.SHA512.Create();
            byte[] hash = SHA512.ComputeHash(saltedInfoBytes);

            return hash.SequenceEqual(licenseHash);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public PCInfo GetPCInfo()
    {
        var searcher = new ManagementObjectSearcher(
            "SELECT InterfaceType, Manufacturer, Model,PNPDeviceID, MediaType, SerialNumber, SystemName, Size FROM Win32_DiskDrive WHERE Index = 0");
        var searcher4 = new ManagementObjectSearcher(
            "SELECT ProcessorId, Name, Description, Caption FROM Win32_Processor");


        PCInfo hd = new PCInfo();
        var typeProperties = typeof(PCInfo).GetProperties();

        foreach (ManagementObject wmi_HD in searcher4.Get())    //  ProcessorId, Name, Description, Caption
        {
            var all = wmi_HD.Properties;
            foreach (var property in all)
            {
                var prp = typeProperties.FirstOrDefault(x => x.Name == property.Name);
                prp.SetValue(hd, all[property.Name].Value.ToString());
            }
        }

        foreach (ManagementObject wmi_HD in searcher.Get())
        {
            var all = wmi_HD.Properties;
            foreach (var property in all)
            {
                var prp = typeProperties.FirstOrDefault(x => x.Name == property.Name);
                if (prp is null) continue;
                prp.SetValue(hd, all[property.Name].Value.ToString());
            }
            hd.DriveSize = wmi_HD["Size"].ToString();
        }

        return hd;
    }

}