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
    internal class ConfigureMedarbejdere : IEntityTypeConfiguration<Medarbejder>
    {
        public void Configure(EntityTypeBuilder<Medarbejder> builder)
        {
            //Holder initalerne unikke
            builder.HasIndex(m => m.Initialer).IsUnique();

            //Data seeding
            builder.HasData(
                new Medarbejder { CPR = "1111111111", Initialer = "KP", Navn = "Karl Petersen" },
                new Medarbejder { CPR = "1212121212", Initialer = "OS", Navn = "Ole Svendsen" },
                new Medarbejder { CPR = "1313131313", Initialer = "JS", Navn = "Julie Hansen" },
                new Medarbejder { CPR = "1414141414", Initialer = "BN", Navn = "Britta Nielsen" },
                new Medarbejder { CPR = "1515151515", Initialer = "SB", Navn = "Stein Bagger" }
            );
        }
    }
}
