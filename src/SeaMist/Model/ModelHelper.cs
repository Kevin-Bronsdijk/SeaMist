using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SeaMist.Model
{
    internal class ModelHelper
    {
        public static string GetSamplingScheme(SamplingScheme samplingScheme)
        {
            var samplingSchemes = new Dictionary<string, string>
            {
                {"Default", "4:2:0"},
                {"S422", "4:2:2"},
                {"S444", "4:2:4"},
            };

            string chroma;
            samplingSchemes.TryGetValue(samplingScheme.ToString(), out chroma);

            return chroma;
        }

        public static OptimizeSetWaitResults JsonToSet(string json)
        {
            JObject jsono = JObject.Parse(json);

            var optimizeSetWaitResults = new OptimizeSetWaitResults();
            optimizeSetWaitResults.Success = true;

            foreach (var result in jsono.Children().Children().Children())
            {
                if (result.Path.StartsWith("results."))
                {
                    foreach (var resultsItem in result.Children())
                    {
                        var optimizeSetWaitResult = JsonConvert.DeserializeObject<OptimizeSetWaitResult>(resultsItem.ToString());
                        optimizeSetWaitResult.Name = result.Path.Replace("results.", string.Empty);
                        optimizeSetWaitResult.Success = true;
                        optimizeSetWaitResults.Results.Add(optimizeSetWaitResult);
                    }
                }
            }

            return optimizeSetWaitResults;
        }
    }
}
