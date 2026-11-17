using EWallet.Core.Dtos.TopUps;
using EWallet.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopUpController(ITopUpService service) : ControllerBase
{
    private readonly ITopUpService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(TopUpResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TopUpResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var topup = await _service.GetAllAsync(cancellationToken);

        return Ok(topup);
    }

    [HttpGet("{id}", Name = "GetTopUpById")]
    [ProducesResponseType(typeof(TopUpResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TopUpResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TopUpResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var topup = await _service.GetByIdAsync(id, cancellationToken);

        if (topup is null) return NotFound();

        return Ok(topup);
    }


    [HttpGet("wallet/{walletId:long}")]
    [ProducesResponseType(typeof(TopUpResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TopUpResponseDto>>> GetByWalletId(long walletId, CancellationToken cancellationToken)
    {
        var topups = await _service.GetByWalletIdAsync(walletId, cancellationToken);

        if (topups is null) return NotFound();

        return Ok(topups);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TopUpResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TopUpResponseDto>> CreateAsync(CreateTopUpRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);

        if (created is null) return Problem(statusCode: StatusCodes.Status409Conflict, title: "TopUp request already exists.");

        return CreatedAtRoute(
            "GetTopUpById",
            new { id = created.Id },
            created
        );
    }
}