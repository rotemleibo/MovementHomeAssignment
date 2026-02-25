using Microsoft.AspNetCore.Mvc;
using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
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
    public async Task<IActionResult> Create(UserDto user)
    {
        var createdUserId = await _userService.CreateUser(user);

        return Ok(createdUserId);
    }
}