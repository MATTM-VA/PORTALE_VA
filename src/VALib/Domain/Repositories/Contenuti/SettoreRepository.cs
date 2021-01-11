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
    public sealed class SettoreRepository : Repository
    {
        private static readonly SettoreRepository _instance = new SettoreRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Settori";

        private SettoreRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static SettoreRepository Instance
        {
            get { return _instance; }
        }

        public List<Settore> RecuperaSettori()
        {
            List<Settore> settori = new List<Settore>();

            settori = this.CacheGet(_webCacheKey) as List<Settore>;

            if (settori == null)
            {
                settori = RecuperaSettoriPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, settori, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, settori, TimeSpan.FromMinutes(15));
            }

            return settori;
        }

        public Settore RecuperaSettore(int id)
        {
            return RecuperaSettori().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<Settore> RecuperaSettoriPrivate()
        {
            List<Settore> settori = new List<Settore>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT SettoreID, Nome_IT, Nome_EN FROM dbo.TBL_Settori;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Settore settore = RiempiIstanza(dr);
                settori.Add(settore);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return settori;
        }

        private Settore RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Settore settore = new Settore();

            settore.ID = dr.GetInt32(0);
            settore._nome_IT = dr.GetString(1);
            settore._nome_EN = dr.GetString(2);

            return settore;
        }
    }
}
