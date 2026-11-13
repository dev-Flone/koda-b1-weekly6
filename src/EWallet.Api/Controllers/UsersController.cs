using EWallet.Core.DTOs.Requests;
using EWallet.Core.DTOs.Responses;
using EWallet.Core.Interfaces.Services;
using EWallet.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    // Get All
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return null!;
    }

    // Get By Id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        return null!;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult> Create(CreateUserRequest request)
    {
        return null!;
    }

    // Update
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserRequest request)
    {
        return null!;
    }
}
