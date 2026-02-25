using MovementHomeAssignment.DTOs;

namespace MovementHomeAssignment.Abstract;

public interface IUserService
{
    Task<UserDto> CreateUser(UserDto user);

    Task<UserDto> GetUserById(int id, CancellationToken cancellationToken);
}

