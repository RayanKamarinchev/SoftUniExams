using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            Casts = new List<Cast>();
            Tickets = new List<Ticket>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Title { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        [Range(0, 10)]
        public float Rating { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        [MaxLength(700)]
        public string Description { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Screenwriter  { get; set; }
        public ICollection<Cast> Casts { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
