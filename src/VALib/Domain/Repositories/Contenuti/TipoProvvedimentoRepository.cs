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
using VALib.Domain.Repositories.UI;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class TipoProvvedimentoRepository : Repository
    {
        private static readonly TipoProvvedimentoRepository _instance = new TipoProvvedimentoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipiProvvedimenti";

        private TipoProvvedimentoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoProvvedimentoRepository Instance
        {
            get { return _instance; }
        }

        public List<TipoProvvedimento> RecuperaTipiProvvedimenti()
        {
            List<TipoProvvedimento> tipoProvvedimento = new List<TipoProvvedimento>();

            tipoProvvedimento = this.CacheGet(_webCacheKey) as List<TipoProvvedimento>;

            if (tipoProvvedimento == null)
            {
                tipoProvvedimento = RecuperaTipiProvvedimentiPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, tipoProvvedimento, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, tipoProvvedimento, TimeSpan.FromMinutes(15));
            }

            return tipoProvvedimento;
        }

        public TipoProvvedimento RecuperaTipoProvvedimento(int id)
        {
            return RecuperaTipiProvvedimenti().FirstOrDefault(x => x.ID == id);
        }

        public TipoProvvedimento RecuperaTipoProvvedimentoPerVoceMenu(int id)
        {
            return RecuperaTipiProvvedimenti().FirstOrDefault(x => x.VoceMenu.ID == id);
        }

        public TipoProvvedimento RecuperaTipoProvvedimento(string nome)
        {
            return RecuperaTipiProvvedimenti().FirstOrDefault(x => x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
        }

        private List<TipoProvvedimento> RecuperaTipiProvvedimentiPrivate()
        {
            List<TipoProvvedimento> tipiProvvedimenti = new List<TipoProvvedimento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT P.TipoProvvedimentoID, P.AreaTipoProvvedimentoID, P.Nome_IT, P.Nome_EN, P.Ordine, S.VoceMenuID 
                            FROM dbo.TBL_TipiProvvedimenti AS P 
                            INNER JOIN dbo.STG_UI_VociMenuTipiProvvedimenti AS S ON P.TipoProvvedimentoID = S.TipoProvvedimentoID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                TipoProvvedimento tipoProvvedimento = RiempiIstanza(dr);
                tipiProvvedimenti.Add(tipoProvvedimento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return tipiProvvedimenti;
        }

        private TipoProvvedimento RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            TipoProvvedimento tipoProvvedimento = new TipoProvvedimento();

            tipoProvvedimento.ID = dr.GetInt32(0);
            tipoProvvedimento.Area = AreaTipoProvvedimentoRepository.Instance.RecuperaAreaTipoProvvedimento(dr.GetInt32(1));
            tipoProvvedimento._nome_IT = dr.GetString(2);
            tipoProvvedimento._nome_EN = dr.GetString(3);
            tipoProvvedimento.Ordine = dr.GetInt32(4);
            tipoProvvedimento.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(dr.GetInt32(5));

            return tipoProvvedimento;
        }
    }
}
