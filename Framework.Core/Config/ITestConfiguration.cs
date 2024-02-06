using Framework.Core.Driver;

namespace Framework.Core.Config
{
    public interface ITestConfiguration
    {
        BrowserType BrowserType { get; set; }
        Uri ApplicationUrl { get; set; }
        bool AcceptInsecureCertificates { get; set; }
        bool Headless { get; set; }
        string Environment { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}