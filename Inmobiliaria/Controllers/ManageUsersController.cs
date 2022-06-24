using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Inmobiliaria.Models;
using Inmobiliaria.Services;
using Inmobiliaria.Data;


namespace Inmobiliaria.Controllers
{
    // [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CheckDigitService _checkDigit;

        public ManageUsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) {
            _userManager = userManager;
            _checkDigit = new CheckDigitService(context);
        }

        public async Task<IActionResult> Index() {
            var admins = (await _userManager
                .GetUsersInRoleAsync("Administrator"))
                .ToArray();

            var everyone = await _userManager.Users
                .ToArrayAsync();

            // var roles = await RoleManager<IdentityRole>.Roles
            //     .ToArrayAsync();

            var model = new ManageUsersViewModel
            {
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }

        public IActionResult DatabaseCorruption() {
            return View();
        }

        [ValidateAntiForgeryToken]
        public IActionResult DatabaseCorruptionPost(DatabaseAction databaseAction) {
            if (databaseAction.Action == "reset_dv") {
                _checkDigit.ResetDV();
                return View("Message", new MessageViewModel() {Message="Digitos verificadores restaurados"});
            }
            if (databaseAction.Action == "restore") {
                return View("Message", new MessageViewModel() {Message="Base de datos restaurada"});
            }

            return View("Message", new MessageViewModel() {Message="Hubo un error"});
        }

        public IActionResult Message() {
            var model = new MessageViewModel(){ Message = "sadklfjdsag"};
            return View(model);
        }
    }
}
