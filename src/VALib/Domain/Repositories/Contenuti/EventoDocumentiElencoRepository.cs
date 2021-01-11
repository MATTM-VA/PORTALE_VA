using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class EventoDocumentiElencoRepository : Repository
    {
        private static readonly EventoDocumentiElencoRepository _instance = new EventoDocumentiElencoRepository(Settings.VAConnectionString);

        private EventoDocumentiElencoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static EventoDocumentiElencoRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<DocumentoElenco> RecuperaDocumentiElenco(int EventoID, int? raggruppamentoID,
                                                                    string lingua, string testoRicerca, string orderBy, string orderDirection,
                                                                    int startrowNum, int endRowNum, out int rows, bool? filtroData = false)
        {
            List<DocumentoElenco> documentiElenco = new List<DocumentoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaDocumentiEvento";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@EventoID", EventoID);
            sseo.SqlParameters.AddWithValue("@RaggruppamentoID", raggruppamentoID.HasValue ? (object)raggruppamentoID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);


            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DocumentoElenco documento = new DocumentoElenco();

                documento.ID = dr.GetInt32(1);
                documento.Raggruppamento = RaggruppamentoEventiRepository.Instance.RecuperaRaggruppamento(dr.GetInt32(2));
                documento.TipoFile = getTipoFile(dr.GetString(8));
                documento.Titolo = dr.GetString(3);
                //documento.CodiceElaborato = dr.GetString(5);
                //documento.Scala = dr.IsDBNull(6) ? "-" : dr.GetString(6);
                documento.Dimensione = dr.GetInt32(4);
                //documento._nome_IT = dr.GetString(8); // nome oggetto IT
                //documento._nome_EN = dr.GetString(9); // nome oggetto EN
                documento.Data = dr.GetDateTime(5);
                //documento.DataScadenzaPresentazioneOsservazioni = dr.IsDBNull(11) ? (DateTime?)null : dr.GetDateTime(11);
                //documento.OggettoID = dr.GetInt32(12);
                //documento.OggettoProceduraID = dr.GetInt32(13);

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


        public DocumentoDownload RecuperaDocumentoDownload(int id)
        {
            DocumentoDownload documento = null;
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT D.DocumentoID, D.PercorsoFile 
                            FROM dbo.GEMMA_AIAtblDocumenti AS D 
                            WHERE D.LivelloVisibilita = 1 AND D.DocumentoID = @DocumentoID";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@DocumentoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            // Documento
            while (dr.Read())
            {
                documento = new DocumentoDownload();

                documento.ID = dr.GetInt32(0);
                //documento.OggettoID = dr.GetInt32(1);
                documento.PercorsoFile = dr.GetString(1);
                //documento.Estensione = getTipoFile(dr.GetString(3)).Estensione;
                //documento.OggettoProceduraID = dr.GetInt32(4);
                //documento.MacroTipoOggettoID = dr.GetInt32(5);
            }


            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documento;
        }

        private TipoFile getTipoFile(String f)
        {
            String ext = System.IO.Path.GetExtension(f).Replace(".", "");
            return TipoFileRepository.Instance.RecuperaTipiFile().First(x => x.Estensione.Equals(ext));
        }

    }
}
