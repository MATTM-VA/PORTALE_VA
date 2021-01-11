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
    public sealed class FaseProgettazioneRepository : Repository
    {
        private static readonly FaseProgettazioneRepository _instance = new FaseProgettazioneRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "FasiProgettazione";

        private FaseProgettazioneRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static FaseProgettazioneRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<FaseProgettazione> RecuperaFasiProgettazione()
        {
            IEnumerable<FaseProgettazione> fasiProgettazione = null;

            fasiProgettazione = this.CacheGet(_webCacheKey) as List<FaseProgettazione>;

            if (fasiProgettazione == null)
            {
                fasiProgettazione = RecuperaFasiProgettazionePrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, fasiProgettazione, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, fasiProgettazione, TimeSpan.FromMinutes(15));
            }

            return fasiProgettazione;
        }

        public FaseProgettazione RecuperaFaseProgettazione(int id)
        {
            return RecuperaFasiProgettazione().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<FaseProgettazione> RecuperaFasiProgettazionePrivate()
        {
            List<FaseProgettazione> fasiProgettazione = new List<FaseProgettazione>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT FaseProgettazioneID, Nome_IT, Nome_EN FROM dbo.TBL_FasiProgettazione;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                FaseProgettazione faseProgettazione = RiempiIstanza(dr);
                fasiProgettazione.Add(faseProgettazione);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return fasiProgettazione;
        }

        private FaseProgettazione RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            FaseProgettazione faseProgettazione = new FaseProgettazione();

            faseProgettazione.ID = dr.GetInt32(0);
            faseProgettazione._nome_IT = dr.GetString(1);
            faseProgettazione._nome_EN = dr.GetString(2);

            return faseProgettazione;
        }
    }
}
