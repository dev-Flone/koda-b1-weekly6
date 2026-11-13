using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethods>
{
    public void Configure(EntityTypeBuilder<PaymentMethods> builder)
    {
        builder.Property(pm => pm.Code)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();
        builder.HasIndex(pm => pm.Code).IsUnique();
    }
}