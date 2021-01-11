using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class RaggruppamentoRepository : Repository
    {
        private static readonly RaggruppamentoRepository _instance = new RaggruppamentoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "Raggruppamenti";

        private RaggruppamentoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RaggruppamentoRepository Instance
        {
            get { return _instance; }
        }

        public List<Raggruppamento> RecuperaRaggruppamenti()
        {
            List<Raggruppamento> raggruppamenti = new List<Raggruppamento>();

            raggruppamenti = this.CacheGet(_webCacheKey) as List<Raggruppamento>;

            if (raggruppamenti == null)
            {
                raggruppamenti = RecuperaRaggruppamentiPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, raggruppamenti, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, raggruppamenti, TimeSpan.FromMinutes(15));
            }

            return raggruppamenti;
        }

        public Raggruppamento RecuperaRaggruppamento(int id)
        {
            return RecuperaRaggruppamenti().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<Raggruppamento> RecuperaRaggruppamentiPrivate()
        {
            List<Raggruppamento> raggruppamenti = new List<Raggruppamento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT RaggruppamentoID, GenitoreID, MacroTipoOggettoID, Nome_IT, Nome_EN, LivelloVisibilita, Ordine FROM dbo.TBL_Raggruppamenti;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Raggruppamento raggruppamento = new Raggruppamento(dr.GetInt32(0),
                                                                    dr.GetInt32(1),
                                                                    dr.GetString(3),
                                                                    dr.GetString(4),
                                                                    dr.GetInt32(6));

                raggruppamenti.Add(raggruppamento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return raggruppamenti;
        }

        #region Documentazione oggetto
        public List<Raggruppamento> RecuperaRaggruppamentiPerOggettoProceduraID(int oggettoProceduraID)
        {
            List<Raggruppamento> raggruppamenti = new List<Raggruppamento>();
            string webCacheKey = string.Format("RaggruppamentiOggettoProcedura_{0}", oggettoProceduraID);

            raggruppamenti = HttpContext.Current.Cache[webCacheKey] as List<Raggruppamento>;

            if (raggruppamenti == null)
            {
                raggruppamenti = RecuperaRaggruppamentiPerOggettoProceduraIDPrivate(oggettoProceduraID);
                //HttpContext.Current.Cache.Insert(webCacheKey, raggruppamenti, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 15));

                this.CacheInsert(_webCacheKey, raggruppamenti, TimeSpan.FromMinutes(15));

            }

            return raggruppamenti;
        }

        private List<Raggruppamento> RecuperaRaggruppamentiPerOggettoProceduraIDPrivate(int oggettoProceduraID)
        {
            List<Raggruppamento> raggruppamenti = new List<Raggruppamento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SP_RecuperaRaggruppamentiDocumentazione";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@OggettoProceduraID", oggettoProceduraID);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Raggruppamento raggruppamento = new Raggruppamento(dr.GetInt32(0),
                                                                    dr.GetInt32(1),
                                                                    dr.GetString(2),
                                                                    dr.GetString(3),
                                                                    dr.GetInt32(4));
                raggruppamento.Figli = dr.GetInt32(5);

                raggruppamenti.Add(raggruppamento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return raggruppamenti;
        }
        #endregion

    }
}
