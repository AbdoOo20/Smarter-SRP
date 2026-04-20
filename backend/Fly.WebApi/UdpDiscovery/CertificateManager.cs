using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Fly.WebApi.UdpDiscovery;

public class CertificateManager
{
    private const string CertFileName = "server-cert.pfx";
    private const string CertPassword = "aaa";
    
    public static X509Certificate2 GetOrCreateCertificate()
    {
        if (File.Exists(CertFileName))
        {
            try
            {
                return new X509Certificate2(CertFileName, CertPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load existing cert: {ex.Message}");
            }
        }
        
        var cert = GenerateSelfSignedCertificate();
        
        var certBytes = cert.Export(X509ContentType.Pfx, CertPassword);
        File.WriteAllBytes(CertFileName, certBytes);
        
        return new X509Certificate2(CertFileName, CertPassword);
    }
    
    private static X509Certificate2 GenerateSelfSignedCertificate()
    {
        using var rsa = RSA.Create(2048);
        
        var request = new CertificateRequest(
            "CN=MyKestrelApp",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment,
                critical: true));
        
        request.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, // Server Authentication
                critical: true));
        
        var cert = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddYears(10));
        
        return cert;
    }
    
    public static string GetCertificateThumbprint(X509Certificate2 cert)
    {
        return cert.Thumbprint;
    }
    
    public static string GetPublicKeyHash(X509Certificate2 cert)
    {
        // Alternative to thumbprint - hash of the public key only
        using var sha256 = SHA256.Create();
        var publicKeyBytes = cert.GetPublicKey();
        var hash = sha256.ComputeHash(publicKeyBytes);
        return Convert.ToBase64String(hash);
    }
}

public class CertificateGenerator
{
    public static X509Certificate2 CreateServerCertificate(string outputPath, string password)
    {
        using var rsa = RSA.Create(2048);
        
        var request = new CertificateRequest(
            "CN=DynamicKestrelServer",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        
        request.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection { 
                    new Oid("1.3.6.1.5.5.7.3.1")
                },
                critical: false));
        
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment,
                critical: false));
        
        var cert = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddYears(3));
        
        var pfxBytes = cert.Export(X509ContentType.Pfx, password);
        File.WriteAllBytes(outputPath, pfxBytes);
        
        return new X509Certificate2(outputPath, password, 
            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
    }
}