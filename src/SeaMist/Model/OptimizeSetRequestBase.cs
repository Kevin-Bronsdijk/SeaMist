using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeSetRequestBase : OptimizeRequestBase
    {
        public void AddSet(SetResizeImage resizeImage)
        {
            if (resizeImage == null) throw new ArgumentException();

            if (Resize.Count == 10)
            {
                throw new Exception("Cannot exceed the quota of 10 instructions per request");
            }

            // Internal use
            Resize.Add(resizeImage.Name, resizeImage);

            if (ResizeImage == null) { ResizeImage = new List<SetResizeImage>(); }
            this.ResizeImage.Add(resizeImage);
        }

        internal Dictionary<string, ResizeImage> Resize { get; set; } = new Dictionary<string, ResizeImage>();

        [JsonProperty("resize")]
        public new List<SetResizeImage> ResizeImage { get; set; }
    }
}
