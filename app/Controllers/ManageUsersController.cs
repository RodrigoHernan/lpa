using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using app.Models;
using app.Services;
using app.Data;


namespace app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CheckDigitService _checkDigit;
        private readonly BackupRestore _backupRestore;

        public ManageUsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) {
            _userManager = userManager;
            _checkDigit = new CheckDigitService(context);
            _backupRestore = new BackupRestore(context);
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
        public async Task<IActionResult> DatabaseCorruptionPost(DatabaseAction databaseAction) {

            if (databaseAction.Action == "reset_dv") {
                _checkDigit.ResetDV();
                return View("Message", new MessageViewModel() {Message="Digitos verificadores restaurados"});
            }
            if (databaseAction.Action == "restore") {
                return RedirectToAction("Backup", new { ShowBackup = false });
            }
            if (databaseAction.Action == "backup") {
                await _backupRestore.CrearPuntoRestauracion("backup");
                return View("Message", new MessageViewModel() {Message="Backup realizado"});
            }

            return View("Message", new MessageViewModel() {Message="Hubo un error"});
        }

        public IActionResult Message() {
            var model = new MessageViewModel(){ Message = "sadklfjdsag"};
            return View(model);
        }

        public async Task<IActionResult> Backup(bool ShowBackup=true) {



            var items = await _backupRestore.GetAll();

            var model = new BackupViewModel(){ Items = items, ShowBackup=ShowBackup};

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestorePost(RestoreAction restoreAction) {
            await _backupRestore.Restore(restoreAction.RestoreId);
            return View("Message", new MessageViewModel() {Message="Restore realizado"});
        }
    }
}
