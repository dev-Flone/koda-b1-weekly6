using Bogus;
using EWallet.Core.Data;
using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Seeding;

public static class UserSeedData
{
    public static void Seed(DbContext context)
    {
        if (context.Set<Users>().Any()) return;

        var users = new List<Users>();

        var faker = new Faker();

        for (int i = 1; i <= 10; i++)
        {
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            var userName = $"{firstName.ToLowerInvariant()}{faker.Random.Number(1, 1000)}";
            var fullName = $"{firstName} {lastName}";
            var email = $"{firstName.ToLowerInvariant()}{faker.Random.Number(1, 99)}@example.com";
            var phone = $"{faker.Phone.PhoneNumber("08##########")}";

            users.Add(new Users(userName, fullName, email, phone));
        }
        context.Set<Users>().AddRange(users);

        context.SaveChanges();
    }
    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        if (await context.Set<Users>().AnyAsync(cancellationToken)) return;

        var users = new List<Users>();

        var faker = new Faker();
        var name = faker.Name;

        for (int i = 1; i <= 10; i++)
        {
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            var userName = $"{firstName.ToLowerInvariant()}{faker.Random.Number(1, 1000)}";
            var fullName = $"{firstName} {lastName}";
            var email = $"{firstName.ToLowerInvariant()}{faker.Random.Number(1, 99)}@example.com";
            var phone = $"{faker.Phone.PhoneNumber("08##########")}";

            users.Add(new Users(userName, fullName, email, phone));
        }
        await context.Set<Users>().AddRangeAsync(users, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}