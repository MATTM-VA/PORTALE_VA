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
    public sealed class TipoEventoRepository : Repository
    {
        private static readonly TipoEventoRepository _instance = new TipoEventoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "TipiEventi";

        private TipoEventoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TipoEventoRepository Instance
        {
            get { return _instance; }
        }

        public List<GM_TipoEvento> RecuperaTipiEventi()
        {
            List<GM_TipoEvento> eventi = new List<GM_TipoEvento>();

            eventi = this.CacheGet(_webCacheKey) as List<GM_TipoEvento>;

            if (eventi == null)
            {
                eventi = RecuperaTipiEventiPrivate();

                this.CacheInsert(_webCacheKey, eventi, TimeSpan.FromMinutes(15));
            }

            return eventi;
        }

        public GM_TipoEvento RecuperaTipoEvento(int id)
        {
            return RecuperaTipiEventi().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<GM_TipoEvento> RecuperaTipiEventiPrivate()
        {
            List<GM_TipoEvento> eventi = new List<GM_TipoEvento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT TipoEventoID, Nome_IT, Nome_EN FROM dbo.GEMMA_AIAtblTipiEvento;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                GM_TipoEvento evento = RiempiIstanza(dr);
                eventi.Add(evento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return eventi;
        }

        private GM_TipoEvento RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            GM_TipoEvento evento = new GM_TipoEvento();

            evento.ID = dr.GetInt32(0);
            evento._nome_IT = dr.GetString(1);
            evento._nome_EN = !dr.IsDBNull(2) ? dr.GetString(2) : "";
            return evento;
        }
    }
}
