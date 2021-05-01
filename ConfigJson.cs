using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Doraemon
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string token { get; private set; }
        [JsonProperty("prefix")]
        public string prefix { get; private set; }
    }
}
