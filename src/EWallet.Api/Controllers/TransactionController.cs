using EWallet.Core.Dtos.Transactions;
using EWallet.Core.Enums;
using EWallet.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService service) : ControllerBase
{
    private readonly ITransactionService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var transaction = await _service.GetAllAsync(cancellationToken);

        if (transaction.Count <= 0) return Problem("There is no transactions");
        return Ok(transaction);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var transaction = await _service.GetByIdAsync(id, cancellationToken);

        return transaction is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: $"Transaction with ID {id} is not found"
        ) : Ok(transaction);
    }

    [HttpGet("reference/{referenceNo}")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponseDto>> GetByReferenceAsync(string referenceNo, CancellationToken cancellationToken)
    {
        var transaction = await _service.GetByReferenceNoAsync(referenceNo, cancellationToken);

        return transaction is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: $"Transaction with Reference Number {referenceNo} is not found"
        ) : Ok(transaction);
    }

    [HttpGet("wallet/{walletId}")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponseDto>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken)
    {
        var transaction = await _service.GetByWalletIdAsync(walletId, cancellationToken);

        return transaction is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: $"Wallet with ID {walletId} not found."
        ) : Ok(transaction);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken)
    {
        var transaction = await _service.GetByStatusAsync(status, cancellationToken);

        return transaction is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: $"Wallet with status {status} not found."
        ) : Ok(transaction);
    }

    [HttpGet("type/{type}")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
    {
        var transaction = await _service.GetByTypeAsync(type, cancellationToken);

        return transaction is null ? Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: $"Wallet with type {type} not found."
        ) : Ok(transaction);
    }
}