using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    internal static class SagMappings
    {
        public static DTO.Model.Sag Map(this DataAccess.Model.Sag daSag)
        {
            return new DTO.Model.Sag(daSag.Nr, daSag.Overskrift, daSag.Beskrivelse, daSag.AfdelingNr);
        }

        public static DataAccess.Model.Sag Map(this DTO.Model.Sag dtoSag)
        {
            return new DataAccess.Model.Sag(dtoSag.Overskrift, dtoSag.Beskrivelse, dtoSag.AfdelingNr);
        }

        public static void UpdateFrom(this DataAccess.Model.Sag daSag, DTO.Model.Sag dtoSag)
        {
            daSag.Overskrift = dtoSag.Overskrift;
            daSag.Beskrivelse = dtoSag.Beskrivelse;
            daSag.AfdelingNr = dtoSag.AfdelingNr;
        }
    }
}
