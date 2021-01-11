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
    public sealed class TipoOggettoRepository : Repository
    {
        private static readonly TipoOggettoRepository _instance = new TipoOggettoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipiOggetto";

        private TipoOggettoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoOggettoRepository Instance
        {
            get { return _instance; }
        }

        public List<TipoOggetto> RecuperaTipiOggetto()
        {
            List<TipoOggetto> tipiOggetto = new List<TipoOggetto>();

            tipiOggetto = this.CacheGet(_webCacheKey) as List<TipoOggetto>;

            if (tipiOggetto == null)
            {
                tipiOggetto = RecuperaTipiOggettoPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, tipiOggetto, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipiOggetto, TimeSpan.FromMinutes(15));

            }

            return tipiOggetto;
        }

        public TipoOggetto RecuperaTipoOggetto(int id)
        {
            return RecuperaTipiOggetto().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<TipoOggetto> RecuperaTipiOggettoPrivate()
        {
            List<TipoOggetto> tipiOggetto = new List<TipoOggetto>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TipoOggettoID, MacroTipoOggettoID, Nome_IT, Nome_EN, Descrizione FROM dbo.TBL_TipiOggetto;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipoOggetto tipoOggetto = RiempiIstanza(dr);

                tipiOggetto.Add(tipoOggetto);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipiOggetto;
        }

        private TipoOggetto RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipoOggetto tipoOggetto = new TipoOggetto();

            tipoOggetto.ID = dr.GetInt32(0);
            tipoOggetto.MacroTipoOggetto = MacroTipoOggettoRepository.Instance.RecuperaMacroTipoOggetto(dr.GetInt32(1));
            tipoOggetto._nome_IT = dr.IsDBNull(2) ? "" : dr.GetString(2);
            tipoOggetto._nome_EN = dr.IsDBNull(3) ? "" : dr.GetString(3);
            tipoOggetto.Descrizione = dr.IsDBNull(4) ? "" : dr.GetString(4);
            tipoOggetto.Enum = (TipoOggettoEnum)dr.GetInt32(0);


            return tipoOggetto;
        }

    }
}
