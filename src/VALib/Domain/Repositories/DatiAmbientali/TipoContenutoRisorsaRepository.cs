using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using VALib.Domain.Entities.DatiAmbientali;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class TipoContenutoRisorsaRepository : Repository
    {
        private static readonly TipoContenutoRisorsaRepository _instance = new TipoContenutoRisorsaRepository(Settings.DivaWebConnectionString);
        private static readonly string _webCacheKey = "TipiContenutiRisorsa";

        private TipoContenutoRisorsaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoContenutoRisorsaRepository Instance
        {
            get { return _instance; }
        }

        public List<TipoContenutoRisorsa> RecuperaTipiContenutoRisorsa()
        {
            List<TipoContenutoRisorsa> elenchi = new List<TipoContenutoRisorsa>();

            elenchi = HttpContext.Current.Cache[_webCacheKey] as List<TipoContenutoRisorsa>;

            if (elenchi == null)
            {
                elenchi = RecuperaTipiContenutoRisorsaPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, elenchi, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, elenchi, TimeSpan.FromMinutes(15));
            }

            return elenchi;
        }

        public TipoContenutoRisorsa RecuperaTipoContenutoRisorsa(int id)
        {
            return RecuperaTipiContenutoRisorsa().FirstOrDefault(x => x.ID == id);
        }

        private List<TipoContenutoRisorsa> RecuperaTipiContenutoRisorsaPrivate()
        {
            List<TipoContenutoRisorsa> tipi = new List<TipoContenutoRisorsa>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT IDTipoContenutoRisorsa, Nome, Estensioni, FileIcona FROM dbo.TBLTipiContenutoRisorsa;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipoContenutoRisorsa elenco = RiempiIstanza(dr);

                tipi.Add(elenco);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            tipi.Add(new TipoContenutoRisorsa() { ID = 999, _nome_IT = "Progetto cartografico", Estensioni = "" });

            return tipi;
        }

        private TipoContenutoRisorsa RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipoContenutoRisorsa tipo = new TipoContenutoRisorsa();

            tipo.ID = dr.GetInt32(0);
            tipo._nome_IT = dr.IsDBNull(1) ? "" : dr.GetString(1);
            tipo._nome_EN = tipo._nome_IT;
            tipo.Estensioni = dr.IsDBNull(2) ? "" : dr.GetString(2);
            tipo.FileIcona = dr.IsDBNull(3) ? "" : dr.GetString(3);

            return tipo;
        }

    }
}
