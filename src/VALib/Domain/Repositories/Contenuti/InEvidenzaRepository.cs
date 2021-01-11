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
    public sealed class InEvidenzaRepository : Repository
    {
        private static readonly InEvidenzaRepository _instance = new InEvidenzaRepository(Settings.VAConnectionString);

        //private static readonly string _webCacheKey = "Settori";

        private InEvidenzaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static InEvidenzaRepository Instance
        {
            get { return _instance; }
        }

        public List<InEvidenza> RecuperaInEvidenza(int id)
        {
            List<InEvidenza> inEvidenzaList = new List<InEvidenza>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT idEvidenza, ordine, widgetID, NotiziaID FROM dbo.TBL_InEvidenza " +
                          " WHERE widgetID = " + id ;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);
            
            while (dr.Read())
            {
                InEvidenza retEvidenza = RiempiIstanza(dr);
                inEvidenzaList.Add(retEvidenza);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }
            
            return inEvidenzaList;
        }

        public List<Notizia> RecuperaAllInEvidenza(int WidgetID = 0)
        {
            List<Notizia> inEvidenzaList = new List<Notizia>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            
          
            string sSql = " SELECT  dbo.TBL_Notizie.NotiziaID, dbo.TBL_Notizie.ImmagineID, " +
                            " dbo.TBL_Notizie.Titolo_IT AS TitoloNotizia_IT, " +
                            " dbo.TBL_Notizie.Titolo_EN, TitoloBreve_IT, TitoloBreve_EN, dbo.TBL_Notizie.Abstract_IT, " +
                            " dbo.TBL_Notizie.Abstract_EN, dbo.TBL_Notizie.Testo_IT, dbo.TBL_Notizie.Testo_EN, " +
                            " dbo.TBL_Notizie.Data, dbo.TBL_Notizie.CategoriaNotiziaID " +                        
                            " FROM dbo.TBL_Notizie INNER JOIN " +
                            " dbo.TBL_UI_Widget ON dbo.TBL_Notizie.NotiziaID = dbo.TBL_UI_Widget.NotiziaID " +
                            " WHERE (dbo.TBL_Notizie.Pubblicata = 1) ";

            if (WidgetID != 0)
            {

                sSql = sSql + " AND  dbo.TBL_UI_Widget.WidgetID = " + WidgetID ;
            }

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Notizia retEvidenza = RiempiIstanzaNotizia(dr);
                inEvidenzaList.Add(retEvidenza);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return inEvidenzaList;
        }

        private Notizia RiempiIstanzaNotizia(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Notizia notizia = new Notizia();

            notizia.ID = dr.GetInt32(0);            
            notizia.ImmagineID = dr.GetInt32(1);            
            notizia.Titolo_IT = dr.GetString(2);
            notizia.Titolo_EN = dr.GetString(3);
            notizia.TitoloBreve_IT = dr.GetString(4);
            notizia.TitoloBreve_EN = dr.GetString(5);
            notizia.Abstract_IT = dr.GetString(6);
            notizia.Abstract_EN = dr.GetString(7);
            notizia.Testo_IT = dr.GetString(8);
            notizia.Testo_EN = dr.GetString(9);                        
            notizia.Immagini = ImmagineRepository.Instance.RecuperaImmaginiFiglio(notizia.ImmagineID);
            notizia.Data = dr.GetDateTime(10);
            notizia.Categoria = CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(dr.GetInt32(11));

            return notizia;
        }

       

        private InEvidenza RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            InEvidenza retEvidenza = new InEvidenza();

            retEvidenza.idEvidenza = dr.GetInt32(0);
            retEvidenza.ordine = dr.GetInt32(1);
            retEvidenza.widgetID = dr.GetInt32(2);
            retEvidenza.NotiziaID = dr.GetInt32(3);

            return retEvidenza;
        }


    }
}
