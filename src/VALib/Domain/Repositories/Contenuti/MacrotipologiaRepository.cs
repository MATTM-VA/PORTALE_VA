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
    public sealed class MacrotipologiaRepository : Repository
    {
        private static readonly MacrotipologiaRepository _instance = new MacrotipologiaRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Macrotipologie";

        private MacrotipologiaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static MacrotipologiaRepository Instance
        {
            get { return _instance; }
        }

        public List<Macrotipologia> RecuperaMacrotipologie()
        {
            List<Macrotipologia> macrotipologie = new List<Macrotipologia>();

            macrotipologie = this.CacheGet(_webCacheKey) as List<Macrotipologia>;

            if (macrotipologie == null)
            {
                macrotipologie = RecuperaMacrotipologiePrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, macrotipologie, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, macrotipologie, TimeSpan.FromMinutes(15));
            }

            return macrotipologie;
        }

        public Macrotipologia RecuperaMacrotipologia(int id)
        {
            return RecuperaMacrotipologie().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<Macrotipologia> RecuperaMacrotipologiePrivate()
        {
            List<Macrotipologia> macrotipologie = new List<Macrotipologia>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT MacrotipologiaID, Nome_IT, Nome_EN FROM dbo.TBL_Macrotipologie;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Macrotipologia macrotipologia = RiempiIstanza(dr);
                macrotipologie.Add(macrotipologia);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return macrotipologie;
        }

        private Macrotipologia RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Macrotipologia macrotipologia = new Macrotipologia();

            macrotipologia.ID = dr.GetInt32(0);
            macrotipologia._nome_IT = dr.GetString(1);
            macrotipologia._nome_EN = dr.GetString(2);

            return macrotipologia;
        }
    }
}
