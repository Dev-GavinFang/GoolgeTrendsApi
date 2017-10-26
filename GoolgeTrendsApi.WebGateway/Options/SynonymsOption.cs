using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace GoolgeTrendsApi.WebGateway.Options
{
    public class SynonymsOption : IOptions<SynonymsOption>
    {
        public SynonymsOption Value => this;

        public List<Syonyms> Syonyms { get; set; } = new List<Syonyms>();
    }

    public class Syonyms
    {
        public List<string> Keywords { get; set; } = new List<string>();
    }
}
