using Framework.Core.Attributes;
using NUnit.Framework;

namespace Framework.Test.Tests
{
    public class LoginTest : StartUp
    {
        [Test]
        [Category("Smoke"), Category("Regression")]
        [Database(schema: "TC01_DataBase", table: "QA_Customer")]
        public async Task LoginAndLogoutTest()
        {
            Step("Given Login into the application");
            await Page.LoginPage.LoginAsync(Data);
        }
    }
}