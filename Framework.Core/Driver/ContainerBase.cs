using Autofac;

namespace Framework.Core.Driver
{
    public class ContainerBase
    {
        public static IContainer Container { get; protected set; }
    }
}