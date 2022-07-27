﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using SoftJail.Data.Models.Enums;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [Required]
        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [Required]
        [XmlArray("Prisoners")]
        public ImportOfficerPrisonerDto[] Prisoners { get; set; }
    }
}