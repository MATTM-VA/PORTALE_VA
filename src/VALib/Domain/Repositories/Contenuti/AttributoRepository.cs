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
    public sealed class AttributoRepository : Repository
    {
        private static readonly AttributoRepository _instance = new AttributoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Attributi";

        private AttributoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static AttributoRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<Attributo> RecuperaAttributi()
        {
            IEnumerable<Attributo> attributi = new List<Attributo>();

            attributi = this.CacheGet(_webCacheKey) as List<Attributo>;

            if (attributi == null)
            {
                attributi = RecuperaAttributiPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, attributi, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, attributi, TimeSpan.FromMinutes(15));

            }

            return attributi;
        }

        public IEnumerable<Attributo> RecuperaAttributi(int tipoAttributoID)
        {
            IEnumerable<Attributo> attributi = RecuperaAttributi();
            List<Attributo> attributiResult = new List<Attributo>();

            foreach (Attributo attributo in attributi)
            {
                if (attributo.TipoAttributo.ID == tipoAttributoID)
                    attributiResult.Add(attributo);
            }

            return attributiResult;
            
        }

        public Attributo RecuperaAttributo(int id)
        {
            return RecuperaAttributi().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<Attributo> RecuperaAttributiPrivate()
        {
            List<Attributo> tipiAttributi = new List<Attributo>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT AttributoID, TipoAttributoID, Nome_IT, Nome_EN, Ordine, MacroTipoOggettoID FROM dbo.TBL_Attributi;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Attributo attributo = RiempiIstanza(dr);

                tipiAttributi.Add(attributo);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipiAttributi;
        }

        private Attributo RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Attributo attributo = new Attributo();

            attributo.ID = dr.GetInt32(0);
            attributo.TipoAttributo = TipoAttributoRepository.Instance.RecuperaTipoAttributo(dr.GetInt32(1));
            attributo._nome_IT = dr.IsDBNull(2) ? "" : dr.GetString(2);
            attributo._nome_EN = dr.IsDBNull(3) ? "" : dr.GetString(3);
            attributo.Ordine = dr.GetInt32(4);
            attributo.MacroTipoOggetto = MacroTipoOggettoRepository.Instance.RecuperaMacroTipoOggetto(dr.GetInt32(5));

            return attributo;
        }

    }
}
