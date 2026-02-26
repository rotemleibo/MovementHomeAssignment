using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.API.InMemory;
using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.DAL.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.API.Services;

/// <summary>
/// Coordinates user operations and the multi-layer cache strategy.
/// </summary>
public class UserService : IUserService
{
    private readonly ICacheService _cacheService;
    private readonly IUserDal _userDal;
    private readonly IUserConverter _userConverter;
    private readonly InMemoryCache<UserDto> _inMemoryCache;

    public UserService(
        IUserDal userDal,
        IUserConverter userConverter,
        ICacheService cacheService,
        InMemoryCache<UserDto> inMemoryCache)
    {
        _userDal = userDal;
        _userConverter = userConverter;
        _cacheService = cacheService;
        _inMemoryCache = inMemoryCache;
    }

    /// <summary>
    /// Persists a user and returns the created identifier.
    /// </summary>
    public async Task<int> CreateUser(UserDto userDto)
    {
        var createdUser = await _userDal.CreateUserAsync(_userConverter.ToUser(userDto));

        return createdUser.Id;
    }

    /// <summary>
    /// Resolves a user via Redis, then in-memory cache, then database.
    /// </summary>
    public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken)
    {
        var userDto = await _cacheService.GetAsync<UserDto>(id.ToString(), cancellationToken);
        if (userDto != null)
        {
            //If data is found in the Redis Cache, return it immediately
            return userDto;
        }

        //If not found in the Redis Cache, check the SDCS.
        userDto = _inMemoryCache.Get(id);
        if (userDto != null)
        {
            //If found, return it and store it in the Redis Cache
            await _cacheService.SetAsync(id.ToString(), userDto, cancellationToken);
            return userDto;
        }

        //If not found in the SDCS, check the database.
        var user = await _userDal.GetUserByIdAsync(id, cancellationToken);
        if (user == null)
        {
            return null;
        }

        //If found, return it and store it in both the SDCS and Redis Cache.
        userDto = _userConverter.ToUserDto(user);
        _inMemoryCache.Set(id, userDto);
        await _cacheService.SetAsync(id.ToString(), userDto, cancellationToken);

        return userDto;
    }
}

