using EWallet.Core.Models;
using EWallet.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Seeding;

public static class PaymentMethodSeedData
{
    public static void Seed(DbContext context)
    {
        var existingCodes = context.Set<PaymentMethods>().Select(pm => pm.Code).ToHashSet();
        var paymentMethods = new List<PaymentMethods>();

        if (!existingCodes.Contains(PaymentMethodCode.BANK_TRANSFER))
        {
            paymentMethods.Add(new PaymentMethods(PaymentMethodCode.BANK_TRANSFER, "Bank Transfer", "Dummy Bank", true
            ));
        }

        if (!existingCodes.Contains(PaymentMethodCode.QRIS))
        {
            paymentMethods.Add(new PaymentMethods(PaymentMethodCode.QRIS, "QRIS", "Dummy Payment Provider", true));
        }

        if (paymentMethods.Count == 0) return;

        context.Set<PaymentMethods>().AddRange(paymentMethods);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        var existingCodes = (await context.Set<PaymentMethods>().Select(pm => pm.Code).ToListAsync(cancellationToken)).ToHashSet();
        var paymentMethods = new List<PaymentMethods>();

        if (!existingCodes.Contains(PaymentMethodCode.BANK_TRANSFER))
        {
            paymentMethods.Add(new PaymentMethods(PaymentMethodCode.BANK_TRANSFER, "Bank Transfer", "Dummy Bank", true
            ));
        }

        if (!existingCodes.Contains(PaymentMethodCode.QRIS))
        {
            paymentMethods.Add(new PaymentMethods(PaymentMethodCode.QRIS, "QRIS", "Dummy Payment Provider", true));
        }

        if (paymentMethods.Count == 0) return;

        await context.Set<PaymentMethods>().AddRangeAsync(paymentMethods, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}