using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace UtilityTools.Modules.ModuleName.Tests
{
    public class ResultTest
    {
        [Fact]
        public void RegexTest()
        {
            var address = @"http://abc/abc/5";

            var nextAddress = Regex.Replace(address, @"/(?<key>[\d]*)$", (mc) =>
            {
                var val = int.Parse(mc.Groups["key"].Value);
                return $"/{val + 1}";
            });

            if (nextAddress == address)
            {
                nextAddress = nextAddress + "/2";
            }

            Assert.Equal("http://abc/abc/6", nextAddress);
        }
    }
}
