using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GoolgeTrendsApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace GoolgeTrendsApi.Tests
{
    public class ModelSerializationTest
    {
        private readonly ITestOutputHelper _output;

        public ModelSerializationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DeserializeTrendsResult()
        {
            var path = GetJsonFile();
            Assert.True(File.Exists(path));

            var json = File.ReadAllText(path);
            var jToken = JToken.Parse(json).SelectToken("trends");

            var trendsJson = jToken.ToString();
            var obj = JsonConvert.DeserializeObject<TrendsResult>(trendsJson);
            Assert.NotNull(obj);

            _output.WriteLine(string.Join(",", obj.Default.averages));
        }

        [Fact]
        public void DeserializeComparedGeoResult()
        {
            var path = GetJsonFile();
            Assert.True(File.Exists(path));

            var json = File.ReadAllText(path);
            var jToken = JToken.Parse(json).SelectToken("geo");

            var geoJson = jToken.ToString();
            var obj = JsonConvert.DeserializeObject<ComparedGeoResult>(geoJson);
            Assert.NotNull(obj);

            _output.WriteLine(obj.Default.geoMapData.Length.ToString());
        }

        private string GetJsonFile()
        {
            var curDir = Directory.GetCurrentDirectory();

            for (var i = 0; i < 3; i++) curDir = Path.GetDirectoryName(curDir);

            var path = Path.Combine(curDir, "ResultSample.json");
            return path;
        }
    }
}
