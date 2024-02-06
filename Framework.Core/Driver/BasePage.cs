using Autofac;
using Framework.Core.Config;
using Framework.Core.Report;
using Microsoft.Playwright;

namespace Framework.Core.Driver
{
    public class BasePage
    {
        protected BasePage()
        {

        }

        public static string If = "Inside Frame";
        public static IPage Page => ContainerBase.Container!.Resolve<IPage>();

        public static TestConfiguration TestConfiguration() => ContainerBase.Container!.Resolve<TestConfiguration>();

        public async Task GoToPageAsync()
        {
            string url = TestConfiguration().ApplicationUrl!.ToString();
            await Page.GotoAsync(url);
            StepsInfo.Pass("URL : " + " <a href = '" + url + "' > " + url + " </a>");
            StepsInfo.Pass("Navigated to the given URL");
        }
    }
}