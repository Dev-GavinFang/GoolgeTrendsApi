using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoolgeTrendsApi.WebGateway.Models
{
    public class GetTrendsDataModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Keys { get; set; }

        public int Category { get; set; } = 0;
        public string Property { get; set; } = string.Empty;
    }
}
