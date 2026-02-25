using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.Converters;
using MovementHomeAssignment.DTOs;

namespace MovementHomeAssignment;

public class UserService : IUserService
{
    private readonly UserConverter _userConverter;

    public UserService(UserConverter userConverter)
    {
        _userConverter = userConverter;
    }

    public Task<UserDto> CreateUser(UserDto userDto)
    {
        var user = _userConverter.ToUser(userDto);

    }

    public Task<UserDto> GetUserById(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

