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
    public sealed class RaggruppamentoEventiRepository : Repository
    {
        private static readonly RaggruppamentoEventiRepository _instance = new RaggruppamentoEventiRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "RaggruppamentiEventi";

        private RaggruppamentoEventiRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RaggruppamentoEventiRepository Instance
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

            string sSql = "SELECT RaggruppamentoID, GenitoreID, Nome_IT, Nome_EN, LivelloVisibilita, Ordine FROM dbo.GEMMA_AIAtblRaggruppamenti;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Raggruppamento raggruppamento = new Raggruppamento(dr.GetInt32(0),
                                                                    dr.IsDBNull(1) ? 0 : dr.GetInt32(1),
                                                                    dr.GetString(2),
                                                                    dr.IsDBNull(3) ? "" : dr.GetString(3),
                                                                    dr.GetInt32(5));

                raggruppamenti.Add(raggruppamento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return raggruppamenti;
        }


        public List<Raggruppamento> RecuperaRaggruppamentiPerEventoID(int EventoID)
        {
            List<Raggruppamento> raggruppamenti = new List<Raggruppamento>();
            string webCacheKey = string.Format("RaggruppamentiEvento_{0}", EventoID);

            raggruppamenti = HttpContext.Current.Cache[webCacheKey] as List<Raggruppamento>;

            if (raggruppamenti == null)
            {
                raggruppamenti = RecuperaRaggruppamentiPerEventoIDPrivate(EventoID);
                //HttpContext.Current.Cache.Insert(webCacheKey, raggruppamenti, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 15));

                this.CacheInsert(_webCacheKey, raggruppamenti, TimeSpan.FromMinutes(15));
            }

            return raggruppamenti;
        }

        private List<Raggruppamento> RecuperaRaggruppamentiPerEventoIDPrivate(int EventoID)
        {
            List<Raggruppamento> raggruppamenti = new List<Raggruppamento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SP_RecuperaRaggruppamentiDocumentazioneEvento";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@EventoID", EventoID);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Raggruppamento raggruppamento = new Raggruppamento(dr.IsDBNull(0) ? 0 : dr.GetInt32(0),
                                                                    dr.IsDBNull(1) ? 0 : dr.GetInt32(1),
                                                                    dr.GetString(2),
                                                                    dr.IsDBNull(3) ? "" : dr.GetString(3),
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


    }
}
