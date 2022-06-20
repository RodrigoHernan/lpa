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
    }
    private readonly CheckDigitService _checkDigit = new CheckDigitService();

    public DbSet<Property> Properties { get; set; }
    public DbSet<ApplicationUser> users { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<VerticalCheckDigit> VerticalCheckDigits { get; set; }

    public IQueryable<Object> GetDbSetFromType(Type type)
    {

        object dbSet;
        // return dbSet;
        // .GetProperty("users")

        switch (type.Name)
        {
            case "Property":
                dbSet = Properties;
                break;
            case "ApplicationUser":
                dbSet = users;
                break;

            case "LogEntry":
                dbSet = LogEntries;
                break;

            case "VerticalCheckDigit":
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
        var tipoDeEntidadesAfectadas = _checkDigit.RecalcularDigitosVerificadores(this);
        var cantidadEntidadesAfectadas = this.DefaultSaveChanges();
        // _checkDigit.ActualizarDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        this.DefaultSaveChanges();
        return cantidadEntidadesAfectadas;
    }
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var tipoDeEntidadesAfectadas = _checkDigit.RecalcularDigitosVerificadores(this);
        var cantidadEntidadesAfectadas = await this.DefaultSaveChangesAsync(cancellationToken);
        _checkDigit.ActualizarDigitosVerificadoresVerticales(this, tipoDeEntidadesAfectadas);
        await this.DefaultSaveChangesAsync(cancellationToken);
        return cantidadEntidadesAfectadas;
    }
    #endregion

}
