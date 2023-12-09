
namespace BuildingBlocks.FileUtility
{
    public class FileManagerSettings
    {
        public Providers? Providers { get; set; }

        public long? FileMaxSize { get; set; }
        public long? ImageMaxSize { get; set; }
        public string? StaticFileFolder { get; set; }
    }

    public class Providers
    {
        public AWS? AWS { get; set; }
        public FTP? FTP { get; set; }
        public string? LocalDisk { get; set; }
        public bool? InMemory { get; set; }
    }

    public class AWS
    {
        public string ServiceURL { get; set; }
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketName { get; set; }
    }

    public class FTP
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
