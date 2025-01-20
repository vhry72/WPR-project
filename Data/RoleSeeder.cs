using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public class RoleSeeder
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[]
        {
            "ZakelijkeHuurder",
            "WagenparkBeheerder",
            "BedrijfsMedewerker",
            "ParticuliereHuurder",
            "FrontofficeMedewerker",
            "BackofficeMedewerker"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
