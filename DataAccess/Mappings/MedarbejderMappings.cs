using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    internal static class MedarbejderMappings
    {
        public static DTO.Model.Medarbejder Map(this DataAccess.Model.Medarbejder daMedarbejder)
        {
            return new DTO.Model.Medarbejder(daMedarbejder.CPR, daMedarbejder.Initialer, daMedarbejder.Navn);
        }

        public static DataAccess.Model.Medarbejder Map(this DTO.Model.Medarbejder dtoMedarbejder)
        {
            return new DataAccess.Model.Medarbejder(dtoMedarbejder.CPR, dtoMedarbejder.Initialer, dtoMedarbejder.Navn);
        }

        public static void UpdateFrom(this DataAccess.Model.Medarbejder daMedarbejder, DTO.Model.Medarbejder dtoMedarbejder)
        {
            daMedarbejder.Initialer = dtoMedarbejder.Initialer;
            daMedarbejder.Navn = dtoMedarbejder.Navn;
        }
    }
}
