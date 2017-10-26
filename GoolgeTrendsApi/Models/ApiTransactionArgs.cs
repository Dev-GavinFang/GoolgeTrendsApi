using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoolgeTrendsApi.Models
{
    public class ApiTransactionArgs
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string[] Keys { get; set; }

        public int Category { get; set; } = 0;
        public string Property { get; set; } = string.Empty;

        public override int GetHashCode()
        {
            return (StartDate?.GetHashCode() ?? 0) ^ (EndDate?.GetHashCode() ?? 0)
                ^ string.Join(",", (Keys ?? Array.Empty<string>()).OrderBy(_ => _)).GetHashCode() ^ Category.GetHashCode()
                ^ (Property?.GetHashCode() ?? 0);
        }
    }
}
