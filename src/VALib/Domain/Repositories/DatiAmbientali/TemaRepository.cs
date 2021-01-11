using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using VALib.Domain.Entities.DatiAmbientali;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class TemaRepository : Repository
    {
        private static readonly TemaRepository _instance = new TemaRepository(Settings.DivaWebConnectionString);
        private static readonly string _webCacheKey = "Temi";

        private TemaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TemaRepository Instance
        {
            get { return _instance; }
        }

        public List<Tema> RecuperaTemi()
        {
            List<Tema> temi = new List<Tema>();

            temi = this.CacheGet(_webCacheKey) as List<Tema>;

            if (temi == null)
            {
                temi = RecuperaTemiPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, temi, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, temi, TimeSpan.FromMinutes(15));
            }

            return temi;
        }

        public Tema RecuperaTema(int id)
        {
            return RecuperaTemi().FirstOrDefault(x => x.ID == id);
        }

        private List<Tema> RecuperaTemiPrivate()
        {
            List<Tema> temi = new List<Tema>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT [ID_TEMA], [NOME_TEMA] FROM [WEBtblTema] ORDER BY [ORDINE_TEMA];";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Tema tema = new Tema();
                tema.ID = dr.GetInt32(0);
                tema.Nome = dr.GetString(1);

                temi.Add(tema);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return temi;
        }
    }
}
