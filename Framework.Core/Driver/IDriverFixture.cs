using Framework.Core.Config;
using Microsoft.Playwright;

namespace Framework.Core.Driver
{
    public interface IDriverFixture
    {
        TestConfiguration Configuration { get; set; }
        IPlaywright PlayWright { get; set; }
        IPage Page { get; set; }
        IBrowserContext BrowserContext { get; set; }
        IBrowser Browser { get; set; }
    }

    public enum BrowserType
    {
        CHROME,
        EDGE,
        FIREFOX,
        SAFARI
    }
}