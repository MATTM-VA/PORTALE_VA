using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using VALib.Domain.Entities.DatiAmbientali;
using System.Data;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class RisorsaInEvidenzaRepository : Repository
    {
        private static readonly RisorsaInEvidenzaRepository _instance = new RisorsaInEvidenzaRepository(Settings.DivaWebConnectionString);
        private static readonly string _webCacheKey = "RisorseInEvidenza";

        private RisorsaInEvidenzaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RisorsaInEvidenzaRepository Instance
        {
            get { return _instance; }
        }

        public List<RisorsaInEvidenza> RecuperaRisorseInEvidenza()
        {
            List<RisorsaInEvidenza> risorse = new List<RisorsaInEvidenza>();

            risorse = this.CacheGet(_webCacheKey) as List<RisorsaInEvidenza>;

            if (risorse == null)
            {
                risorse = RecuperaRisorseInEvidenzaPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, risorse, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, risorse, TimeSpan.FromMinutes(15));
            }

            return risorse;
        }

        private List<RisorsaInEvidenza> RecuperaRisorseInEvidenzaPrivate()
        {
            List<RisorsaInEvidenza> risorse = new List<RisorsaInEvidenza>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "spRecuperaRisorseEvidenza";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                RisorsaInEvidenza risorsa = RiempiIstanza(dr);

                risorse.Add(risorsa);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return risorse;
        }

        private RisorsaInEvidenza RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            RisorsaInEvidenza risorsa = new RisorsaInEvidenza();

            risorsa.ID = dr.GetGuid(0);
            risorsa.FileIcona = dr.GetString(1);
            risorsa.Url = dr.GetString(2);
            risorsa.Titolo = dr.GetString(3);
            risorsa.Tipo = dr.GetString(6);
            risorsa.ServizioWMS = dr.GetString(13);
            risorsa.ServizioWFS = dr.GetString(14);
            risorsa.GoogleEarth = dr.GetString(15);
            risorsa.Descrizione = dr.GetString(16);
            risorsa.IDTipoContenutoRisorsa = dr.GetInt32(17);

            return risorsa;
        }

    }
}
