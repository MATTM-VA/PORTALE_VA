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
    public sealed class CategoriaImpiantoRepository : Repository
    {
        private static readonly CategoriaImpiantoRepository _instance = new CategoriaImpiantoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "CategorieImpianti";

        private CategoriaImpiantoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static CategoriaImpiantoRepository Instance
        {
            get { return _instance; }
        }

        public List<CategoriaImpianto> RecuperaCategorie()
        {
            List<CategoriaImpianto> categorie = new List<CategoriaImpianto>();

            categorie = this.CacheGet(_webCacheKey) as List<CategoriaImpianto>;

            if (categorie == null)
            {
                categorie = RecuperaCategoriePrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, settori, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, categorie, TimeSpan.FromMinutes(15));
            }

            return categorie;
        }

        public CategoriaImpianto RecuperaCategoria(int id)
        {
            return RecuperaCategorie().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<CategoriaImpianto> RecuperaCategoriePrivate()
        {
            List<CategoriaImpianto> categorie = new List<CategoriaImpianto>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT CategoriaImpiantoID, Nome_IT, Nome_EN, FileIcona FROM dbo.TBL_CategorieImpianti;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                CategoriaImpianto categoria = RiempiIstanza(dr);
                categorie.Add(categoria);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return categorie;
        }

        private CategoriaImpianto RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            CategoriaImpianto categoria = new CategoriaImpianto();

            categoria.ID = dr.GetInt32(0);
            categoria._nome_IT = dr.GetString(1);
            categoria._nome_EN = dr.GetString(2);
            categoria.FileIcona = dr.GetString(3);
            return categoria;
        }
    }
}
