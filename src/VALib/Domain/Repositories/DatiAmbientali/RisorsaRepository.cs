using System;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.DatiAmbientali;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class RisorsaRepository : Repository
    {
        private static readonly RisorsaRepository _instance = new RisorsaRepository(Settings.DivaWebConnectionString);
        //private static readonly string _webCacheKey = "CategorieDocumentiCondivisione";

        private RisorsaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RisorsaRepository Instance
        {
            get { return _instance; }
        }

 

        public Risorsa RecuperaRisorsa(Guid id)
        {
            Risorsa risorsa = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT R.IDRisorsa, R.IDTipoContenutoRisorsa, R.Titolo, R.Url, R.Soggetto, 
                                R.DataPubblicazione, R.DataCreazione, R.DataValidita AS DataScadenza, 
                                R.Abstract, R.Origine, R.Commenti, R.Scopo, R.ParoleChiave, R.Riferimenti, 
                                R.Diritti, dbo.fnDocumentiConcatenaArgomenti(R.IDRisorsa) 
                            FROM dbo.TBLRisorse AS R WHERE R.IDRisorsa = @IDRisorsa;
                            SELECT T.Nome AS NomeAssociazione, E.Nome 
                            FROM dbo.TBLTipiAsscociazioneEntita AS T 
                                INNER JOIN dbo.STGRisorseEntita AS S ON S.IDTipoAssociazioneEntita = T.IDTipoAssociazioneEntita 
                                INNER JOIN dbo.TBLEntita AS E ON E.IDEntita = S.IDEntita
                            WHERE S.IDRisorsa = @IDRisorsa;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@IDRisorsa", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                risorsa = RiempiIstanza(dr);
            }

            dr.NextResult();

            while (dr.Read())
            {
                risorsa.Entita.Add(new Tuple<string, string>(dr.GetString(0), dr.GetString(1)));
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return risorsa;
        }

        private Risorsa RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Risorsa d = new Risorsa();

            d.ID = dr.GetGuid(0);
            d.TipoContenutoRisorsa = TipoContenutoRisorsaRepository.Instance.RecuperaTipoContenutoRisorsa(dr.GetInt32(1));
            d.Titolo = dr.GetString(2);
            d.Url = dr.IsDBNull(3) ? "" : dr.GetString(3);
            d.Soggetto = dr.IsDBNull(4) ? "" : dr.GetString(4);
            d.DataPubblicazione = dr.IsDBNull(5) ? null : (DateTime?)dr.GetDateTime(5);
            d.DataCreazione = dr.IsDBNull(6) ? null : (DateTime?)dr.GetDateTime(6);
            d.DataScadenza = dr.IsDBNull(7) ? null : (DateTime?)dr.GetDateTime(7);
            d.Abstract = dr.IsDBNull(8) ? "" : dr.GetString(8);
            d.Origine = dr.IsDBNull(9) ? "" : dr.GetString(9);
            d.Commenti = dr.IsDBNull(10) ? "" : dr.GetString(10);
            d.Scopo = dr.IsDBNull(11) ? "" : dr.GetString(11);
            d.ParoleChiave = dr.IsDBNull(12) ? "" : dr.GetString(12);
            d.Riferimenti = dr.IsDBNull(13) ? "" : dr.GetString(13);
            d.Diritti = dr.IsDBNull(14) ? "" : dr.GetString(14);
            d.Argomenti = dr.IsDBNull(15) ? "" : dr.GetString(15);

            return d;
        }
    }
}
