using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Trucks.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportClient
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Trucks")]
        public ExportTruck[] Trucks { get; set; }
    }
}
