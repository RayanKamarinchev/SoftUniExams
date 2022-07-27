using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            Mails = new List<Mail>();
            PrisonerOfficers = new List<OfficerPrisoner>();
        }
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string FullName { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        [Range(18, 65)]
        public int Age { get; set; }
        [Required]
        public DateTime IncarcerationDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        [Range(0, int.MaxValue)]
        public decimal? Bail { get; set; }
        public int? CellId { get; set; }
        [ForeignKey("CellId")]
        public Cell Cell { get; set; }
        public ICollection<Mail> Mails { get; set; }
        public ICollection<OfficerPrisoner> PrisonerOfficers  { get; set; }
    }
}