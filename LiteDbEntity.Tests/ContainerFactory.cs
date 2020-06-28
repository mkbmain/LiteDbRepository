using Autofac;
using DbLiteCore.Implementation;
using LiteDbEntity.Tests.Data.Entities;
using RepositoryBase;

namespace LiteDbEntity.Tests
{
    public class ContainerFactory
    {
        public static IContainer GetContainer(string dbName)
        {
            var container = new ContainerBuilder();
            container.Register<IRepositoryBase<MainDb>>(f => new LiteDbRepo<MainDb>(dbName));
            return container.Build();
        }
    }
}