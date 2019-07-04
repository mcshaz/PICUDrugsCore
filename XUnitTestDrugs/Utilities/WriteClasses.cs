using DrugClasses.RepositoryClasses;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace XUnitTestDrugs.Utilities
{
    public class WriteClasses
    {
        private readonly ITestOutputHelper output;

        public WriteClasses(ITestOutputHelper output)
        {
            this.output = output;
        }
        
//[Fact]
public void CreateVIV()
{
    IEnumerable<VariableInfusionView> newborn;
    IEnumerable<VariableInfusionView> adolescent;
    IEnumerable<VariableInfusionView> infant;
    using (var db = new MyContext())
    {
        newborn = db.GetVariableInfusionView(2.8, 1, 0);
        infant = db.GetVariableInfusionView(5.7, 1, 3);
        adolescent = db.GetVariableInfusionView(73, 1, 156);
    }
    using (var sw = new StringWriter())
    {
        var ci = new CreateCSTextInstances<VariableInfusionView>(sw);
        ci.AddObjects(newborn.Take(1));
        output.WriteLine(sw.ToString());
        output.WriteLine(string.Empty);

        sw.GetStringBuilder().Clear();
        ci.AddObjects(infant);
        output.WriteLine(sw.ToString());
        output.WriteLine(string.Empty);

        sw.GetStringBuilder().Clear();
        ci.AddObjects(adolescent);
        output.WriteLine(sw.ToString());
    }

}
//[Theory]
//[MemberData(nameof(Data))]
public void CreateFIV(int drugId, int dilutionMethodId, int ageMonths, double weight)
{
    ICollection<FixedInfusionView> data;
    using (var db = new MyContext())
    {
        data = db.GetFixedInfusionView(weight, drugId, ageMonths);
    }
    using (var sw = new StringWriter())
    {
        output.WriteLine($"// Dilution Method Id: { dilutionMethodId } wt: { weight } ageMth: { ageMonths }");
        var ci = new CreateCSTextInstances<FixedInfusionView>(sw);
        ci.AddObjects(data);
        output.WriteLine(sw.ToString());
        output.WriteLine(string.Empty);
    }
}

public static TheoryData<int, int, int, double> Data =>
    new TheoryData<int, int, int, double>
    {
        {9,2,600,50},
        {6,3,600,70},
        {8,4,600,50},
        {7,6,606,28},
        {1,7,71,10}
    };
    }
}

/*
 * Use PicuDrugData;

WITH summary AS (
   SELECT d.*,
          ROW_NUMBER() OVER(PARTITION BY d.DilutionMethodId
                               ORDER BY d.WeightMin) AS rk
     FROM FixedTimeDilutions d)
SELECT s.InfusionDrugId, s.DilutionMethodId, (s.AgeMinMonths +s.AgeMaxMonths)/2 as AgeMonths, (s.WeightMin + s.WeightMax)/2 as WeightKg
 FROM summary s 
WHERE s.rk = 1
 */
