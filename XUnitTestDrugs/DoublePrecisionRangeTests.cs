using DrugClasses.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestDrugs
{
    public class DoublePrecisionRangeTests
    {
        [Fact]
        public void TestPassingDoublePrecisionRange()
        {
            var ir = new DoublePrecisionRange();
            Assert.Equal("–", ir.Separator);
            Assert.Equal(2, ir.Precision);
            Assert.Equal(RoundingMethod.ToPrecision, ir.Rounding);
            ir.LowerBound = 3.0;
            ir.UpperBound = 5.0;
            Assert.Equal("3–5", ir.ToString());
            ir = ir * 2;
            Assert.Equal("6–10", ir.ToString());
            ir.UpperBound = 6.0;
            Assert.Equal("6", ir.ToString());
            ir = new DoublePrecisionRange(3012, 5859);
            Assert.Equal("3000–5900", ir.ToString());
            ir = new DoublePrecisionRange(5.003, 3.678);
            Assert.Equal("3.7–5", ir.ToString());
            ir.Rounding = RoundingMethod.FixedDecimalPlaces;
            Assert.Equal("3.68–5.00", ir.ToString());
        }
        [Fact]
        public void TestDoublePrecisionRangeExceptions()
        {
            var ir = new DoublePrecisionRange
            {
                LowerBound = 5
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => ir.UpperBound = 3);
        }
    }
}
