using System;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class EmailRepository : Repository
    {
        private static readonly EmailRepository _instance = new EmailRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private EmailRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static EmailRepository Instance
        {
            get { return _instance; }
        }

        internal int SalvaEmail(Email email)
        {
            int result = 0;

            result = InserisciEmail(email);

            return result;
        }

        private int InserisciEmail(Email email)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_Email (Testo, IndirizzoEmail, Tipo, DataInvio) VALUES " +
                            "(@Testo, @IndirizzoEmail, @Tipo, @DataInvio);" +
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Testo", email.Testo);
            sseo.SqlParameters.AddWithValue("@IndirizzoEmail", email.IndirizzoEmail);
            sseo.SqlParameters.AddWithValue("@Tipo", email.Tipo);
            sseo.SqlParameters.AddWithValue("@DataInvio", email.Data);

            result = (int)SqlProvider.ExecuteScalarObject(sseo);

            return result;
        }

        public int InserisciEmail(string testo, string indirizzoEmail, string tipo, DateTime data)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_Email (Testo, IndirizzoEmail, Tipo, DataInvio) VALUES " +
                   "(@Testo, @IndirizzoEmail, @Tipo, GETDATE());" +
                   "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@Testo", testo);
            sseo.SqlParameters.AddWithValue("@IndirizzoEmail", indirizzoEmail);
            sseo.SqlParameters.AddWithValue("@Tipo", tipo);

            result = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return result;
        }

       
    }
}
