using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatcher
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }
        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; }
        [XmlArray("Trucks")]
        public ExportDespatcherTruck[] Trucks { get; set; }
    }
}
