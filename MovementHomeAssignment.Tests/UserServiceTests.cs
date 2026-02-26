using Microsoft.Extensions.Options;
using Moq;
using MovementHomeAssignment.API;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.API.InMemory;
using MovementHomeAssignment.API.Services;
using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.DAL.Abstract;
using MovementHomeAssignment.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Tests;

[TestClass]
public class UserServiceTests
{
    const string FirstName = "First";
    const string LastName = "Last";
    const int UserId = 7;

    [TestMethod]
    public async Task CreateUser_ReturnsCreatedId_AndPassesConvertedUserToDal()
    {
        var userDto = new UserDto { FirstName = FirstName, LastName = LastName };
        var expectedUser = new User { FirstName = FirstName, LastName = LastName };
        var createdUser = new User { Id = UserId, FirstName = FirstName, LastName = LastName };

        var mockCacheService = new Mock<ICacheService>();
        var mockUserDal = new Mock<IUserDal>();
        var mockConverter = new Mock<IUserConverter>();
        var inMemoryCache = CreateCache();

        mockConverter.Setup(c => c.ToUser(userDto)).Returns(expectedUser);
        mockUserDal.Setup(d => d.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdUser);

        var service = new UserService(mockUserDal.Object, mockConverter.Object, mockCacheService.Object, inMemoryCache);

        var result = await service.CreateUser(userDto);

        Assert.AreEqual(UserId, result);

        mockConverter.Verify(c => c.ToUser(userDto), Times.Once);
        mockUserDal.Verify(d => d.CreateUserAsync(It.Is<User>(u =>
            u.FirstName == userDto.FirstName &&
            u.LastName == userDto.LastName),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetUserById_RedisHit_ReturnsCachedUser_AndSkipsFallbacks()
    {
        var cached = CreateUserDto(UserId);

        var mockCacheService = new Mock<ICacheService>();
        mockCacheService.Setup(c => c.GetAsync<UserDto>(UserId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cached);

        var mockUserDal = new Mock<IUserDal>();
        var mockConverter = new Mock<IUserConverter>();
        var inMemoryCache = CreateCache();

        var service = new UserService(mockUserDal.Object, mockConverter.Object, mockCacheService.Object, inMemoryCache);

        var result = await service.GetUserById(7, CancellationToken.None);

        Assert.AreSame(cached, result);
        mockCacheService.Verify(c => c.GetAsync<UserDto>(UserId.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        mockCacheService.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<UserDto>(), It.IsAny<CancellationToken>()), Times.Never);
        mockUserDal.Verify(d => d.GetUserByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task GetUserById_InMemoryHit_SetsRedisAndReturnsUser()
    {
        var cached = CreateUserDto(UserId);

        var mockCacheService = new Mock<ICacheService>();
        mockCacheService.Setup(c => c.GetAsync<UserDto>(UserId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto)null);

        var mockUserDal = new Mock<IUserDal>();
        var mockConverter = new Mock<IUserConverter>();
        var inMemoryCache = CreateCache();
        inMemoryCache.Set(5, cached);

        var service = new UserService(mockUserDal.Object, mockConverter.Object, mockCacheService.Object, inMemoryCache);

        var result = await service.GetUserById(5, CancellationToken.None);

        Assert.AreSame(cached, result);
        mockCacheService.Verify(c => c.GetAsync<UserDto>(UserId.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        mockCacheService.Verify(c => c.SetAsync(UserId.ToString(), cached, It.IsAny<CancellationToken>()), Times.Once);
        mockUserDal.Verify(d => d.GetUserByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task GetUserById_DatabaseHit_SetsCachesAndReturnsUser()
    {
        var user = CreateUser(UserId);
        var userDto = CreateUserDto(UserId);

        var mockCacheService = new Mock<ICacheService>();
        mockCacheService.Setup(c => c.GetAsync<UserDto>(UserId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto)null);

        var mockUserDal = new Mock<IUserDal>();
        mockUserDal.Setup(d => d.GetUserByIdAsync(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var mockConverter = new Mock<IUserConverter>();
        mockConverter.Setup(c => c.ToUserDto(user)).Returns(userDto);

        var inMemoryCache = CreateCache();

        var service = new UserService(mockUserDal.Object, mockConverter.Object, mockCacheService.Object, inMemoryCache);

        var result = await service.GetUserById(9, CancellationToken.None);

        Assert.IsNotNull(result);
        Assert.AreEqual(9, result.Id);
        mockUserDal.Verify(d => d.GetUserByIdAsync(UserId, It.IsAny<CancellationToken>()), Times.Once);
        mockCacheService.Verify(c => c.SetAsync(UserId.ToString(), userDto, It.IsAny<CancellationToken>()), Times.Once);
        Assert.AreSame(result, inMemoryCache.Get(UserId));
    }

    [TestMethod]
    public async Task GetUserById_DatabaseMiss_ReturnsNull_AndDoesNotSetCache()
    {
        var mockCacheService = new Mock<ICacheService>();
        mockCacheService.Setup(c => c.GetAsync<UserDto>(UserId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto)null);

        var mockUserDal = new Mock<IUserDal>();
        mockUserDal.Setup(d => d.GetUserByIdAsync(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        var mockConverter = new Mock<IUserConverter>();
        var inMemoryCache = CreateCache();

        var service = new UserService(mockUserDal.Object, mockConverter.Object, mockCacheService.Object, inMemoryCache);

        var result = await service.GetUserById(UserId, CancellationToken.None);

        Assert.IsNull(result);
        mockUserDal.Verify(d => d.GetUserByIdAsync(UserId, It.IsAny<CancellationToken>()), Times.Once);
        mockCacheService.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<UserDto>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.IsNull(inMemoryCache.Get(UserId));
    }

    private static InMemoryCache<UserDto> CreateCache(int capacity = 3) =>
        new(Options.Create(new InMemoryCacheOptions { Capacity = capacity }));

    private static UserDto CreateUserDto(int id) =>
        new() { Id = id, FirstName = $"First{id}", LastName = $"Last{id}" };

    private static User CreateUser(int id) =>
        new() { Id = id, FirstName = $"First{id}", LastName = $"Last{id}" };
}