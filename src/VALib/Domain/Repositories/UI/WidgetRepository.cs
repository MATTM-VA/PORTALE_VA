using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.Contenuti;
using System.Web;

namespace VALib.Domain.Repositories.UI
{
    public sealed class WidgetRepository : Repository
    {
        private static readonly WidgetRepository _instance = new WidgetRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Widget";

        private WidgetRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static WidgetRepository Instance
        {
            get { return _instance; }
        }

        public List<Widget> RecuperaWidget(string testo, TipoWidget? tipo)
        {
            List<Widget> widget = new List<Widget>();

            widget = this.CacheGet(_webCacheKey) as List<Widget>;

            if (widget == null)
            {
                widget = RecuperaWidgetPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, widget, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, widget, TimeSpan.FromMinutes(15));
            }

            return widget;

        }

        private List<Widget> RecuperaWidgetPrivate()
        {
            List<Widget> widget = new List<Widget>();

            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";

            sSql = "SELECT W.WidgetID, W.TipoWidget, W.Nome_IT, W.Nome_EN, W.CategoriaNotiziaID, W.NumeroElementi, W.DataInserimento, W.DataUltimaModifica, W.VoceMenuID, W.Contenuto_IT, W.Contenuto_EN, W.MostraTitolo, W.NotiziaID FROM dbo.TBL_UI_Widget AS W;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            
            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Widget dwidget = RiempiIstanza(dr, vociMenu);
                widget.Add(dwidget);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return widget;
        }

        public List<Widget> RecuperaWidget(string testo, TipoWidget? tipo, int startrowNum, int endRowNum, out int rows)
        {
            List<Widget> widget = new List<Widget>();
            rows = 0;

            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            
            sSql = "SELECT * FROM (" +
                "SELECT W.WidgetID, W.TipoWidget, W.Nome_IT, W.Nome_EN, W.CategoriaNotiziaID, W.NumeroElementi, W.DataInserimento, W.DataUltimaModifica, W.VoceMenuID, W.Contenuto_IT, W.Contenuto_EN, W.MostraTitolo, W.NotiziaID, ROW_NUMBER() " +
                "OVER(ORDER BY DataInserimento DESC) " +
                "ROWNUM " +
                "FROM dbo.TBL_UI_Widget AS W WHERE (W.Nome_IT LIKE @testo) AND ((W.TipoWidget = @TipoWidget) OR (@TipoWidget IS NULL))" +
                ") " +
                "R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum order by Nome_IT;" +
                "SELECT COUNT(*) FROM dbo.TBL_UI_Widget AS W WHERE (W.Nome_IT LIKE @testo) AND ((W.TipoWidget = @TipoWidget) OR (@TipoWidget IS NULL));";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@testo", string.IsNullOrWhiteSpace(testo) ? "%%" : string.Format("%{0}%", testo));
            sseo.SqlParameters.AddWithValue("@TipoWidget", tipo.HasValue ? (object)((int)tipo) : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Widget dwidget = RiempiIstanza(dr, vociMenu);
                widget.Add(dwidget);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return widget;
        }

        public Widget RecuperaWidget(int id)
        {
            Widget Widget = null;

            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT W.WidgetID, W.TipoWidget, W.Nome_IT, W.Nome_EN, W.CategoriaNotiziaID, W.NumeroElementi, W.DataInserimento, W.DataUltimaModifica, W.VoceMenuID, W.Contenuto_IT, W.Contenuto_EN, W.MostraTitolo, W.NotiziaID FROM dbo.TBL_UI_Widget AS W WHERE WidgetID = @WidgetID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@WidgetID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Widget = RiempiIstanza(dr, vociMenu);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return Widget;
        }

        internal int SalvaWidget(Widget Widget)
        {
            int result = 0;

            if (Widget.IsNew)
                result = InserisciWidget(Widget);
            else
                result = ModificaWidget(Widget);

            //RemoveCacheDependency(_webCacheKey);
            this.CacheRemove(_webCacheKey);

            return result;
        }
        
        private int ModificaWidget(Widget widget)
        {
            int result = 0;
            
            ElogToolkit.Data.SqlServer.SqlServerProvider.SqlServerTransactionObject tran = SqlProvider.CreateTransactionObject();
            
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "UPDATE dbo.TBL_UI_Widget SET Nome_IT = @Nome_IT, Nome_EN = @Nome_EN, CategoriaNotiziaID = @CategoriaNotiziaID, " +
                            "NumeroElementi = @NumeroElementi, DataUltimaModifica = @DataUltimaModifica, VoceMenuID = @VoceMenuID, " +
                            "Contenuto_IT = @Contenuto_IT, Contenuto_EN = @Contenuto_EN, MostraTitolo = @MostraTitolo, " +
                            "NotiziaID = @NotiziaID " +
                            "WHERE WidgetID = @WidgetID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Nome_IT", widget.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", string.IsNullOrWhiteSpace(widget.Nome_EN) ? widget.Nome_IT : widget.Nome_EN);
            sseo.SqlParameters.AddWithValue("@CategoriaNotiziaID", widget.Categoria != null ? (object)widget.Categoria.ID : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@NumeroElementi", widget.NumeroElementi.HasValue ? (object)widget.NumeroElementi : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", widget.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@WidgetID", widget.ID);
            sseo.SqlParameters.AddWithValue("@VoceMenuID", widget.VoceMenuID.HasValue ? (object)widget.VoceMenuID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Contenuto_IT", string.IsNullOrWhiteSpace(widget.Contenuto_IT) ? DBNull.Value : (object)widget.Contenuto_IT);
            sseo.SqlParameters.AddWithValue("@Contenuto_EN", string.IsNullOrWhiteSpace(widget.Contenuto_EN) ? DBNull.Value : (object)widget.Contenuto_EN);
            sseo.SqlParameters.AddWithValue("@MostraTitolo", widget.MostraTitolo);
            sseo.SqlParameters.AddWithValue("@NotiziaID", widget.NotiziaID != null ? (object)widget.NotiziaID : DBNull.Value);

            SqlProvider.ExecuteNonQueryObject(sseo);


            
            result = widget.ID;

            return result;
        }
        
        private int InserisciWidget(Widget widget)
        {
            int idWidget = 0;

            ElogToolkit.Data.SqlServer.SqlServerProvider.SqlServerTransactionObject tran = SqlProvider.CreateTransactionObject();

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_UI_Widget (TipoWidget, Nome_IT, Nome_EN, CategoriaNotiziaID, NumeroElementi, DataInserimento, DataUltimaModifica, VoceMenuID, Contenuto_IT, Contenuto_EN, MostraTitolo, NotiziaID) VALUES " +
                            "(@TipoWidget, @Nome_IT, @Nome_EN, @CategoriaNotiziaID, @NumeroElementi, @DataInserimento, @DataUltimaModifica, @VoceMenuID, @Contenuto_IT, @Contenuto_EN, @MostraTitolo," +
                            "@NotiziaID);"+ 
             
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@TipoWidget", (int)widget.Tipo);
            sseo.SqlParameters.AddWithValue("@Nome_IT", widget.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", widget.Nome_EN);
            sseo.SqlParameters.AddWithValue("@CategoriaNotiziaID", widget.Categoria != null ? (object)widget.Categoria.ID : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@NumeroElementi", widget.NumeroElementi.HasValue ? (object)widget.NumeroElementi : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@DataInserimento", widget.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", widget.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@VoceMenuID", widget.VoceMenuID.HasValue ? (object)widget.VoceMenuID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Contenuto_IT", string.IsNullOrWhiteSpace(widget.Contenuto_IT) ? DBNull.Value : (object)widget.Contenuto_IT);
            sseo.SqlParameters.AddWithValue("@Contenuto_EN", string.IsNullOrWhiteSpace(widget.Contenuto_EN) ? DBNull.Value : (object)widget.Contenuto_EN);
            sseo.SqlParameters.AddWithValue("@MostraTitolo", widget.MostraTitolo);
            sseo.SqlParameters.AddWithValue("@NotiziaID", widget.NotiziaID != null ? (object)widget.NotiziaID : DBNull.Value);

            idWidget = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return idWidget;

            
        }

        private Widget RiempiIstanza(SqlDataReader dr, List<VoceMenu> vociMenu)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Widget widget = new Widget();

            widget.ID = dr.GetInt32(0);
            widget.Tipo = (TipoWidget)dr.GetInt32(1);
            widget.Nome_IT = dr.GetString(2);
            widget.Nome_EN = dr.GetString(3);
            widget.Categoria = dr.IsDBNull(4) ? null : CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(dr.GetInt32(4));
            widget.NumeroElementi = dr.IsDBNull(5) ? new int?() : dr.GetInt32(5);
            widget.DataInserimento = dr.GetDateTime(6);
            widget.DataUltimaModifica = dr.GetDateTime(7);
            widget.VoceMenuID = dr.IsDBNull(8) ? null : (int?)dr.GetInt32(8);
            if (widget.VoceMenuID.HasValue)
                widget.VoceMenu = vociMenu.Find(x => x.ID == widget.VoceMenuID.Value);
            widget.Contenuto_IT = dr.IsDBNull(9) ? null : dr.GetString(9);
            widget.Contenuto_EN = dr.IsDBNull(10) ? null : dr.GetString(10);
            widget.MostraTitolo = dr.GetBoolean(11);
            widget.NotiziaID = dr.IsDBNull(12) ? null : (int?)dr.GetInt32(12);
            return widget;
        }

      

        public void Elimina(int id)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.TBL_UI_Widget WHERE WidgetID = @WidgetID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@WidgetID", id);

            SqlProvider.ExecuteNonQueryObject(sseo);

         
            this.CacheRemove(_webCacheKey);
        }

        public void EliminaCache() {

            this.CacheReset();
        }
    }
}
