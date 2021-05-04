using System;
using System.Linq;
using System.Reflection;
using LiteDbEntity.Tests.Data.Entities;

namespace LiteDbEntity.Tests.Data
{
    public static class LiteDbEntityMapping
    {
        private static bool Done=false;
        public static void Setup()
        {
            if (Done)
            {
                return;
            }

            Done = true;
            foreach (var allType in Assembly.GetAssembly(typeof(Customer)).GetTypes().Where(f=> f is DbLiteCore.LiteDbEntity))
            {
                var efe = (DbLiteCore.LiteDbEntity) Activator.CreateInstance(allType);
                efe.Mappings();
            }
        }
    }
}