using Autofac;
using Framework.Core.Driver;
using Framework.Core.WebElements;
using Microsoft.Playwright;

namespace Framework.Object.Base
{
    public class Browser : ContainerBase
    {
        public static async Task SwitchToParentWindow()
        {
            try
            {
                var windows = ElementBase.GetBrowserContext.Pages;
                var Parentwindow = windows[0];
                bool state = false;

                for (int childWindow = windows.Count - 1; childWindow > 0; childWindow--)
                {
                    await windows[childWindow].CloseAsync();
                    state = true;
                }

                if (state)
                {
                    await Parentwindow.WaitForLoadStateAsync();
                }

                ReInitializingAutoFac(Parentwindow);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Unable to switch to Parent Window...");
                throw ex;
            }
        }

        public static async Task SwitchToChildWindow()
        {
            try
            {
                IReadOnlyList<IPage> windows = ElementBase.GetBrowserContext.Pages;

                int attempt = 0;
                int windowsCount = windows.Count;

                while (windowsCount <= 1 && attempt < 3)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    windowsCount = windows.Count;
                    attempt++;
                }

                var switchPage = windows[1];
                await switchPage.WaitForLoadStateAsync();
                ReInitializingAutoFac(switchPage);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Unable to switch to Child Window...");
                throw ex;
            }
        }

        public static async Task SwitchToLastWindow()
        {
            try
            {
                IReadOnlyList<IPage> windows = ElementBase.GetBrowserContext.Pages;

                int attempt = 0;
                int windowsCount = windows.Count;

                while (windowsCount <= 1 && attempt < 3)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    windowsCount = windows.Count;
                    attempt++;
                }

                var switchPage = windows[windowsCount - 1];
                await switchPage.WaitForLoadStateAsync();
                ReInitializingAutoFac(switchPage);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Unable to switch to Last Window...");
                throw ex;
            }
        }

        /**
         * Re-Initializing the Autofac container & builder since updating the autofac has been deprecated...
         */
        public static void ReInitializingAutoFac(IPage newPage)
        {
            // Initializing the Autofac Container...
            var builder = new ContainerBuilder();

            // Getting the instances from the existing container...
            builder.RegisterInstance<IPage>(newPage).AsImplementedInterfaces();
            builder.RegisterInstance(ElementBase.GetBrowser).AsImplementedInterfaces();
            builder.RegisterInstance(ElementBase.GetBrowserContext).AsImplementedInterfaces();
            builder.RegisterInstance(ElementBase.GetPlaywright).AsImplementedInterfaces();
            builder.RegisterInstance(ElementBase.GetTestConfiguration).AsSelf().SingleInstance();
            builder.RegisterModule(new PageObjectAutofac(ElementBase.GetTestConfiguration));

            // Building the Autofac Container...
            Container = builder.Build();
        }
    }
}