using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities;
using Darak.Infrastructure.Persistence;
using Darak.Infrastructure.Repositories;
using Darak.Infrastructure.Seeders;
using Darak.Infrastructure.Services.Email;
using Darak.Infrastructure.Services.Identity;
using Darak.Infrastructure.Services.Storage;
using Darak.Infrastructure.Services.TimeConversion;
using Darak.Infrastructure.Services.UnitOfWork;
using Darak.Infrastructure.Startup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Darak.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
         .EnableSensitiveDataLogging());


        services.AddScoped<IMailService, MailService>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IAppSeeder, AppSeeder>();

        services.AddScoped<ITimeZoneConverter, TimeZoneConverter>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IStartupTask, EnsureStorageFoldersTask>();

    }

}
