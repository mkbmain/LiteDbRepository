using System;

namespace Mkb.LiteDbRepo
{
    public abstract class LiteDbEntity
    {
        public LiteDbEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        public Guid Id { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public virtual void Mappings()
        {
        }
    }
}
