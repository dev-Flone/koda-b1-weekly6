using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class WalletMutationConfiguration : IEntityTypeConfiguration<WalletMutations>
{
    public void Configure(EntityTypeBuilder<WalletMutations> builder)
    {
        builder.Property(wm => wm.Direction)
            .HasConversion<string>()
            .HasMaxLength(3)
            .IsRequired();
    }
}