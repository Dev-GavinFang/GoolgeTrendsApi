using System.Collections.Generic;
using System.Linq;
using GoolgeTrendsApi.Models;
using GoolgeTrendsApi.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoolgeTrendsApi.TransactionJobs
{
    public sealed class GetWidgetsJob : ApiTransactionJob<Dictionary<string, JToken>>
    {
        public GetWidgetsJob(ApiTransactionArgs args) : base(args, null)
        {
        }

        protected override string CreateUrl()
        {
            var comparisonItems = _keys.Select(key => new { geo = string.Empty, keyword = key, time = TimeRange }).ToArray();

            var req = new
            {
                category = _category,
                property = _property,
                comparisonItem = comparisonItems
            };

            var queries = new Dictionary<string, string>
            {
                ["hl"] = "zh-CN",
                ["tz"] = "-480",
                ["req"] = JsonConvert.SerializeObject(req)
            };

            var url = GenerateFullUrl(GoogleTrendsUrls.GetInitWidgetUrl, queries);
            return url;
        }

        protected override Dictionary<string, JToken> ParseContent(string content)
        {
            return ApiUtilities.ExtractHasIdWidgets(content);
        }
    }
}
