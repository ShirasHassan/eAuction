using System;
namespace eAuction.BaseLibrary.Domain
{
    public class EntityStatus
     : Enumeration
    {
        public static EntityStatus Active = new(1, nameof(Active));
        public static EntityStatus Deleted = new(2, nameof(Deleted));
        public static EntityStatus Suspended = new(3, nameof(Suspended));

        public EntityStatus(int id, string name)
            : base(id, name)
        {
        }
    }
}
