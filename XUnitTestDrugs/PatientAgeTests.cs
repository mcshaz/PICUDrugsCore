using DrugClasses.PresentationClasses.Patient;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestDrugs
{
    
    public class PatientAgeTests
    {
        [Theory]
        [ClassData(typeof(DobTestData))]
        public void DOBAgeTests(DateTime today, DateTime dob, int years, int months, int days)
        {
            var todayMock = new TodayMock { Today = today };
            var age = new ChildAgeFromDOB(dob, todayMock);
            Assert.Equal(years, age.Years);
            Assert.Equal(months, age.Months);
            Assert.Equal(days, age.Days);
        }
        [Fact]
        public void TestFutureDob()
        {
            var dob = DateTime.Today.AddDays(1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new ChildAgeFromDOB(dob));
        }
        [Fact]
        public void TestBornYesterday()
        {
            var dob = DateTime.Today.AddDays(-1);
            var age = new ChildAgeFromDOB(dob);
            Assert.Equal(0, age.Years);
            Assert.Equal(0, age.Months);
            Assert.Equal(1, age.Days);
        }
        class TodayMock : ITodayProvider
        {
            public DateTime Today { get; set; }
        }
    }
    public class DobTestData : TheoryData<DateTime, DateTime, int, int, int>
    {
        public DobTestData()
        {
            Add(new DateTime(2020, 1, 31), new DateTime(2016, 1, 31), 4, 0, 0);
            Add(new DateTime(2020, 2, 1), new DateTime(2016, 1, 31), 4, 0, 1);
            Add(new DateTime(2020, 1, 31), new DateTime(2016, 2, 1), 3, 11, 30);
            //leap day tests
            Add(new DateTime(2020, 2, 29), new DateTime(2016, 2, 29), 4, 0, 0);
            Add(new DateTime(2019, 2, 28), new DateTime(2016, 2, 29), 2, 11, 30);
            //arguable below should be 1 day, but 0 seems acceptable
            Add(new DateTime(2019, 3, 1), new DateTime(2016, 2, 29), 3, 0, 0);
        }
    }
}
