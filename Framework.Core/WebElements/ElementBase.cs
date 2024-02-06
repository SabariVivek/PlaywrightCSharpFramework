using Autofac;
using Framework.Core.Config;
using Framework.Core.Driver;
using Framework.Core.Report;
using Microsoft.Playwright;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Framework.Core.WebElements
{
    public class ElementBase
    {
        public string Selector { get; set; }
        public string Label { get; set; }

        public ElementBase(string label, string locatorValue)
        {
            if (!ContainsSpecialCharacters(locatorValue) && !locatorValue.Contains("//"))
            {
                Selector = "#" + locatorValue;
                Label = label;
            }
            else if (!locatorValue.Equals(""))
            {
                Selector = locatorValue;
                Label = label;
            }
            else
            {
                throw new NotImplementedException($"Selector: {Selector} not handled in Implementation");
            }
        }

        /**
         * Resolving Core Framework Autofac Objects...
         */
        public static TestConfiguration GetTestConfiguration => ContainerBase.Container!.Resolve<TestConfiguration>();

        public static IPlaywright GetPlaywright => ContainerBase.Container!.Resolve<IPlaywright>();

        public static IPage GetPage => ContainerBase.Container!.Resolve<IPage>();

        public static IBrowserContext GetBrowserContext => ContainerBase.Container!.Resolve<IBrowserContext>();

        public static IBrowser GetBrowser => ContainerBase.Container!.Resolve<IBrowser>();

        public static IFrameLocator GetFrameLocator => DriverFixture.FrameLocator;

        /**
         * To check the input contains special characters if any...
         * @returns true if matches and vice versa...
         */
        static bool ContainsSpecialCharacters(string input)
        {
            // Define a regular expression pattern for special characters
            string pattern = @"[^a-zA-Z0-9\s]"; // Allow only letters, numbers, and spaces

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to check if the input contains special characters
            return regex.IsMatch(input);
        }

        public static void KillProcesses(Process[] Processes)
        {
            try
            {
                if (Processes.Length > 0)
                {
                    foreach (var Process in Processes)
                    {
                        Process.Kill();
                    }
                }
            }
            catch
            {
            }
        }

        /**
         * Button, Click related functions...
         */
        public async Task ClickAsync(string insideFrame = null)
        {
            try
            {
                if (insideFrame == null)
                {
                    await GetPage.Locator(Selector).ClickAsync();
                }
                else
                {
                    await GetFrameLocator.Locator(Selector).ClickAsync();
                }
                StepsInfo.Pass("Clicked on the \"" + Label + "\" element");
            }
            catch (Exception ex)
            {
                StepsInfo.Fail("Clicked on the \"" + Label + "\" element", ExtentManager.CaptureAsync().Result);
                throw new Exception(ex.Message);
            }
        }

        public async Task DoubleClickAsync(string insideFrame = null)
        {
            try
            {
                if (insideFrame == null)
                {
                    await GetPage.Locator(Selector).DblClickAsync();
                }
                else
                {
                    await GetFrameLocator.Locator(Selector).DblClickAsync();
                }
                StepsInfo.Pass("Double Clicked on the \"" + Label + "\" element");
            }
            catch (Exception ex)
            {
                StepsInfo.Fail(ExtentManager.CaptureAsync().Result, "Double Clicked on the \"" + Label + "\" element");
                throw new Exception(ex.Message);
            }
        }


        /**
         *  Textbox, TextArea related functions...
         */
        public async Task ClearAndTypeTextAsync(string elementValue, string insideFrame = null)
        {
            try
            {
                if (insideFrame == null)
                {
                    await GetPage.Locator(Selector).ClearAsync();
                    await GetPage.Locator(Selector).FillAsync(elementValue);
                }
                else
                {
                    await GetFrameLocator.Locator(Selector).ClearAsync();
                    await GetFrameLocator.Locator(Selector).FillAsync(elementValue);
                }
                StepsInfo.Pass("Cleared & Entered the value : \"" + elementValue + "\" in the \"" + Label + "\" element");
            }
            catch (Exception ex)
            {
                StepsInfo.Fail(ExtentManager.CaptureAsync().Result,
                    "Cleared & Entered the value : \"" + elementValue + "\" in the \"" + Label + "\" element");
                throw new Exception(ex.Message);
            }
        }

        public async Task TypeTextAsync(string elementValue, string insideFrame = null)
        {
            try
            {
                if (insideFrame == null)
                {
                    await GetPage.Locator(Selector).FillAsync(elementValue);
                }
                else
                {
                    await GetFrameLocator.Locator(Selector).FillAsync(elementValue);
                }
                StepsInfo.Pass("Entered the value : \"" + elementValue + "\" in the \"" + Label + "\" element");
            }
            catch (Exception ex)
            {
                StepsInfo.Fail(ExtentManager.CaptureAsync().Result,
                    "Entered the value : \"" + elementValue + "\" in the \"" + Label + "\" element");
                throw new Exception(ex.Message);
            }
        }

        public void SetFrameLocator()
        {
            DriverFixture.FrameLocator = GetPage.FrameLocator(Selector);
        }

        public string GetText(string insideFrame = null)
        {
            if (insideFrame == null)
            {
                return GetPage.Locator(Selector).TextContentAsync().Result.ToString();
            }
            else
            {
                return GetFrameLocator.Locator(Selector).TextContentAsync().Result.ToString();
            }
        }
    }
}