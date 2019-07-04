using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestDrugs.Utilities.Linq
{
    public class AsSetsTests
    {
        [Fact]
        public void TestSets()
        {
            var a = new[] { 1,1,2,2,3,3 };
            var b = new[] { 6,6,4,4,2,2 };
            var res = a.ToSets(b);
            Assert.Equal(new[] { 1, 3 }, res[SetResult.LeadingSetOnly]);
            Assert.Equal(new[] { 2 }, res[SetResult.Intersect]);
            Assert.Equal(new[] { 6, 4 }, res[SetResult.TrailingSetOnly]);
        }
    }
}
