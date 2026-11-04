namespace Fly.Shared.Common;

public class SharedConstants
{
    public const string InternalTableColumn = "INTERNAL";

    public static class DiscoveryConfig
    {
        public const string AppId = "LeaPayBackend";
        public const string SharedThumbprint = "C5F42C091FE031129D87DB236A6ED15630084EE6";
        public const int DiscoveryPort = 40000;

        public static readonly byte[] SharedDiscoveryConfig =
            Convert.FromHexString("A47D93C95DD496A0759203ECDB7D84AB4859721EB745869A01CAADD72CD24ED3");
    }
}