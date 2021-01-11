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
    public sealed class MacroTipoOggettoRepository : Repository
    {
        private static readonly MacroTipoOggettoRepository _instance = new MacroTipoOggettoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "MacroTipiOggetto";

        private MacroTipoOggettoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static MacroTipoOggettoRepository Instance
        {
            get { return _instance; }
        }

        public List<MacroTipoOggetto> RecuperaMacroTipiOggetto()
        {
            List<MacroTipoOggetto> macroTipiOggetto = new List<MacroTipoOggetto>();

            macroTipiOggetto = this.CacheGet(_webCacheKey) as List<MacroTipoOggetto>;

            if (macroTipiOggetto == null)
            {
                macroTipiOggetto = RecuperaMacroTipiOggettoPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, macroTipiOggetto, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, macroTipiOggetto, TimeSpan.FromMinutes(15));
            }

            return macroTipiOggetto;
        }

        public MacroTipoOggetto RecuperaMacroTipoOggetto(int id)
        {
            return RecuperaMacroTipiOggetto().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<MacroTipoOggetto> RecuperaMacroTipiOggettoPrivate()
        {
            List<MacroTipoOggetto> macroTipiOggetto = new List<MacroTipoOggetto>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT MacroTipoOggettoID, Nome_IT, Nome_EN, NomeAbbreviato FROM dbo.TBL_MacroTipiOggetto;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                MacroTipoOggetto macroTipoOggetto = RiempiIstanza(dr);

                macroTipiOggetto.Add(macroTipoOggetto);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return macroTipiOggetto;
        }

        private MacroTipoOggetto RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            MacroTipoOggetto macroTipoOggetto = new MacroTipoOggetto();

            macroTipoOggetto.ID = dr.GetInt32(0);
            macroTipoOggetto._nome_IT = dr.IsDBNull(1) ? "" : dr.GetString(1);
            macroTipoOggetto._nome_EN = dr.IsDBNull(2) ? "" : dr.GetString(2);
            macroTipoOggetto.NomeAbbreviato = dr.IsDBNull(3) ? "" : dr.GetString(3);
            macroTipoOggetto.Enum = (MacroTipoOggettoEnum)dr.GetInt32(0);


            return macroTipoOggetto;
        }

    }
}
