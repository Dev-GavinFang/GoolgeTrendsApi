using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GoolgeTrendsApi.Models
{
    public class TrendsResult
    {
        [JsonProperty("default")]
        public Default Default { get; set; }
    }

    public class Default
    {
        public Timelinedata[] timelineData { get; set; }
        public int[] averages { get; set; }
    }

    public class Timelinedata
    {
        public string time { get; set; }
        public string formattedTime { get; set; }
        public string formattedAxisTime { get; set; }
        public int[] value { get; set; }
        public string[] formattedValue { get; set; }
    }


    public class ComparedGeoResult
    {
        [JsonProperty("default")]
        public GeoDefault Default { get; set; }
    }

    public class GeoDefault
    {
        public GeoMapData[] geoMapData { get; set; }
    }

    public class GeoMapData
    {
        public Coordinates coordinates { get; set; }
        public string geoName { get; set; }
        public int[] value { get; set; }
        public string[] formattedValue { get; set; }
        public int maxValueIndex { get; set; }
    }

    public class Coordinates
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }
}
