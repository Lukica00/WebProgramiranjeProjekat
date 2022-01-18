using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Pacijent
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(20)]
        [MinLength(3)]
        public string Ime { get; set; }

        [MaxLength(20)]
        [MinLength(3)]       
         public string Prezime { get; set; }
        
        [MaxLength(13)]
        [MinLength(13)]
        [RegularExpression("^[0-9]+$")]
        public string JMBG { get; set; }
    }
}
