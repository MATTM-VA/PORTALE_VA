using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Repositories.UI
{
    public sealed class VoceDizionarioRepository : Repository
    {
        private static readonly VoceDizionarioRepository _instance = new VoceDizionarioRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "VociDizionario";

        private VoceDizionarioRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static VoceDizionarioRepository Instance
        {
            get { return _instance; }
        }

        public List<VoceDizionario> RecuperaVociDizionario()
        {
            List<VoceDizionario> vociDizionario = new List<VoceDizionario>();

            vociDizionario = this.CacheGet(_webCacheKey) as List<VoceDizionario>;

            if (vociDizionario == null)
            {
                vociDizionario = RecuperaVociDizionarioPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, vociDizionario, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, vociDizionario, TimeSpan.FromMinutes(15));
            }

            return vociDizionario;
        }

        public VoceDizionario RecuperaVoceDizionario(int id)
        {
            VoceDizionario voceDizionario = RecuperaVociDizionario().FirstOrDefault(x => (int)x.ID == id);
            
            return voceDizionario;
        }

        public VoceDizionario RecuperaVoceDizionario(string nome)
        {
            VoceDizionario voceDizionario = RecuperaVociDizionario().SingleOrDefault(x => x.Nome.ToLower().Equals(nome.ToLower()));
            
            return voceDizionario;
        }

        private List<VoceDizionario> RecuperaVociDizionarioPrivate()
        {
            List<VoceDizionario> vociDizionario = new List<VoceDizionario>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT VoceDizionarioID, Sezione, Nome, Valore_IT, Valore_EN FROM dbo.TBL_UI_VociDizionario;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                VoceDizionario voceDizionario = RiempiIstanza(dr);
                vociDizionario.Add(voceDizionario);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return vociDizionario;
        }

        private VoceDizionario RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            VoceDizionario voceDizionario = new VoceDizionario();

            voceDizionario.ID = dr.GetInt32(0);
            voceDizionario.Sezione = dr.GetString(1);
            voceDizionario.Nome = dr.GetString(2);
            voceDizionario._valore_IT = dr.GetString(3);
            voceDizionario._valore_EN = dr.GetString(4);

            return voceDizionario;
        }
    }
}
