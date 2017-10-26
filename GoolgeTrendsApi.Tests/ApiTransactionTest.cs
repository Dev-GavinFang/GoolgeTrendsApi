using System;
using System.Linq;
using System.Threading.Tasks;
using GoolgeTrendsApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace GoolgeTrendsApi.Tests
{
    public class ApiTransactionTest
    {
        private readonly ITestOutputHelper _output;

        public ApiTransactionTest(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public async Task GetWidgetAsync()
        {
            var args = CreateTransactionArgs();
            var tran = await ApiTransaction.StartNewAsync(args);
            var widgets = tran.Widgets;

            var widget = widgets[GoogleTrendsTokenIds.TIMESERIES];
            _output.WriteLine("widget:{0}", widget);
            Assert.NotNull(widget);
            
            Assert.NotNull(widgets);
            Assert.True(widgets.Count > 0);

            _output.WriteLine(string.Join(Environment.NewLine, widgets.Select(p => p.Key + "-" + p.Value)));
        }


        [Fact]
        public async Task GetTrendsAsync()
        {
            var args = CreateTransactionArgs();
            var tran = await ApiTransaction.StartNewAsync(args);

            var widget = tran.Widgets[GoogleTrendsTokenIds.TIMESERIES];
            _output.WriteLine("widget:{0}", widget);
            Assert.NotNull(widget);

            var trends = await tran.GetTrendsAsync();
            _output.WriteLine(trends);
        }

        [Fact]
        public async Task GetComparedGeoAsync()
        {
            var args = CreateTransactionArgs();
            var tran = await ApiTransaction.StartNewAsync(args);

            var widget = tran.Widgets[GoogleTrendsTokenIds.GEO_MAP];
            _output.WriteLine("widget:{0}", widget);
            Assert.NotNull(widget);

            var trends = await tran.GetComparedGeoAsync();
            _output.WriteLine(trends);
        }

        private ApiTransactionArgs CreateTransactionArgs()
        {
            var time = GenerateTimeRange();
            var args = new ApiTransactionArgs
            {
                Category = 0,
                Property = "",
                StartDate = time.start,
                EndDate = time.end,
                Keys = new[] { "北京" }
            };

            return args;
        }

        public static (string start, string end) GenerateTimeRange(string type = "m")
        {
            if (type == "m")
            {
                var now = DateTime.Now;
                var start = now.AddMonths(-1).ToString("yyyy-MM-dd");
                var end = now.ToString("yyyy-MM-dd");

                return (start, end);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
