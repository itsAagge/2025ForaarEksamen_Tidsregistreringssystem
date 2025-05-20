using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Model
{
    public class Sag
    {
        public int Nr { get; set; }

        public string Overskrift { get; set; }

        public string Beskrivelse { get; set; }

        public int AfdelingNr { get; set; }

        //From DB
        public Sag(int nr, string overskrift, string beskrivelse, int afdelingNr)
        {
            Nr = nr;
            Overskrift = overskrift;
            Beskrivelse = beskrivelse;
            AfdelingNr = afdelingNr;
        }

        //From Controller (To DB)
        public Sag(string overskrift, string beskrivelse, int afdelingNr)
        {
            Overskrift = overskrift;
            Beskrivelse = beskrivelse;
            AfdelingNr= afdelingNr;
        }
    }
}
