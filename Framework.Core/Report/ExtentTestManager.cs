using AventStack.ExtentReports;
using System.Runtime.CompilerServices;

namespace Framework.Core.Report
{
    public class ExtentTestManager
    {
        [ThreadStatic]
        private static ExtentTest _parentTest;

        [ThreadStatic]
        private static ExtentTest _childTest;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateParentTest(string testName, string description = null!)
        {
            _parentTest = ExtentManager.GetInstance().CreateTest(testName, description);
            return _parentTest;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateTest(string testName, string description = null!)
        {
            _childTest = _parentTest!.CreateNode(testName, description);
            return _childTest;
        }
    }
}