using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.ComponentModel;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

using Microsoft.EntityFrameworkCore;




namespace Inmobiliaria.Services
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
        bool EsValido(IEntidadConDigitoVerificador entity);
        byte[] CalcularDigitoVerificadorParaEntidad(IEntidadConDigitoVerificador entity);
        byte[] CalcularDigitoVerificadorDesdeMultiplesDigitos(IEnumerable<byte[]> crcs);
    }


    public class CheckDigitService : ICheckDigitService
    {
        private readonly Hash _hash = new Hash();


        public bool EsValido(IEntidadConDigitoVerificador entidad)
        {
            var calculatedHash = this.CalcularDigitoVerificadorParaEntidad(entidad);
            var currentHash = entidad.DVH ?? new byte[0];
            return calculatedHash.SequenceEqual(currentHash);
        }

        public byte[] CalcularDigitoVerificadorDesdeMultiplesDigitos(IEnumerable<byte[]> crcs)
        {
            return _hash.CreateHash(crcs);
        }

        public byte[] CalcularDigitoVerificadorParaEntidad(IEntidadConDigitoVerificador entidad)
        {
            var seed = new StringBuilder();
            var tipoEntidad = entidad.GetType();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(tipoEntidad))
            {
                var fieldMarkedForRc = propertyDescriptor.Attributes;
                    // .OfType<DatoSensibleAttribute>()
                    // .FirstOrDefault();

                if (fieldMarkedForRc != null)
                {
                    //TODO: Verificar el orden de los campos
                    var propertyValueSerialized = Convert.ToString(propertyDescriptor.GetValue(entidad) ?? string.Empty);
                    seed.Append(propertyValueSerialized);
                }
            }

            return _hash.CreateHash(seed.ToString());
        }

    public Type[] RecalcularDigitosVerificadores(ApplicationDbContext context) {
        var grupoDeEntidadesAfectadas = new List<Type>();
        var entidadesMarcadasParaAgregar = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        var entidadesMarcadasParaEliminar = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
        var entidadesMarcadasParaModificar = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

        this.RecalcularDigitosVerificadoresHorizontales(grupoDeEntidadesAfectadas, entidadesMarcadasParaAgregar.Concat(entidadesMarcadasParaModificar), context);
        this.RecalcularDigitosVerificadoresVerticales(grupoDeEntidadesAfectadas, entidadesMarcadasParaEliminar); //Afectan solo al Digito Vertical
        return grupoDeEntidadesAfectadas.Distinct().ToArray();
    }

    public void RecalcularDigitosVerificadoresHorizontales(IList<Type> grupoDeEntidadesAfectadas, IEnumerable<EntityEntry> entries, ApplicationDbContext context) {
        foreach (var dbEntityEntry in entries)
        {
            var entidadConDigitoVerificador = dbEntityEntry.Entity as IEntidadConDigitoVerificador;
            if (entidadConDigitoVerificador != null)
            {
                entidadConDigitoVerificador.DVH = this.CalcularDigitoVerificadorParaEntidad(entidadConDigitoVerificador);
                var entityType = entidadConDigitoVerificador;
                grupoDeEntidadesAfectadas.Add(entityType.GetType());
            }
        }
    }

    public void RecalcularDigitosVerificadoresVerticales(IList<Type> grupoDeEntidadesAfectadas, IEnumerable<EntityEntry> entries)
    {
        foreach (var dbEntityEntry in entries)
        {
            var entidadConDigitoVerificador = dbEntityEntry.Entity as IEntidadConDigitoVerificador;
            if (entidadConDigitoVerificador != null)
                grupoDeEntidadesAfectadas.Add(entidadConDigitoVerificador.GetType());
        }
    }

    public void ActualizarDigitosVerificadoresVerticales(ApplicationDbContext context, Type[] affectedTypes)
    {
        if (affectedTypes.Any())
        {
            foreach (var affectedGroupType in affectedTypes)
            {
                var entityName = affectedGroupType.FullName;
                var checksum = this.CalculateChecksumForEntityType(context, affectedGroupType);
                var digitoVerificadorVerticalDbSet = context.Set<VerticalCheckDigit>();
                var vcd = digitoVerificadorVerticalDbSet.Find(entityName);
                if (vcd == null)
                {
                    vcd = new VerticalCheckDigit();
                    vcd.Entity = entityName;
                    digitoVerificadorVerticalDbSet.Add(vcd);
                }

                vcd.Checksum = checksum;
            }
        }
    }

    private byte[] CalculateChecksumForEntityType(ApplicationDbContext context, Type entityType)
    {
        var crcs = new List<byte[]>();
        IQueryable<IEntidadConDigitoVerificador> dbSet = (IQueryable<IEntidadConDigitoVerificador>)context.GetDbSetFromType(entityType);
        dbSet.ToList().ForEach(entity => crcs.Add(entity.DVH));

        var verticalChecksum = this.CalcularDigitoVerificadorDesdeMultiplesDigitos(crcs);
        return verticalChecksum;
    }

    }
}
