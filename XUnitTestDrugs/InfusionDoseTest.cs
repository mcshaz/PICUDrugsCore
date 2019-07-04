using DBToJSON.RepositoryClasses.Enums;
using DrugClasses.PresentationClasses.Dosing;
using DrugClasses.RepositoryClasses;
using System;
using Xunit;

namespace XUnitTestDrugs
{
    public class InfusionDoseTest
    {
        [Fact]
        public void PassingInfusionDose() {
            var id = new DrugDoseUnit(0,Unit.gram,true);
            Assert.Equal("g/kg", id.ToShortString());
            Assert.Equal("grams/kg", id.ToShortUserSafeString()); //note not g/kg - could argue point, but i believe safer prescription
            Assert.Equal("grams/kg", id.ToString());
            id = new DrugDoseUnit(-3, Unit.gram, true);
            Assert.Equal("mg/kg", id.ToShortString());
            Assert.Equal("mg/kg", id.ToShortUserSafeString());
            Assert.Equal("milligrams/kg", id.ToString());
            id = new DrugDoseUnit(-6, "gram", false);
            Assert.Equal("µg", id.ToShortString());
            Assert.Equal("micrograms", id.ToShortUserSafeString());
            Assert.Equal("micrograms", id.ToString());
        }
        [Fact]
        public void ThrowingInfusionDose()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DrugDoseUnit(-2, Unit.gram, true));
        }
        [Fact]
        public void PassingInfusuionRate()
        {
            var id = new InfusionRateUnit(0, Unit.gram, true,true);
            Assert.Equal("g/kg/min", id.ToShortString());
            Assert.Equal("grams/kg/min", id.ToShortUserSafeString()); //note not g/kg - could argue point, but i believe safer prescription
            Assert.Equal("grams/kg/minute", id.ToString());
            id = new InfusionRateUnit(-3, Unit.gram, true,false);
            Assert.Equal("mg/kg/hr", id.ToShortString());
            Assert.Equal("mg/kg/hr", id.ToShortUserSafeString());
            Assert.Equal("milligrams/kg/hour", id.ToString());
            var ru = (DrugDoseUnit)id;
            Assert.Equal("milligrams/kg", ru.ToString());
            var u = (SiUnitMeasure)id;
            Assert.Equal("milligrams", u.ToString());
            id = new InfusionRateUnit(-3, Unit.gram, false, false);
            Assert.Equal("mg/hr", id.ToShortString());
            Assert.Equal("mg/hr", id.ToShortUserSafeString());
            Assert.Equal("milligrams/hour", id.ToString());
            ru = id;
            Assert.Equal("milligrams", ru.ToString());
        }
        [Fact]
        public void SiConcentrationTests()
        {
            var s = new SiConcentration(-3, Unit.gram);
            Assert.Equal("mg/mL", s.ToShortString());
            Assert.Equal("mg/mL", s.ToShortUserSafeString());
            Assert.Equal("milligrams/mL", s.ToString());
            var u = (SiUnitMeasure)s;
            Assert.Equal("milligrams", u.ToString());

            s = new SiConcentration(-6, Unit.gram);
            Assert.Equal("µg/mL", s.ToShortString());
            Assert.Equal("micrograms/mL", s.ToShortUserSafeString());
            Assert.Equal("micrograms/mL", s.ToString());
        }
    }
}
