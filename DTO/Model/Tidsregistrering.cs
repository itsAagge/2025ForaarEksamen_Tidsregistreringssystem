using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Model
{
    public class Tidsregistrering
    {
        public int Nr { get; set; }

        public DateTime StartTid { get; set; }

        public DateTime SlutTid { get; set; }

        public string MedarbejderCPR { get; set; }

        public int? SagNr { get; set; }

        //From DB
        public Tidsregistrering(int nr, DateTime startTid, DateTime slutTid, string medarbejderCPR, int? sagNr)
        {
            Nr = nr;
            StartTid = startTid;
            SlutTid = slutTid;
            MedarbejderCPR = medarbejderCPR;
            SagNr = sagNr;
        }

        //From Controller (To DB)
        public Tidsregistrering(DateTime startTid, DateTime slutTid, string medarbejderCPR, int? sagNr)
        {
            StartTid = startTid;
            SlutTid = slutTid;
            MedarbejderCPR = medarbejderCPR;
            SagNr = sagNr;
        }

        public override string ToString()
        {
            return "Tidsregistrering " + Nr + ". " + StartTid.ToString() + ". Medarbejder: " + MedarbejderCPR;
        }
    }
}
