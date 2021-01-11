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
    public sealed class StatoProceduraVIPERARepository : Repository
    {
        private static readonly StatoProceduraVIPERARepository _instance = new StatoProceduraVIPERARepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "StatiProceduraVIPERA";

        private StatoProceduraVIPERARepository(string connectionString)
            : base(connectionString)
        {

        }

        public static StatoProceduraVIPERARepository Instance
        {
            get { return _instance; }
        }

        public List<StatoProcedura> RecuperaStatiProceduraVIPERA()
        {
            List<StatoProcedura> statiProceduraVIPERA = new List<StatoProcedura>();

            statiProceduraVIPERA = this.CacheGet(_webCacheKey) as List<StatoProcedura>;

            if (statiProceduraVIPERA == null)
            {
                statiProceduraVIPERA = RecuperaStatiProceduraVIPERAPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, statiProceduraVIPERA, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, statiProceduraVIPERA, TimeSpan.FromMinutes(15));
            }

            return statiProceduraVIPERA;
        }

        public StatoProcedura RecuperaStatoProceduraVIPERA(int id)
        {
            return RecuperaStatiProceduraVIPERA().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<StatoProcedura> RecuperaStatiProceduraVIPERAPrivate()
        {
            List<StatoProcedura> statiProceduraVIPERA = new List<StatoProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT ProSDeId, Nome_IT, Nome_EN FROM dbo.TBL_StatiProceduraVIPERA;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                StatoProcedura statoProceduraVIPERA = RiempiIstanza(dr);
                statiProceduraVIPERA.Add(statoProceduraVIPERA);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            statiProceduraVIPERA.Add(new StatoProcedura() { ID = 0, _nome_IT = "in corso", _nome_EN = "on going" });

            return statiProceduraVIPERA;
        }

        private StatoProcedura RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            StatoProcedura statoProceduraVIPERA = new StatoProcedura();

            statoProceduraVIPERA.ID = dr.GetInt32(0);
            statoProceduraVIPERA._nome_IT = dr.GetString(1);
            statoProceduraVIPERA._nome_EN = dr.GetString(2);

            return statoProceduraVIPERA;
        }
    }
}
