using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class TipoLinkRepository : Repository
    {
        private static readonly TipoLinkRepository _instance = new TipoLinkRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipiLink";

        private TipoLinkRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoLinkRepository Instance
        {
            get { return _instance; }
        }

        public List<TipoLink> RecuperaTipiLink()
        {
            List<TipoLink> tipiLink = new List<TipoLink>();

            tipiLink = this.CacheGet(_webCacheKey) as List<TipoLink>;

            if (tipiLink == null)
            {
                tipiLink = RecuperaTipiLinkPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, tipiLink, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipiLink, TimeSpan.FromMinutes(15));
            }

            return tipiLink;
        }

        private List<TipoLink> RecuperaTipiLinkPrivate()
        {
            List<TipoLink> tipiLink = new List<TipoLink>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TipoLinkID, Nome_IT, Nome_EN FROM dbo.TBL_TipiLink;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipoLink tipoLink = RiempiIstanza(dr);
                tipiLink.Add(tipoLink);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipiLink;
        }

        private TipoLink RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipoLink tipoLink = new TipoLink();

            tipoLink.ID = dr.GetInt32(0);
            tipoLink._nome_IT = dr.GetString(1);
            tipoLink._nome_EN = dr.GetString(2);
            tipoLink.Enum = (TipoLinkEnum)dr.GetInt32(0);

            return tipoLink;
        }
    }
}
