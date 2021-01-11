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
    public sealed class StatoProceduraAIARepository : Repository
    {
        private static readonly StatoProceduraAIARepository _instance = new StatoProceduraAIARepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "StatiProceduraAIA";

        private StatoProceduraAIARepository(string connectionString)
            : base(connectionString)
        {

        }

        public static StatoProceduraAIARepository Instance
        {
            get { return _instance; }
        }

        public List<StatoProcedura> RecuperaStatiProceduraAIA()
        {
            List<StatoProcedura> statiProceduraVIPERA = new List<StatoProcedura>();

            statiProceduraVIPERA = this.CacheGet(_webCacheKey) as List<StatoProcedura>;

            if (statiProceduraVIPERA == null)
            {
                statiProceduraVIPERA = RecuperaStatiProceduraAIAPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, statiProceduraVIPERA, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, statiProceduraVIPERA, TimeSpan.FromMinutes(15));
            }

            return statiProceduraVIPERA;
        }

        public StatoProcedura RecuperaStatoProceduraAIA(int id)
        {
            return RecuperaStatiProceduraAIA().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<StatoProcedura> RecuperaStatiProceduraAIAPrivate()
        {
            List<StatoProcedura> statiProceduraAIA = new List<StatoProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT StatoAiaID, Nome_IT, Nome_EN FROM dbo.TBL_StatiProceduraAIA;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                StatoProcedura statoProceduraAIA = RiempiIstanza(dr);
                statiProceduraAIA.Add(statoProceduraAIA);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            statiProceduraAIA.Add(new StatoProcedura() { ID = 0, _nome_IT = "in corso", _nome_EN = "on going" });

            return statiProceduraAIA;
        }

        private StatoProcedura RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            StatoProcedura statoProceduraAIA = new StatoProcedura();

            statoProceduraAIA.ID = dr.GetInt32(0);
            statoProceduraAIA._nome_IT = dr.GetString(1);
            statoProceduraAIA._nome_EN = dr.GetString(2);

            return statoProceduraAIA;
        }
    }
}

