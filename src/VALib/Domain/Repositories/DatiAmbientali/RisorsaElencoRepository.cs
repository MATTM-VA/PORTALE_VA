using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using VALib.Domain.Entities.DatiAmbientali;
using System.Data;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class RisorsaElencoRepository : Repository
    {
        private static readonly RisorsaElencoRepository _instance = new RisorsaElencoRepository(Settings.DivaWebConnectionString);
       
        private RisorsaElencoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RisorsaElencoRepository Instance
        {
            get { return _instance; }
        }

        

        public List<RisorsaElenco> RecuperaRisorseElenco(int? idTema, string testo, string orderBy, int startrowNum, int endRowNum, out int rows)
        {
            List<RisorsaElenco> risorse = new List<RisorsaElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "spFTS_RicercaRisorse";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@stringaRicerca", testo);
            sseo.SqlParameters.AddWithValue("@criterio", 0);
            sseo.SqlParameters.AddWithValue("@tipo", "");
            sseo.SqlParameters.AddWithValue("@ID_GEN", idTema.HasValue ? (object)idTema.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@ID_CT", DBNull.Value);
            sseo.SqlParameters.AddWithValue("@livelloAccesso", 0);
            
            SqlParameter prm = new SqlParameter("@stringaRicercaModificata", SqlDbType.VarChar, 300);
            prm.Direction = ParameterDirection.Output;

            sseo.SqlParameters.Add(prm);

            dr = SqlProvider.ExecuteReaderObject(sseo);

          

                while (dr.Read())
                {
                    RisorsaElenco risorsa = RiempiIstanza(dr);

                    risorse.Add(risorsa);
                }
         
            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            rows = risorse.Count;

            return risorse.Skip(startrowNum).Take(endRowNum - startrowNum).ToList();
        }

        private RisorsaElenco RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            RisorsaElenco risorsa = new RisorsaElenco();

            risorsa.ID = dr.GetGuid(2);
            risorsa.TipoContenuto = TipoContenutoRisorsaRepository.Instance.RecuperaTipoContenutoRisorsa(dr.GetInt32(20));
            risorsa.Tema = TemaRepository.Instance.RecuperaTema(dr.GetInt32(10));
            risorsa.Titolo = dr.GetString(5);
            risorsa.Scala = dr.GetString(7);
            risorsa.Url = dr.IsDBNull(4) ? "" : dr.GetString(4);
            risorsa.UrlWms = dr.IsDBNull(15) ? "" : dr.GetString(15);
            risorsa.UrlGoogleEarth = dr.IsDBNull(17) ? "" : dr.GetString(17);
            risorsa.Tipo = dr.GetString(8);

            return risorsa;
        }

    }
}
