using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.DTOs;
using MovementHomeAssignment.Infrastructure.Data;

namespace MovementHomeAssignment.Converters;

/// <summary>
/// Implements conversion between User domain models and UserDto data transfer objects.
/// </summary>
public class UserConverter : IUserConverter
{
    /// <summary>
    /// Converts a UserDto to a User domain model.
    /// </summary>
    /// <param name="userDto">The UserDto to convert.</param>
    /// <returns>A User domain model with properties mapped from the UserDto.</returns>
    public User ToUser(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };
    }

    /// <summary>
    /// Converts a User domain model to a UserDto.
    /// </summary>
    /// <param name="user">The User to convert.</param>
    /// <returns>A UserDto with properties mapped from the User domain model.</returns>
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
