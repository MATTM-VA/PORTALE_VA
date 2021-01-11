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
    public sealed class AttivitaIppcRepository : Repository
    {
        private static readonly AttivitaIppcRepository _instance = new AttivitaIppcRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "AttivitaIPPC";

        private AttivitaIppcRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static AttivitaIppcRepository Instance
        {
            get { return _instance; }
        }

        public List<AttivitaIPPC> RecuperaAttivitaIPPC()
        {
            List<AttivitaIPPC> settori = new List<AttivitaIPPC>();

            settori = this.CacheGet(_webCacheKey) as List<AttivitaIPPC>;

            if (settori == null)
            {
                settori = RecuperaAttivitaIPPCPrivate();
                
                this.CacheInsert(_webCacheKey, settori, TimeSpan.FromMinutes(15));
            }

            return settori;
        }

        public AttivitaIPPC RecuperaSingolaAttivitaIPPC(int id)
        {
            return RecuperaAttivitaIPPC().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<AttivitaIPPC> RecuperaAttivitaIPPCPrivate()
        {
            List<AttivitaIPPC> lista = new List<AttivitaIPPC>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = " SELECT AttivitaIppcID, case when len(Nome_IT) > 100 then SUBSTRING(Nome_IT, 1, 100) + '...' Else Nome_IT end as nome_it, " +
                " case when len(Nome_EN) > 100 then SUBSTRING(Nome_EN, 1, 100) + '...'  Else Nome_EN end as nome_en from[TBL_AttivitaIppc] order by Nome_IT, Nome_En; ";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                AttivitaIPPC attivita = RiempiIstanza(dr);
                lista.Add(attivita);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        private AttivitaIPPC RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            AttivitaIPPC attivita = new AttivitaIPPC();

            attivita.ID = dr.GetInt32(0);
            attivita._nome_IT = dr.GetString(1);
            attivita._nome_EN = dr.GetString(2);

            return attivita;
        }
    }
}
