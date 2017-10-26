using System;
using System.Collections.Generic;
using System.Linq;
using GoolgeTrendsApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoolgeTrendsApi.TransactionJobs
{
    public sealed class GetComparedGeoJob : ApiTransactionJob<string>
    {
        public GetComparedGeoJob(ApiTransactionArgs args, JToken jWidget = null) : base(args, jWidget)
        {
        }

        protected override string CreateUrl()
        {
            var jReq = _jWidget.Value<JToken>("request");

            var req = new
            {
                geo = jReq.Value<JToken>("geo"),
                resolution = GoogleTrendsGeoResolutions.CITY,
                locale = jReq.Value<string>("locale"),
                comparisonItem = jReq.Value<JToken>("comparisonItem"),
                requestOptions = jReq.Value<JToken>("requestOptions"),
                includeLowSearchVolumeGeos = true
            };
            
            var queries = new Dictionary<string, string>
            {
                ["hl"] = "zh-CN",
                ["tz"] = "-480",
                ["req"] = JsonConvert.SerializeObject(req),
                ["token"] = _token
            };
            
            return GenerateFullUrl(GoogleTrendsUrls.GetComparedGeoUrl, queries);
        }

        protected override string ParseContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;

            const string wrongPrefix = ")]}',";
            if (content.StartsWith(wrongPrefix))
            {
                return content.Substring(wrongPrefix.Length);
            }

            if (content.First() != '{')
            {
                var index = content.IndexOf('{');
                return content.Substring(index);
            }

            return base.ParseContent(content);
        }
    }
}
