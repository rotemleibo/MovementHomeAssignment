using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.Data;

namespace MovementHomeAssignment.API.Abstract;

/// <summary>
/// Defines methods for converting between User domain models and UserDto data transfer objects.
/// </summary>
public interface IUserConverter
{
    /// <summary>
    /// Converts a UserDto to a User domain model.
    /// </summary>
    /// <param name="userDto">The UserDto to convert.</param>
    /// <returns>A User domain model converted from the provided UserDto.</returns>
    User ToUser(UserDto userDto);

    /// <summary>
    /// Converts a User domain model to a UserDto.
    /// </summary>
    /// <param name="user">The User to convert.</param>
    /// <returns>A UserDto converted from the provided User domain model.</returns>
    UserDto ToUserDto(User user);
}