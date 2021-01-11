using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Caching;
using VALib.Web;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class ProvvedimentoHomeRepository : Repository
    {
        private static readonly ProvvedimentoHomeRepository _instance = new ProvvedimentoHomeRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "ProvvedimentiHome";

        private ProvvedimentoHomeRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ProvvedimentoHomeRepository Instance
        {
            get { return _instance; }
        }

        public List<List<ProvvedimentoHome>> RecuperaProvvedimentiHome()
        {
            List<List<ProvvedimentoHome>> provvedimentiHome = new List<List<ProvvedimentoHome>>();

            provvedimentiHome = this.CacheGet(_webCacheKey) as List<List<ProvvedimentoHome>>;

            if (provvedimentiHome == null)
            {
                provvedimentiHome = RecuperaProvvedimentiHomePrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, provvedimentiHome, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, provvedimentiHome, TimeSpan.FromMinutes(15));
            }

            return provvedimentiHome;
        }

        private List<List<ProvvedimentoHome>> RecuperaProvvedimentiHomePrivate()
        {
            List<List<ProvvedimentoHome>> provvedimentiHome = new List<List<ProvvedimentoHome>>(3);

            List<ProvvedimentoHome> provvedimentiHomeFigli1 = new List<ProvvedimentoHome>();
            List<ProvvedimentoHome> provvedimentiHomeFigli2 = new List<ProvvedimentoHome>();
            List<ProvvedimentoHome> provvedimentiHomeFigli3 = new List<ProvvedimentoHome>();
            List<ProvvedimentoHome> provvedimentiHomeFigli4 = new List<ProvvedimentoHome>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "dbo.SP_RecuperaProvvedimentiHome";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            // Valutazione Impatto Ambientale
            while (dr.Read())
            {
                ProvvedimentoHome provvedimentoHome = RiempiIstanza(dr);
                provvedimentiHomeFigli1.Add(provvedimentoHome);
            }

            provvedimentiHome.Add(provvedimentiHomeFigli1);

            // Verifica di assoggettabilità alla VIA
            dr.NextResult();

            while (dr.Read())
            {
                ProvvedimentoHome provvedimentoHome = RiempiIstanza(dr);
                provvedimentiHomeFigli2.Add(provvedimentoHome);
            }

            provvedimentiHome.Add(provvedimentiHomeFigli2);

            // Valutazione Ambientale Strategica
            dr.NextResult();

            while (dr.Read())
            {
                ProvvedimentoHome ProvvedimentoHome = RiempiIstanza(dr);
                provvedimentiHomeFigli3.Add(ProvvedimentoHome);
            }

            provvedimentiHome.Add(provvedimentiHomeFigli3);

            // AIA
            dr.NextResult();

            while (dr.Read())
            {
                ProvvedimentoHome ProvvedimentoHome = RiempiIstanza(dr);
                provvedimentiHomeFigli4.Add(ProvvedimentoHome);
            }

            provvedimentiHome.Add(provvedimentiHomeFigli4);


            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return provvedimentiHome;
        }

        private ProvvedimentoHome RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            ProvvedimentoHome provvedimentoHome = new ProvvedimentoHome();
            string immagineLocalizzazione = dr.IsDBNull(6) ? "" : dr.GetString(6);

            provvedimentoHome.ID = dr.GetInt32(0);
            provvedimentoHome._nome_IT = dr.GetString(1);
            provvedimentoHome._nome_EN = dr.GetString(2);
            provvedimentoHome._descrizione_IT = dr.GetString(3);
            provvedimentoHome._descrizione_EN = dr.GetString(4);
            provvedimentoHome.DataProvvedimento = dr.GetDateTime(5);
            provvedimentoHome.ProvvedimentoID = dr.GetInt32(7);
            provvedimentoHome.Tipologia = TipologiaRepository.Instance.RecuperaTipologia(dr.GetInt32(8));
            provvedimentoHome.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(9));

            provvedimentoHome.LinkLocalizzazione = LinkUtility.LinkLocalizzazione(immagineLocalizzazione, provvedimentoHome.ID);

            return provvedimentoHome;
        }

 
    }
}
