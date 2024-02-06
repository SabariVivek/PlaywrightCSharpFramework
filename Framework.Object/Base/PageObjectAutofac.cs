using Autofac;
using Framework.Core.Config;
using System.Reflection;
using Module = Autofac.Module;

namespace Framework.Object.Base
{
    public class PageObjectAutofac : Module
    {
        private readonly ITestConfiguration Configuration;

        public PageObjectAutofac(ITestConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var testAssembly = Assembly.GetAssembly(typeof(PageObjectRegister));
            builder.RegisterAssemblyTypes(testAssembly!).AsSelf();
        }
    }
}