using Microsoft.AspNetCore.Mvc;
using MovementHomeAssignment.DTOs;

namespace MovementHomeAssignment.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserById(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<UserDto> Create(UserDto user)
    {
        var createdUser = await _userService.CreateUser(user);
        return createdUser;
    }
}