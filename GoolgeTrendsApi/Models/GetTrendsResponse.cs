using Newtonsoft.Json;

namespace GoolgeTrendsApi.Models
{
    public class GetTrendsResponse
    {
        [JsonProperty("trends")]
        public TrendsResult Trends { get; set; }

        [JsonProperty("geo")]
        public ComparedGeoResult Geo { get; set; }
    }
}
