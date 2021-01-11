using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VAPortale.Models.Support
{
    public class GoogleChartCell
    {
        [JsonProperty(PropertyName="v")]
        public object Value { get; set; }

        [JsonProperty("f")]
        public string Formatted { get; set; }
    }
}