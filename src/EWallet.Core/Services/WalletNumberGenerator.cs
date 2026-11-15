using System.Security.Cryptography;
using EWallet.Core.Services.Interfaces;

namespace EWallet.Core.Services;

public sealed class WalletNumberGenerator : IWalletNumberGenerator
{
    public string Generate()
    {
        var date = DateTime.UtcNow.AddHours(7).ToString("ddMMyyyy");
        var identifier = RandomNumberGenerator.GetInt32(0, 100000000);

        return $"WLT-{date}-{identifier:D8}";
    }
}
