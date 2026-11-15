using EWallet.Core.Data;
using EWallet.Core.Dtos.Users;
using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Repositories.Interfaces;
using EWallet.Core.Services.Interfaces;

namespace EWallet.Core.Services;

public sealed class UserService(AppDbContext context, IUserRepository userRepository, IWalletRepository walletRepository, IWalletNumberGenerator walletNumberGenerator) : IUserService
{

    public async Task<UserResponseDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        // Validation
        // Duplicate Username
        if (await userRepository.ExistsByUsernameAsync(request.UserName, cancellationToken))
        {
            throw new InvalidOperationException("Username is already registered.");
        }

        // Duplicate Email
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        // Duplicate Phone Number
        if (await userRepository.ExistsByPhoneAsync(request.Phone, cancellationToken))
        {
            throw new InvalidOperationException("Phone number is already registered.");
        }

        // Begin Database Transaction
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        // Create User and Wallet Automatically
        string walletNumber;

        do
        {
            walletNumber = walletNumberGenerator.Generate();
        } while (await walletRepository.ExistsByWalletNumber(walletNumber, cancellationToken));

        try
        {
            // Create User
            var user = new Users(
                request.UserName,
                request.FullName,
                request.Email,
                request.Phone
            );

            // Add User to Model
            await userRepository.AddAsync(user, cancellationToken);

            // Save changes to Database
            await context.SaveChangesAsync(cancellationToken);

            // Create Wallet Automatically
            var wallet = new Wallets(
                user.Id,
                walletNumber,
                0m
            );

            // Add Wallet to Model
            await walletRepository.AddAsync(wallet, cancellationToken);

            // Save changes to Database
            await context.SaveChangesAsync(cancellationToken);

            // Commit Operations
            await transaction.CommitAsync(cancellationToken);

            return MapToResponse(user);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null) return false;

        if (user.Status == UserStatus.SUSPENDED) return true;

        user.Suspend();

        userRepository.Update(user);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<List<UserResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAllAsync(cancellationToken);

        return [.. user.Select(MapToResponse)];
    }

    public async Task<UserResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        return user is null ? null : MapToResponse(user);
    }

    public async Task<UserResponseDto?> UpdateAsync(long id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null) return null;

        if (await userRepository.ExistsByUsernameAsync(request.UserName, cancellationToken))
        {
            throw new InvalidOperationException("Username is already registered.");
        }

        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        if (await userRepository.ExistsByPhoneAsync(request.Phone, cancellationToken))
        {
            throw new InvalidOperationException("Phone Numberis already registered.");
        }

        user.UpdateProfile(
            request.UserName,
            request.FullName,
            request.Email,
            request.Phone
        );

        userRepository.Update(user);

        await context.SaveChangesAsync(cancellationToken);

        return MapToResponse(user);
    }

    // Methods
    private static UserResponseDto MapToResponse(Users user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Status = user.Status.ToString(),
            CreatedAtUtc = user.CreatedAtUtc,
            UpdatedAtUtc = user.UpdatedAtUtc
        };
    }
}