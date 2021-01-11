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
    public sealed class EventoRepository : Repository
    {
        private static readonly EventoRepository _instance = new EventoRepository(Settings.VAConnectionString);

        private EventoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static EventoRepository Instance
        {
            get { return _instance; }
        }

        public List<GM_Evento> RecuperaEventi(int? OggettoID = null, TipoEventoEnum? TipoEvento = null)
        {
            List<GM_Evento> eventi = new List<GM_Evento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT E.EventoID, E.Nome_IT, E.Nome_EN, E.DataInizio, E.DataFine, E.TipoEventoID
                            FROM GEMMA_AIAtblEventi E
                        	  INNER JOIN GEMMA_AIAstgEventiOggetti EO ON EO.EventoID = E.EventoID
	                        WHERE (@OggettoID IS NULL OR EO.OggettoID = @OggettoID)
	                          AND (@TipoEventoID IS NULL OR E.TipoEventoID = @TipoEventoID)
                              AND E.EventoID IN (SELECT EventoID FROM GEMMA_AIAtblDocumenti WHERE LivelloVisibilita = 1)";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            sseo.SqlParameters.Add("@OggettoID", SqlDbType.Int).Value = OggettoID != null ? (object)OggettoID : DBNull.Value;
            sseo.SqlParameters.Add("@TipoEventoID", SqlDbType.Int).Value = TipoEvento.HasValue ? (object)TipoEvento.Value : DBNull.Value;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
                eventi.Add(RiempiIstanza(dr));

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return eventi;
        }

        public GM_Evento RecuperaEvento(int id)
        {
            return RecuperaEventi(null, null).FirstOrDefault(x => (int)x.ID == id);
        }


        private GM_Evento RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            GM_Evento evento = new GM_Evento();
            //E.EventoID, E.Nome_IT, E.Nome_EN, E.DataInizio, E.DataFine, E.TipoEventoID
            evento.ID = dr.GetInt32(0);
            evento._nome_IT = dr.GetString(1);
            evento._nome_EN = dr.IsDBNull(2) ? null : dr.GetString(2);
            evento.DataInizio = dr.IsDBNull(3) ? null : (DateTime?)dr.GetDateTime(3);
            evento.DataFine = dr.IsDBNull(4) ? null : (DateTime?)dr.GetDateTime(4);
            evento.TipoEvento = (TipoEventoEnum)dr.GetInt32(5);
            return evento;
        }
    }
}
