using Microsoft.Win32;
using Newtonsoft.Json;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1.License;


internal class PCInfo
{
    //  Caption, InterfaceType, Manufacturer, Model,PNPDeviceID, MediaType, SerialNumber, SystemName, Size
    public string Name { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string InterfaceType { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string PNPDeviceID { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string SystemName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
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


public static class BasicLicenseValidation
{

    private static string hashFilePath;
    private static string regestryKeyPath = "MMTech\\Aqwas";
    private static string regestryKeyAppPath = "Salt";
    private static Encoding encoding = Encoding.ASCII;

    public static void Starter()
    {
        SetValues();

        //RemoveAqwasLicense();
        GenerateAqwasLicense();

        bool Check()
        {
            var status = ValidateLicense();

            if (status)
            {
                Console.WriteLine("License is valid");
            }
            else
            {
                Console.WriteLine("License is not valid");
            }
            return status;
        }
        Check();
        //if (Check())
        //{
        //    Console.WriteLine("removing license");
        //    RemoveAqwasLicense();
        //    Console.WriteLine("removing license done");
        //}
        //else
        //{

        //    Console.WriteLine("generating license");
        //    GenerateAqwasLicense();
        //    Console.WriteLine("generating license done");
        //}




    }

    private static void SetValues()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        hashFilePath = Path.Combine(documentsPath, "AqwasErp_mmtech.mtk");
    }

    private static void RemoveAqwasLicense()
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

    private static void GenerateAqwasLicense()
    {
        try
        {

            /*
            var info = GetPCInfo();
            //byte[] salt = new byte[16];
            var rng = RandomNumberGenerator.GetBytes(512);
            var salt = rng;// encoding.GetBytes(rng);

            //System.Security.Cryptography.SHA512
            using var SHA512 = System.Security.Cryptography.SHA512.Create();
            var sctringFingerPrint = JsonConvert.SerializeObject(info);

            byte[] infoBytes = new byte[1024];
            var d = encoding.GetBytes(sctringFingerPrint);
            var startSource = 0;
            var startDest = 1024 - d.Length;
            var endSource = d.Length;
            Buffer.BlockCopy(d, startSource, infoBytes, startDest, endSource);

            byte[] saltedInfoBytes = new byte[infoBytes.Length + salt.Length];
            Buffer.BlockCopy(infoBytes, 0, saltedInfoBytes, 0, infoBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedInfoBytes, infoBytes.Length, salt.Length);
            byte[] hash = SHA512.ComputeHash(saltedInfoBytes);

            File.WriteAllBytes(hashFilePath, hash);

            using var mmtechRegistryKey = Registry.CurrentUser.CreateSubKey(regestryKeyPath);
            mmtechRegistryKey.SetValue(regestryKeyAppPath, salt);
             */

            var info = GetPCInfo();
            //byte[] salt = new byte[16];
            var rng = RandomNumberGenerator.GetBytes(512);
            var salt = rng;// encoding.GetBytes(rng);

            //System.Security.Cryptography.SHA512
            using var SHA512 = System.Security.Cryptography.SHA512.Create();
            var sctringFingerPrint = JsonConvert.SerializeObject(info);

            var infoBytes = encoding.GetBytes(sctringFingerPrint);
            byte[] saltedInfoBytes = new byte[infoBytes.Length + salt.Length];
            Buffer.BlockCopy(infoBytes, 0, saltedInfoBytes, 0, infoBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedInfoBytes, infoBytes.Length, salt.Length);
            byte[] hash = SHA512.ComputeHash(saltedInfoBytes);

            File.WriteAllBytes(hashFilePath, hash);

            using var mmtechRegistryKey = Registry.CurrentUser.CreateSubKey(regestryKeyPath);
            mmtechRegistryKey.SetValue(regestryKeyAppPath, salt);

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);

        }
    }

    private static bool ValidateLicense()
    {
        try
        {
            // Check if license file exists
            if (!File.Exists(hashFilePath))
            {
                return false;
            }

            // Read license file
            byte[] licenseHash = File.ReadAllBytes(hashFilePath);

            // Check if registry key exists
            using var registryKey = Registry.CurrentUser.OpenSubKey(regestryKeyPath);
            if (registryKey is null)
            {
                return false;
            }

            // Get salt from registry
            var salt = registryKey.GetValue(regestryKeyAppPath) as byte[];
            if (salt is null)
            {
                return false;
            }

            // Calculate fingerprint
            var info = GetPCInfo();
            var sctringFingerPrint = JsonConvert.SerializeObject(info);
            byte[] infoBytes = encoding.GetBytes(sctringFingerPrint);
            byte[] saltedInfoBytes = new byte[infoBytes.Length + salt.Length];
            Buffer.BlockCopy(infoBytes, 0, saltedInfoBytes, 0, infoBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedInfoBytes, infoBytes.Length, salt.Length);

            // Calculate hash
            using var SHA512 = System.Security.Cryptography.SHA512.Create();
            byte[] hash = SHA512.ComputeHash(saltedInfoBytes);

            // Compare hashes
            return hash.SequenceEqual(licenseHash);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    private static PCInfo GetPCInfo()
    {
        var searcher = new ManagementObjectSearcher(
            "SELECT InterfaceType, Manufacturer, Model,PNPDeviceID, MediaType, SerialNumber, SystemName, Size FROM Win32_DiskDrive WHERE Index = 0");
        var searcher4 = new ManagementObjectSearcher(
            "SELECT ProcessorId, Name, Description, Caption FROM Win32_Processor");
        /*
        var searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
        var searcher3 = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        var searcher5 = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
        var searcher6 = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
        */

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
                prp.SetValue(hd, all[property.Name].Value.ToString());
            }
        }

        return hd;

        //searcher = new
        //ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

        //int i = 0;
        //foreach (ManagementObject wmi_HD in searcher.Get())
        //{
        //    // get the hard drive from collection
        //    // using index
        //    HardDrive hd = (HardDrive)hdCollection[i];

        //    // get the hardware serial no.
        //    if (wmi_HD["SerialNumber"] == null)
        //        hd.SerialNo = "None";
        //    else
        //        hd.SerialNo = wmi_HD["SerialNumber"].ToString();

        //    ++i;
        //}
    }


    private static string GetDriveCHash()
    {
        // Implement the code to get the hash of the C drive
        // For example, you can use the System.Security.Cryptography.MD5 class
        // to calculate the hash of the C drive
        // Here is a sample implementation:
        try
        {
            using (var md5 = MD5.Create())
            {
                string drivePath = @"C:\";
                byte[] driveBytes = Directory.GetFiles(drivePath, "*.*", SearchOption.AllDirectories)
                    .SelectMany(file => File.ReadAllBytes(file))
                    .ToArray();
                byte[] hash = md5.ComputeHash(driveBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
            // For example, log the error or display a message to the user
            Console.WriteLine($"An error occurred while calculating the hash of the C drive: {ex.Message}");
        }

        return null;
    }

    private static string GetRegistryValue(string keyName)
    {
        // Implement the code to get the registry key value
        // For example, you can use the Microsoft.Win32.RegistryKey class
        // to read the registry key value
        // Here is a sample implementation:
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key != null)
                {
                    return (string)key.GetValue(null);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
            // For example, log the error or display a message to the user
            Console.WriteLine($"An error occurred while reading the registry key: {ex.Message}");
        }

        return null;
    }

    private static void PrintAllData(PropertyDataCollection propertyDatas, string className)
    {
        Console.WriteLine("*******************************************");
        Console.WriteLine(className);
        Console.WriteLine("*******************************************");
        foreach (var property in propertyDatas) { Console.WriteLine(property.Name + " : " + property.Value); }
        Console.WriteLine("====================================================================================");
    }

}
