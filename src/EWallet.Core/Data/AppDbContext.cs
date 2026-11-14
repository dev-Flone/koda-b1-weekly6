using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Wallets> Wallets => Set<Wallets>();
    public DbSet<Transactions> Transactions => Set<Transactions>();
    public DbSet<WalletMutations> WalletMutations => Set<WalletMutations>();
    public DbSet<PaymentMethods> PaymentMethods => Set<PaymentMethods>();
    public DbSet<TopUpRequests> TopUpRequests => Set<TopUpRequests>();
    public DbSet<AuditLogs> AuditLogs => Set<AuditLogs>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Users>(entity =>
        {
            entity.ToTable("Users", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Users_Status", "[Status] IN ('ACTIVE', 'SUSPENDED')");
            });
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnType("bigint").UseIdentityColumn();

            entity.Property(u => u.UserName).HasMaxLength(50).IsRequired();
            entity.HasIndex(u => u.UserName).IsUnique();

            entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();

            entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Phone).HasMaxLength(20).IsRequired();
            entity.HasIndex(u => u.Phone).IsUnique();

            entity.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired(false);
            entity.Property(u => u.CreatedAtUtc).HasColumnType("datetime2").IsRequired();
            entity.Property(u => u.UpdatedAtUtc).HasColumnType("datetime2").IsRequired(false);

            entity.HasIndex(u => new
            {
                u.UserName,
                u.CreatedAtUtc
            });
        });

        modelBuilder.Entity<Wallets>(entity =>
        {
            entity.ToTable("Wallets", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Wallets_Balance", "[Balance] >= 0");
                table.HasCheckConstraint("CK_Wallets_Status", "[Status] IN ('ACTIVE', 'FROZEN', 'CLOSED')");
            });
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).HasColumnType("bigint").UseIdentityColumn();

            entity.HasOne(w => w.Users)
                .WithOne(u => u.Wallets)
                .HasForeignKey<Wallets>(w => w.UserId)
                .HasConstraintName("FK_Wallets_Users")
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(w => w.UserId).HasColumnType("bigint");
            entity.HasIndex(w => w.UserId).IsUnique();

            entity.Property(w => w.WalletNumber).HasMaxLength(30).IsRequired();
            entity.HasIndex(w => w.WalletNumber).IsUnique();

            entity.Property(w => w.Balance).HasPrecision(18, 2).IsRequired();
            entity.Property(w => w.RowVersion).IsRowVersion().IsRequired();
            entity.Property(w => w.CreatedAtUtc).HasColumnType("datetime2").IsRequired();
            entity.Property(w => w.UpdatedAtUtc).HasColumnType("datetime2").IsRequired(false);

            entity.HasIndex(w => new
            {
                w.UserId,
                w.WalletNumber,
                w.CreatedAtUtc
            }).HasDatabaseName("IX_Wallets_UserId_WalletNumber_CreatedAtUtc");
        });

        modelBuilder.Entity<Transactions>(entity =>
        {
            entity.ToTable("Transactions", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Transactions_Amounts", "[Amount] > 0");
                table.HasCheckConstraint("CK_Transactions_Type", "[Type] IN ('TOPUP', 'TRANSFER')");
                table.HasCheckConstraint("CK_Transactions_Status", "[Status] IN ('PENDING', 'SUCCESS', 'FAILED')");
            });
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnType("bigint").UseIdentityColumn();

            entity.Property(t => t.ReferenceNo).HasMaxLength(50).IsRequired();
            entity.HasIndex(t => t.ReferenceNo).IsUnique();

            entity.HasOne(t => t.SenderWallet)
                .WithMany(w => w.SentTransactions)
                .HasForeignKey(t => t.SenderWalletId)
                .HasConstraintName("FK_Transactions_SenderWallet")
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(t => t.SenderWalletId).HasColumnType("bigint");
            entity.HasIndex(tr => tr.SenderWalletId);

            entity.HasOne(t => t.ReceiverWallet)
                .WithMany(w => w.ReceivedTransactions)
                .HasForeignKey(t => t.ReceiverWalletId)
                .HasConstraintName("FK_Transactions_ReceiverWallet")
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(t => t.ReceiverWalletId).HasColumnType("bigint");
            entity.HasIndex(tr => tr.ReceiverWalletId);

            entity.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(255).IsRequired(false);
            entity.Property(t => t.CreatedAtUtc).HasColumnType("datetime2").IsRequired();
            entity.Property(t => t.CompletedAtUtc).HasColumnType("datetime2").IsRequired(false);

            entity.HasIndex(t => new
            {
                t.ReferenceNo,
                t.SenderWalletId,
                t.ReceiverWalletId,
                t.CreatedAtUtc
            }).HasDatabaseName("IX_Transactions_ReferenceNo_SenderWalletId_ReceiverWalletId_CreatedAtUtc");
        });

        modelBuilder.Entity<WalletMutations>(entity =>
        {
            entity.ToTable("WalletMutations", "dbo", table =>
            {
                table.HasCheckConstraint("CK_WalletMutations_Direction", "[Direction] IN ('IN', 'OUT')");
                table.HasCheckConstraint("CK_WalletMutations_Amount", "[Amount] > 0");
            });
            entity.HasKey(wm => wm.Id);
            entity.Property(wm => wm.Id).HasColumnType("bigint").UseIdentityColumn();

            entity.HasOne(wm => wm.Transaction)
                .WithMany(t => t.WalletMutations)
                .HasForeignKey(wm => wm.TransactionId)
                .HasConstraintName("FK_WalletMutations_Transactions")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(wm => wm.TransactionId);

            entity.HasOne(wm => wm.Wallets)
                .WithMany(w => w.WalletMutations)
                .HasForeignKey(wm => wm.WalletId)
                .HasConstraintName("FK_WalletMutations_Wallets")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(wm => wm.WalletId);

            entity.Property(wm => wm.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(wm => wm.BalanceBefore).HasPrecision(18, 2).IsRequired();
            entity.Property(wm => wm.BalanceAfter).HasPrecision(18, 2).IsRequired();
            entity.Property(wm => wm.CreatedAtUtc).HasColumnType("datetime2").IsRequired();

            entity.HasIndex(wm => new
            {
                wm.TransactionId,
                wm.WalletId,
                wm.CreatedAtUtc
            }).HasDatabaseName("IX_WalletMutations_TransactionId_WalletId_CreatedAtUtc");
        });

        modelBuilder.Entity<PaymentMethods>(entity =>
        {
            entity.ToTable("PaymentMethods", "dbo", table =>
            {
                table.HasCheckConstraint("CK_PaymentMethods_Code", "[Code] IN ('BANK_TRANSFER', 'QRIS')");
            });
            entity.HasKey(pm => pm.Id);
            entity.Property(pm => pm.Id).UseIdentityColumn();

            entity.Property(pm => pm.Name).HasMaxLength(100).IsRequired();
            entity.Property(pm => pm.ProviderName).HasMaxLength(100).IsRequired(false);
            entity.Property(pm => pm.IsActive).IsRequired();
            entity.Property(pm => pm.CreatedAtUtc).HasColumnType("datetime2").IsRequired();

            entity.HasIndex(pm => new
            {
                pm.Name,
                pm.IsActive,
                pm.CreatedAtUtc
            }).HasDatabaseName("IX_PaymentMethods_Name_IsActive_CreatedAtUtc");
        });

        modelBuilder.Entity<TopUpRequests>(entity =>
        {
            entity.ToTable("TopUpRequests", "dbo", table =>
            {
                table.HasCheckConstraint("CK_TopUpRequests_Amount", "[Amount] > 0");
                table.HasCheckConstraint("CK_TopUpRequests_Status", "[Status] IN ('PENDING', 'PAID', 'FAILED', 'EXPIRED')");
            });
            entity.HasKey(tr => tr.Id);
            entity.Property(tr => tr.Id).HasColumnType("bigint").UseIdentityColumn();

            entity.HasOne(tr => tr.Transaction)
                .WithOne(t => t.TopUpRequest)
                .HasForeignKey<TopUpRequests>(tr => tr.TransactionId)
                .HasConstraintName("FK_TopUpRequests_TransactionId")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(tr => tr.TransactionId).IsUnique();

            entity.HasOne(tr => tr.Wallet)
                .WithMany(t => t.TopUpRequests)
                .HasForeignKey(tr => tr.WalletId)
                .HasConstraintName("FK_TopUpRequests_WalletId")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(tr => tr.WalletId);

            entity.HasOne(tr => tr.PaymentMethod)
                .WithMany(pm => pm.TopUpRequests)
                .HasForeignKey(tr => tr.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(tr => tr.PaymentMethodId);

            entity.Property(tr => tr.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(tr => tr.ProviderReference).HasMaxLength(100).IsRequired(false);
            entity.Property(tr => tr.CreatedAtUtc).HasColumnType("datetime2").IsRequired();
            entity.Property(tr => tr.ExpiresAtUtc).HasColumnType("datetime2");
            entity.Property(tr => tr.PaidAtUtc).HasColumnType("datetime2");

            entity.HasIndex(tr => new
            {
                tr.TransactionId,
                tr.WalletId,
                tr.PaymentMethodId,
                tr.CreatedAtUtc
            }).HasDatabaseName("TopUpRequests_TransactionId_WalletId_PamentMethodId_CreatedAtUtc");
        });

        modelBuilder.Entity<AuditLogs>(entity =>
        {
            entity.ToTable("AuditLogs", "dbo", table =>
            {
                table.HasCheckConstraint("CK_AuditLogs_Outcome", "[Outcome] IN ('SUCCESS', 'REJECTED', 'FAILED')");
            });
            entity.HasKey(al => al.Id);
            entity.Property(al => al.Id).UseIdentityColumn();

            entity.HasOne(al => al.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.ActorUserId)
                .HasConstraintName("FK_AuditLogs_ActorUserId").
                OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(al => al.ActorUserId);

            entity.HasOne(al => al.Transaction)
                .WithMany(t => t.AuditLogs)
                .HasForeignKey(al => al.TransactionId)
                .HasConstraintName("FK_AuditLogs_TransactionId")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(al => al.TransactionId);

            entity.Property(al => al.EntityId).HasMaxLength(60).IsRequired(false);
            entity.Property(al => al.Description).HasMaxLength(500).IsRequired(false);
            entity.Property(al => al.CorrelationId).HasMaxLength(100).IsRequired(false);
            entity.Property(al => al.CreatedAtUtc).HasColumnType("datetime2").IsRequired();

            entity.HasIndex(al => new
            {
                al.ActorUserId,
                al.TransactionId,
                al.CreatedAtUtc
            }).HasDatabaseName("IX_ActorUserId_TransactionId_CreatedAtUtc");
        });
    }
}