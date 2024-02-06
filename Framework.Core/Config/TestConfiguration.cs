using Framework.Core.Driver;

namespace Framework.Core.Config
{
    public class TestConfiguration : ITestConfiguration
    {
        public BrowserType BrowserType { get; set; }
        public Uri ApplicationUrl { get; set; }
        public bool AcceptInsecureCertificates { get; set; }
        public bool Headless { get; set; }
        public string Environment { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}