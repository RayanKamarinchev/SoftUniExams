using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Trucks.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportClientDto
    {
        [Required]
        [JsonProperty("Name")]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 2)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; }
        [Required]
        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("Trucks")]
        public int[] Trucks { get; set; }
    }
}
