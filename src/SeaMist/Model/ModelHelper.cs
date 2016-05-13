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
    }
}
