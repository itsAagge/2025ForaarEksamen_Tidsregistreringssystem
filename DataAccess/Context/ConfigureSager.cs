using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context
{
    internal class ConfigureSager : IEntityTypeConfiguration<Sag>
    {
        public void Configure(EntityTypeBuilder<Sag> builder)
        {
            //Data seeding
            builder.HasData(
                new Sag { Nr = 1, AfdelingNr = 2, Overskrift = "Årsrapport 2025", Beskrivelse = "Vi skal have lavet en årsrapport for 2025" },
                new Sag { Nr = 2, AfdelingNr = 3, Overskrift = "Forbered demo til messe", Beskrivelse = "Vi skal have forberedt den nye demo til messen" },
                new Sag { Nr = 3, AfdelingNr = 4, Overskrift = "Reklamekampagner 2024", Beskrivelse = "Oversig over vores reklamekampagner for 2024" },
                new Sag { Nr = 4, AfdelingNr = 4, Overskrift = "Reklamekampagner 2025", Beskrivelse = "Oversig over vores reklamekampagner for 2025" }
            );
        }
    }
}
