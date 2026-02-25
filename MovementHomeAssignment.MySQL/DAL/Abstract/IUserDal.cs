using MovementHomeAssignment.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Infrastructure.DAL.Abstract;

public interface IUserDal
{
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
}
