﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDto
    {
        [Required]
        [Range(1, 1000)]
        public int CellNumber { get; set; }
        [Required]
        public bool HasWindow { get; set; }
    }
}
