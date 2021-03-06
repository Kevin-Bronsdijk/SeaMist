﻿using Newtonsoft.Json;
using System;

namespace SeaMist.Model
{
    public class OptimizeSetUploadRequest : OptimizeSetRequestBase, IOptimizeSetUploadRequest
    {
        public OptimizeSetUploadRequest(Uri callbackUrl)
        {
            CallbackUrl = callbackUrl;
        }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }
    }
}
