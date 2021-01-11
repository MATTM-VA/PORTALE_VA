using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Repositories.UI
{
    public sealed class VoceMenuRepository : Repository
    {
        private static readonly VoceMenuRepository _instance = new VoceMenuRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "VociMenu";

        private VoceMenuRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static VoceMenuRepository Instance
        {
            get { return _instance; }
        }

        public List<VoceMenu> RecuperaVociMenu()
        {
            List<VoceMenu> vociMenu = new List<VoceMenu>();

            vociMenu = this.CacheGet(_webCacheKey) as List<VoceMenu>;

            if (vociMenu == null)
            {
                vociMenu = RecuperaVociMenuPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, vociMenu, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, vociMenu, TimeSpan.FromMinutes(15));
            }

            return vociMenu;
        }

        public List<VoceMenu> RecuperaVociMenuFrontEnd()
        {
            return RecuperaVociMenu().Where(x => x.VisibileFrontEnd).ToList();
        }

        public List<VoceMenu> RecuperaVociMenuFigliFrontEnd(int voceMenuID)
        {
            return RecuperaVociMenu().Where(x => x.GenitoreID == voceMenuID).ToList();
        }

        public VoceMenu RecuperaVoceMenu(int id)
        {
            return RecuperaVociMenu().FirstOrDefault(x => (int)x.ID == id);
        }

        public VoceMenu RecuperaVoceMenu(string voce)
        {
            return RecuperaVociMenu().FirstOrDefault(x => x.Voce.Equals(voce, StringComparison.CurrentCultureIgnoreCase));
        }

        private List<VoceMenu> RecuperaVociMenuPrivate()
        {
            List<VoceMenu> vociMenu = new List<VoceMenu>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT TOP (2000) VoceMenuID, GenitoreID, TipoMenu, Nome_IT, Nome_EN, Descrizione_IT, Descrizione_EN, 
                Sezione, Voce, Link, Editabile, VisibileFrontEnd, VisibileMappa, WidgetAbilitati, Ordine FROM dbo.TBL_UI_VociMenu ORDER BY Ordine ASC;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                VoceMenu voceMenu = RiempiIstanza(dr);
                vociMenu.Add(voceMenu);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return vociMenu;
        }

        private VoceMenu RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            VoceMenu voceMenu = new VoceMenu();

            voceMenu.ID = dr.GetInt32(0);
            voceMenu.GenitoreID = dr.GetInt32(1);
            voceMenu.TipoMenu = dr.GetInt32(2);
            voceMenu._nome_IT = dr.GetString(3);
            voceMenu._nome_EN = dr.GetString(4);
            voceMenu._descrizione_IT = dr.GetString(5);
            voceMenu._descrizione_EN = dr.GetString(6);
            voceMenu.Sezione = dr.GetString(7);
            voceMenu.Voce = dr.GetString(8);
            voceMenu.Link = dr.GetBoolean(9);
            voceMenu.Editabile = dr.GetBoolean(10);
            voceMenu.VisibileFrontEnd = dr.GetBoolean(11);
            voceMenu.VisibileMappa = dr.GetBoolean(12);
            voceMenu.WidgetAbilitati = dr.GetBoolean(13);
            voceMenu.Ordine = dr.GetInt32(14);

            return voceMenu;
        }

        public List<VoceMenu> RecuperaGenitori(VoceMenu voce)
        {
            List<VoceMenu> genitori = new List<VoceMenu>();
            genitori.Add(voce);
            VoceMenu genitore = RecuperaVoceMenu(voce.GenitoreID);
            
            while (genitore != null)
            {
                genitori.Add(genitore);
                genitore = RecuperaVoceMenu(genitore.GenitoreID);
            }

            VoceMenu voceHome = RecuperaVoceMenu(1);

            if (voceHome != null)
                genitori.Add(voceHome);

            genitori.Reverse();

            return genitori;
        }
    }
}
