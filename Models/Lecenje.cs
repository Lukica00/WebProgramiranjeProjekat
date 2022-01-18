using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Lecenje
    {
        [Key]
        public int ID { get; set; }
        public DateTime Pocetak {get; set;}
        public DateTime Kraj {get; set;}
        public int SobaID {get; set;}
        public Bolnica Bolnica { get; set; }
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
    }
}
