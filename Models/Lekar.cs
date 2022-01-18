using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Lekar
    {
        [Key]
        public int ID { get; set; }
        
        [MaxLength(20)]
        [MinLength(3)]
        public string Ime { get; set; }
        
        [MaxLength(20)]
        [MinLength(3)]
        public string Prezime { get; set; }
        public List<Bolnica> Bolnice {get; set;}
    }
}
