using Autofac;
using DbLiteCore.Implementation;
using LiteDbEntity.Tests.Data.Entities;

namespace LiteDbEntity.Tests
{
    public class ContainerFactory
    {
        public static IContainer GetContainer(string dbName)
        {
            var container = new ContainerBuilder();
            container.Register(f => new LiteDbRepo<MainDb>(dbName));
            return container.Build();
        }
    }
}