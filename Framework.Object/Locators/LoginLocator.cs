using Framework.Core.WebElements;

namespace Framework.Object.Locators
{
    public class LoginLocator
    {
        public Locator Username = new("Username", "email");
        public Locator Password = new("Password", "//fieldset[@class='fieldset login']//input[@id='pass']");
        public Locator Login = new("Login", "//fieldset[@class='fieldset login']//span[contains(text(),'Sign In')]");
        public Locator WhatsNew = new("What's New", "//a[contains(@href,'what-is-new')]");
    }
}