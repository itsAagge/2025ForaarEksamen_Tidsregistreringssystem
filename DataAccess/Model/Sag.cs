using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    internal class Sag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nr { get; set; }

        [MaxLength(100)]
        public string Overskrift { get; set; }

        [MaxLength(1000)]
        public string Beskrivelse { get; set; }

        [ForeignKey("Afdelinger")]
        public int AfdelingNr { get; set; }

        public Sag() { }

        public Sag(string overskrift, string beskrivelse, int afdelingsNr)
        {
            Overskrift = overskrift;
            Beskrivelse = beskrivelse;
            AfdelingNr = afdelingsNr;
        }
    }
}
