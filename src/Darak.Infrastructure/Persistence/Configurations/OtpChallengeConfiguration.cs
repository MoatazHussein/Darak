using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Darak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darak.Infrastructure.Persistence.Configurations;
public class OtpChallengeConfiguration : IEntityTypeConfiguration<OtpChallenge>
{
    public void Configure(EntityTypeBuilder<OtpChallenge> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.PhoneNumber).HasMaxLength(32).IsRequired();
        b.Property(x => x.Purpose).HasMaxLength(32).IsRequired();
        b.Property(x => x.CodeHash).HasMaxLength(128).IsRequired();
        b.Property(x => x.Salt).HasMaxLength(64).IsRequired();

        // fast lookup for active challenge by phone+purpose
        b.HasIndex(x => new { x.PhoneNumber, x.Purpose, x.Consumed, x.ExpiresAtUtc });
    }
}
