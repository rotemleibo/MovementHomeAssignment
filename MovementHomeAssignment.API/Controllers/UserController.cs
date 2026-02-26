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

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The user if found; otherwise a 404 response.</returns>
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

    /// <summary>
    /// Creates a new user record.
    /// </summary>
    /// <param name="user">User details for creation.</param>
    /// <returns>The newly created user identifier.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(UserDto user)
    {
        var createdUserId = await _userService.CreateUser(user);

        return Ok(createdUserId);
    }
}