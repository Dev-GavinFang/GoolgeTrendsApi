using System.Collections.Generic;

namespace GoolgeTrendsApi.WebGateway.Services
{
    public interface ISynonymsProvider
    {
        IList<string> Get(string key);
    }
}
