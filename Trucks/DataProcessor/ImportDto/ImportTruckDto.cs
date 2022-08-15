using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression("^[A-Z]{2}[0-9]{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; }
        [Required]
        [StringLength(17, MinimumLength = 17)]
        public string VinNumber { get; set; }
        [Range(950, 1420)]
        public int TankCapacity { get; set; }
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }
        [Required]
        public int CategoryType { get; set; }
        [Required]
        public int MakeType { get; set; }
    }
}
