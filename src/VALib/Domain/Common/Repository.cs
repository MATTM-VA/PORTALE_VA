using System;
using System.Collections.Generic;
using ElogToolkit.Data.SqlServer;
using System.IO;
using VALib.Configuration;
using System.Runtime.Caching;

namespace VALib.Domain.Common
{
    public abstract class Repository
    {
        private SqlServerProvider _sqlProvider;
        private string _cacheTempDir;

        public Repository(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            else if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("L'argomento non può essere una stringa vuota o contenere solo spazi.", "connectionString");

            _sqlProvider = new SqlServerProvider(connectionString);
            _cacheTempDir = Path.Combine(Settings.PathBase, "VaPortaleCache");

            if (!Directory.Exists(_cacheTempDir)) Directory.CreateDirectory(_cacheTempDir);
        }

        protected SqlServerProvider SqlProvider { get { return _sqlProvider; } }

    

        #region CACHE SUPPORT
        protected object CacheGet(string key)
        {
            return MemoryCache.Default[key];
        }

        protected void CacheInsert(string key, object value, TimeSpan slidingExpiration)
        {
            string filename = System.IO.Path.Combine(_cacheTempDir, key + ".cache");
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy cip = new CacheItemPolicy();
            cip.SlidingExpiration = slidingExpiration;

            if (!System.IO.File.Exists(filename))
                this.CacheRemove(key);

            cip.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { filename }));

            cache.Set(key, value, cip);

        }

        protected void CacheRemove(string key)
        {
            string filename = System.IO.Path.Combine(_cacheTempDir, key + ".cache");

            System.IO.File.WriteAllText(filename, DateTime.Now.ToString());
        }


        /// <summary>
        /// Elimina tutti gli elementi dalla cache ed i relativi file su disco
        /// </summary>
        protected void CacheReset()
        {
            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
                //string filename = System.IO.Path.Combine(_cacheTempDir, element.Key + ".cache");
                //System.IO.File.Delete(filename);
            }

            string[] directoryFiles = System.IO.Directory.GetFiles(_cacheTempDir, "*.cache");
            foreach (string directoryFile in directoryFiles)
            {
                System.IO.File.Delete(directoryFile);
            }
        }

        #endregion
    }
}
