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
    public sealed class ProceduraRepository : Repository
    {
        private static readonly ProceduraRepository _instance = new ProceduraRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Procedure";

        private ProceduraRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ProceduraRepository Instance
        {
            get { return _instance; }
        }

        public List<Procedura> RecuperaProcedure()
        {
            List<Procedura> procedure = new List<Procedura>();

            procedure = this.CacheGet(_webCacheKey) as List<Procedura>;

            if (procedure == null)
            {
                procedure = RecuperaProcedurePrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, procedure, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));
                //HttpContext.Current.Cache.Insert(_webCacheKey, procedure, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, procedure, TimeSpan.FromMinutes(15));
            }

            return procedure;
        }

        public List<Procedura> RecuperaProcedure(MacroTipoOggettoEnum macroTipoOggetto)
        {
            return RecuperaProcedure().Where(x => x.MacroTipoOggettoEnum == macroTipoOggetto).ToList();
        }

        public Procedura RecuperaProcedura(int id)
        {
            return RecuperaProcedure().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<Procedura> RecuperaProcedurePrivate()
        {
            List<Procedura> procedure = new List<Procedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT ProceduraID, MacroTipoOggettoID, Nome_IT, Nome_EN, AmbitoProceduraID, Ordine FROM dbo.TBL_Procedure;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Procedura procedura = RiempiIstanza(dr);
                procedure.Add(procedura);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return procedure;
        }

        private Procedura RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Procedura procedura = new Procedura();

            procedura.ID = dr.GetInt32(0);
            procedura.MacroTipoOggettoEnum = (MacroTipoOggettoEnum)dr.GetInt32(1);
            procedura._nome_IT = dr.GetString(2);
            procedura._nome_EN = dr.GetString(3);
            procedura.AmbitoProcedura = AmbitoProceduraRepository.Instance.RecuperaAmbitoProcedura(dr.GetInt32(4));
            procedura.Ordine = dr.GetInt32(5);
            return procedura;
        }
    }
}
