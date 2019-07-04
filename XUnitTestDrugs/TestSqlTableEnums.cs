using DBToJSON;
using DBToJSON.SqlEntities;
using DBToJSON.SqlEntities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace XUnitTestDrugs
{

    public class TestEnums
    {
        //no longer in use where this is important
        public void TestSqlTableEnums()
        {
            var enumVals = Enum.GetNames(typeof(NosqlTable)).OrderBy(n=>n);
            var tables = (from p in typeof(DrugSqlContext).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          where p.CanRead && p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)
                          let genericType = p.PropertyType.GetGenericArguments()[0]
                          where genericType.GetInterfaces().Any(i => i.Name == "INosqlTable") //magic string because it is internal
                          select genericType.Name);
            Assert.Equal(enumVals,tables);

        }
    }
}
