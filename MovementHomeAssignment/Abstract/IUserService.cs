using MovementHomeAssignment.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Abstract;

public interface IUserService
{
    Task<UserDto> CreateUser(UserDto userDto);

    Task<UserDto> GetUserById(int id, CancellationToken cancellationToken);
}

