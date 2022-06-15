using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Inmobiliaria.Models;
using Inmobiliaria.Services;

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

    #region "Sobrescritura de los métodos "Save""
    protected int DefaultSaveChanges() {
        return base.SaveChanges();
    }
    protected Task<int> DefaultSaveChangesAsync() {
        return base.SaveChangesAsync();
    }
    protected Task<int> DefaultSaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges() {
        var tipoDeEntidadesAfectadas = _checkDigit.RecalcularDigitosVerificadores(this);
        var cantidadEntidadesAfectadas = this.DefaultSaveChanges();
        // this.ActualizarDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        return cantidadEntidadesAfectadas;
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var tipoDeEntidadesAfectadas = _checkDigit.RecalcularDigitosVerificadores(this);
        var cantidadEntidadesAfectadas = this.DefaultSaveChangesAsync(cancellationToken);
        // this.ActualizarDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        return cantidadEntidadesAfectadas;
    }
    #endregion

}
