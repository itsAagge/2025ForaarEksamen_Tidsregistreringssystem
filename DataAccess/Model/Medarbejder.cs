using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model
{
    internal class Medarbejder
    {
        [Key]
        [MaxLength(10)]
        public string CPR { get; set; }

        [MaxLength(5)]
        public string Initialer { get; set; }

        [MaxLength(100)]
        public string Navn { get; set; }

        public virtual List<Afdeling> Afdelinger { get; }

        public Medarbejder()
        {
            Afdelinger = new List<Afdeling>();
        }

        public Medarbejder(string cpr, string initialer, string navn) : base()
        {
            CPR = cpr;
            Initialer = initialer;
            Navn = navn;
        }
    }
}
