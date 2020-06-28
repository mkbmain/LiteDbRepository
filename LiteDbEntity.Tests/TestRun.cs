using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using LiteDbEntity.Tests.Data;
using LiteDbEntity.Tests.Data.Entities;
using RepositoryBase;
using Shouldly;
using Xunit;

namespace LiteDbEntity.Tests
{
    // this is by no means a decent test class but should cover all cases and just give a green light on something if i make changes its 8am been going since yesterday

    public class TestRun
    {
        [Fact]
        public async Task Ensure_add_works_and_get_all_with_and_withOut_includes()
        {
            LiteDbEntityMapping.Setup();
            var db = Guid.NewGuid();
            var container = ContainerFactory.GetContainer(db.ToString());
            var repo = container.Resolve<IRepositoryBase<MainDb>>();
            var user = new User {LogInName = "mike"};

            // add
            await repo.Add(user);
            var customer = new Customer {CreatedAt = DateTime.Now,user = user};
            await repo.Add(customer);

            // getall
            var items = repo.GetAll<Customer>();
            items.Count().ShouldBe(1);
            items.First().Id.ShouldBe(customer.Id);

            // get all with includes
            var includes = repo.GetAll<Customer, Customer>(f => true, f => f, true, null, null, f => f.user).ToArray();
            includes.Count().ShouldBe(1);
            includes.First().user.LogInName.ShouldBe("mike");

            // update tested
            includes.First().user.LogInName = "gg";
            includes.Count().ShouldBe(1);
            await repo.Update(includes.First().user);

            includes = repo.GetAll<Customer, Customer>(f => true, f => f, true, null, null, f => f.user).ToArray();
            includes.Count().ShouldBe(1);
            includes.First().user.LogInName.ShouldBe("gg");
            
            // delete 
            await repo.Delete(includes.First());
            includes = repo.GetAll<Customer, Customer>(f => true, f => f, true, null, null, f => f.user).ToArray();
            includes.Count().ShouldBe(0);
        }
    }
}