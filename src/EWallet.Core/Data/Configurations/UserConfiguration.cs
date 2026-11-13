using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWallet.Core.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> builder)
    {
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}