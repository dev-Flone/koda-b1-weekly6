namespace EWallet.Core.Dtos.PaymentMethods;

public sealed record PaymentMethodResponseDto
{
    public int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? ProviderName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
}