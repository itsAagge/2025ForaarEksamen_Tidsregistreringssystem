using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    internal class Afdeling
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nr { get; set; }

        [MaxLength(50)]
        public string Navn { get; set; }

        public virtual List<Medarbejder> Medarbejdere { get; }

        public Afdeling()
        {
            Medarbejdere = new List<Medarbejder>();
        }

        public Afdeling(string navn) : base()
        {
            Navn = navn;
        }
    }
}
