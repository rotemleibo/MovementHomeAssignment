using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.Converters;
using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.DAL.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.API.Services;

public class UserService : IUserService
{
    private readonly ICacheService _cacheService;
    private readonly IUserDal _userDal;
    private readonly UserConverter _userConverter;

    public UserService(IUserDal userDal, UserConverter userConverter, ICacheService cacheService)
    {
        _userDal = userDal;
        _userConverter = userConverter;
        _cacheService = cacheService;
    }

    public async Task<int> CreateUser(UserDto userDto)
    {
        var createdUser = await _userDal.CreateUserAsync(_userConverter.ToUser(userDto));

        return createdUser.Id;
    }

    public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken)
    {

        var userDto = await _cacheService.GetAsync<UserDto>(id.ToString(), cancellationToken);
        if (userDto != null)
        {
            return userDto;
        }

        var user = await _userDal.GetUserByIdAsync(id, cancellationToken);
        userDto = _userConverter.ToUserDto(user);

        await _cacheService.SetAsync(id.ToString(), userDto, cancellationToken);
        return userDto;
    }
}

