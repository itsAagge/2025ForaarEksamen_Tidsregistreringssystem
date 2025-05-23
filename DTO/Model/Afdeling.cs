using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Model
{
    public class Afdeling
    {
        public int Nr { get; set; }

        public string Navn { get; set; }

        public virtual List<Medarbejder> Medarbejdere { get; set; }

        private Afdeling()
        {
            Medarbejdere = new List<Medarbejder>();
        }

        //From DB
        public Afdeling(int nr, string navn) : base()
        {
            Nr = nr;
            Navn = navn;
        }

        //From Controller (to DB)
        public Afdeling(string navn) : base()
        {
            Navn = navn;
        }

        public override string ToString()
        {
            return Navn;
        }
    }
}
