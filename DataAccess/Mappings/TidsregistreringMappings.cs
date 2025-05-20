using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    internal static class TidsregistreringMappings
    {
        public static DTO.Model.Tidsregistrering Map(this DataAccess.Model.Tidsregistrering daTidsregistrering)
        {
            return new DTO.Model.Tidsregistrering(daTidsregistrering.Nr, daTidsregistrering.StartTid, daTidsregistrering.SlutTid, daTidsregistrering.MedarbejderCPR, daTidsregistrering.SagNr);
        }

        public static DataAccess.Model.Tidsregistrering Map(this DTO.Model.Tidsregistrering dtoTidsregistrering)
        {
            return new DataAccess.Model.Tidsregistrering(dtoTidsregistrering.StartTid, dtoTidsregistrering.SlutTid, dtoTidsregistrering.MedarbejderCPR, dtoTidsregistrering.SagNr);
        }

        public static void UpdateFrom(this DataAccess.Model.Tidsregistrering daTidsregistrering, DTO.Model.Tidsregistrering dtoTidsregistrering)
        {
            daTidsregistrering.StartTid = dtoTidsregistrering.StartTid;
            daTidsregistrering.SlutTid = dtoTidsregistrering.SlutTid;
            daTidsregistrering.MedarbejderCPR = dtoTidsregistrering.MedarbejderCPR;
            daTidsregistrering.SagNr = dtoTidsregistrering.SagNr;
        }
    }
}
