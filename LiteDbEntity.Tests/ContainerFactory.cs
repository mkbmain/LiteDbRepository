using Autofac;
using LiteDbEntity.Tests.Data.Entities;
using Mkb.LiteDbRepo.Implementation;

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