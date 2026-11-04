namespace Fly.Shared.Common;

public class SharedConstants
{
    public const string InternalTableColumn = "INTERNAL";

    public static class DiscoveryConfig
    {
        public const string AppId = "LeaPayBackend";
        // Certificate the backend in this repository serves (Resources/leapay-backend.pfx).
        public const string SharedThumbprint = "C5F42C091FE031129D87DB236A6ED15630084EE6";

        // Certificate of the backends already deployed at customers. Kept so an
        // updated client can still connect to an existing backend installation.
        public const string DeployedThumbprint = "C4FABBAA580F179529ACDCF468426CCCF59E65CD";

        public static readonly string[] TrustedThumbprints = [SharedThumbprint, DeployedThumbprint];
        public const int DiscoveryPort = 40000;

        public static readonly byte[] SharedDiscoveryConfig =
            Convert.FromHexString("A47D93C95DD496A0759203ECDB7D84AB4859721EB745869A01CAADD72CD24ED3");
    }
}