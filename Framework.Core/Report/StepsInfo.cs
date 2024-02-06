using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports;

namespace Framework.Core.Report
{
    public class StepsInfo
    {
        public static ExtentTest Child { get; set; }

        public static void Label(string Description)
        {
            Child.Pass(MarkupHelper.CreateLabel($"<b>{Description}</b>", ExtentColor.Grey));
        }

        public static void Pass(string Description)
        {
            Child.Pass(Description);
        }

        public static void Fail(string Description, string ScreenshotAsBase64)
        {
            Child.Fail(Description, MediaEntityBuilder.CreateScreenCaptureFromBase64String(ScreenshotAsBase64).Build());
        }

        public static void Skip(string Description)
        {
            Child.Skip(Description);
        }
    }
}