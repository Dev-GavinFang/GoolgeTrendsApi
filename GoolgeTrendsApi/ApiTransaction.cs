using System.Collections.Generic;
using System.Threading.Tasks;
using GoolgeTrendsApi.Models;
using GoolgeTrendsApi.TransactionJobs;
using Newtonsoft.Json.Linq;

namespace GoolgeTrendsApi
{
    public class ApiTransaction
    {
        private ApiTransactionArgs _args;
        public Dictionary<string, JToken> Widgets { get; private set; } = new Dictionary<string, JToken>();
        
        public ApiTransaction(ApiTransactionArgs args)
        {
            _args = args;
        }

        public static async Task<ApiTransaction> StartNewAsync(ApiTransactionArgs args)
        {
            var tran = new ApiTransaction(args);
            await tran.StartAsync();

            return tran;
        }


        public async Task StartAsync()
        {
            var job = new GetWidgetsJob(_args);
            Widgets = await job.CallAsync();
        }

        public async Task<string> GetTrendsAsync()
        {
            var job = new GetTrendsJob(_args, Widgets[GoogleTrendsTokenIds.TIMESERIES]);
            return await job.CallAsync();
        }

        public async Task<string> GetComparedGeoAsync()
        {
            var job = new GetComparedGeoJob(_args, Widgets[GoogleTrendsTokenIds.GEO_MAP]);
            return await job.CallAsync();
        }
    }
}
