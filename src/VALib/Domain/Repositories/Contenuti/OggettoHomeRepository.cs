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
    public sealed class OggettoHomeRepository : Repository
    {
        private static readonly OggettoHomeRepository _instance = new OggettoHomeRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "OggettiHome";

        private OggettoHomeRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static OggettoHomeRepository Instance
        {
            get { return _instance; }
        }

        public List<List<OggettoHome>> RecuperaOggettiHome()
        {
            List<List<OggettoHome>> oggettiHome = new List<List<OggettoHome>>();

            oggettiHome = this.CacheGet(_webCacheKey) as List<List<OggettoHome>>;

            if (oggettiHome == null)
            {
                oggettiHome = RecuperaOggettiHomePrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, oggettiHome, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, oggettiHome, TimeSpan.FromMinutes(15));
            }

            return oggettiHome;
        }
        
        private List<List<OggettoHome>> RecuperaOggettiHomePrivate()
        {
            List<List<OggettoHome>> oggettiHome = new List<List<OggettoHome>>(3);

            List<OggettoHome> oggettiHomeFigli1 = new List<OggettoHome>();
            /* SERIO - 27/11/17 */
            //List<OggettoHome> oggettiHomeFigli2 = new List<OggettoHome>();
            //List<OggettoHome> oggettiHomeFigli3 = new List<OggettoHome>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "dbo.SP_RecuperaOggettiHome";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            // Valutazione Impatto Ambientale
            while (dr.Read())
            {
                OggettoHome oggettoHome = RiempiIstanza(dr);
                oggettiHomeFigli1.Add(oggettoHome);
            }

            oggettiHome.Add(oggettiHomeFigli1);
 

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiHome;
        }

        private OggettoHome RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoHome oggettoHome = new OggettoHome();
            string immagineLocalizzazione = dr.IsDBNull(6) ? "" : dr.GetString(6);

            oggettoHome.ID = dr.GetInt32(0);
            oggettoHome._nome_IT = dr.GetString(1);
            oggettoHome._nome_EN = dr.GetString(2);
            oggettoHome._descrizione_IT = dr.GetString(3);
            oggettoHome._descrizione_EN = dr.GetString(4);
            oggettoHome.DataScadenzaPresentazione = dr.GetDateTime(5);
            oggettoHome.DocumentoID = dr.GetInt32(7);

            if (dr.GetInt32(9).Equals((int)TipoOggettoEnum.Impianto)) {
                oggettoHome.CategoriaImpianto = CategoriaImpiantoRepository.Instance.RecuperaCategoria(dr.GetInt32(11));
            }
            else {
                oggettoHome.Tipologia = TipologiaRepository.Instance.RecuperaTipologia(dr.GetInt32(8));
            }
            oggettoHome.ProponenteGestore = dr.IsDBNull(12) ? "" : dr.GetString(12);

            oggettoHome.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(9));

            oggettoHome.LinkLocalizzazione = LinkUtility.LinkLocalizzazione(immagineLocalizzazione, oggettoHome.ID);

            /* SERIO - 27/11/17 */
            oggettoHome.TipoElenco = (TipoElencoEnum) dr.GetInt32(10);
            
            return oggettoHome;
        }

    }
}
