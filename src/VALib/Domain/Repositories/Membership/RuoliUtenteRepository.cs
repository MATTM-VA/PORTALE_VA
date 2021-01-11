using ElogToolkit.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using VALib.Configuration;
using VALib.Domain.Common;
using VALib.Domain.Entities.Membership;

namespace VALib.Domain.Repositories.Membership
{
    public class RuoliUtenteRepository : Repository
    {
        private static readonly RuoliUtenteRepository _instance = new RuoliUtenteRepository(Settings.VAConnectionString);

        private RuoliUtenteRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RuoliUtenteRepository Instance
        {
            get { return _instance; }
        }

        private RuoloUtente RiempiIstanzaRuoloUtente(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            RuoloUtente ruoloUtente = new RuoloUtente();
            ruoloUtente.RuoloUtenteID=dr.GetInt32(0);
            ruoloUtente.Codice=dr.GetString(1);
            ruoloUtente.Nome=dr.GetString(2);

            return ruoloUtente;
        }

        public RuoloUtente RecuperaRuoloUtente(int ruoloID)
        {
            RuoloUtente ruoloUtente = RecuperaTuttiRuoliUtente().SingleOrDefault(x => x.RuoloUtenteID == ruoloID);
            return ruoloUtente;
        }

        public List<RuoloUtente> RecuperaTuttiRuoliUtente()
        {
            List<RuoloUtente> listaRuoli = null;
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "ruoliUtente";

            listaRuoli = cache[cacheKey] as List<RuoloUtente>;

            if (listaRuoli == null)
            {

                sseo = new SqlServerExecuteObject();
                sseo.CommandText = @"SELECT RuoloUtenteID, Codice, Nome FROM TBL_RuoliUtente";
                dr = SqlProvider.ExecuteReaderObject(sseo);

                listaRuoli = new List<RuoloUtente>();

                while (dr.Read())
                {
                    RuoloUtente ruolo = RiempiIstanzaRuoloUtente(dr);
                    listaRuoli.Add(ruolo);
                }

                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
            return listaRuoli;
        }

    }
}
