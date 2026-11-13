using EWallet.Core.DTOs.Requests;
using EWallet.Core.DTOs.Responses;
using EWallet.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController(IWalletService walletService) : ControllerBase
{
    private readonly IWalletService _walletService = walletService;

    // Get by UserId
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        return null!;

        // Create
        [HttpPost("{userId:int}/topups")]
        async Task<IActionResult> TopUp(int userId, TopUpRequest request)
        {
            return null!;
        }
    }
}