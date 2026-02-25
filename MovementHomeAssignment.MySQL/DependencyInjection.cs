using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovementHomeAssignment.Infrastructure.DAL;
using MovementHomeAssignment.Infrastructure.DAL.Abstract;
using MovementHomeAssignment.Infrastructure.Data;

namespace MovementHomeAssignment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddMySql(
        this IServiceCollection services,
        IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySQL(connectionString));

        services.AddScoped<IUserDal, UserDal>();

        return services;
    }
}