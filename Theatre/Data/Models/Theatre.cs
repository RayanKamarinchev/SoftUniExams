using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        public Theatre()
        {
            Tickets = new List<Ticket>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Director { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
