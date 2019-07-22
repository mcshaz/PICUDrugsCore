using DBToJSON;
using DBToJSON.JsonSerializers;
using DBToJSON.JsonSerializers.Helpers;
using DBToJSON.JSONSerializers.Helpers;
using DBToJSON.SqlEntities.Infusions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using XUnitTestDrugs.Utilities;

namespace XUnitTestDrugs
{
    //[CollectionDefinition("Json",DisableParallelization = true)]
    public class TestJson
    {
        public TestJson(ITestOutputHelper log)
        {
            Log = log;
        }
        private ITestOutputHelper Log { get; }
        [Fact]
        public void TestSerializeExceptResolver()
        {
            var c = GetCompany();
            var resolve = new SerializeExceptResolver();
            resolve.ExcludeAttribute<KeyAttribute>(mi => mi.DeclaringType != typeof(Company));
            resolve.ExcludeAttribute<ForeignKeyAttribute>();
            resolve.ExcludeOtherPropByAttribute<ForeignKeyAttribute>(fk => fk.Name);
            var JSON = JsonConvert.SerializeObject(
                c,
                new JsonSerializerSettings
                {
                    ContractResolver = resolve,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            Assert.Equal("{\"CompanyId\":3,\"Name\":\"Foo\",\"Customers\":[{\"Name\":\"John\"},{\"Name\":\"Jill\"}]}", JSON);
        }
        [Fact]
        public void TestEnumerableToValueArrayConverter()
        {
            var c = GetCompany();
            var conv = new EnumerableClassToValueArrayConverter();
            conv.AddConverter<Customer>(cust => cust.CustomerId);
            var JSON = JsonConvert.SerializeObject(c, conv);
            Assert.Equal("{\"CompanyId\":3,\"Name\":\"Foo\",\"Customers\":[12,13]}", JSON);
        }
        [Fact]
        public async Task TestDatabaseFullSerializers()
        {
            var sb = new StringBuilder();
            StringWriter sw = null;
            TextWriter tw() => sw = new StringWriter(sb);
            await GetAllJson.WriteNosqlData(null, tw);
            sw.Dispose(); //don't really need to dispose it, just figuring out how to use it with a real stream
            Log.WriteLine(sb.ToString());
        }
        [Fact]
        public async Task TestDatabaseFromDateSerializers()
        {
            var sb = new StringBuilder();
            StringWriter sw = null;
            TextWriter tw() => sw = new StringWriter(sb);
            await GetAllJson.WriteNosqlData(DateTime.UtcNow, tw);
            sw.Dispose(); //don't really need to dispose it, just figuring out how to use it with a real stream
        }

        //[Fact]
#pragma warning disable xUnit1013 // Public method should be marked as test
        public async Task TestInfusionSerializer()
#pragma warning restore xUnit1013 // Public method should be marked as test
        {
            IEnumerable<InfusionDrug> id;
            using (var db = new DrugSqlContext())
            {
                id = await InfusionDrugSerializer.GetAndSortInfusions(null, db);
            }
            var sb = new StringBuilder();
            using (TextWriter sw = new StringWriter(sb))
            {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    //InfusionDrugSerializer.InfusionDrugJsonSerializer.Serialize(jw, id.Take(5));
                }
            }
            Log.WriteLine(sb.ToString());
        }

        //[Theory(Timeout = 500)]
        //[ClassData(typeof(IncludeData))]
#pragma warning disable xUnit1013 // Public method should be marked as test
        public void FindBadCombos(string includes, IEnumerable<InfusionDrug> infusion)
#pragma warning restore xUnit1013 // Public method should be marked as test
        {
            Log.WriteLine("starting:\r\n" + includes);
            JsonConvert.SerializeObject(infusion, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            Log.WriteLine("success");
        }
        private static Company GetCompany()
        {
            var c = new Company
            {
                CompanyId = 3,
                Name = "Foo"
            };
            var cust1 = new Customer
            {
                CustomerId = 12, CompanyId = 3, Name = "John", Company = c
            };
            var cust2 = new Customer
            {
                CustomerId = 13,
                CompanyId = 3,
                Name = "Jill",
                Company = c
            };
            c.Customers = new[] { cust1, cust2 };
            return c;
        }

    }

    class IncludeData : TheoryData<int, string, IEnumerable<InfusionDrug>>
    {
        public IncludeData()
        {
            var h = new IncludeHelper<InfusionDrug>();
            h.Add(id => id.DrugAmpuleConcentrations)
            .Add(id => id.DrugReferenceSource)
            .Add("FixedTimeDilutions.FixedTimeConcentrations")
            .Add("VariableTimeDilutions.VariableTimeConcentrations.DoseCat");
            using (var db = new DrugSqlContext())
            {
                for (int groupSize = 3; groupSize <= 5; groupSize++)
                {
                    foreach (var p in UnorderedPermutations.GetPermutations(h.Includes, groupSize))
                    {
                        var ih = new IncludeHelper<InfusionDrug>(p);
                        var infusions = ih.AddIncludes(db.InfusionDrugs.AsNoTracking()).Take(3).ToList();
                        Add(groupSize, ih.ToString(), infusions);
                    }
                }
            }
        }
    }


    class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
    class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
