using Bogus;
using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Seeding;

public static class WalletSeedData
{
    public static void Seed(DbContext context)
    {
        if (context.Set<Wallets>().Any()) return;

        var users = context.Set<Users>().ToList();
        var existingWallets = context.Set<Wallets>().ToList();
        var existingUserIds = existingWallets.Select(w => w.UserId).ToHashSet();
        var existingWalletNumbers = existingWallets.Select(w => w.WalletNumber).ToHashSet();

        var faker = new Faker();
        var wallets = new List<Wallets>();

        foreach (var user in users)
        {
            if (existingUserIds.Contains(user.Id)) continue;

            var walletNumber = GenerateWalletNumber(faker, existingWalletNumbers);

            wallets.Add(new Wallets(user.Id, walletNumber, 0));
        }
        ;

        if (wallets.Count == 0) return;

        context.Set<Wallets>().AddRange(wallets);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        if (await context.Set<Wallets>().AnyAsync(cancellationToken)) return;

        var users = await context.Set<Users>().ToListAsync(cancellationToken);
        var existingWallets = await context.Set<Wallets>().ToListAsync(cancellationToken);
        var existingUserIds = existingWallets.Select(w => w.UserId).ToHashSet();
        var existingWalletNumbers = existingWallets.Select(w => w.WalletNumber).ToHashSet();

        var faker = new Faker();
        var wallets = new List<Wallets>();

        foreach (var user in users)
        {
            if (existingUserIds.Contains(user.Id)) continue;

            var walletNumber = GenerateWalletNumber(faker, existingWalletNumbers);

            wallets.Add(new Wallets(user.Id, walletNumber, 0));
        }
        ;

        if (wallets.Count == 0) return;

        await context.Set<Wallets>().AddRangeAsync(wallets, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

    }

    private static string GenerateWalletNumber(Faker faker, HashSet<string> existingWalletNumbers)
    {
        string walletNumber;

        do
        {
            walletNumber = $"WLT-{faker.Random.ReplaceNumbers("#######")}";
        } while (!existingWalletNumbers.Add(walletNumber)); ;

        return walletNumber;
    }
}