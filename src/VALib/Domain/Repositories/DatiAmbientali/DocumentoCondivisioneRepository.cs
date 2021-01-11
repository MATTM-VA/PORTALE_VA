using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.DatiAmbientali;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class DocumentoCondivisioneRepository : Repository
    {
        private static readonly DocumentoCondivisioneRepository _instance = new DocumentoCondivisioneRepository(Settings.DivaWebConnectionString);
        //private static readonly string _webCacheKey = "CategorieDocumentiCondivisione";

        private DocumentoCondivisioneRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static DocumentoCondivisioneRepository Instance
        {
            get { return _instance; }
        }


        public List<DocumentoCondivisione> RecuperaDocumentiCondivisione(string testo, string elencoID, int startrowNum, int endRowNum, out int rows)
        {
            
            List<DocumentoCondivisione> documentiCondivisione = new List<DocumentoCondivisione>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql, sSqlW = "";

            if (elencoID.Contains(","))
            {
                sSqlW = " WHERE S.IDElenco in (" + elencoID + ") AND ((R.Titolo LIKE @testo) OR (R.Soggetto LIKE @testo))";
            }                        
            else
            {
                sSqlW = " WHERE (S.IDElenco = " + elencoID + " OR " + elencoID + " IS NULL) AND ((R.Titolo LIKE @testo) OR (R.Soggetto LIKE @testo))";
            }

            sSql = @"SELECT * FROM (
                SELECT R.IDRisorsaCondivisione, R.IDTipoContenutoRisorsa, R.Titolo, R.Url, R.Soggetto, R.Scopo, 
                        R.Abstract, R.Lingua, R.Commenti, R.Origine, R.Riferimenti, R.ParoleChiave, R.Territori, 
                        R.DataValidita AS DataScadenza, R.DataPubblicazione, R.DataCreazione, R.Autore, R.ResponsabileMetadato, 
                        R.ResponsabilePubblicazione, R.Dimensione, ROW_NUMBER() 
                OVER(ORDER BY S.Ordine ASC) 
                ROWNUM 
                FROM dbo.TBLRisorseCondivisione AS R INNER JOIN 
                    dbo.STGRisorseElenchi AS S ON R.IDRisorsaCondivisione = S.IDRisorsa " + sSqlW             
                + ") R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum; SELECT COUNT(*) FROM dbo.TBLRisorseCondivisione AS R INNER JOIN dbo.STGRisorseElenchi AS S ON R.IDRisorsaCondivisione = S.IDRisorsa " + sSqlW + ";";
            

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@testo", string.Format("%{0}%", testo));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DocumentoCondivisione documento = RiempiIstanza(dr);
                documentiCondivisione.Add(documento);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documentiCondivisione;
        }

        public DocumentoCondivisione RecuperaDocumentoCondivisione(Guid id)
        {
            DocumentoCondivisione documento = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT R.IDRisorsaCondivisione, R.IDTipoContenutoRisorsa, R.Titolo, R.Url, R.Soggetto, R.Scopo, 
                                R.Abstract, R.Lingua, R.Commenti, R.Origine, R.Riferimenti, R.ParoleChiave, R.Territori, 
                                R.DataValidita AS DataScadenza, R.DataPubblicazione, R.DataCreazione, R.Autore, R.ResponsabileMetadato, 
                                R.ResponsabilePubblicazione, R.Dimensione
                            FROM dbo.TBLRisorseCondivisione AS R WHERE R.IDRisorsaCondivisione = @IDRisorsaCondivisione;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@IDRisorsaCondivisione", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                documento = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return documento;
        }

        private DocumentoCondivisione RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            DocumentoCondivisione d = new DocumentoCondivisione();

            d.ID = dr.GetGuid(0);
            d.TipoContenutoRisorsa = TipoContenutoRisorsaRepository.Instance.RecuperaTipoContenutoRisorsa(dr.GetInt32(1));
            d.Titolo = dr.GetString(2);
            d.Url = dr.IsDBNull(3) ? "" : dr.GetString(3);
            d.Soggetto = dr.IsDBNull(4) ? "" : dr.GetString(4);
            d.Scopo = dr.IsDBNull(5) ? "" : dr.GetString(5);
            d.Abstract = dr.IsDBNull(6) ? "" : dr.GetString(6);
            d.Lingua = dr.IsDBNull(7) ? "" : dr.GetString(7);
            d.Commenti = dr.IsDBNull(8) ? "" : dr.GetString(8);
            d.Origine = dr.IsDBNull(9) ? "" : dr.GetString(9);
            d.Riferimenti = dr.IsDBNull(10) ? "" : dr.GetString(10);
            d.ParoleChiave = dr.IsDBNull(11) ? "" : dr.GetString(11);
            d.Territori = dr.IsDBNull(12) ? "" : dr.GetString(12);
            d.DataScadenza = dr.IsDBNull(13) ? null : (DateTime?)dr.GetDateTime(13);
            d.DataPubblicazione = dr.IsDBNull(14) ? null : (DateTime?)dr.GetDateTime(14);
            d.DataCreazione = dr.IsDBNull(15) ? null : (DateTime?)dr.GetDateTime(15);
            d.Autore = dr.IsDBNull(16) ? "" : dr.GetString(16);
            d.ResponsabileMetadato = dr.IsDBNull(17) ? "" : dr.GetString(17);
            d.ResponsabilePubblicazione = dr.IsDBNull(18) ? "" : dr.GetString(18);
            d.Dimensione = dr.IsDBNull(19) ? null : (int?)dr.GetInt32(19);

            return d;
        }
    }
}
