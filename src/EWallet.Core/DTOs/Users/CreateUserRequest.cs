using System.ComponentModel.DataAnnotations;

namespace EWallet.Core.Dtos.Users;

public sealed record CreateUserRequest
{
    [Required, StringLength(50, MinimumLength = 3)] public string UserName { get; private set; } = string.Empty;
    [Required, StringLength(150, MinimumLength = 3)] public string FullName { get; private set; } = string.Empty;
    [Required, EmailAddress, StringLength(100)] public string Email { get; private set; } = string.Empty;
    [Required, Phone, StringLength(15)] public string Phone { get; private set; } = string.Empty;
}