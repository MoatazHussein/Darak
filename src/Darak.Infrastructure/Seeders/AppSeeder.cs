using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Darak.Domain.Constants;
using Darak.Domain.Entities;
using Darak.Infrastructure.Persistence;

namespace Darak.Infrastructure.Seeders;

internal class AppSeeder(AppDbContext dbContext) : IAppSeeder
{
    public async Task Seed()
    {

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }

        }
    }

    private IEnumerable<AppRole> GetRoles()
    {
        List<AppRole> roles =
            [

            new AppRole
            {
                Id = Guid.NewGuid(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Admin.ToLower()),
                NormalizedName  = UserRoles.Admin.ToUpper(),
            },
            new AppRole
            {
                Id = Guid.NewGuid(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Client.ToLower()),
                NormalizedName = UserRoles.Client.ToUpper(),
            },
            new AppRole
            {
                Id = Guid.NewGuid(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Contractor.ToLower()),
                NormalizedName = UserRoles.Contractor.ToUpper(),
            },
            ];

        return roles;
    }
}
