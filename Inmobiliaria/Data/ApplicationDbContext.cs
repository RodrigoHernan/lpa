using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;

namespace Inmobiliaria.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Property> Properties { get; set; }
    public DbSet<ApplicationUser> users { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }

    #region "Sobrescritura de los métodos "Save""
    protected int DefaultSaveChanges()
    {
        return base.SaveChanges();
    }

    public override int SaveChanges()
    {
        // var tipoDeEntidadesAfectadas = this.RecalcularDigitosVerificadores();
        var cantidadEntidadesAfectadas = this.DefaultSaveChanges();
        // this.ActualizarDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        return cantidadEntidadesAfectadas;
    }

    protected Task<int> DefaultSaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected Task<int> DefaultSaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // var tipoDeEntidadesAfectadas = this.RecalcularDigitosVerificadores();
        var cantidadEntidadesAfectadas = this.DefaultSaveChangesAsync(cancellationToken);
        // this.ActualizarDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        return cantidadEntidadesAfectadas;
    }
    #endregion
}
