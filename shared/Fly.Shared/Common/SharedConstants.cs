namespace Fly.Shared.Common;

public class SharedConstants
{
    public const string InternalTableColumn = "INTERNAL";

    public static class DiscoveryConfig
    {
        public const string AppId = "LeaPayBackend";
        public const string SharedThumbprint = "C4FABBAA580F179529ACDCF468426CCCF59E65CD";
        public const int DiscoveryPort = 40000;

        public static readonly byte[] SharedDiscoveryConfig =
            Convert.FromHexString("A47D93C95DD496A0759203ECDB7D84AB4859721EB745869A01CAADD72CD24ED3");
    }
}