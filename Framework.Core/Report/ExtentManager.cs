using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using Framework.Core.WebElements;
using System.Reflection;

namespace Framework.Core.Report
{
    public class ExtentManager
    {
        public static ExtentReports ExtentReports;
            
        public static ExtentReports GetInstance()
        {
            if (null == ExtentReports) CreateInstance();
            return ExtentReports;
        }

        private static void CreateInstance()
        {
            string RootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string WorkSpaceDirectory = RootDirectory.Split(new string[] { "\\bin", "\\Debug" }, StringSplitOptions.None)[0];
            ExtentSparkReporter htmlReporter = new(WorkSpaceDirectory + "\\ExecutionReports\\FinalReport.html");

            //------ Extent Spark Report Configuration ------//
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Automation Report";
            htmlReporter.Config.ReportName = "Automation Report";

            ExtentReports = new ExtentReports();
            ExtentReports.AttachReporter(htmlReporter);
        }

        public static Task<string> CaptureAsync()
        {
            // Capture a screenshot
            var screenshot = ElementBase.GetPage.ScreenshotAsync().GetAwaiter().GetResult();

            // Convert the screenshot to base64
            var base64Screenshot = Convert.ToBase64String(screenshot);

            return Task.FromResult(base64Screenshot);
        }

        private ExtentManager() { }
    }
}