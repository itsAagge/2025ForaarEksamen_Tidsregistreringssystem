using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    internal class Tidsregistrering
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nr { get; set; }

        public DateTime StartTid { get; set; }

        public DateTime SlutTid { get; set; }

        [ForeignKey("Medarbejdere")]
        public string MedarbejderCPR { get; set; }

        [ForeignKey("Sager")]
        public int? SagNr { get; set; }

        public Tidsregistrering() { }

        public Tidsregistrering(DateTime startTid, DateTime slutTid, string medarbejderCPR, int? sagNr)
        {
            StartTid = startTid;
            SlutTid = slutTid;
            MedarbejderCPR = medarbejderCPR;
            SagNr = sagNr;
        }
    }
}
