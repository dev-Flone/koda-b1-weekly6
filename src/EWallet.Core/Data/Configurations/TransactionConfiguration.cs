using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transactions>
{
    public void Configure(EntityTypeBuilder<Transactions> builder)
    {
        builder.Property(t => t.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}