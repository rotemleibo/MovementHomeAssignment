using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.Data;

namespace MovementHomeAssignment.API.Abstract;

public interface IUserConverter
{
    User ToUser(UserDto userDto);

    UserDto ToUserDto(User user);
}