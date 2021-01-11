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
    public sealed class TipologiaRepository : Repository
    {
        private static readonly TipologiaRepository _instance = new TipologiaRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Tipologie";

        private TipologiaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipologiaRepository Instance
        {
            get { return _instance; }
        }

        public List<Tipologia> RecuperaTipologie()
        {
            List<Tipologia> tipologie = new List<Tipologia>();

            tipologie = this.CacheGet(_webCacheKey) as List<Tipologia>;

            if (tipologie == null)
            {
                tipologie = RecuperaTipologiePrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, tipologie, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipologie, TimeSpan.FromMinutes(15));
            }

            return tipologie;
        }

        public Tipologia RecuperaTipologia(int id)
        {
            return RecuperaTipologie().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<Tipologia> RecuperaTipologiePrivate()
        {
            List<Tipologia> tipologie = new List<Tipologia>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TipologiaID, Nome_IT, Nome_EN, MacroTipologiaID, FileIcona FROM dbo.TBL_Tipologie;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Tipologia tipologia = RiempiIstanza(dr);
                tipologie.Add(tipologia);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipologie;
        }

        private Tipologia RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Tipologia tipologia = new Tipologia();

            tipologia.ID = dr.GetInt32(0);
            tipologia._nome_IT = dr.GetString(1);
            tipologia._nome_EN = dr.GetString(2);
            tipologia.Macrotipologia = MacrotipologiaRepository.Instance.RecuperaMacrotipologia(dr.GetInt32(3));
            tipologia.FileIcona = dr.GetString(4);

            return tipologia;
        }
    }
}
