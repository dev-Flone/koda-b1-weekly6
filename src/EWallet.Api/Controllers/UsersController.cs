using EWallet.Core.Dtos.Users;
using EWallet.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{
    private readonly IUserService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var user = await _service.GetByIdAsync(id, cancellationToken);

        if (user is null) return NotFound(new
        {
            message = $"User with ID {id} was not found."
        });

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemDetails>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponseDto>> CreateAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(request, cancellationToken);

            if (created is null) return Problem(statusCode: StatusCodes.Status409Conflict, title: "User already exists");

            return CreatedAtAction(nameof(GetById), new
            {
                created.Id
            }, created);
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid User data", detail: e.Message);
        }
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponseDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> UpdateAsync([FromRoute] long id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, request, cancellationToken);

            return updated is null ? Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found",
                detail: $"There is no User with ID {id}"
            ) : Ok(updated);
        }
        catch (ArgumentException e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid User",
                detail: e.Message
                );
        }
    }

}
