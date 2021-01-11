using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class AmbitoProceduraRepository : Repository
    {
        private static readonly AmbitoProceduraRepository _instance = new AmbitoProceduraRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "AmbitiProcedura";

        private AmbitoProceduraRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static AmbitoProceduraRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<AmbitoProcedura> RecuperaAmbitiProcedura()
        {
            IEnumerable<AmbitoProcedura> ambitiProcedura = new List<AmbitoProcedura>();

            ambitiProcedura = this.CacheGet(_webCacheKey) as List<AmbitoProcedura>;

            if (ambitiProcedura == null)
            {
                ambitiProcedura = RecuperaAmbitiProceduraPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, ambitiProcedura, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, ambitiProcedura, TimeSpan.FromMinutes(15));
            }

            return ambitiProcedura;
        }

        public AmbitoProcedura RecuperaAmbitoProcedura(int id)
        {
            return RecuperaAmbitiProcedura().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<AmbitoProcedura> RecuperaAmbitiProceduraPrivate()
        {
            List<AmbitoProcedura> ambitiProcedura = new List<AmbitoProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT AmbitoProceduraID, Nome_IT, Nome_EN, Ordine FROM dbo.TBL_AmbitiProcedure ORDER BY Ordine;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                AmbitoProcedura AmbitoProcedura = RiempiIstanza(dr);

                ambitiProcedura.Add(AmbitoProcedura);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return ambitiProcedura;
        }

        private AmbitoProcedura RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            AmbitoProcedura ambitoProcedura = new AmbitoProcedura();

            ambitoProcedura.ID = dr.GetInt32(0);
            ambitoProcedura._nome_IT = dr.IsDBNull(1) ? "" : dr.GetString(1);
            ambitoProcedura._nome_EN = dr.IsDBNull(2) ? "" : dr.GetString(2);
            ambitoProcedura.Ordine = dr.GetInt32(3);

            return ambitoProcedura;
        }

    }
}
