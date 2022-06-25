using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Inmobiliaria.Models;
using Inmobiliaria.Services;

using System;
using System.Linq;
using System.Reflection;

namespace Inmobiliaria.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        _checkDigit = new CheckDigitService(this);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }


    private readonly CheckDigitService _checkDigit;

    public DbSet<Property> Properties { get; set; }
    public DbSet<ApplicationUser> users { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<VerticalCheckDigit> VerticalCheckDigits { get; set; }
    public DbSet<BackupModel> Backups { get; set; }

    public IQueryable<Object> GetDbSet(Type type) {
        return this.GetDbSet(type.FullName);
    }

    public IQueryable<Object> GetDbSet(string type) {

        object dbSet;

        switch (type)
        {
            case "Inmobiliaria.Models.Property":
                dbSet = Properties;
                break;
            case "Inmobiliaria.Models.ApplicationUser":
                dbSet = users;
                break;
            case "Inmobiliaria.Models.LogEntry":
                dbSet = LogEntries;
                break;
            case "Inmobiliaria.Models.VerticalCheckDigit":
                dbSet = VerticalCheckDigits;
                break;
            default:
                dbSet = null;
                break;
        }
        return (IQueryable<Object>)dbSet;
    }

    #region "Sobrescritura de los métodos "Save""
    public int DefaultSaveChanges() {
        return base.SaveChanges();
    }
    public Task<int> DefaultSaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges() {
        var tipoDeEntidadesAfectadas = _checkDigit.RecalcularDigitosVerificadoresHorizontales();
        var cantidadEntidadesAfectadas = this.DefaultSaveChanges();
        _checkDigit.RecalcularDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        this.DefaultSaveChanges();
        return cantidadEntidadesAfectadas;
    }
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var tipoDeEntidadesAfectadas = _checkDigit.RecalcularDigitosVerificadoresHorizontales();
        var cantidadEntidadesAfectadas = await this.DefaultSaveChangesAsync(cancellationToken);
        _checkDigit.RecalcularDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        await this.DefaultSaveChangesAsync(cancellationToken);
        return cantidadEntidadesAfectadas;
    }
    #endregion

}
