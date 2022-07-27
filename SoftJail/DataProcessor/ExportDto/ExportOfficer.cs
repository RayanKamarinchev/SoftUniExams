using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ExportDto
{
    public class ExportOfficer
    {
        [JsonProperty("OfficerName")]
        public string Name { get; set; }

        public string Department { get; set; }
    }
}
