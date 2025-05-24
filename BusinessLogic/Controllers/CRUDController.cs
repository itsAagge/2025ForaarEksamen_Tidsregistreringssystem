using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccess.Repository;
using DTO.Model;

namespace BusinessLogic.Controllers
{
    public static class CRUDController
    {
        //Afdeling
        public static List<Afdeling> HentAlleAfdelinger()
        {
            return DataRepository.HentAlleAfdelinger();
        }

        public static List<Afdeling> HentAlleAfdelingerMedMedarbejdere()
        {
            return DataRepository.HentAlleAfdelingerMedMedarbejdere();
        }

        public static Afdeling HentAfdelingMedMedarbejdere(int afdelingNr)
        {
            return DataRepository.HentAfdelingMedMedarbejdere(afdelingNr);
        }

        public static List<Afdeling> HentAfdelingerSomMedarbejderIkkeErI(string cpr)
        {
            return DataRepository.HentAfdelingerSomMedarbejderIkkeErI(cpr);
        }

        public static void OpretAfdeling(string navn)
        {
            if (navn.Length <= 0 || navn.Length > 50) throw new ArgumentException("Afdelingens navn skal være på mellem 1 og 50 tegn.");

            Afdeling afdeling = new Afdeling(navn);
            DataRepository.OpretAfdeling(afdeling);
        }

        public static void OpdaterAfdeling(Afdeling afdeling, string nytNavn)
        {
            if (nytNavn.Length <= 0 || nytNavn.Length > 50) throw new ArgumentException("Afdelingens navn skal være på mellem 1 og 50 tegn.");

            afdeling.Navn = nytNavn;
            DataRepository.OpdaterAfdeling(afdeling);
        }

        public static void SletAfdeling(Afdeling afdeling)
        {
            DataRepository.SletAfdeling(afdeling);
        }

        public static void TilføjMedarbejderTilAfdeling(Afdeling afdeling, Medarbejder medarbejder)
        {
            DataRepository.TilføjMedarbejderTilAfdeling(afdeling, medarbejder);
        }

        public static void FjernMedarbejderFraAfdeling(Afdeling afdeling, Medarbejder medarbejder)
        {
            DataRepository.FjernMedarbejderFraAfdeling(afdeling, medarbejder);
        }

        //Medarbejder
        public static List<Medarbejder> HentAlleMedarbejdereMedAfdelinger()
        {
            return DataRepository.HentAlleMedarbejdereMedAfdelinger();
        }

        public static Medarbejder HentMedarbejderMedAfdelinger(string cpr)
        {
            if (cpr.Length != 10) throw new ArgumentException("Et cpr-nummer må kun bestå af 10 tal.");
            if (!Regex.IsMatch(cpr, "\\d{10}")) throw new ArgumentException("Et cpr-nummer må kun bestå af 10 tal.");

            return DataRepository.HentMedarbejderMedAfdelinger(cpr);
        }

        public static void OpretMedarbejder(Afdeling afdeling, string cpr, string initialer, string navn)
        {
            if (cpr.Length != 10) throw new ArgumentException("Et cpr-nummer må kun bestå af 10 tal.");
            if (!Regex.IsMatch(cpr, "\\d{10}")) throw new ArgumentException("Et cpr-nummer må kun bestå af 10 tal.");
            if (!Modulus11Check(cpr)) throw new ArgumentException("Det indtastede cpr-nummer er ikke gyldigt.");

            if (initialer.Length <= 0 || initialer.Length > 5) throw new ArgumentException("Initialerne skal være mellem 1 og 5 bogstaver lange.");
            if (navn.Length <= 0 || navn.Length > 100) throw new ArgumentException("Systemet kan kun håndtere navne på mellem 1 og 100 tegn.");

            Medarbejder medarbejder = new Medarbejder(cpr, initialer, navn);
            DataRepository.OpretMedarbejder(medarbejder);
        }

        private static bool Modulus11Check(string cpr)
        {
            if (cpr.StartsWith("0101")) return true; //Modulus11 virker ikke længere på 1. januar, da for mange cpr-numre er givet ud med dette

            int[] weightArr = { 4, 3, 2, 7, 6, 5, 4, 3, 2, 1 };
            char[] cprArr = cpr.ToCharArray();
            int checkSum = 0;

            for (int i = 0; i < cprArr.Length; i++)
            {
                int cprInt = Convert.ToInt32(cprArr[i]);
                int weightInt = weightArr[i];
                checkSum += (cprInt * weightInt);
            }

            return checkSum % 11 == 0;
        }

        public static void OpdaterMedarbejder(Medarbejder medarbejder, string nyeInitialer, string nytNavn)
        {
            if (nyeInitialer.Length <= 0 || nyeInitialer.Length > 5) throw new ArgumentException("Initialerne skal være mellem 1 og 5 bogstaver lange.");
            if (nytNavn.Length <= 0 || nytNavn.Length > 100) throw new ArgumentException("Systemet kan kun håndtere navne på mellem 1 og 100 tegn.");

            medarbejder.Initialer = nyeInitialer;
            medarbejder.Navn = nytNavn;
            DataRepository.OpdaterMedarbejder(medarbejder);
        }

        public static void SletMedarbejder(Medarbejder medarbejder)
        {
            DataRepository.SletMedarbejder(medarbejder);
        }

        //Sag

        public static Sag HentSag(int sagNr)
        {
            return DataRepository.HentSag(sagNr);
        }
        public static List<Sag> HentAlleSager()
        {
            return DataRepository.HentAlleSager();
        }

        public static List<Sag> HentAlleSagerFraAfdelingNr(int afdelingNr)
        {
            return DataRepository.HentAlleSagerFraAfdelingNr(afdelingNr);
        }

        public static void OpretSag(string overskrift, string beskrivelse, Afdeling afdeling)
        {
            if (overskrift.Length <= 0 || overskrift.Length > 100) throw new ArgumentException("Systemet kan kun håndtere overskrifter på mellem 1 og 100 tegn.");
            if (beskrivelse.Length <= 0 || beskrivelse.Length > 1000) throw new ArgumentException("Systemet kan kun håndtere beskrivelser på mellem 1 og 1000 tegn.");

            Sag sag = new Sag(overskrift, beskrivelse, afdeling.Nr);
            DataRepository.OpretSag(sag);
        }

        public static void OpdaterSag(Sag sag, string nyOverskrift, string nyBeskrivelse, Afdeling afdeling)
        {
            if (nyOverskrift.Length <= 0 || nyOverskrift.Length > 100) throw new ArgumentException("Systemet kan kun håndtere overskrifter på mellem 1 og 100 tegn.");
            if (nyBeskrivelse.Length <= 0 || nyBeskrivelse.Length > 1000) throw new ArgumentException("Systemet kan kun håndtere beskrivelser på mellem 1 og 1000 tegn.");

            sag.Overskrift = nyOverskrift;
            sag.Beskrivelse = nyBeskrivelse;
            sag.AfdelingNr = afdeling.Nr;
            DataRepository.OpdaterSag(sag);
        }

        public static void SletSag(Sag sag)
        {
            DataRepository.SletSag(sag);
        }

        //Tidsregistrering
        public static List<Tidsregistrering> HentAlleTidsregistreringerFraMedarbejderCPR(string cpr)
        {
            return DataRepository.HentAlleTidsregistreringerFraMedarbejderCPR(cpr);
        }

        public static List<Tidsregistrering> HentAlleTIdsregistreringerFraSagNr(int sagNr)
        {
            return DataRepository.HentAlleTidsregistreringerFraSagNr(sagNr);
        }

        public static void OpretTidsregistrering(DateTime startTid, DateTime slutTid, string cpr, int? sagNr)
        {
            if (startTid >= slutTid) throw new ArgumentException("Tidsregistreringen er invalid. Tidsregistreringen skal slutte efter den starter - ikke omvendt.");
            if (cpr.Length != 10) throw new ArgumentException("Et cpr-nummer må kun bestå af 10 tal.");
            if (!Regex.IsMatch(cpr, "\\d{10}")) throw new ArgumentException("Et cpr-nummer må kun bestå af 10 tal.");

            Tidsregistrering tidsregistrering = new Tidsregistrering(startTid, slutTid, cpr, sagNr);
            DataRepository.OpretTidsregistrering(tidsregistrering);
        }

        public static void OpdaterTidsregistrering(Tidsregistrering tidsregistrering, DateTime nyStartTid, DateTime nySlutTid, int? nySagNr)
        {
            if (nyStartTid >= nySlutTid) throw new ArgumentException("Tidsregistreringen er invalid. Tidsregistreringen skal slutte efter den starter - ikke omvendt.");

            tidsregistrering.StartTid = nyStartTid;
            tidsregistrering.SlutTid = nySlutTid;
            tidsregistrering.SagNr = nySagNr;
            DataRepository.OpdaterTidsregistrering(tidsregistrering);
        }

        public static void SletTidsregistrering(Tidsregistrering tidsregistrering)
        {
            DataRepository.SletTidsregistrering(tidsregistrering);
        }
    }
}