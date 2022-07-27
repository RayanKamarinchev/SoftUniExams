using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            Cells = new List<Cell>();
        }
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }
        public ICollection<Cell> Cells { get; set; }
    }
}
