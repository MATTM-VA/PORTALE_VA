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
    public sealed class DocumentoRepository : Repository
    {
        private static readonly DocumentoRepository _instance = new DocumentoRepository(Settings.VAConnectionString);

        private DocumentoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static DocumentoRepository Instance
        {
            get { return _instance; }
        }

        public Documento RecuperaDocumento(int id)
        {
            Documento documento = null;
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "dbo.SP_RecuperaDettaglioDocumento";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@DocumentoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            // Documento
            while (dr.Read())
            {
                documento = RiempiIstanzaDocumento(dr);
            }

            // Entita
            dr.NextResult();

            while (dr.Read())
            {
                EntitaCollegata entitaCollegata = new EntitaCollegata();

                entitaCollegata.Entita = new Entita(dr.GetInt32(0), dr.GetString(1), "","","",
                                                    "","","");

             
                entitaCollegata.Ruolo = RuoloEntitaRepository.Instance.RecuperaRuoloEntita(dr.GetInt32(2));

                documento.Entita.Add(entitaCollegata);
            }

            // Raggruppamenti
            dr.NextResult();

            while (dr.Read())
            {
                Raggruppamento raggruppamento = new Raggruppamento(dr.GetInt32(0), dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetInt32(4));

                documento.Raggruppamenti.Add(raggruppamento);
            }

            // Argomenti
            dr.NextResult();

            while (dr.Read())
            {
                Argomento argomento = new Argomento();

                argomento.ID = dr.GetGuid(0);
                argomento._nome_IT = dr.GetString(1);
                argomento._nome_EN = dr.IsDBNull(2) ? "" : dr.GetString(2);

                documento.Argomenti.Add(argomento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documento;
        }

        private Documento RiempiIstanzaDocumento(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Documento documento = new Documento();

            documento.ID = dr.GetInt32(0);
            documento.TipoFile = TipoFileRepository.Instance.RecuperaTipoFile(dr.GetInt32(1));
            documento.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(2));
            documento.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(3));
            documento.CodiceElaborato = dr.IsDBNull(4) ? "" : dr.GetString(4);
            documento.Titolo = dr.IsDBNull(5) ? "" : dr.GetString(5);
            documento.Descrizione = dr.IsDBNull(6) ? "" : dr.GetString(6);
            documento.Tipologia = dr.IsDBNull(7) ? "" : dr.GetString(7);
            documento.Scala = dr.IsDBNull(8) ? "" : dr.GetString(8);
            documento.Diritti = dr.IsDBNull(9) ? "" : dr.GetString(9);
            documento.LinguaDocumento = dr.IsDBNull(10) ? "" : dr.GetString(10);
            documento.Dimensione = dr.GetInt32(11);
            documento.OggettoID = dr.GetInt32(12);
            documento.NomeOggetto_IT = dr.GetString(13);
            documento.NomeOggetto_EN = dr.GetString(14);
            documento.Riferimenti = dr.IsDBNull(15) ? "" : dr.GetString(15);
            documento.Origine = dr.IsDBNull(16) ? "" : dr.GetString(16);
            documento.Copertura = dr.IsDBNull(17) ? "" : dr.GetString(17);
            documento.DataPubblicazione = dr.GetDateTime(18);
            documento.DataStesura = dr.GetDateTime(19);

            return documento;
        }

        public DocumentoDownload RecuperaDocumentoDownload(int id)
        {
            DocumentoDownload documento = null;
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT D.DocumentoID, OP.OggettoID, D.PercorsoFile, TF.Estensione, OP.OggettoProceduraID, T0.MacroTipoOggettoID
                            FROM dbo.TBL_Documenti AS D 
                                INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = D.OggettoProceduraID 
                                INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID 
                                INNER JOIN dbo.TBL_TipiOggetto AS T0 ON T0.TipoOggettoID = O.TipoOggettoID
                                INNER JOIN dbo.TBL_TipiFile AS TF ON TF.TipoFileID = D.TipoFileID
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
                documento.OggettoID = dr.GetInt32(1);
                documento.PercorsoFile = dr.GetString(2);
                documento.Estensione = dr.GetString(3);
                documento.OggettoProceduraID = dr.GetInt32(4);
                documento.MacroTipoOggettoID = dr.GetInt32(5);
            }


            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documento;
        }

        public IEnumerable<DocumentoDownload> RecuperaDocumentiDownloadPerProvvedimento(int provvedimentoID)
        {
            List<DocumentoDownload> documenti = new List<DocumentoDownload>();
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT D.DocumentoID, OP.OggettoID, D.PercorsoFile, TF.Estensione, OP.OggettoProceduraID, T0.MacroTipoOggettoID
                            FROM dbo.TBL_Documenti AS D 
                                INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = D.OggettoProceduraID 
                                INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID 
                                INNER JOIN dbo.TBL_TipiOggetto AS T0 ON T0.TipoOggettoID = O.TipoOggettoID
                                INNER JOIN dbo.TBL_TipiFile AS TF ON TF.TipoFileID = D.TipoFileID 
                                INNER JOIN dbo.STG_ProvvedimentiDocumenti AS SPD ON SPD.DocumentoID = D.DocumentoID
                            WHERE D.LivelloVisibilita = 1 AND SPD.ProvvedimentoID = @ProvvedimentoID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@ProvvedimentoID", provvedimentoID);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            // Documento
            while (dr.Read())
            {
                DocumentoDownload documento = new DocumentoDownload();

                documento.ID = dr.GetInt32(0);
                documento.OggettoID = dr.GetInt32(1);
                documento.PercorsoFile = dr.GetString(2);
                documento.Estensione = dr.GetString(3);
                documento.OggettoProceduraID = dr.GetInt32(4);
                documento.MacroTipoOggettoID = dr.GetInt32(5);

                documenti.Add(documento);
            }


            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documenti;
        }
    }
}
