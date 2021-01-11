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
    public sealed class VariabileRepository : Repository
    {
        private static readonly VariabileRepository _instance = new VariabileRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Variabili";

        private VariabileRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static VariabileRepository Instance
        {
            get { return _instance; }
        }

        public List<Variabile> RecuperaVariabili()
        {
            List<Variabile> variabili = new List<Variabile>();

            variabili = this.CacheGet(_webCacheKey) as List<Variabile>;

            if (variabili == null)
            {
                variabili = RecuperaVariabiliPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, variabili, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, variabili, TimeSpan.FromMinutes(15));

            }

            return variabili;
        }

        public Variabile RecuperaVariabile(string chiave)
        {
            Variabile variabile = RecuperaVariabili().FirstOrDefault(x => x.Chiave.Trim() == chiave.Trim());
            
            return variabile;
        }

        private List<Variabile> RecuperaVariabiliPrivate()
        {
            List<Variabile> variabili = new List<Variabile>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT Chiave, Valore FROM dbo.TBL_UI_Variabili;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Variabile variabile = RiempiIstanza(dr);
                variabili.Add(variabile);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return variabili;
        }

        private Variabile RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Variabile variabile = new Variabile();

            variabile.Chiave = dr.GetString(0);
            variabile.Valore = dr.GetString(1);

            return variabile;
        }

        internal void SalvaVariabile(string chiave, string valore)
        {
            ModificaVariabile(chiave, valore);

            //RemoveCacheDependency(_webCacheKey);
            this.CacheRemove(_webCacheKey);
        }

        private void ModificaVariabile(string chiave, string valore)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "UPDATE dbo.TBL_UI_Variabili SET Valore = @Valore WHERE Chiave = @Chiave;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Valore", valore.Trim());
            sseo.SqlParameters.AddWithValue("@Chiave", chiave.Trim());

            SqlProvider.ExecuteNonQueryObject(sseo);
        }

        public void Elimina(string chiave)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.TBL_UI_Variabili WHERE Chiave = @chiave;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@chiave", chiave);

            SqlProvider.ExecuteNonQueryObject(sseo);

            //RemoveCacheDependency(_webCacheKey);
            this.CacheRemove(_webCacheKey);
        }
    }
}
