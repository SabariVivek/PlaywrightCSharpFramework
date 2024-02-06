using Autofac;
using Framework.Core.Driver;
using Framework.Object.Pages;

namespace Framework.Object.Base
{
    public class PageObjectRegister
    {
        private readonly IComponentContext _context;

        public PageObjectRegister(IComponentContext context)
        {
            _context = context;
        }

        public LoginPage LoginPage => _context.Resolve<LoginPage>();
    }
}