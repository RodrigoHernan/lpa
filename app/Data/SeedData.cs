using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using app.Models;


namespace app.Data
{
    public static class SeedData
    {


        public static async Task InitializeAsync(WebApplication app)
        {
            Guid SuperUserPermisoId = Guid.Parse("{CF9EE097-EE5B-4704-4D84-08DA7B0ABCA2}");
            String SuperUserUserId = "0E6A694A-6474-406A-2998-08DA7B0B8A23";

            using var scope = app.Services.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await EnsureSuperUserPatenteAsync(dataContext, SuperUserPermisoId);
            await EnsureSuperUserUserAsync(scope, SuperUserPermisoId, SuperUserUserId);
        }
        private static async Task EnsureSuperUserPatenteAsync(ApplicationDbContext dataContext, Guid SuperUserPermisoId)
        {

            var superUserPatente = await dataContext.Permisos.FirstOrDefaultAsync(p => p.Id == SuperUserPermisoId);
            if (superUserPatente == null)
            {
                superUserPatente = new Patente {
                    Nombre = "SuperUser" ,
                    TipoPermiso = TipoPermiso.SuperUser
                };
                superUserPatente.Id = SuperUserPermisoId;
                await dataContext.Permisos.AddAsync(superUserPatente);
                await dataContext.SaveChangesAsync();
            }
        }

        private static async Task EnsureSuperUserUserAsync(IServiceScope scope, Guid SuperUserPermisoId, String SuperUserUserId)

        {
            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var superUser = await userManager.Users.FirstOrDefaultAsync(p => p.Id == SuperUserUserId);
            if (superUser == null)
            {
                superUser = new ApplicationUser {
                    Id = SuperUserUserId,
                    UserName = "admin@gmail.com", Email = "ADMIN@gmail.com", NormalizedEmail="ADMIN@GMAIL.COM",
                    NormalizedUserName="ADMIN@GMAIL.COM", EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(superUser, "P@ssw0rd");

                UserPermission userPermission = new UserPermission {
                    ApplicationUserId = SuperUserUserId,
                    PermisoId = SuperUserPermisoId
                };
                await dataContext.UserPermissions.AddAsync(userPermission);
                await dataContext.SaveChangesAsync();
            }
        }

    }
}
