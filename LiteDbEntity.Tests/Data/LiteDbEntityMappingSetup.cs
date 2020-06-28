using System;
using System.Reflection;
using LiteDbEntity.Tests.Data.Entities;
using Mkb.Util;

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
            foreach (var allType in Assembly.GetAssembly(typeof(Customer)).GetAllTypes<DbLiteCore.LiteDbEntity>(false, false))
            {
                var efe = (DbLiteCore.LiteDbEntity) Activator.CreateInstance(allType);
                efe.Mappings();
            }
        }
    }
}