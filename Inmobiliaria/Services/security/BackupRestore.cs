using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;

using System;
using System.Data.SqlClient;
using System.Text;

using Inmobiliaria.Data;
using Inmobiliaria.Models;

namespace Inmobiliaria.Services
{
    public interface IBackupRestore
    {
        Task<bool> CrearPuntoRestauracion(string nombre);
        // IEnumerable<Backup> ListarPuntosRestauracion();
        // bool Recuperar(Backup copiaDeSeguridad);
    }

    internal class BackupRestore : IBackupRestore
    {
        private class SqlServerBackupManager
        {
            private readonly string _databaseName;
            private readonly DatabaseFacade _currentDatabase;
            private readonly IDbConnection _masterDbConnection;

            public SqlServerBackupManager(ApplicationDbContext context)
            {
                _currentDatabase = context.Database;
                // _currentDatabase.OpenConnection();
                _databaseName = _currentDatabase.GetDbConnection().Database;
                // _currentDatabase.CloseConnection();

                //Cambio a la master asociada
                var cnnStringBuilder = new SqlConnectionStringBuilder(_currentDatabase.GetConnectionString())
                {
                    AttachDBFilename = "",
                    InitialCatalog = "master"
                };
                var masterCnnString = cnnStringBuilder.ToString();
                _masterDbConnection = new SqlConnection(masterCnnString);
            }

            public string Backup(string name)
            {
                var sql = string.Format("BACKUP DATABASE [{0}] TO DISK = '{1}_{2:yyyyMMddhhmmss}.bak'", _databaseName, name, DateTime.Now);
                _currentDatabase.ExecuteSqlRaw(sql);
                return true;
            }

            public bool Restore(string name)
            {
                _masterDbConnection.Open();
                try
                {

                    var command = _masterDbConnection.CreateCommand();
                    // command.CommandText = string.Format("use master ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE ", _databaseName);
                    // command.ExecuteNonQuery();


                    var sql = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH RECOVERY, REPLACE", _databaseName, name);
                    command = _masterDbConnection.CreateCommand();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();


                    command = _masterDbConnection.CreateCommand();
                    command.CommandText = string.Format("ALTER DATABASE {0} SET MULTI_USER", _databaseName);
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (_masterDbConnection.State != ConnectionState.Closed)
                        _masterDbConnection.Close();
                }

                return true;
            }

            // public IList<string> GetAvailableBackups()
            // {
            //     string sql;
            //     using (var stream = this.GetType().Assembly.GetManifestResourceStream(this.GetType(), "Scripts.ListAvailableBackups.sql"))
            //         sql = new System.IO.StreamReader(stream).ReadToEnd();

            //     var backupSetInfos = _currentDatabase.SqlQuery<BackupSetInfo>(sql).ToArray();
            //     return backupSetInfos.Select(b => b.physical_device_name).ToList();
            // }

            // private class BackupSetInfo
            // {
            //     public string Server { get; set; }
            //     public string physical_device_name { get; set; }
            // }

        }

        private SqlServerBackupManager _backupManager;
        private readonly ApplicationDbContext _context;

        public BackupRestore(ApplicationDbContext context)
        {
            _context = context;
            _backupManager = new SqlServerBackupManager(context);
        }

        public async Task<bool> CrearPuntoRestauracion(string nombre) {
            _backupManager.Backup(nombre);
            _context.Backups.Add(
                new BackupModel {Fecha = DateTime.Now, RutaDelArchivo = nombre + ".bak", Nombre = nombre}
            );
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<List<BackupModel>> GetAll() {
            return await _context.Backups.ToListAsync();
        }
        // public IEnumerable<Backup> ListarPuntosRestauracion()
        // {
        //     return _backupManager.GetAvailableBackups().Select(b => new Backup
        //     {
        //         Fecha = DateTime.Now,
        //         Nombre = b,
        //         RutaDelArchivo = b
        //     });
        // }


        public async Task<BackupModel> GetById(int id) {
            return await _context.Backups.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<bool> Restore(int restoreId) {
            BackupModel copiaDeSeguridad = await this.GetById(restoreId);
            _backupManager.Restore(copiaDeSeguridad.RutaDelArchivo);
            return true;
        }
    }
}