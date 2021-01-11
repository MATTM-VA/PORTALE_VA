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
    public sealed class WebEventsRepository : Repository
    {
        private static readonly WebEventsRepository _instance = new WebEventsRepository(Settings.VAConnectionString);

        private WebEventsRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static WebEventsRepository Instance
        {
            get { return _instance; }
        }

        public List<WebEvent> RecuperaEventi(int? WebEventTypeID = null)
        {
            List<WebEvent> eventi = new List<WebEvent>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
 
            string sSql = @"WITH OrderedEvents AS
                            (
                                SELECT EventID,
                                ROW_NUMBER() OVER(ORDER BY EventTime DESC) AS 'RowNumber'
                                FROM TBL_WebEvents
                            )
                            SELECT E.EventID, EventTime,RequestUrl,UserHostAddress,ExceptionMessage
                            FROM TBL_WebEvents E
                            inner join OrderedEvents OE on OE.EventID = E.EventID
                            WHERE RowNumber < 250";

            sseo = new SqlServerExecuteObject();

            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
          
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

   
        private WebEvent RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            WebEvent evento = new WebEvent();
            evento.EventID = dr.GetGuid(0);
            evento.EventTime = dr.GetDateTime(1);
            evento.RequestUrl = dr.GetString(2);
            evento.UserHostAddress = dr.GetString(3);
            evento.ExceptionMessage = dr.IsDBNull(4) ? "" : dr.GetString(4);
            return evento;
        }
    }
}
