using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Model
{
    public class Medarbejder
    {
        public string CPR { get; set; }

        public string Initialer { get; set; }

        public string Navn { get; set; }

        public List<Afdeling> Afdelinger { get; set; }


        //To and from DB (since cpr isn't a self-incrementing int)
        public Medarbejder(string cpr, string initialer, string navn)
        {
            CPR = cpr;
            Initialer = initialer;
            Navn = navn;
            Afdelinger = new List<Afdeling>();
        }
    }
}
