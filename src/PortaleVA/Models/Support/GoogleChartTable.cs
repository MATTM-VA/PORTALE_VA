using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VAPortale.Models.Support
{
    public class GoogleChartTable
    {
        public GoogleChartTable()
        {
            Cols = new List<GoogleChartColumn>();
            Rows = new List<GoogleChartRow>();
        }

        [JsonProperty(PropertyName = "cols")]
        public List<GoogleChartColumn> Cols { get; private set; }

        [JsonProperty(PropertyName = "rows")]
        public List<GoogleChartRow> Rows { get; private set; }

    }
}