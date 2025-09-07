using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rommie.Domain.Entities;

namespace Rommie.Persistence.EntityConfigs
{
    public class UserVerificationRequestConfig : IEntityTypeConfiguration<UserVerificationRequest>
    {
        public void Configure(EntityTypeBuilder<UserVerificationRequest> builder)
        {
            builder.HasKey(uvr => uvr.Id);
            builder.Property(uvr => uvr.Id).ValueGeneratedOnAdd();
            builder.Property(uvr => uvr.UserID).IsRequired();
            builder.Property(uvr => uvr.CreatedAtUtc).IsRequired();
            builder.Property(uvr => uvr.Status).IsRequired();
            builder.HasOne(uvr => uvr.User)
                   .WithMany(u => u.VerificationRequests)
                   .HasForeignKey(uvr => uvr.UserID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}