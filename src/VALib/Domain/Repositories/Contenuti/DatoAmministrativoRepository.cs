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
    public sealed class DatoAmministrativoRepository : Repository
    {
        private static readonly DatoAmministrativoRepository _instance = new DatoAmministrativoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "DatiAmministrativi";

        private DatoAmministrativoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static DatoAmministrativoRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<DatoAmministrativo> RecuperaDatiAmministrativi()
        {
            IEnumerable<DatoAmministrativo> datiAmministrativi = null;

            datiAmministrativi = this.CacheGet(_webCacheKey) as List<DatoAmministrativo>;

            if (datiAmministrativi == null)
            {
                datiAmministrativi = RecuperaDatiAmministrativiPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, datiAmministrativi, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, datiAmministrativi, TimeSpan.FromMinutes(15));
            }

            return datiAmministrativi;
        }

        public DatoAmministrativo RecuperaDatoAmministrativo(int id)
        {
            return RecuperaDatiAmministrativi().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<DatoAmministrativo> RecuperaDatiAmministrativiPrivate()
        {
            List<DatoAmministrativo> datiAmministrativi = new List<DatoAmministrativo>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT DatoAmministrativoID, Nome_IT, Nome_EN, TipoDati, Ordine FROM dbo.TBL_DatiAmministrativi;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DatoAmministrativo datoAmministrativo = RiempiIstanza(dr);

                datiAmministrativi.Add(datoAmministrativo);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return datiAmministrativi;
        }

        private DatoAmministrativo RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            DatoAmministrativo datoAmministrativo = new DatoAmministrativo();

            datoAmministrativo.ID = dr.GetInt32(0);
            datoAmministrativo._nome_IT = dr.IsDBNull(1) ? "" : dr.GetString(1);
            datoAmministrativo._nome_EN = dr.IsDBNull(2) ? "" : dr.GetString(2);
            datoAmministrativo.TipoDati = dr.GetString(3);
            datoAmministrativo.Ordine = dr.GetInt32(4);

            return datoAmministrativo;
        }

    }
}
