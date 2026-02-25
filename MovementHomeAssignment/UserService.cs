using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.Converters;
using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.DAL;
using MovementHomeAssignment.Infrastructure.DAL.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;
    private readonly UserConverter _userConverter;

    public UserService(IUserDal userDal, UserConverter userConverter)
    {
        _userConverter = userConverter;
    }

    public async Task<UserDto> CreateUser(UserDto userDto)
    {
        var createdUser =  await _userDal.CreateUserAsync(_userConverter.ToUser(userDto));

        return _userConverter.ToUserDto(createdUser);
    }

    public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken)
    {
        var user = await _userDal.GetUserByIdAsync(id, cancellationToken);

        return _userConverter.ToUserDto(user);
    }
}

