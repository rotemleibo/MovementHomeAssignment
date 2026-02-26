using MovementHomeAssignment.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Abstract;

/// <summary>
/// Business operations for user management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a user and returns its generated identifier.
    /// </summary>
    Task<int> CreateUser(UserDto userDto);

    /// <summary>
    /// Retrieves a user using the multi-layer cache and database fallback.
    /// </summary>
    Task<UserDto> GetUserById(int id, CancellationToken cancellationToken);
}

