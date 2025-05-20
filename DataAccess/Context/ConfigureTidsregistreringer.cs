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
    internal class ConfigureTidsregistreringer : IEntityTypeConfiguration<Tidsregistrering>
    {
        public void Configure(EntityTypeBuilder<Tidsregistrering> builder)
        {
            //Data seeding
            builder.HasData(
                new Tidsregistrering { Nr = 1, MedarbejderCPR = "1414141414", SagNr = 1, StartTid = new DateTime(2025, 5, 10, 12, 00, 00), SlutTid = new DateTime(2025, 5, 10, 17, 00, 00) },
                new Tidsregistrering { Nr = 2, MedarbejderCPR = "1414141414", SagNr = 1, StartTid = new DateTime(2025, 5, 11, 12, 30, 00), SlutTid = new DateTime(2025, 5, 11, 17, 00, 00) },
                new Tidsregistrering { Nr = 3, MedarbejderCPR = "1212121212", SagNr = 1, StartTid = new DateTime(2025, 5, 10, 12, 00, 00), SlutTid = new DateTime(2025, 5, 10, 16, 30, 00) },
                new Tidsregistrering { Nr = 4, MedarbejderCPR = "1414141414", StartTid = new DateTime(2025, 5, 11, 12, 30, 00), SlutTid = new DateTime(2025, 5, 11, 17, 00, 00) }
            );
        }
    }
}
