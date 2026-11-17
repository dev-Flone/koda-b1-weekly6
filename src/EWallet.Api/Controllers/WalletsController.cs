using EWallet.Core.Dtos.Wallets;
using EWallet.Core.Enums;
using EWallet.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/wallets")]
public sealed class WalletController(IWalletService service) : ControllerBase
{
    private readonly IWalletService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(WalletResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WalletResponseDto>> GetAllAync(CancellationToken cancellationToken)
    {
        var wallets = await _service.GetAllAsync(cancellationToken);

        return Ok(wallets);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(WalletResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletResponseDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var wallet = await _service.GetByIdAsync(id, cancellationToken);

        return wallet is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Wallet not found"
            ) : Ok(wallet);
    }

    [HttpGet("users/{userid:long}")]
    [ProducesResponseType(typeof(WalletResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WalletResponseDto>> GetByUserId(long userId, CancellationToken cancellationToken)
    {
        var wallet = await _service.GetByUserIdAsync(userId, cancellationToken);

        return wallet is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: $"Wallet for {userId} not found"
        ) : Ok(wallet);
    }

    [HttpGet("number/{walletNumber}")]
    [ProducesResponseType(typeof(WalletResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletResponseDto>> GetByWalletNumber([FromRoute] string walletNumber, CancellationToken cancellationToken)
    {
        var wallet = await _service.GetByWalletNumberAsync(
            walletNumber,
            cancellationToken);

        if (wallet is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: $"Wallet with number {walletNumber} not found"
            );
        }

        return Ok(wallet);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(WalletResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<WalletResponseDto>>> GetByStatus(WalletStatus status, CancellationToken cancellationToken)
    {
        var wallet = await _service.GetByStatus(status, cancellationToken);

        if (wallet.Count <= 0)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: $"No wallets found with status {status}"
            );
        }

        return Ok(wallet);
    }

    [HttpPatch("{id:long}/status")]
    [ProducesResponseType(typeof(WalletResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WalletResponseDto>> UpdateStatus(long id, [FromBody] WalletStatusUpdateDto request, CancellationToken cancellationToken)
    {
        var wallet = await _service.UpdateStatusAsync(id, request, cancellationToken);

        if (wallet is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: $"Wallet with ID {id} not found"
            );
        }
        ;

        return Ok(wallet);
    }

}