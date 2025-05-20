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
    internal class ConfigureAfdelinger : IEntityTypeConfiguration<Afdeling>
    {
        public void Configure(EntityTypeBuilder<Afdeling> builder)
        {
            //Data seeding
            builder.HasData(
                new Afdeling { Nr = 1, Navn = "Ledelse" },
                new Afdeling { Nr = 2, Navn = "Økonomi" },
                new Afdeling { Nr = 3, Navn = "Salg" },
                new Afdeling { Nr = 4, Navn = "Marketing" },
                new Afdeling { Nr = 5, Navn = "HR" }
            );
        }
    }
}
