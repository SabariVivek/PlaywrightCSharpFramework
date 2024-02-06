using Module = Autofac.Module;
using Framework.Core.Config;
using Microsoft.Playwright;
using System.Reflection;
using Autofac;
using Framework.Core.WebElements;

namespace Framework.Core.Driver
{
    public class DriverFixture : Module, IDriverFixture
    {
        public TestConfiguration Configuration { get; set; }
        public IPlaywright PlayWright { get; set; }
        public IPage Page { get; set; }
        public IBrowserContext BrowserContext { get; set; }
        public IBrowser Browser { get; set; }
        public static IFrameLocator FrameLocator { get; set; }

        public DriverFixture(TestConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Task.Run(async () => await RegisterBrowser(builder)).Wait();

            var testSiteAssembly = Assembly.GetAssembly(typeof(ElementBase));
            builder.RegisterAssemblyTypes(testSiteAssembly)
                .Where(t => typeof(ElementBase)
                .IsAssignableFrom(t))
                .InstancePerDependency()
                .AsSelf();
        }

        private async Task RegisterBrowser(ContainerBuilder builder)
        {
            // Playwrigt Declaration...
            PlayWright = await Playwright.CreateAsync();

            // Assiging the configuration data to local variables...
            bool _headless = Configuration.Headless;
            string _browser = Configuration.BrowserType.ToString();

            // Browser Type Selection...
            if (Configuration.BrowserType.Equals(BrowserType.FIREFOX.ToString()))
            {
                Browser = await PlayWright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = _browser,
                    Headless = _headless,
                }).ConfigureAwait(false);
            }
            else if (Configuration.BrowserType.Equals(BrowserType.SAFARI.ToString()))
            {
                Browser = await PlayWright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = _browser,
                    Headless = _headless,
                }).ConfigureAwait(false);
            }
            else if (Configuration.BrowserType.Equals(BrowserType.CHROME.ToString()))
            {
                Browser = await PlayWright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = _browser,
                    Headless = _headless,
                    Args = new[] { "--start-maximized" },
                }).ConfigureAwait(false);
            }
            else
            {
                Browser = await PlayWright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "chrome",
                    Headless = false,
                    Args = new[] { "--start-maximized" },
                }).ConfigureAwait(false);
            }

            // For Maximum View Mode...
            var contextOptions = new BrowserNewContextOptions()
            {
                ViewportSize = ViewportSize.NoViewport
            };

            // Browser and Page Configuration...
            BrowserContext = await Browser.NewContextAsync(contextOptions);
            Page = await BrowserContext.NewPageAsync();

            // Autofac Configuration...
            builder.RegisterInstance(Page).AsImplementedInterfaces();
            builder.RegisterInstance(Browser).AsImplementedInterfaces();
            builder.RegisterInstance(BrowserContext).AsImplementedInterfaces();
            builder.RegisterInstance(PlayWright).AsImplementedInterfaces();
            builder.RegisterInstance(Configuration).AsSelf().SingleInstance();
        }
    }
}