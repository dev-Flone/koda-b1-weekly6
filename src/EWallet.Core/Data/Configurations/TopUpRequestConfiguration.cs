using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class TopUpRequestConfiguration : IEntityTypeConfiguration<TopUpRequests>
{
    public void Configure(EntityTypeBuilder<TopUpRequests> builder)
    {
        builder.Property(tr => tr.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}