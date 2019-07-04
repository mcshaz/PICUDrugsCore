using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestDrugs.Utilities.Linq
{
    public class TestToEcmaPath
    {
        [Fact]
        public void TestRelativePath()
        {
            var a = CSToTStool.MakeEcmaPath(@"\foo", @"\foo");
            Assert.Equal("./", a);
            a = CSToTStool.MakeEcmaPath(@"\foo", @"\foo\bar");
            Assert.Equal("./bar/", a);
            a = CSToTStool.MakeEcmaPath("", @"\foo\bar");
            Assert.Equal("./foo/bar/", a);
            a = CSToTStool.MakeEcmaPath(@"\foo\bar", "");
            Assert.Equal("./../../", a);
            a = CSToTStool.MakeEcmaPath(@"\bar\foo", @"\foo\bar");
            Assert.Equal("./../../foo/bar/", a);
        }
    }
}