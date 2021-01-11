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
    public sealed class DocumentoPortaleRepository : Repository
    {
        private static readonly DocumentoPortaleRepository _instance = new DocumentoPortaleRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private DocumentoPortaleRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static DocumentoPortaleRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<DocumentoPortale> RecuperaDocumentiPortale(string testo, int startrowNum, int endRowNum, out int rows)
        {
            List<DocumentoPortale> documentiPortale = new List<DocumentoPortale>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            
            sSql = "SELECT * FROM (" +
                "SELECT D.DocumentoPortaleID, D.TipoFileID, D.Nome_IT, D.Nome_EN, D.NomeFileOriginale, D.DataInserimento, D.DataUltimaModifica, D.Dimensione, ROW_NUMBER() " +
                "OVER(ORDER BY DataInserimento DESC) " +
                "ROWNUM " +
                "FROM dbo.TBL_DocumentiPortale AS D WHERE (D.Nome_IT LIKE @testo)" +
                ") " +
                "R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;" +
                "SELECT COUNT(*) FROM dbo.TBL_DocumentiPortale AS D WHERE (D.Nome_IT LIKE @testo);";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@testo", string.IsNullOrWhiteSpace(testo) ? "%%" : string.Format("%{0}%", testo));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DocumentoPortale documentoPortale = RiempiIstanza(dr);
                documentiPortale.Add(documentoPortale);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documentiPortale;
        }

        public DocumentoPortale RecuperaDocumentoPortale(int id)
        {
            DocumentoPortale DocumentoPortale = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT D.DocumentoPortaleID, D.TipoFileID, D.Nome_IT, D.Nome_EN, D.NomeFileOriginale, D.DataInserimento, D.DataUltimaModifica, D.Dimensione FROM dbo.TBL_DocumentiPortale AS D WHERE DocumentoPortaleID = @DocumentoPortaleID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@DocumentoPortaleID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DocumentoPortale = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return DocumentoPortale;
        }

        internal int SalvaDocumentoPortale(DocumentoPortale DocumentoPortale)
        {
            int result = 0;

            if (DocumentoPortale.IsNew)
                result = InserisciDocumentoPortale(DocumentoPortale);
            else
                result = ModificaDocumentoPortale(DocumentoPortale);

            return result;
        }
        
        private int ModificaDocumentoPortale(DocumentoPortale DocumentoPortale)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "UPDATE dbo.TBL_DocumentiPortale SET TipoFileID = @TipoFileID, Nome_IT = @Nome_IT, Nome_EN = @Nome_EN, NomeFileOriginale = @NomeFileOriginale, " +
                            "DataUltimaModifica = @DataUltimaModifica, Dimensione = @Dimensione " +
                            "WHERE DocumentoPortaleID = @DocumentoPortaleID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@TipoFileID", DocumentoPortale.TipoFile.ID);
            sseo.SqlParameters.AddWithValue("@Nome_IT", DocumentoPortale.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", DocumentoPortale.Nome_EN);
            sseo.SqlParameters.AddWithValue("@NomeFileOriginale", DocumentoPortale.NomeFileOriginale);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", DocumentoPortale.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Dimensione", DocumentoPortale.Dimensione);
            sseo.SqlParameters.AddWithValue("@DocumentoPortaleID", DocumentoPortale.ID);

            SqlProvider.ExecuteNonQueryObject(sseo);

            result = DocumentoPortale.ID;

            return result;
        }
        
        private int InserisciDocumentoPortale(DocumentoPortale DocumentoPortale)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_DocumentiPortale (TipoFileID, Nome_IT, Nome_EN, NomeFileOriginale, DataInserimento, DataUltimaModifica, Dimensione) VALUES " +
                            "(@TipoFileID, @Nome_IT, @Nome_EN, @NomeFileOriginale, @DataInserimento, @DataUltimaModifica, @Dimensione);" +
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@TipoFileID", DocumentoPortale.TipoFile.ID);
            sseo.SqlParameters.AddWithValue("@Nome_IT", DocumentoPortale.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", DocumentoPortale.Nome_EN);
            sseo.SqlParameters.AddWithValue("@NomeFileOriginale", DocumentoPortale.NomeFileOriginale);
            sseo.SqlParameters.AddWithValue("@DataInserimento", DocumentoPortale.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", DocumentoPortale.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Dimensione", DocumentoPortale.Dimensione);

            result = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return result;
        }

        private DocumentoPortale RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            DocumentoPortale documentoPortale = new DocumentoPortale();

            documentoPortale.ID = dr.GetInt32(0);
            documentoPortale.TipoFile = TipoFileRepository.Instance.RecuperaTipoFile(dr.GetInt32(1));
            documentoPortale.Nome_IT = dr.GetString(2);
            documentoPortale.Nome_EN = dr.GetString(3);
            documentoPortale.NomeFileOriginale = dr.GetString(4);
            documentoPortale.DataInserimento = dr.GetDateTime(5);
            documentoPortale.DataUltimaModifica = dr.GetDateTime(6);
            documentoPortale.Dimensione = dr.GetInt32(7);

            return documentoPortale;
        }

        public void Elimina(int id)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.TBL_DocumentiPortale WHERE DocumentoPortaleID = @DocumentoPortaleID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@DocumentoPortaleID", id);

            SqlProvider.ExecuteNonQueryObject(sseo);
        }
    }
}
