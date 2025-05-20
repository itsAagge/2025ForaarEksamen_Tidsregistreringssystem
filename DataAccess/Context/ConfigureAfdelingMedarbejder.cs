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
    internal class ConfigureAfdelingMedarbejder : IEntityTypeConfiguration<Afdeling>
    {
        public void Configure(EntityTypeBuilder<Afdeling> builder)
        {
            //Opretter associationstabellen og seeder data
            builder.HasMany(a => a.Medarbejdere)
                   .WithMany(m => m.Afdelinger)
                   .UsingEntity(am => am.HasData(
                       new { AfdelingerNr = 1, MedarbejdereCPR = "1515151515" },
                       new { AfdelingerNr = 2, MedarbejdereCPR = "1414141414" },
                       new { AfdelingerNr = 2, MedarbejdereCPR = "1212121212" },
                       new { AfdelingerNr = 3, MedarbejdereCPR = "1111111111" },
                       new { AfdelingerNr = 4, MedarbejdereCPR = "1313131313" },
                       new { AfdelingerNr = 1, MedarbejdereCPR = "1414141414" }
                       ));
        }
    }
}
