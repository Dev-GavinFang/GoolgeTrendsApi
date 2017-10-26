using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoolgeTrendsApi.Models;
using Newtonsoft.Json.Linq;

namespace GoolgeTrendsApi.Utilities
{
    public static class ApiUtilities
    {
        public static Dictionary<string, JToken> ExtractHasIdWidgets(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return null;

            const string prefix = ")]}'";
            if (content.StartsWith(prefix))
            {
                content = content.Substring(prefix.Length);
            }

            var widgets = JToken.Parse(content).SelectTokens("widgets[*]");
            var pairs = widgets.Where(_ => _.HasValues && !string.IsNullOrEmpty(_.Value<string>("id")))
                        .ToDictionary(_ => _.Value<string>("id"), _ => _);

            return pairs;
        }


        public static GetTrendsResponse CombineResponses(IList<GetTrendsResponse> responses)
        {
            var trends = CombineTrendsResults(responses.Select(_ => _.Trends).ToList());
            ScaleValues(trends.Default.timelineData.Select(_ => _.value));

            var geo = CombineComparedGeoResults(responses.Select(_ => _.Geo).ToList());
            ScaleValues(geo.Default.geoMapData.Select(_ => _.value));

            return new GetTrendsResponse
            {
                Trends = trends,
                Geo = geo
            };

            void ScaleValues(IEnumerable<int[]> values, int to = 100)
            {
                var max = values.SelectMany(_ => _).Max();
                if (max < 100)
                {
                    var rate = 100f / max;
                    foreach (var v in values)
                    {
                        for (var i = 0; i < v.Length; i++)
                        {
                            v[i] = (int)(Math.Floor(v[i] * rate));
                        }
                    }
                }
            }
        }

        public static TrendsResult CombineTrendsResults(IList<TrendsResult> results)
        {
            if (results.Count <= 1) return results.FirstOrDefault();

            var grouped = results.SelectMany(_ => _.Default.timelineData).GroupBy(_ => $"{_.time}-{_.formattedTime}-{_.formattedAxisTime}");
            var combinedTimeline = new List<Timelinedata>();

            foreach (var g in grouped)
            {
                var any = g.First();
                var values = CombineArrarys(g.Select(_ => _.value));

                var timeline = new Timelinedata
                {
                    time = any.time,
                    formattedTime = any.formattedTime,
                    formattedAxisTime = any.formattedAxisTime,
                    value = values,
                    formattedValue = values.Select(_ => _.ToString()).ToArray()
                };

                combinedTimeline.Add(timeline);
            }

            var combined = new TrendsResult
            {
                Default = new Default
                {
                    averages = CombineArrarys(results.Select(_ => _.Default.averages)),
                    timelineData = combinedTimeline.OrderBy(_ => _.time).ToArray()
                }
            };

            return combined;
        }

        public static ComparedGeoResult CombineComparedGeoResults(IList<ComparedGeoResult> results)
        {
            if (results.Count <= 1) return results.FirstOrDefault();

            var grouped = results.SelectMany(_ => _.Default.geoMapData).GroupBy(_ => $"{_.geoName}-{_.coordinates.lat}-{_.coordinates.lng}");
            var combinedMapData = new List<GeoMapData>();

            foreach (var g in grouped)
            {
                var any = g.First();
                var values = CombineArrarys(g.Select(_ => _.value));

                var timeline = new GeoMapData
                {
                    coordinates = new Coordinates { lat = any.coordinates.lat, lng = any.coordinates.lng },
                    geoName = any.geoName,
                    value = values,
                    formattedValue = values.Select(_ => _.ToString()).ToArray(),
                    maxValueIndex = 0
                };

                combinedMapData.Add(timeline);
            }

            return new ComparedGeoResult
            {
                Default = new GeoDefault
                {
                    geoMapData = combinedMapData.ToArray()
                }
            };
        }

        public static int[] CombineArrarys(IEnumerable<int[]> source)
        {
            var combined = source.FirstOrDefault();
            if (combined == null) return null;

            foreach (var a in source.Skip(1))
            {
                for (var i = 0; i < combined.Length; i++)
                {
                    combined[i] += a[i];
                }
            }

            var totalCount = source.Count();
            for (var i = 0; i < combined.Length; i++)
            {
                combined[i] = combined[i] / totalCount;
            }

            return combined;
        }
    }
}
