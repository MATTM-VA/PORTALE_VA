using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class DocumentoElencoRepository : Repository
    {
        private static readonly DocumentoElencoRepository _instance = new DocumentoElencoRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "OggettiHome";

        private DocumentoElencoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static DocumentoElencoRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<DocumentoElenco> RecuperaDocumentiElenco(int macroTipoOggetto, int? oggettoProceduraID, int? raggruppamentoID, 
                                                                    string lingua, string testoRicerca, string orderBy, string orderDirection, 
                                                                    int startrowNum, int endRowNum, out int rows, bool? filtroData = false)
        {
            List<DocumentoElenco> documentiElenco = new List<DocumentoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaDocumenti";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", macroTipoOggetto);
            sseo.SqlParameters.AddWithValue("@OggettoProceduraID", oggettoProceduraID.HasValue ? (object)oggettoProceduraID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@RaggruppamentoID", raggruppamentoID.HasValue ? (object)raggruppamentoID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", string.IsNullOrWhiteSpace(orderBy) ? DBNull.Value : (object)orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", string.IsNullOrWhiteSpace(orderDirection) ? DBNull.Value : (object)orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@FiltroData", filtroData);


            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DocumentoElenco documento = new DocumentoElenco();

                documento.ID = dr.GetInt32(1);
                documento.Raggruppamento = RaggruppamentoRepository.Instance.RecuperaRaggruppamento(dr.GetInt32(2));
                documento.TipoFile = TipoFileRepository.Instance.RecuperaTipoFile(dr.GetInt32(3));
                documento.Titolo = dr.GetString(4);
                documento.CodiceElaborato = dr.GetString(5);
                documento.Scala = dr.IsDBNull(6) ? "-" : dr.GetString(6);
                documento.Dimensione = dr.GetInt32(7);
                documento._nome_IT = dr.GetString(8); // nome oggetto IT
                documento._nome_EN = dr.GetString(9); // nome oggetto EN
                documento.Data = dr.GetDateTime(10);
                documento.DataScadenzaPresentazioneOsservazioni = dr.IsDBNull(11) ? (DateTime?)null : dr.GetDateTime(11);
                documento.OggettoID = dr.GetInt32(12);
                documento.OggettoProceduraID = dr.GetInt32(13);

                documentiElenco.Add(documento);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documentiElenco;
        }

        
    }
}
