using DrugClasses.Utilities;
using System;
using Xunit;

namespace XUnitTestDrugs
{
    public class NumericRangeTests
    {
        [Fact]
        public void TestPassingIntegerRange()
        {
            var ir = new NumericRange<int>();
            Assert.Equal("–", ir.Separator);
            ir.LowerBound = 3;
            ir.UpperBound = 5;
            Assert.Equal("3–5", ir.ToString());
            ir.UpperBound = 3;
            Assert.Equal("3", ir.ToString());
            ir = new NumericRange<int>(3,5);
            Assert.Equal("3–5", ir.ToString());
            ir = new NumericRange<int>(5,3);
            Assert.Equal("3–5", ir.ToString());
        }
        [Fact]
        public void TestFailingIntegerRange()
        {
            var ir = new NumericRange<int>
            {
                LowerBound = 5
            };
            Assert.Throws<ArgumentOutOfRangeException>(()=>ir.UpperBound = 3);
        }
    }
}
