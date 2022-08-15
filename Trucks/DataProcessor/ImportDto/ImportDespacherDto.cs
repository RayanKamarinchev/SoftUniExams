using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespacherDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Position")]
        [Required]
        public string Position { get; set; }
        [XmlArray("Trucks")]
        public ImportTruckDto[] Trucks { get; set; }
    }
}
