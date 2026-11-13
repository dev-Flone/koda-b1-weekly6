using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLogs>
{
    public void Configure(EntityTypeBuilder<AuditLogs> builder)
    {
        builder.Property(al => al.Action)
            .HasConversion<string>()
            .HasMaxLength(60)
            .IsRequired();
        builder.Property(al => al.Type)
            .HasConversion<string>()
            .HasMaxLength(60)
            .IsRequired();
        builder.Property(al => al.Outcome)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}