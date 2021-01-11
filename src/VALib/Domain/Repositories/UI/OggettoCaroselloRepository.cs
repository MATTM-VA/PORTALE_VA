using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.UI;
using System.Web;
using VALib.Domain.Repositories.Contenuti;

namespace VALib.Domain.Repositories.UI
{
    public sealed class OggettoCaroselloRepository : Repository
    {
        private static readonly OggettoCaroselloRepository _instance = new OggettoCaroselloRepository(Settings.VAConnectionString);
        public static readonly string _webCacheKey = "OggettiCaroselloHome";

        private OggettoCaroselloRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static OggettoCaroselloRepository Instance
        {
            get { return _instance; }
        }

        public List<OggettoCarosello> RecuperaOggettiCarosello(bool? pubblicato)
        {
            List<OggettoCarosello> oggettiCarosello = new List<OggettoCarosello>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";

            sSql = @"SELECT TOP(99999999) OggettoCaroselloID, TipoContenutoID, ContenutoID, ImmagineID, Data, Nome_IT, Nome_EN, 
                                Descrizione_IT, Descrizione_EN, LinkProgettoCartografico, Pubblicato,  
                                DataInserimento, DataUltimaModifica 
                    FROM dbo.TBL_UI_OggettiCarosello 
                    WHERE (@Pubblicato IS NULL) OR (Pubblicato = @Pubblicato) 
                    ORDER BY Data DESC;";
            
            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Pubblicato", pubblicato.HasValue ? (object)pubblicato.Value : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoCarosello oggettoCarosello = RiempiIstanza(dr);
                oggettiCarosello.Add(oggettoCarosello);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiCarosello;
        }

        public List<OggettoCarosello> RecuperaOggettiCaroselloHome()
        {
            List<OggettoCarosello> oggettiCarosello = new List<OggettoCarosello>();

            oggettiCarosello = this.CacheGet(_webCacheKey) as List<OggettoCarosello>;

            if (oggettiCarosello == null)
            {
                oggettiCarosello = RecuperaOggettiCarosello(true);

                //HttpContext.Current.Cache.Insert(_webCacheKey, oggettiCarosello, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, oggettiCarosello, TimeSpan.FromMinutes(15));
            }

            return oggettiCarosello;
        }

        public OggettoCarosello RecuperaOggettoCarosello(int id)
        {
            OggettoCarosello oggettoCarosello = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT OggettoCaroselloID, TipoContenutoID, ContenutoID, ImmagineID, Data, Nome_IT, Nome_EN, 
                                Descrizione_IT, Descrizione_EN, LinkProgettoCartografico, Pubblicato,  
                                DataInserimento, DataUltimaModifica 
                            FROM dbo.TBL_UI_OggettiCarosello 
                            WHERE OggettoCaroselloID = @OggettoCaroselloID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@OggettoCaroselloID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                oggettoCarosello = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettoCarosello;
        }

        internal int SalvaOggettoCarosello(OggettoCarosello oggettoCarosello)
        {
            int result = 0;

            if (oggettoCarosello.IsNew)
                result = InserisciOggettoCarosello(oggettoCarosello);
            else
                result = ModificaOggettoCarosello(oggettoCarosello);

            //RemoveCacheDependency(_webCacheKey);
            this.CacheRemove(_webCacheKey);

            return result;
        }

        private int ModificaOggettoCarosello(OggettoCarosello oggettoCarosello)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = @"UPDATE dbo.TBL_UI_OggettiCarosello SET TipoContenutoID = @TipoContenutoID, ContenutoID = @ContenutoID, 
                            ImmagineID = @ImmagineID, Data = @Data, Nome_IT = @Nome_IT, Nome_EN = @Nome_EN, 
                            Descrizione_IT = @Descrizione_IT, Descrizione_EN = @Descrizione_EN, 
                            LinkProgettoCartografico = @LinkProgettoCartografico, Pubblicato = @Pubblicato,
                            DataUltimaModifica = @DataUltimaModifica  
                            WHERE OggettoCaroselloID = @OggettoCaroselloID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@TipoContenutoID", (int)oggettoCarosello.TipoContenuto);
            sseo.SqlParameters.AddWithValue("@ContenutoID", oggettoCarosello.ContenutoID);
            sseo.SqlParameters.AddWithValue("@ImmagineID", oggettoCarosello.ImmagineID);
            sseo.SqlParameters.AddWithValue("@Data", oggettoCarosello.Data);
            sseo.SqlParameters.AddWithValue("@Nome_IT", oggettoCarosello.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", oggettoCarosello.Nome_EN);
            sseo.SqlParameters.AddWithValue("@Descrizione_IT", oggettoCarosello.Descrizione_IT);
            sseo.SqlParameters.AddWithValue("@Descrizione_EN", oggettoCarosello.Descrizione_EN);
            sseo.SqlParameters.AddWithValue("@LinkProgettoCartografico", string.IsNullOrWhiteSpace(oggettoCarosello.LinkProgettoCartografico) ? "" : oggettoCarosello.LinkProgettoCartografico);
            sseo.SqlParameters.AddWithValue("@Pubblicato", oggettoCarosello.Pubblicato);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", oggettoCarosello.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@OggettoCaroselloID", oggettoCarosello.ID);

            SqlProvider.ExecuteNonQueryObject(sseo);

            result = oggettoCarosello.ID;

            return result;
        }
        
        private int InserisciOggettoCarosello(OggettoCarosello oggettoCarosello)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = @"INSERT INTO dbo.TBL_UI_OggettiCarosello (TipoContenutoID, ContenutoID, ImmagineID, Data, Nome_IT, Nome_EN, 
                        Descrizione_IT, Descrizione_EN, LinkProgettoCartografico, Pubblicato, DataInserimento, DataUltimaModifica) 
                            VALUES (@TipoContenutoID, @ContenutoID, @ImmagineID, @Data, @Nome_IT, @Nome_EN, 
                        @Descrizione_IT, @Descrizione_EN, @LinkProgettoCartografico, @Pubblicato, @DataInserimento, @DataUltimaModifica) ;
                    SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@TipoContenutoID", (int)oggettoCarosello.TipoContenuto);
            sseo.SqlParameters.AddWithValue("@ContenutoID", oggettoCarosello.ContenutoID);
            sseo.SqlParameters.AddWithValue("@ImmagineID", oggettoCarosello.ImmagineID);
            sseo.SqlParameters.AddWithValue("@Data", oggettoCarosello.Data);
            sseo.SqlParameters.AddWithValue("@Nome_IT", oggettoCarosello.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", oggettoCarosello.Nome_EN);
            sseo.SqlParameters.AddWithValue("@Descrizione_IT", oggettoCarosello.Descrizione_IT);
            sseo.SqlParameters.AddWithValue("@Descrizione_EN", oggettoCarosello.Descrizione_EN);
            sseo.SqlParameters.AddWithValue("@LinkProgettoCartografico", string.IsNullOrWhiteSpace(oggettoCarosello.LinkProgettoCartografico) ? "" : oggettoCarosello.LinkProgettoCartografico);
            sseo.SqlParameters.AddWithValue("@Pubblicato", oggettoCarosello.Pubblicato);
            sseo.SqlParameters.AddWithValue("@DataInserimento", oggettoCarosello.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", oggettoCarosello.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@OggettoCaroselloID", oggettoCarosello.ID);

            result = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return result;
        }

        private OggettoCarosello RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoCarosello oggettoCarosello = new OggettoCarosello();

            oggettoCarosello.ID = dr.GetInt32(0);
            oggettoCarosello.TipoContenuto = (ContenutoOggettoCaroselloTipo)dr.GetInt32(1);
            oggettoCarosello.ContenutoID = dr.GetInt32(2);
            oggettoCarosello.ImmagineID = dr.GetInt32(3);
            oggettoCarosello.Data = dr.GetDateTime(4);
            oggettoCarosello.Nome_IT = dr.GetString(5);
            oggettoCarosello.Nome_EN = dr.GetString(6);
            oggettoCarosello.Descrizione_IT = dr.GetString(7);
            oggettoCarosello.Descrizione_EN = dr.GetString(8);
            oggettoCarosello.LinkProgettoCartografico = dr.IsDBNull(9) ? "" : dr.GetString(9);
            oggettoCarosello.Pubblicato = dr.GetBoolean(10);
            oggettoCarosello.DataInserimento = dr.GetDateTime(11);
            oggettoCarosello.DataUltimaModifica = dr.GetDateTime(12);
            
            oggettoCarosello.Contenuto = ContenutoOggettoCaroselloRepository.Instance.RecuperaContenutoOggettoCarosello(oggettoCarosello.ContenutoID, oggettoCarosello.TipoContenuto);

            return oggettoCarosello;
        }

        public void Elimina(int id)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.TBL_UI_OggettiCarosello WHERE OggettoCaroselloID = @OggettoCaroselloID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@OggettoCaroselloID", id);

            SqlProvider.ExecuteNonQueryObject(sseo);

            //RemoveCacheDependency(_webCacheKey);
            this.CacheRemove(_webCacheKey);
        }
    }
}
