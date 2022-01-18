using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Bolnica
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(30)]
        public string Ime { get; set; }
        
        [Range(6,20)]
        public int BrMesta { get; set; }
        public List<Lekar> Lekari { get; set; }
    }
}
