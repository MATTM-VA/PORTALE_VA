using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VAPortale.Models.Support
{
    public class GoogleChartRow
    {
        public GoogleChartRow()
        {
            Cells = new List<GoogleChartCell>();
        }

        [JsonProperty(PropertyName = "c")]
        public List<GoogleChartCell> Cells { get; private set; }
    }
}