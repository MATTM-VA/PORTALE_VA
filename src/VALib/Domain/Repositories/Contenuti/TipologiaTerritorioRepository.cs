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
    public sealed class TipologiaTerritorioRepository : Repository
    {
        private static readonly TipologiaTerritorioRepository _instance = new TipologiaTerritorioRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipologieTerritorio";

        private TipologiaTerritorioRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipologiaTerritorioRepository Instance
        {
            get { return _instance; }
        }

        public List<TipologiaTerritorio> RecuperaTipologieTerritorio()
        {
            List<TipologiaTerritorio> tipologieTerritorio = new List<TipologiaTerritorio>();

            tipologieTerritorio = this.CacheGet(_webCacheKey) as List<TipologiaTerritorio>;

            if (tipologieTerritorio == null)
            {
                tipologieTerritorio = RecuperaTipologieTerritorioPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, tipologieTerritorio, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipologieTerritorio, TimeSpan.FromMinutes(15));
            }

            return tipologieTerritorio;
        }

        public TipologiaTerritorio RecuperaTipologiaTerritorio(int id)
        {
            return RecuperaTipologieTerritorio().FirstOrDefault(x => (int)x.ID == id);
        }

        public List<TipologiaTerritorio> RecuperaTipologieTerritorioPerRicerca()
        {
            return RecuperaTipologieTerritorio().Where(x => x.ID > 1 && x.ID < 5).ToList();
        }
        
        private List<TipologiaTerritorio> RecuperaTipologieTerritorioPrivate()
        {
            List<TipologiaTerritorio> tipologieTerritorio = new List<TipologiaTerritorio>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TipologiaTerritorioID, Nome, Nome_EN, MostraRicerca FROM dbo.TBL_TipologieTerritorio;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipologiaTerritorio tipologiaTerritorio = RiempiIstanza(dr);
                tipologieTerritorio.Add(tipologiaTerritorio);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }
            
            return tipologieTerritorio;
        }

        private TipologiaTerritorio RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipologiaTerritorio tipologiaTerritorio = new TipologiaTerritorio();

            tipologiaTerritorio.ID = dr.GetInt32(0);
            tipologiaTerritorio._nome_IT = dr.GetString(1);
            tipologiaTerritorio._nome_EN = dr.GetString(2);
            tipologiaTerritorio.Enum = (TipologiaTerritorioEnum)dr.GetInt32(0);
            tipologiaTerritorio.MostraRicerca = dr.GetBoolean(3);

            return tipologiaTerritorio;
        }
    }
}
