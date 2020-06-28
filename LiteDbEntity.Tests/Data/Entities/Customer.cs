using System;

namespace LiteDbEntity.Tests.Data.Entities
{
    public class Customer : MainDb
    {
        public DateTime CreatedAt { get; set; }
        public User user { get; set; }

        public override void Mappings()
        {
            LiteDB.BsonMapper.Global.Entity<Customer>().DbRef(x => x.user, nameof(user));
        }
    }
}