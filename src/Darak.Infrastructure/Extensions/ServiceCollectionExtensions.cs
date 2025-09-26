using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Messaging;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities;
using Darak.Infrastructure.Persistence;
using Darak.Infrastructure.Repositories;
using Darak.Infrastructure.Seeders;
using Darak.Infrastructure.Services.Email;
using Darak.Infrastructure.Services.Messaging.Infobip;
using Darak.Infrastructure.Services.Security;
using Darak.Infrastructure.Services.Storage;
using Darak.Infrastructure.Services.TimeConversion;
using Darak.Infrastructure.Services.UnitOfWork;
using Darak.Infrastructure.Startup;
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


        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.User.RequireUniqueEmail = false;     // phone-only users can have null email
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IAppSeeder, AppSeeder>();

        services.AddScoped<ITimeZoneConverter, TimeZoneConverter>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IStartupTask, EnsureStorageFoldersTask>();

        // OTP (DB implementation)
        services.AddScoped<IOtpService, OtpServiceDb>();

        // Infobip typed client
        services.AddHttpClient<ISmsSender, InfobipSmsSender>();

    }

}
