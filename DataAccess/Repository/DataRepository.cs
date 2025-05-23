using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.Repository
{
    public static class DataRepository
    {
        //Afdeling
        public static List<DTO.Model.Afdeling> HentAlleAfdelinger()
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Afdeling> daAfdelinger = context.Afdelinger.ToList();
                List<DTO.Model.Afdeling> dtoAfdelinger = new List<DTO.Model.Afdeling>();
                foreach (DataAccess.Model.Afdeling daAfdeling in daAfdelinger)
                {
                    DTO.Model.Afdeling dtoAfdeling = daAfdeling.Map();
                    dtoAfdelinger.Add(dtoAfdeling);
                }
                return dtoAfdelinger;
            }
        }

        public static List<DTO.Model.Afdeling> HentAlleAfdelingerMedMedarbejdere()
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Afdeling> daAfdelinger = context.Afdelinger.Include(a => a.Medarbejdere).ToList();
                List<DTO.Model.Afdeling> dtoAfdelinger = new List<DTO.Model.Afdeling>();
                foreach (DataAccess.Model.Afdeling daAfdeling in daAfdelinger)
                {
                    DTO.Model.Afdeling dtoAfdeling = daAfdeling.Map();
                    dtoAfdeling.Medarbejdere = HentAlleMedarbejdereFraAfdeling_Internal(daAfdeling, context);
                    dtoAfdelinger.Add(dtoAfdeling);
                }
                return dtoAfdelinger;
            }
        }

        public static DTO.Model.Afdeling HentAfdelingMedMedarbejdere(int afdelingNr)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Afdeling? daAfdeling = context.Afdelinger.Include(a => a.Medarbejdere).Where(a => a.Nr == afdelingNr).FirstOrDefault();
                if (daAfdeling == default) throw new NullReferenceException("Der findes ikke en afdeling med nr: " + afdelingNr + " i databasen.");

                DTO.Model.Afdeling dtoAfdeling = daAfdeling.Map();
                dtoAfdeling.Medarbejdere = HentAlleMedarbejdereFraAfdeling_Internal(daAfdeling, context);
                return dtoAfdeling;
            }
        }

        public static List<DTO.Model.Afdeling> HentAfdelingerSomMedarbejderIkkeErI(string medarbejderCPR)
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Afdeling> daAfdelinger = context.Afdelinger.Where(a => a.Medarbejdere.All(m => m.CPR != medarbejderCPR)).ToList();
                List<DTO.Model.Afdeling> dtoAfdelinger = new List<DTO.Model.Afdeling>();
                foreach (DataAccess.Model.Afdeling daAfdeling in daAfdelinger)
                {
                    DTO.Model.Afdeling dtoAfdeling = daAfdeling.Map();
                    dtoAfdelinger.Add(dtoAfdeling);
                }
                return dtoAfdelinger;
            }
        }

        private static List<DTO.Model.Medarbejder> HentAlleMedarbejdereFraAfdeling_Internal(DataAccess.Model.Afdeling daAfdeling, DataContext context)
        {
            List<DTO.Model.Medarbejder> dtoMedarbejdere = new List<DTO.Model.Medarbejder>();
            foreach (DataAccess.Model.Medarbejder daMedarbejder in daAfdeling.Medarbejdere)
            {
                dtoMedarbejdere.Add(daMedarbejder.Map());
            }
            return dtoMedarbejdere;
        }

        public static void OpretAfdeling(DTO.Model.Afdeling dtoAfdeling)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Afdeling daAfdeling = dtoAfdeling.Map();
                context.Afdelinger.Add(daAfdeling);
                context.SaveChanges();
            }
        }

        public static void OpdaterAfdeling(DTO.Model.Afdeling dtoAfdeling)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Afdeling? daAfdeling = context.Afdelinger.Find(dtoAfdeling.Nr);
                if (daAfdeling == null) throw new NullReferenceException("Der findes ikke en afdeling med nr: " + dtoAfdeling.Nr + " i databasen.");
                daAfdeling.UpdateFrom(dtoAfdeling);
                context.SaveChanges();
            }
        }

        public static void SletAfdeling(DTO.Model.Afdeling dtoAfdeling)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Afdeling? daAfdeling = context.Afdelinger.Include(a => a.Medarbejdere).Where(a => a.Nr == dtoAfdeling.Nr).FirstOrDefault();
                if (daAfdeling == default) throw new NullReferenceException("Der findes ikke en afdeling med nr: " + dtoAfdeling.Nr + " i databasen.");
                if (!daAfdeling.Medarbejdere.IsNullOrEmpty()) throw new ArgumentException("Du kan ikke slette en afdeling, som har medarbejdere tilknyttet.");
                context.Afdelinger.Remove(daAfdeling); //Cascading deletes burde slette alle sager
                context.SaveChanges();
            }
        }

        public static void TilføjMedarbejderTilAfdeling(DTO.Model.Afdeling dtoAfdeling, DTO.Model.Medarbejder dtoMedarbejder)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Afdeling? daAfdeling = context.Afdelinger.Include(a => a.Medarbejdere).Where(a => a.Nr == dtoAfdeling.Nr).FirstOrDefault();
                if (daAfdeling == default) throw new NullReferenceException("Der findes ikke en afdeling med nr: " + dtoAfdeling.Nr + " i databasen.");
                DataAccess.Model.Medarbejder? daMedarbejder = context.Medarbejdere.Find(dtoMedarbejder.CPR);
                if (daMedarbejder == null) throw new NullReferenceException("Der findes ikke en medarbejder med cpr: " + dtoMedarbejder.CPR + " i databasen.");

                daAfdeling.Medarbejdere.Add(daMedarbejder);
                context.SaveChanges();
            }
        }

        public static void FjernMedarbejderFraAfdeling(DTO.Model.Afdeling dtoAfdeling, DTO.Model.Medarbejder dtoMedarbejder)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Afdeling? daAfdeling = context.Afdelinger.Find(dtoAfdeling.Nr);
                if (daAfdeling == null) throw new NullReferenceException("Der findes ikke en afdeling med nr: " + dtoAfdeling.Nr + " i database.n");
                DataAccess.Model.Medarbejder? daMedarbejder = context.Medarbejdere.Include(m => m.Afdelinger).Where(m => m.CPR == dtoMedarbejder.CPR).FirstOrDefault();
                if (daMedarbejder == default) throw new NullReferenceException("Der findes ikke en medarbejder med cpr: " + dtoMedarbejder.CPR + " i databasen.");
                if (daMedarbejder.Afdelinger.Count() <= 1) throw new ArgumentException("Medarbejderen kunne ikke fjernes. En medarbejder skal altid være tilknyttet mindst 1 afdeling.");

                daMedarbejder.Afdelinger.Remove(daAfdeling);
                context.SaveChanges();
            }
        }

        //Medarbejder
        public static List<DTO.Model.Medarbejder> HentAlleMedarbejdereMedAfdelinger()
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Medarbejder> daMedarbejdere = context.Medarbejdere.Include(m => m.Afdelinger).ToList();
                List<DTO.Model.Medarbejder> dtoMedarbejdere = new List<DTO.Model.Medarbejder>();
                foreach (DataAccess.Model.Medarbejder daMedarbejder in daMedarbejdere)
                {
                    DTO.Model.Medarbejder dtoMedarbejder = daMedarbejder.Map();
                    dtoMedarbejder.Afdelinger = HentAlleAfdelingerFraMedarbejder_Internal(daMedarbejder, context);
                    dtoMedarbejdere.Add(dtoMedarbejder);
                }
                return dtoMedarbejdere;
            }
        }

        public static DTO.Model.Medarbejder HentMedarbejderMedAfdelinger(string medarbejderCPR)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Medarbejder? daMedarbejder = context.Medarbejdere.Include(m => m.Afdelinger).Where(m => m.CPR.Equals(medarbejderCPR)).FirstOrDefault();
                if (daMedarbejder == default) throw new NullReferenceException("Der findes ikke en medarbejder med cpr: " + medarbejderCPR + " i databasen.");

                DTO.Model.Medarbejder dtoMedarbejder = daMedarbejder.Map();
                dtoMedarbejder.Afdelinger = HentAlleAfdelingerFraMedarbejder_Internal(daMedarbejder, context);
                return dtoMedarbejder;
            }
        }

        private static List<DTO.Model.Afdeling> HentAlleAfdelingerFraMedarbejder_Internal(DataAccess.Model.Medarbejder daMedarbejder, DataContext context)
        {
            List<DTO.Model.Afdeling> dtoAfdelinger = new List<DTO.Model.Afdeling>();
            foreach (DataAccess.Model.Afdeling daAfdeling in daMedarbejder.Afdelinger)
            {
                dtoAfdelinger.Add(daAfdeling.Map());
            }
            return dtoAfdelinger;
        }

        public static void OpretMedarbejder(DTO.Model.Medarbejder dtoMedarbejder)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Medarbejder cprMedarbejder = context.Medarbejdere.Where(m => m.CPR == dtoMedarbejder.CPR).FirstOrDefault();
                if (cprMedarbejder != default) throw new ArgumentException("Der findes allerede en medarbejder i systemet med det cpr-nummer.");
                DataAccess.Model.Medarbejder initialMedarbejder = context.Medarbejdere.Where(m => m.Initialer == dtoMedarbejder.Initialer).FirstOrDefault();
                if (initialMedarbejder != default) throw new ArgumentException("Der findes allerede en medarbejder i systemet med disse initialer.");
                DataAccess.Model.Medarbejder daMedarbejder = dtoMedarbejder.Map();
                context.Medarbejdere.Add(daMedarbejder);
                context.SaveChanges();
            }
        }

        public static void OpdaterMedarbejder(DTO.Model.Medarbejder dtoMedarbejder)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Medarbejder? daMedarbejder = context.Medarbejdere.Find(dtoMedarbejder.CPR);
                if (daMedarbejder == null) throw new NullReferenceException("Der findes ikke en medarbejder med cpr: " + dtoMedarbejder.CPR + " i databasen.");
                daMedarbejder.UpdateFrom(dtoMedarbejder);
                context.SaveChanges();
            }
        }

        public static void SletMedarbejder(DTO.Model.Medarbejder dtoMedarbejder)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Medarbejder? daMedarbejder = context.Medarbejdere.Include(m => m.Afdelinger).Where(m => m.CPR == dtoMedarbejder.CPR).FirstOrDefault();
                if (daMedarbejder == default) throw new NullReferenceException("Der findes ikke en medarbejder med cpr: " + dtoMedarbejder.CPR + " i databasen.");
                context.Medarbejdere.Remove(daMedarbejder); //Cascading deletes burde automatisk slette alle tidsregistreringer samt connections til afdelinger
                context.SaveChanges();
            }
        }

        //Sag
        public static List<DTO.Model.Sag> HentAlleSager()
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Sag> daSager = context.Sager.ToList();
                List<DTO.Model.Sag> dtoSager = new List<DTO.Model.Sag>();
                foreach (DataAccess.Model.Sag daSag in daSager)
                {
                    dtoSager.Add(daSag.Map());
                }
                return dtoSager;
            }
        }

        public static List<DTO.Model.Sag> HentAlleSagerFraAfdelingNr(int afdelingNr)
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Sag> daSager = context.Sager.Where(s => s.AfdelingNr == afdelingNr).ToList();
                List<DTO.Model.Sag> dtoSager = new List<DTO.Model.Sag>();
                foreach (DataAccess.Model.Sag daSag in daSager)
                {
                    dtoSager.Add(daSag.Map());
                }
                return dtoSager;
            }
        }

        public static void OpretSag(DTO.Model.Sag dtoSag)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Sag daSag = dtoSag.Map();
                context.Sager.Add(daSag);
                context.SaveChanges();
            }
        }

        public static void OpdaterSag(DTO.Model.Sag dtoSag)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Sag? daSag = context.Sager.Find(dtoSag.Nr);
                if (daSag == null) throw new NullReferenceException("Der findes ikke en sag med nr: " + dtoSag.Nr + " i databasen.");
                daSag.UpdateFrom(dtoSag);
                context.SaveChanges();
            }
        }

        public static void SletSag(DTO.Model.Sag dtoSag)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Sag? daSag = context.Sager.Find(dtoSag.Nr);
                if (daSag == null) throw new NullReferenceException("Der findes ikke en sag med nr: " + dtoSag.Nr + " i databasen.");
                context.Sager.Remove(daSag);
                context.SaveChanges();
            }
        }

        //Tidsregistrering
        public static List<DTO.Model.Tidsregistrering> HentAlleTidsregistreringerFraMedarbejderCPR(string medarbejderCPR)
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Tidsregistrering> daTidsregistreringer = context.Tidsregistreringer.Where(t => t.MedarbejderCPR == medarbejderCPR).ToList();
                List<DTO.Model.Tidsregistrering> dtoTidsregistreringer = new List<DTO.Model.Tidsregistrering>();
                foreach (DataAccess.Model.Tidsregistrering daTidsregistrering in daTidsregistreringer)
                {
                    dtoTidsregistreringer.Add(daTidsregistrering.Map());
                }
                return dtoTidsregistreringer;
            }
        }

        public static List<DTO.Model.Tidsregistrering> HentAlleTidsregistreringerFraSagNr(int sagNr)
        {
            using (DataContext context = new DataContext())
            {
                List<DataAccess.Model.Tidsregistrering> daTidsregistreringer = context.Tidsregistreringer.Where(t => t.SagNr == sagNr).ToList();
                List<DTO.Model.Tidsregistrering> dtoTidsregistreringer = new List<DTO.Model.Tidsregistrering>();
                foreach (DataAccess.Model.Tidsregistrering daTidsregistrering in daTidsregistreringer)
                {
                    dtoTidsregistreringer.Add(daTidsregistrering.Map());
                }
                return dtoTidsregistreringer;
            }
        }

        public static void OpretTidsregistrering(DTO.Model.Tidsregistrering dtoTidsregistrering)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Tidsregistrering daTidsregistrering = dtoTidsregistrering.Map();
                context.Tidsregistreringer.Add(daTidsregistrering);
                context.SaveChanges();
            }
        }

        public static void OpdaterTidsregistrering(DTO.Model.Tidsregistrering dtoTidsregistrering)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Tidsregistrering? daTidsregistrering = context.Tidsregistreringer.Find(dtoTidsregistrering.Nr);
                if (daTidsregistrering == null) throw new NullReferenceException("Der findes ikke en tidsregistrering med nr: " + dtoTidsregistrering.Nr + " i databasen.");
                daTidsregistrering.UpdateFrom(dtoTidsregistrering);
                context.SaveChanges();
            }
        }

        public static void SletTidsregistrering(DTO.Model.Tidsregistrering dtoTidsregistrering)
        {
            using (DataContext context = new DataContext())
            {
                DataAccess.Model.Tidsregistrering? daTidsregistrering = context.Tidsregistreringer.Find(dtoTidsregistrering.Nr);
                if (daTidsregistrering == null) throw new NullReferenceException("Der findes ikke en tidsregistrering med nr: " + dtoTidsregistrering.Nr + " i databasen.");
                context.Tidsregistreringer.Remove(daTidsregistrering);
                context.SaveChanges();
            }
        }
    }
}