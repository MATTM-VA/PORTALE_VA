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
    public sealed class CategoriaNotiziaRepository : Repository
    {
        private static readonly CategoriaNotiziaRepository _instance = new CategoriaNotiziaRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "CategorieNotizie";

        private CategoriaNotiziaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static CategoriaNotiziaRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<CategoriaNotizia> RecuperaCategorieNotizie()
        {
            IEnumerable<CategoriaNotizia> categorieNotizia = null;

            categorieNotizia = this.CacheGet(_webCacheKey) as IEnumerable<CategoriaNotizia>;

            if (categorieNotizia == null)
            {
                categorieNotizia = RecuperaCategorieNotiziePrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, categorieNotizia, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, categorieNotizia, TimeSpan.FromMinutes(15));
            }

            return categorieNotizia;
        }

        public CategoriaNotizia RecuperaCategoriaNotizia(int id)
        {
            return RecuperaCategorieNotizie().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<CategoriaNotizia> RecuperaCategorieNotiziePrivate()
        {
            List<CategoriaNotizia> categorieNotizia = new List<CategoriaNotizia>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT CategoriaNotiziaID, Nome_IT, Nome_EN FROM dbo.TBL_CategorieNotizie;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                CategoriaNotizia categoriaNotizia = RiempiIstanza(dr);
                categorieNotizia.Add(categoriaNotizia);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return categorieNotizia;
        }

        private CategoriaNotizia RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            CategoriaNotizia categoriaNotizia = new CategoriaNotizia();

            categoriaNotizia.ID = dr.GetInt32(0);
            categoriaNotizia._nome_IT = dr.GetString(1);
            categoriaNotizia._nome_EN = dr.GetString(2);

            categoriaNotizia.Enum = (CategoriaNotiziaEnum)categoriaNotizia.ID;

            return categoriaNotizia;
        }
    }
}
