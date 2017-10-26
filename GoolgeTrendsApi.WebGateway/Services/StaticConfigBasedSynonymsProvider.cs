using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoolgeTrendsApi.WebGateway.Options;
using Microsoft.Extensions.Options;

namespace GoolgeTrendsApi.WebGateway.Services
{
    class StaticConfigBasedSynonymsProvider : ISynonymsProvider
    {
        private readonly SynonymsOption _option;

        public StaticConfigBasedSynonymsProvider(IOptions<SynonymsOption> option)
        {
            _option = option.Value;
        }

        public IList<string> Get(string key)
        {
            return _option.Syonyms.FirstOrDefault(_=> _.Keywords.Contains(key))?.Keywords;
        }
    }
}
