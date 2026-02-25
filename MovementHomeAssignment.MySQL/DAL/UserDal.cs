using Microsoft.EntityFrameworkCore;
using MovementHomeAssignment.Infrastructure.DAL.Abstract;
using MovementHomeAssignment.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.Infrastructure.DAL;

public class UserDal : IUserDal
{
    private readonly ApplicationDbContext _context;

    public UserDal(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}
