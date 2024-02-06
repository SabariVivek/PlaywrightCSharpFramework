using Framework.Core.Config;
using Framework.Core.Driver;
using Framework.Core.WebElements;
using Framework.Object.Base;
using Framework.Object.Locators;

namespace Framework.Object.Pages
{
    public class LoginPage : BasePage
    {
        public readonly LoginLocator obj;

        public LoginPage() : base()
        {
            obj = new LoginLocator();
        }

        public async Task LoginAsync(TestConfiguration Data)
        {
            await GoToPageAsync();
            await obj.Username.TypeTextAsync(Data.Username);
            await obj.Password.TypeTextAsync(Data.Password);

            // Sample Swich To Window Code...
            await Browser.SwitchToChildWindow();
            string url = await ElementBase.GetPage.TitleAsync();

            await Browser.SwitchToParentWindow();
            await obj.Login.ClickAsync();

            // Sample Frame Handling...
            obj.Username.SetFrameLocator();
            await obj.Password.ClickAsync(If);
        }
    }
}