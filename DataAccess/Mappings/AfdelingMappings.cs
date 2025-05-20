using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    internal static class AfdelingMappings
    {
        public static DTO.Model.Afdeling Map(this DataAccess.Model.Afdeling daAfdeling)
        {
            return new DTO.Model.Afdeling(daAfdeling.Nr, daAfdeling.Navn);
        }

        public static DataAccess.Model.Afdeling Map(this DTO.Model.Afdeling dtoAfdeling)
        {
            return new DataAccess.Model.Afdeling(dtoAfdeling.Navn);
        }

        public static void UpdateFrom(this DataAccess.Model.Afdeling daAfdeling, DTO.Model.Afdeling dtoAfdeling)
        {
            daAfdeling.Navn = dtoAfdeling.Navn;
        }
    }
}
