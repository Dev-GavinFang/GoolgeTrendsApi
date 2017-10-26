using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoolgeTrendsApi.Models;
using GoolgeTrendsApi.Utilities;
using GoolgeTrendsApi.WebGateway.Models;
using GoolgeTrendsApi.WebGateway.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoolgeTrendsApi.WebGateway.Controllers
{
    [Route("api/[controller]")]
    public class GoogleTrendsController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GoogleTrendsController> _logger;
        private readonly ISynonymsProvider _synonymsProvider;

        public GoogleTrendsController(IMemoryCache memoryCache, ILogger<GoogleTrendsController> logger, ISynonymsProvider synonymsProvider)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _synonymsProvider = synonymsProvider;
        }


        [HttpGet]
        public async Task<GetTrendsResponse> Get([FromQuery] GetTrendsDataModel model)
        {
            var tranArgs = Mapper.Map<ApiTransactionArgs>(model);
            if (!tranArgs.Keys.Any())
            {
                return new GetTrendsResponse();
            }

            if (string.IsNullOrWhiteSpace(tranArgs.StartDate) || string.IsNullOrWhiteSpace(tranArgs.EndDate))
            {
                var format = "yyyy-MM-dd";

                tranArgs.StartDate = DateTime.Now.AddMonths(-1).ToString(format);
                tranArgs.EndDate = DateTime.Now.ToString(format);
            }


            var obj = _memoryCache.Get<GetTrendsResponse>(tranArgs.GetHashCode());
            if (obj != null)
            {
                _logger.LogInformation("Cache hint !");
            }
            else
            {
                _logger.LogInformation("Cach missed !");

                obj = await GetFromGoogleAsync(tranArgs);
                _memoryCache.Set(tranArgs.GetHashCode(), obj, DateTimeOffset.Now.AddHours(1));
            }

            return obj;
        }

        [HttpGet("combined")]
        public async Task<GetTrendsResponse> Cobmine([FromQuery] GetTrendsDataModel model)
        {
            var tranArgs = Mapper.Map<ApiTransactionArgs>(model);
            if (!tranArgs.Keys.Any())
            {
                return new GetTrendsResponse();
            }

            if (string.IsNullOrWhiteSpace(tranArgs.StartDate) || string.IsNullOrWhiteSpace(tranArgs.EndDate))
            {
                var format = "yyyy-MM-dd";

                tranArgs.StartDate = DateTime.Now.AddMonths(-1).ToString(format);
                tranArgs.EndDate = DateTime.Now.ToString(format);
            }


            var obj = _memoryCache.Get<GetTrendsResponse>(tranArgs.GetHashCode() + "Combined");
            if (obj != null)
            {
                _logger.LogInformation("Cache hint !");
            }
            else
            {
                _logger.LogInformation("Cach missed !");

                obj = await GetCombainedAsync(tranArgs);
                _memoryCache.Set(tranArgs.GetHashCode() + "Combined", obj, DateTimeOffset.Now.AddHours(1));
            }

            return obj;
        }

        private async Task<GetTrendsResponse> GetCombainedAsync(ApiTransactionArgs tranArgs)
        {
            var key = tranArgs.Keys.First();
            var synoyms = _synonymsProvider.Get(key);
            if (synoyms == null) return await GetFromGoogleAsync(tranArgs);

            var args = synoyms.Select(_ => new ApiTransactionArgs
            {
                Category = tranArgs.Category,
                StartDate = tranArgs.StartDate,
                EndDate = tranArgs.EndDate,
                Keys = new[] { _ },
                Property = tranArgs.Property
            });

            var tasks = args.Select(GetFromGoogleAsync);
            var responses = await Task.WhenAll(tasks);

            return ApiUtilities.CombineResponses(responses);
        }

        private async Task<GetTrendsResponse> GetFromGoogleAsync(ApiTransactionArgs tranArgs)
        {
            var tran = await ApiTransaction.StartNewAsync(tranArgs);
            var trends = await tran.GetTrendsAsync();
            var geo = await tran.GetComparedGeoAsync();

            return new GetTrendsResponse
            {
                Trends = JsonConvert.DeserializeObject<TrendsResult>(trends),
                Geo = JsonConvert.DeserializeObject<ComparedGeoResult>(geo)
            };
        }

    }
}
