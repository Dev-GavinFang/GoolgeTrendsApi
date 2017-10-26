using System.Collections.Generic;
using System.Linq;
using GoolgeTrendsApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoolgeTrendsApi.TransactionJobs
{
    public sealed class GetTrendsJob : ApiTransactionJob<string>
    {
        public GetTrendsJob(ApiTransactionArgs args, JToken jWidget) : base(args, jWidget)
        {
        }

        protected override string CreateUrl()
        {
            var req = _jWidget.Value<JToken>("request");

            var value = new Dictionary<string, string>
            {
                ["hl"] = "zh-CN",
                ["tz"] = "-480",
                ["req"] = JsonConvert.SerializeObject(req),
                ["token"] = _token
            };

            return GenerateFullUrl(GoogleTrendsUrls.GetTrendsUrl, value);
        }

        protected override string ParseContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;

            const string wrongPrefix = ")]}',";
            if (content.StartsWith(wrongPrefix))
            {
                return content.Substring(wrongPrefix.Length);
            }

            if(content.First() != '{')
            {
                var index = content.IndexOf('{');
                return content.Substring(index);
            }

            return base.ParseContent(content);
        }
    }
}
