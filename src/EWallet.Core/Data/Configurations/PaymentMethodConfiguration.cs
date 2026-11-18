using EWallet.Core.Enums;
using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class PaymentMethodConfiguration
    : IEntityTypeConfiguration<PaymentMethods>
{
    public void Configure(EntityTypeBuilder<PaymentMethods> builder)
    {
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Id)
            .ValueGeneratedNever();

        builder.Property(pm => pm.Code)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(pm => pm.Code)
            .IsUnique();

        builder.Property(pm => pm.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(pm => pm.ProviderName)
            .HasMaxLength(100);

        builder.Property(pm => pm.CreatedAtUtc)
            .IsRequired();

        builder.HasData(
            new
            {
                Id = 0,
                Code = PaymentMethodCode.BANK_TRANSFER,
                Name = "Bank Transfer",
                ProviderName = "Bank",
                IsActive = true,
                CreatedAtUtc = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 1,
                Code = PaymentMethodCode.QRIS,
                Name = "QRIS",
                ProviderName = "QRIS",
                IsActive = true,
                CreatedAtUtc = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}