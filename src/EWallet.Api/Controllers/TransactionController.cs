using EWallet.Core.DTOs.Requests;
using EWallet.Core.DTOs.Responses;
using EWallet.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    [HttpPost]
    public async Task<IActionResult> Transfer(TransferRequest request)
    {
        return null!;
    }

    [HttpGet("/api/users/{userId:int}/transactions")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        return null!;
    }
}