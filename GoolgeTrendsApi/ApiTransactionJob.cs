using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoolgeTrendsApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoolgeTrendsApi
{
    public abstract class ApiTransactionJob
    {

    }

    public abstract class ApiTransactionJob<T> : ApiTransactionJob
    {
        private string _startDate;
        private string _endDate;

        protected string _token;
        protected JToken _jWidget;
        protected string[] _keys;
        protected int _category;
        protected string _property;

        protected string TimeRange => _startDate + " " + _endDate;
        protected virtual bool AllowAutoRedirect { get; } = true;

        public ApiTransactionJob(ApiTransactionArgs args, JToken jWidget = null)
        {
            _startDate = args.StartDate;
            _endDate = args.EndDate;
            _jWidget = jWidget;
            _keys = args.Keys;
            _category = args.Category;
            _property = args.Property ?? string.Empty;

            _token = _jWidget?.Value<string>("token");
        }

        public async Task<T> CallAsync()
        {
            var url = CreateUrl();
            var request = CreateRequest(url);

            var rawContent = await request.GetTextResponseAsync();

            return ParseContent(rawContent);
        }

        protected abstract string CreateUrl();

        protected virtual T ParseContent(string content)
        {
            if (typeof(T) == typeof(string)) return (T)Convert.ChangeType(content, typeof(T));

            return JsonConvert.DeserializeObject<T>(content);
        }


        private HttpWebRequest CreateRequest(string url)
        {
            var req = WebRequest.CreateHttp(url);
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
            req.ContentType = "application/json; charset=utf-8";
            req.Method = "GET";
            req.Referer = GenerateReferer();
            req.AllowAutoRedirect = AllowAutoRedirect;

            req.Headers[HttpRequestHeader.AcceptLanguage] = "zh-CN,zh;q=0.8,zh-TW;q=0.6,en;q=0.4";
            req.Headers.Add("x-client-data", "CIS2yQEIo7bJAQjEtskBCIuYygEI+pzKAQipncoBCNKdygE=");

            return req;
        }

        protected string GenerateReferer()
        {
            var categoryPart = _category > 0 ? ("cat=" + _category + "&") : string.Empty;
            var propertyPart = !string.IsNullOrWhiteSpace(_property) ? ("gprop=" + _property + "&") : string.Empty;

            var referer = $"{GoogleTrendsUrls.PageUrl}?${categoryPart}${propertyPart}q=${string.Join(",", _keys.Select(Uri.EscapeUriString))}";
            return referer;
        }


        protected static string GenerateFullUrl(string prefix, IDictionary<string, string> queries)
        {
            var queryStrings = queries.Select(p => p.Key + "=" + Uri.EscapeUriString(p.Value));
            var url = $"{prefix}?${string.Join("&", queryStrings)}";

            return url;
        }
    }
}
