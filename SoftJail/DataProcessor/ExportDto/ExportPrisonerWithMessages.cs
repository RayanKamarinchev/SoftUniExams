﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerWithMessages
    {
        [XmlElement("Id")]
        public int Id { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }
        [XmlArray("EncryptedMessages")]
        public ExportMessage[] EncryptedMessages { get; set; }
    }
}
