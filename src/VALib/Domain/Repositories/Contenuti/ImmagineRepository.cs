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
    public sealed class ImmagineRepository : Repository
    {
        private static readonly ImmagineRepository _instance = new ImmagineRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private ImmagineRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ImmagineRepository Instance
        {
            get { return _instance; }
        }

        public List<Immagine> RecuperaImmagini(string nome_IT, int startrowNum, int endRowNum, out int rows)
        {
            List<Immagine> immagini = new List<Immagine>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            
            sSql = "SELECT * FROM (" +
                "SELECT I.ImmagineID, I.ImmagineMasterID, I.FormatoImmagineID, I.Nome_IT, I.Nome_EN, I.DataInserimento, I.DataUltimaModifica, I.Altezza, I.Larghezza, I.NomeFile, ROW_NUMBER() " +
                "OVER(ORDER BY DataInserimento DESC) " +
                "ROWNUM " +
                "FROM dbo.TBL_Immagini AS I WHERE (I.FormatoImmagineID = 0) AND (I.Nome_IT LIKE @Testo)" +
                ") " +
                "R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;" +
                "SELECT COUNT(*) FROM dbo.TBL_Immagini AS I WHERE (I.FormatoImmagineID = 0) AND (I.Nome_IT LIKE @Testo);";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@Testo", string.Format("%{0}%", nome_IT));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Immagine immagine = RiempiIstanza(dr);
                immagini.Add(immagine);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return immagini;
        }

        public List<Immagine> RecuperaImmaginiFiglio(int immagineMasterID)
        {
            List<Immagine> immagini = new List<Immagine>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";

            sSql = "SELECT * FROM (" +
                "SELECT I.ImmagineID, I.ImmagineMasterID, I.FormatoImmagineID, I.Nome_IT, I.Nome_EN, I.DataInserimento, I.DataUltimaModifica, I.Altezza, I.Larghezza, I.NomeFile, ROW_NUMBER() " +
                "OVER(ORDER BY DataInserimento DESC) " +
                "ROWNUM " +
                "FROM dbo.TBL_Immagini AS I WHERE (I.FormatoImmagineID <> 0) AND (ImmagineMasterID = @ImmagineMasterID)" +
                ") " +
                "R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@ImmagineMasterID", immagineMasterID);
            sseo.SqlParameters.AddWithValue("@StartRowNum", 0);
            sseo.SqlParameters.AddWithValue("@EndRowNum", 100);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Immagine immagine = RiempiIstanza(dr);
                immagini.Add(immagine);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return immagini;
        }

        private List<Immagine> RecuperaImmaginiPerFormato(FormatoImmagineEnum formato)
        {
            List<Immagine> immagini = new List<Immagine>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";

            sSql = "SELECT * FROM (" +
                "SELECT I.ImmagineID, I.ImmagineMasterID, I.FormatoImmagineID, I.Nome_IT, I.Nome_EN, I.DataInserimento, I.DataUltimaModifica, I.Altezza, I.Larghezza, I.NomeFile, ROW_NUMBER() " +
                "OVER(ORDER BY DataInserimento DESC) " +
                "ROWNUM " +
                "FROM dbo.TBL_Immagini AS I WHERE (I.FormatoImmagineID = @FormatoImmagineID)" +
                ") " +
                "R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@FormatoImmagineID", (int)formato);
            sseo.SqlParameters.AddWithValue("@StartRowNum", 0);
            sseo.SqlParameters.AddWithValue("@EndRowNum", int.MaxValue);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Immagine immagine = RiempiIstanza(dr);
                immagini.Add(immagine);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return immagini;
        }

        public List<Immagine> RecuperaImmaginiPerCarosello()
        {
            return RecuperaImmaginiPerFormato(FormatoImmagineEnum.CaroselloHome);
          
        }

        public List<Immagine> RecuperaImmaginiPerDatoAmbientaleHome()
        {
            return RecuperaImmaginiPerFormato(FormatoImmagineEnum.WidgetHome);
        }

        public List<Immagine> RecuperaImmaginiLibere()
        {
            return RecuperaImmaginiPerFormato(FormatoImmagineEnum.Libero);
        }

        public Immagine RecuperaImmagine(int id)
        {
            Immagine immagine = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT I.ImmagineID, I.ImmagineMasterID, I.FormatoImmagineID, I.Nome_IT, I.Nome_EN, I.DataInserimento, I.DataUltimaModifica, I.Altezza, I.Larghezza, I.NomeFile FROM dbo.TBL_Immagini AS I WHERE ImmagineID = @ImmagineID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@ImmagineID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                immagine = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return immagine;
        }

        internal int SalvaImmagine(Immagine immagine)
        {
            int result = 0;

            if (immagine.IsNew)
                result = InserisciImmagine(immagine);
            else
                result = ModificaImmagine(immagine);

            return result;
        }
        
        private int ModificaImmagine(Immagine immagine)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "UPDATE dbo.TBL_Immagini SET Nome_IT = @Nome_IT, Nome_EN = @Nome_EN, DataUltimaModifica = @DataUltimaModifica, NomeFile = @NomeFile, Altezza = @Altezza, Larghezza = @Larghezza " +
                            "WHERE ImmagineID = @ImmagineID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Nome_IT", immagine.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", immagine.Nome_EN);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", immagine.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@NomeFile", immagine.NomeFile);
            sseo.SqlParameters.AddWithValue("@Larghezza", immagine.Larghezza);
            sseo.SqlParameters.AddWithValue("@Altezza", immagine.Altezza);
            sseo.SqlParameters.AddWithValue("@ImmagineID", immagine.ID);

            SqlProvider.ExecuteNonQueryObject(sseo);

            result = immagine.ID;

            return result;
        }
        
        private int InserisciImmagine(Immagine immagine)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_Immagini (ImmagineMasterID, FormatoImmagineID, Nome_IT, Nome_EN, DataInserimento, DataUltimaModifica, Altezza, Larghezza, NomeFile) VALUES " +
                            "(@ImmagineMasterID, @FormatoImmagineID, @Nome_IT, @Nome_EN, @DataInserimento, @DataUltimaModifica, @Altezza, @Larghezza, @NomeFile);" +
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@ImmagineMasterID", immagine.ImmagineMasterID.Value);
            sseo.SqlParameters.AddWithValue("@FormatoImmagineID", immagine.FormatoImmagine.ID);
            sseo.SqlParameters.AddWithValue("@Nome_IT", immagine.Nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", immagine.Nome_EN);
            sseo.SqlParameters.AddWithValue("@DataInserimento", immagine.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", immagine.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Altezza", immagine.Altezza);
            sseo.SqlParameters.AddWithValue("@Larghezza", immagine.Larghezza);
            sseo.SqlParameters.AddWithValue("@NomeFile", immagine.NomeFile);

            result = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return result;
        }

        private Immagine RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Immagine immagine = new Immagine();

            immagine.ID = dr.GetInt32(0);
            immagine.ImmagineMasterID = dr.IsDBNull(1) ? null : (int?)dr.GetInt32(1);
            immagine.FormatoImmagine = FormatoImmagineRepository.Instance.RecuperaFormatoImmagine(dr.GetInt32(2));
            immagine.Nome_IT = dr.GetString(3);
            immagine.Nome_EN = dr.GetString(4);
            immagine.DataInserimento= dr.GetDateTime(5);
            immagine.DataUltimaModifica = dr.GetDateTime(6);
            immagine.Altezza = dr.GetInt32(7);
            immagine.Larghezza = dr.GetInt32(8);
            immagine.NomeFile = dr.GetString(9);

            return immagine;
        }

        public void Elimina(int id)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM TBL_Immagini WHERE ImmagineID = @ImmagineID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@ImmagineID", id);

            SqlProvider.ExecuteNonQueryObject(sseo);
        }
    }
}
