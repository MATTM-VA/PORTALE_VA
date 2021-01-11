using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Repositories.UI
{
    public sealed class WidgetCorrelatoRepository : Repository
    {
        private static readonly WidgetCorrelatoRepository _instance = new WidgetCorrelatoRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private WidgetCorrelatoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static WidgetCorrelatoRepository Instance
        {
            get { return _instance; }
        }

        public List<WidgetCorrelato> RecuperaWidgetCorrelati(int voceMenuID)
        {
            int c = 0;
            List<WidgetCorrelato> widget = new List<WidgetCorrelato>();
            List<Widget> widgetList = WidgetRepository.Instance.RecuperaWidget("", null);

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            
            sSql = "SELECT VoceMenuID, WidgetID, Ordine FROM dbo.STG_UI_VociMenuWidget WHERE VoceMenuID = @VoceMenuID ORDER BY Ordine ASC;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@VoceMenuID", voceMenuID);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                WidgetCorrelato dwidget = RiempiIstanza(dr, widgetList);
                widget.Add(dwidget);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return widget;
        }
        
        public void InserisciWidgetCorrelato(WidgetCorrelato widget)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.STG_UI_VociMenuWidget (VoceMenuID, WidgetID, Ordine) VALUES " +
                            "(@VoceMenuID, @WidgetID, @Ordine);";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@VoceMenuID", widget.VoceMenuID);
            sseo.SqlParameters.AddWithValue("@WidgetID", widget.WidgetID);
            sseo.SqlParameters.AddWithValue("@Ordine", widget.Ordine);

            SqlProvider.ExecuteNonQueryObject(sseo);
        }

        private WidgetCorrelato RiempiIstanza(SqlDataReader dr, List<Widget> widgetList)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            WidgetCorrelato widget = new WidgetCorrelato();

            widget.VoceMenuID = dr.GetInt32(0);
            widget.WidgetID = dr.GetInt32(1);
            widget.Ordine = dr.GetInt32(2);

            widget.Widget = widget.Widget = widgetList.Find(x => x.ID == widget.WidgetID);

            return widget;
        }

        public void Elimina(int voceMenuID)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.STG_UI_VociMenuWidget WHERE VoceMenuID = @VoceMenuID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@VoceMenuID", voceMenuID);

            SqlProvider.ExecuteNonQueryObject(sseo);
        }
    }
}
