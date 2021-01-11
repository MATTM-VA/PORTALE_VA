using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class TipoAttributoRepository : Repository
    {
        private static readonly TipoAttributoRepository _instance = new TipoAttributoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipiAttributi";

        private TipoAttributoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoAttributoRepository Instance
        {
            get { return _instance; }
        }

        public List<TipoAttributo> RecuperaTipiAttributi()
        {
            List<TipoAttributo> tipiAttributo = new List<TipoAttributo>();

            tipiAttributo = this.CacheGet(_webCacheKey) as List<TipoAttributo>;

            if (tipiAttributo == null)
            {
                tipiAttributo = RecuperaTipiAttributiPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, tipiAttributo, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipiAttributo, TimeSpan.FromMinutes(15));
            }

            return tipiAttributo;
        }

        public TipoAttributo RecuperaTipoAttributo(int id)
        {
            return RecuperaTipiAttributi().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<TipoAttributo> RecuperaTipiAttributiPrivate()
        {
            List<TipoAttributo> tipiAttributi = new List<TipoAttributo>();
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TA.TipoAttributoID, TA.Nome_IT, TA.Nome_EN, TA.Ordine, VMTA.VoceMenuID FROM dbo.TBL_TipiAttributi AS TA INNER JOIN STG_UI_VociMenuTipiAttributi AS VMTA ON TA.TipoAttributoID = VMTA.TipoAttributoID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipoAttributo tipoAttributo = RiempiIstanza(dr, vociMenu);

                tipiAttributi.Add(tipoAttributo);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipiAttributi;
        }

        private TipoAttributo RiempiIstanza(SqlDataReader dr, List<VoceMenu> vociMenu)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipoAttributo tipoAttributo = new TipoAttributo();
            VoceMenu voceMenu = vociMenu.FirstOrDefault(x => x.ID == dr.GetInt32(4));

            tipoAttributo.ID = dr.GetInt32(0);
            tipoAttributo._nome_IT = dr.IsDBNull(1) ? "" : dr.GetString(1);
            tipoAttributo._nome_EN = dr.IsDBNull(2) ? "" : dr.GetString(2);
            tipoAttributo.Ordine = dr.GetInt32(3);
            tipoAttributo.VoceMenu = voceMenu;

            return tipoAttributo;
        }

    }
}
