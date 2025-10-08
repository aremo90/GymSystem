using GymSystemDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });


            builder.HasOne(s => s.SessionCategory)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId);

            builder.HasOne(s => s.SessionTrainer)
                .WithMany(X => X.TrainerSession)
                .HasForeignKey(X => X.TrainerId);



        }
    }
}
