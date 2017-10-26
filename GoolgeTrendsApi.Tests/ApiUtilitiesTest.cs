using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoolgeTrendsApi.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace GoolgeTrendsApi.Tests
{
    public class ApiUtilitiesTest
    {
        private readonly ITestOutputHelper _output;

        public ApiUtilitiesTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CombindArrays_Single()
        {
            var array = Enumerable.Range(1, 10).ToArray();

            var combined = ApiUtilities.CombineArrarys(new[] { array });

            Assert.Equal(array, combined);
        }

        [Fact]
        public void CombindArrays_Two()
        {
            var array1 = Enumerable.Range(1, 10).ToArray();
            var array2 = Enumerable.Range(11, 10).ToArray();

            var combined = ApiUtilities.CombineArrarys(new[] { array1, array2 });

            var expected = Enumerable.Range((1 + 11) / 2, 10);
            Assert.Equal(combined, expected);
        }


        [Fact]
        public void CombindArrays_Multiple()
        {
            var starts = new[] { 1, 11, 21, 31 };
            var arrays = starts.Select(_ => Enumerable.Range(_, 10).ToArray()).ToArray();

            var combined = ApiUtilities.CombineArrarys(arrays);

            var expected = Enumerable.Range(starts.Sum() / starts.Length, 10);
            Assert.Equal(combined, expected);
        }
    }
}
