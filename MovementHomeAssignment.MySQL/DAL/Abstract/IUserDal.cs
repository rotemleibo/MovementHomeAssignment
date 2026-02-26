using MovementHomeAssignment.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Infrastructure.DAL.Abstract;

/// <summary>
/// Data access abstraction for user persistence.
/// </summary>
public interface IUserDal
{
    /// <summary>
    /// Creates a user entity in the database.
    /// </summary>
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
}
