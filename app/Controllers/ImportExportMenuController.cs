using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Services;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace app.Controllers;

public class ImportExportMenuController : Controller
{
    private readonly IDishService _dishService;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IClaimService _claims;

    public IWebHostEnvironment _IWebHostEnvironment { get; }

    public ImportExportMenuController(IDishService dishService,
                          UserManager<ApplicationUser> userManager, IClaimService claims)
    {
        _dishService = dishService;
        _userManager = userManager;
        _claims = claims;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _dishService.GetDishes();

        var model = new DishViewModel(){ Items = items };

        return View(model);
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

            // return File(xmlDoc.InnerXml, "application/xml");

            var contentType = "text/xml";
            var content = xmlDoc.InnerXml;
            var bytes = Encoding.UTF8.GetBytes(content);
            var result = new FileContentResult(bytes, contentType);
            result.FileDownloadName = "myfile.xml";
            return result;
        }



        // var gradeExportDto =  Mapper.Map<Dish>(items);
        // var writer = new System.Xml.Serialization.XmlSerializer(gradeExportDto.GetType());
        // var stream = new MemoryStream();
        // writer.Serialize(stream, gradeExportDto);

        // var fileName = responseGradeDto.Code + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".xml";

        // return File(stream.ToArray(), "application/xml", fileName);

    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import()
    {
        if (!await _claims.hasAccess(HttpContext.User, TipoPermiso.PuedeImportarDatos)) return Redirect("/Identity/Account/AccessDenied");

        return RedirectToAction("Index");
    }

}
