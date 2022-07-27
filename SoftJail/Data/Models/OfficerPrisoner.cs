using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        [Required]
        public int PrisonerId { get; set; }
        [ForeignKey(nameof(PrisonerId))]
        public Prisoner Prisoner { get; set; }
        [Required]
        public int OfficerId { get; set; }
        [ForeignKey(nameof(OfficerId))]
        public Officer Officer { get; set; }
    }
}
