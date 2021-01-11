using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class TipoFileRepository : Repository
    {
        private static readonly TipoFileRepository _instance = new TipoFileRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipiFile";

        private TipoFileRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoFileRepository Instance
        {
            get { return _instance; }
        }

        public List<TipoFile> RecuperaTipiFile()
        {
            List<TipoFile> tipiFile = new List<TipoFile>();

            tipiFile = this.CacheGet(_webCacheKey) as List<TipoFile>;

            if (tipiFile == null)
            {
                tipiFile = RecuperaTipiFilePrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, tipiFile, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));
                //HttpContext.Current.Cache.Insert(_webCacheKey, tipiFile, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipiFile, TimeSpan.FromMinutes(15));
            }

            return tipiFile;
        }

        public TipoFile RecuperaTipoFile(int id)
        {
            return RecuperaTipiFile().FirstOrDefault(x => (int)x.ID == id);
        }

        public TipoFile RecuperaTipoFile(string ext)
        {
            return RecuperaTipiFile().FirstOrDefault(x => x.Estensione.Equals(ext.Replace(".", "")));
        }

        private List<TipoFile> RecuperaTipiFilePrivate()
        {
            List<TipoFile> tipiFile = new List<TipoFile>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TipoFileID, FileIcona, Estensione, TipoMIME, Software FROM dbo.TBL_TipiFile;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipoFile tipoFile = RiempiIstanza(dr);
                tipiFile.Add(tipoFile);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipiFile;
        }

        private TipoFile RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipoFile tipoFile = new TipoFile();

            tipoFile.ID = dr.GetInt32(0);
            tipoFile.FileIcona = dr.GetString(1);
            tipoFile.Estensione = dr.GetString(2);
            tipoFile.TipoMime = dr.GetString(3);
            tipoFile.Software = dr.GetString(4);

            return tipoFile;
        }
    }
}
