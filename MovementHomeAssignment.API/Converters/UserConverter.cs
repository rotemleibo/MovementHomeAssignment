using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.Data;

namespace MovementHomeAssignment.Converters;

public class UserConverter : IUserConverter
{
    public User ToUser(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };
    }

    public UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}
