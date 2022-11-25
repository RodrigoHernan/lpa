using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Services;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using app.Data;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers;

public class ImportExportMenuController : Controller
{
    private readonly IDishService _dishService;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IClaimService _claims;

    public IWebHostEnvironment _IWebHostEnvironment { get; }

    // add context
    private readonly ApplicationDbContext _context;
    

    public ImportExportMenuController(IDishService dishService,
                                      UserManager<ApplicationUser> userManager,
                                      IClaimService claims,
                                      ApplicationDbContext context)
    {
        _dishService = dishService;
        _userManager = userManager;
        _claims = claims;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
       return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Export()
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeExportarDatos)) return Redirect("/Identity/Account/AccessDenied");

        var items = _userManager.Users;
        var xmlDoc = new XmlDocument();

        var type2 = (await _dishService.GetDishes()).GetType();

        var type = items.GetType();

        var xmlSerializer = new XmlSerializer(items.GetType());

        using (var xmlStream = new MemoryStream())
        {
            xmlSerializer.Serialize(xmlStream, items);
            xmlStream.Position = 0;
            xmlDoc.Load(xmlStream);

            var contentType = "text/xml";
            var content = xmlDoc.InnerXml;
            var bytes = Encoding.UTF8.GetBytes(content);
            var result = new FileContentResult(bytes, contentType);
            result.FileDownloadName = "myfile.xml";
            return result;
        }
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(ApplicationUserView importModel)
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeImportarDatos)) return Redirect("/Identity/Account/AccessDenied");

        var reader = importModel.XmlFile;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(reader.OpenReadStream()); 

        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/ArrayOfApplicationUser");

        
        List<ApplicationUser> users = new List<ApplicationUser>();

        foreach(XmlNode childNode in node.ChildNodes)
        {
            var applicationUser = new ApplicationUser();
            foreach(XmlNode campo in childNode.ChildNodes)
            {
                switch(campo.Name){
                        case "Id":
                            applicationUser.Id = campo.InnerText;
                            break;
                        case "UserName":
                            applicationUser.UserName = campo.InnerText;
                            break;  
                        case "NormalizedUserName":
                            applicationUser.NormalizedUserName = campo.InnerText;
                            break;
                        case "Email":
                            applicationUser.Email = campo.InnerText;
                            break;
                        case "NormalizedEmail":
                            applicationUser.NormalizedEmail = campo.InnerText;
                            break;  
                        case "EmailConfirmed":
                            applicationUser.EmailConfirmed = bool.Parse(campo.InnerText);
                            break;
                        case "PasswordHash":
                            applicationUser.PasswordHash = campo.InnerText;
                            break;
                        case "SecurityStamp":
                            applicationUser.SecurityStamp = campo.InnerText;
                            break;
                        case "ConcurrencyStamp":
                            applicationUser.ConcurrencyStamp = campo.InnerText;
                            break;
                        case "PhoneNumber":
                            applicationUser.PhoneNumber = campo.InnerText;
                            break;
                        case "PhoneNumberConfirmed":
                            applicationUser.PhoneNumberConfirmed = bool.Parse(campo.InnerText);
                            break;
                        case "TwoFactorEnabled":
                            applicationUser.TwoFactorEnabled = bool.Parse(campo.InnerText);
                            break;
                        case "LockoutEnd":
                            if(campo.InnerText != "") applicationUser.LockoutEnd = DateTime.Parse(campo.InnerText);
                            break;
                        case "LockoutEnabled":
                            applicationUser.LockoutEnabled = bool.Parse(campo.InnerText);
                            break;
                        case "AccessFailedCount":
                            applicationUser.AccessFailedCount = int.Parse(campo.InnerText);
                            break;
                        case "InvalidPasswordAttempt":
                            applicationUser.InvalidPasswordAttempt = int.Parse(campo.InnerText);
                            break;
                    }
            }
            users.Add(applicationUser);
        }


        foreach(var user in users)
        {
            var DbUser = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

            if(DbUser == null) {
                _context.Users.Add(user);
            }
            else {

                DbUser.Id = user.Id;
                DbUser.UserName = user.UserName;
                DbUser.NormalizedUserName = user.NormalizedUserName;
                DbUser.Email = user.Email;
                DbUser.NormalizedEmail = user.NormalizedEmail;
                DbUser.EmailConfirmed = user.EmailConfirmed;
                DbUser.PasswordHash = user.PasswordHash;
                DbUser.SecurityStamp = user.SecurityStamp;
                DbUser.ConcurrencyStamp = user.ConcurrencyStamp;
                DbUser.PhoneNumber = user.PhoneNumber;
                DbUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                DbUser.TwoFactorEnabled = user.TwoFactorEnabled;
                DbUser.LockoutEnd = user.LockoutEnd;
                DbUser.LockoutEnabled = user.LockoutEnabled;
                DbUser.AccessFailedCount = user.AccessFailedCount;
            }
        }
        _context.SaveChanges();
        return RedirectToAction("Index", "ApplicationUser");
    }

}
