using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using app.Data;
using app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.ComponentModel;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

using Microsoft.EntityFrameworkCore;




namespace app.Services
{
    public class Hash : IDisposable
    {
        private SHA256 _sha256 = SHA256.Create();

        public byte[] CreateHash(IEnumerable<byte[]> seed)
        {
            byte[] hashValue;
            using (var memoryStream = new MemoryStream(4096))
            {
                foreach (var bytes in seed)
                    memoryStream.Write(bytes, 0, 32);

                memoryStream.Position = 0;

                hashValue = _sha256.ComputeHash(memoryStream);
            }

            return hashValue;
        }

        public byte[] CreateHash(string seed)
        {
            var sourceBytes = Encoding.UTF8.GetBytes(seed);
            var hashValue = _sha256.ComputeHash(sourceBytes);

            return hashValue;
        }

        public void Dispose()
        {
            if (_sha256 != null)
            {
                _sha256.Dispose();
                _sha256 = null;
            }
        }
    }


    public interface ICheckDigitService
    {
        bool ValidateDVH(IEntidadConDigitoVerificador entity);
        byte[] CalcularDigitoVerificadorHotizontal(IEntidadConDigitoVerificador entity);
        byte[] CalcularDigitoVerificadorDesdeMultiplesDigitos(IEnumerable<byte[]> crcs);
        bool DbCorrupta();

        List<ObjetosCorruptos> GetObjetosCorruptos();
    }


    public class CheckDigitService : ICheckDigitService
    {
        private readonly Hash _hash = new Hash();
        private readonly ApplicationDbContext _context;

        public CheckDigitService(ApplicationDbContext context) {
            _context = context;
        }

        public bool DbCorrupta() {
            foreach (var verticalCheckDigit in _context.VerticalCheckDigits) {
                try {
                    if (!this.ValidateDVV(verticalCheckDigit)) return true;
                } catch {
                    return true;
                }
                if (!this.ValidateDVHForAllRowsInTable(verticalCheckDigit.Entity)) return true;
            }
            return false;
        }

        public List<ObjetosCorruptos> GetObjetosCorruptos() {
            var corruptos = new List<ObjetosCorruptos>();
            foreach (var verticalCheckDigit in _context.VerticalCheckDigits) {
                IQueryable<IEntidadConDigitoVerificador> dbSet = (IQueryable<IEntidadConDigitoVerificador>) _context.GetDbSet(verticalCheckDigit.Entity);
                
                string IdsTracker =  verticalCheckDigit.IdsTracker;
                List<string> Ids = new List<string>(IdsTracker.Split(",")); 
                                
                foreach (IEntidadConDigitoVerificador row in dbSet.ToList()) {
                    if (!this.ValidateDVH(row)) 
                        corruptos.Add(new ObjetosCorruptos{Id=row.Id, NombreTabla=verticalCheckDigit.Entity, Accion="Modificado"});
                    Ids.RemoveAll(x => x == row.Id.ToString());
                }

                foreach (string id in Ids) {
                    corruptos.Add(new ObjetosCorruptos {
                        Id = new Guid(id),
                        NombreTabla = verticalCheckDigit.Entity,
                        Accion = "Eliminado"
                    });
                }

            }
            return corruptos;
        }

        public bool ValidateDVV(VerticalCheckDigit verticalCheckDigit) {
            var checksum = this.CalculateDigitoVerificadorVertical(verticalCheckDigit.Entity)?? new byte[0];
            return checksum.SequenceEqual(verticalCheckDigit.Checksum);
        }

        public bool ValidateDVH(IEntidadConDigitoVerificador row) {
            var calculatedHash = this.CalcularDigitoVerificadorHotizontal(row);
            var currentHash = row.DVH ?? new byte[0];
            return calculatedHash.SequenceEqual(currentHash);
        }

        public bool ValidateDVHForAllRowsInTable(string fullNameEntityType) {
            IQueryable<IEntidadConDigitoVerificador> dbSet = (IQueryable<IEntidadConDigitoVerificador>) _context.GetDbSet(fullNameEntityType);
            foreach (IEntidadConDigitoVerificador row in dbSet.ToList()) {
                if (!this.ValidateDVH(row)) return false;
            }
            return true;
        }

        public byte[] CalcularDigitoVerificadorDesdeMultiplesDigitos(IEnumerable<byte[]> crcs) {
            return _hash.CreateHash(crcs);
        }

        public byte[] CalcularDigitoVerificadorHotizontal(IEntidadConDigitoVerificador entidad) {
            var seed = new StringBuilder();
            var tipoEntidad = entidad.GetType();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(tipoEntidad)) {
                var fieldMarkedForRc = propertyDescriptor.Attributes;

                if (fieldMarkedForRc != null && propertyDescriptor.Name != "DVH" && propertyDescriptor.PropertyType != typeof(IFormFile)) {
                    //TODO: Verificar el orden de los campos
                    var propertyValueSerialized = Convert.ToString(propertyDescriptor.GetValue(entidad) ?? string.Empty);
                    seed.Append(propertyValueSerialized);
                }
            }

            return _hash.CreateHash(seed.ToString());
        }

    public void ResetDV(){
        var tipoDeEntidadesAfectadas = this.RecalcularDigitosVerificadoresHorizontales(true);
        _context.DefaultSaveChanges();
        this.RecalcularDigitosVerificadoresVerticales(tipoDeEntidadesAfectadas);
        _context.DefaultSaveChanges();
    }

    public Type[] RecalcularDigitosVerificadoresHorizontales(bool allValues=false) {
        var grupoDeEntidadesAfectadas = new List<Type>();
        IEnumerable<Object> entries = Enumerable.Empty<Object>();

        if (allValues) {
            foreach (var verticalCheckDigit in _context.VerticalCheckDigits) {
                IQueryable<Object> dbSet = _context.GetDbSet(verticalCheckDigit.Entity);
                var tete = dbSet;
                entries = entries.Concat(dbSet.ToList());
            }
        } else {
            var entidadesMarcadasParaAgregar = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            var entidadesMarcadasParaModificar = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            var entidadesMarcadasParaBorrar = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            grupoDeEntidadesAfectadas = entidadesMarcadasParaBorrar.Select(e => e.Entity.GetType()).Distinct().ToList();
            entries = entidadesMarcadasParaAgregar.Concat(entidadesMarcadasParaModificar);
        };

        foreach (var dbEntityEntry in entries) {
            Object entry;
            var entityEntry = dbEntityEntry as EntityEntry;
            if (entityEntry != null)
            {
                entry = entityEntry.Entity;
            } else {
                entry = dbEntityEntry;
            }


            var entidadConDigitoVerificador = entry as IEntidadConDigitoVerificador;
            if (entidadConDigitoVerificador != null)
            {
                entidadConDigitoVerificador.DVH = this.CalcularDigitoVerificadorHotizontal(entidadConDigitoVerificador);
                var entityType = entidadConDigitoVerificador;
                grupoDeEntidadesAfectadas.Add(entityType.GetType());
            }
        }
        return grupoDeEntidadesAfectadas.Distinct().ToArray();
    }

    public void RecalcularDigitosVerificadoresVerticales(IList<Type> grupoDeEntidadesAfectadas) {
        var entries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);

        foreach (var dbEntityEntry in entries)
        {
            var entidadConDigitoVerificador = dbEntityEntry.Entity as IEntidadConDigitoVerificador;
            if (entidadConDigitoVerificador != null)
                grupoDeEntidadesAfectadas.Add(entidadConDigitoVerificador.GetType());
        }

        this.ActualizarDigitosVerificadoresVerticales(grupoDeEntidadesAfectadas);
    }

    private void ActualizarDigitosVerificadoresVerticales(IList<Type> affectedTypes)
    {
        if (affectedTypes.Any())
        {
            foreach (var affectedGroupType in affectedTypes)
            {
                var entityName = affectedGroupType.FullName;
                var checksum = this.CalculateDigitoVerificadorVertical(affectedGroupType);
                if (checksum == null) continue;

                var digitoVerificadorVerticalDbSet = _context.Set<VerticalCheckDigit>();
                var vcd = digitoVerificadorVerticalDbSet.Find(entityName);
                if (vcd == null)
                {
                    vcd = new VerticalCheckDigit();
                    vcd.Entity = entityName;
                    digitoVerificadorVerticalDbSet.Add(vcd);
                }

                vcd.Checksum = checksum;
                vcd.IdsTracker = this.GetIdsTracker(affectedGroupType.FullName);
            }
        }
    }

    private byte[]? CalculateDigitoVerificadorVertical(string fullNameEntityType) {
        var crcs = new List<byte[]>();
        var dbSet = _context.GetDbSet(fullNameEntityType) as IQueryable<IEntidadConDigitoVerificador>;
        if (dbSet == null){
            return null;
        }
        dbSet.ToList().ForEach(entity => crcs.Add(entity.DVH));
        var verticalChecksum = this.CalcularDigitoVerificadorDesdeMultiplesDigitos(crcs);
        return verticalChecksum;
    }

    private string GetIdsTracker(string fullNameEntityType) {
        var Ids = new List<string>();
        var dbSet = _context.GetDbSet(fullNameEntityType) as IQueryable<IEntidadConDigitoVerificador>;
        dbSet.ToList().ForEach(entity => Ids.Add(entity.Id.ToString()));
        return string.Join(",", Ids);
    }


    private byte[] CalculateDigitoVerificadorVertical(Type entityType) {
        return this.CalculateDigitoVerificadorVertical(entityType.FullName);
    }

    }
}
