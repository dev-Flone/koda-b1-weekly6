using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallets>
{
    public void Configure(EntityTypeBuilder<Wallets> builder)
    {
        builder.Property(w => w.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}