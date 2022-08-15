using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }
        [Required]
        public int TruckId { get; set; }
        [ForeignKey(nameof(TruckId))]
        public Truck Truck { get; set; }
    }
}
