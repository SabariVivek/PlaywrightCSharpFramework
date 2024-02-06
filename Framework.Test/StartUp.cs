using Autofac;
using Framework.Core.Attributes;
using Framework.Core.Config;
using Framework.Core.Driver;
using Framework.Core.Report;
using Framework.Core.WebElements;
using Framework.Object.Base;
using Framework.Test.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Diagnostics;

namespace Framework.Test
{
    public class StartUp : ContainerBase
    {
        public static TestConfiguration Config { get; set; }
        public static TestConfiguration Data { get; set; } = null!;
        public PageObjectRegister Page { get; private set; } = null!;

        [OneTimeSetUp]
        public void BeforeEverything()
        {
            try
            {
                // Terminating all the browser instance if any...
                TerminateDriverInstance();

                // Setting up the Autofac...
                SetupAutofac(ReadConfig(TestDataService.CORE));

                // Setting up the Extent Report...
                ExtentTestManager.CreateParentTest(GetType().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        [SetUp]
        public void BeforeMethod()
        {
            try
            {
                // Data reading from config.json...
                Data = ReadConfig(TestDataService.ENVIRONMENT, Config!.Environment);

                // Resolving the pageobjects from the autofac...
                Page = Container!.Resolve<PageObjectRegister>();

                // Reading Custom Attributes...
                CustomAttributesReader();

                // Creating child node & Logging category in Extent Report...
                StepsInfo.Child = ExtentTestManager.CreateTest(TestContext.CurrentContext.Test.Name);

                // Adding the Categories to the Extent Report...
                AddCategoryToExtentReport();

                StepsInfo.Pass("<b><<<<< Test Execution : Started >>>>></b>");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        [TearDown]
        public async Task AfterMethodAsync()
        {
            try
            {
                // Closing the instance of the browser...
                await Container!.Resolve<IPage>().CloseAsync();

                StepsInfo.Pass("<b><<<<< Test Execution : Ended >>>>></b>");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        [OneTimeTearDown]
        public void AfterEverything()
        {
            try
            {
                // Flushing the Extent Report...
                ExtentManager.GetInstance().Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void SetupAutofac(TestConfiguration config)
        {
            // Initializing the Autofac Container...
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DriverFixture(config));
            builder.RegisterModule(new PageObjectAutofac(config));

            // Building the Autofac Container...
            Container = builder.Build();

            // Assigning the Environment Value to the TestDataService file...
            Config = Container.Resolve<TestConfiguration>();
        }

        /**
         * This function returns the project directory...
         */
        public static string GetProjectDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            int binIndex = baseDirectory.IndexOf("\\bin\\", StringComparison.OrdinalIgnoreCase);
            return baseDirectory[..binIndex];
        }

        /**
         * Terminating all the driver instance...
         */
        private static void TerminateDriverInstance()
        {
            try
            {
                Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
                ElementBase.KillProcesses(chromeDriverProcesses);
                Process[] EdgeDriverProcesses = Process.GetProcessesByName("msedgedriver");
                ElementBase.KillProcesses(EdgeDriverProcesses);
            }
            catch
            {
            }
        }

        /**
         * Reading the configuration file and return it as in the mentioned return type...
         */
        private static TestConfiguration ReadConfig(string sectionKey, string subSectionKey = null!)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(GetProjectDirectory() + "\\Resources\\config.json")
                .Build();

            if (subSectionKey != null)
            {
                return configuration.GetSection(sectionKey).GetRequiredSection(subSectionKey).Get<TestConfiguration>()!;
            }
            else if (subSectionKey == null)
            {
                return configuration.GetSection(sectionKey).Get<TestConfiguration>()!;
            }

            return null!;
        }

        /**
         * Reading custom attributes...
         */
        private void CustomAttributesReader()
        {
            string className = TestContext.CurrentContext.Test.ClassName!;
            string methodName = TestContext.CurrentContext.Test.Name;

            var methodInfo = Type.GetType(className)!.GetMethod(methodName);
            object[] classAttributes = methodInfo!.GetCustomAttributes(typeof(DatabaseAttribute), false);

            foreach (DatabaseAttribute attribute in classAttributes)
            {
                Console.WriteLine($"Schema : {attribute.Schema}, Table : {attribute.Table}");
            }
        }

        /**
         * Writing the step into the extent report like BDD...
         */
        public void Step(string description)
        {
            StepsInfo.Label(description);
        }

        /**
         * Logging the Category in the Extent Report...
         */
        public void LogCategoryInExtentReport()
        {
            string category = TestContext.CurrentContext.Test.Properties.Get("Category")!.ToString()!;
            StepsInfo.Child!.AssignCategory(category);
        }

        /**
         * Adding the Category details mentioned in the Test Method to Extent Reports...
         */
        public void AddCategoryToExtentReport()
        {
            var testCategories = TestContext.CurrentContext.Test.Properties["Category"];

            if (testCategories != null)
            {
                foreach (var categoryValue in testCategories.ToArray())
                {
                    StepsInfo.Child.AssignCategory(categoryValue.ToString());
                }
            }
            else
            {
                Console.WriteLine("Category attribute has not been mentioned for \"" + TestContext.CurrentContext.Test.Name + "\" method");
                StepsInfo.Child.AssignCategory("Unknown Category");
            }
        }
    }
}