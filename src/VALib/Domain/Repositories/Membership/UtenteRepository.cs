using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElogToolkit.Data.SqlServer;
using VALib.Domain.Entities.Membership;
using VALib.Domain.Common;
using System.Data.SqlClient;
using VALib.Configuration;
using System.Runtime.Caching;


namespace VALib.Domain.Repositories.Membership
{
    public class UtenteRepository : Repository
    {
        private static readonly UtenteRepository _instance = new UtenteRepository(Settings.VAConnectionString);

        private const string _selectBase1 = @"SELECT UtenteID, Ruolo, NomeUtente, Abilitato, DataUltimoCamBioPassword,
                                            DataUltimoLogin, Email, Nome, Cognome FROM dbo.TBL_Utenti WHERE {0} = @{1};
                                            SELECT STG.RuoloUtenteID FROM STG_UtentiRuoliUtente STG INNER JOIN dbo.TBL_Utenti TU 
                                            ON STG.UtenteID=TU.UtenteID WHERE {2}=@{3}";

        private UtenteRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static UtenteRepository Instance
        {
            get { return _instance; }
        }

        private Utente EseguiRecupero(SqlDataReader dr)
        {
            Utente utente = null;

            if (dr.Read())
                utente = RiempiIstanzaUtente(dr);

            if (utente != null)
            {
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        int ruoloUtenteID = dr.GetInt32(0);
                        RuoloUtente ruolo = RuoliUtenteRepository.Instance.RecuperaRuoloUtente(ruoloUtenteID);
                        if (ruolo != null)
                        {
                            utente.ListaRuoli.Add(ruolo);
                        }
                    }
                }
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }
            return utente;
        }

        private Utente RiempiIstanzaUtente(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Utente utente = new Utente();

            utente.ID = dr.GetInt32(0);
            utente.Ruolo = dr.GetInt32(1);
            utente.NomeUtente = dr.GetString(2);
            utente.Abilitato = dr.GetBoolean(3);
            if (!dr.IsDBNull(4))
                utente.DataUltimoCambioPassword = dr.GetDateTime(4);
            if (!dr.IsDBNull(5))
                utente.DataUltimoLogin = dr.GetDateTime(5);
            utente.Email = dr.GetString(6);
            utente.Nome = dr.GetString(7);
            utente.Cognome = dr.GetString(8);
            return utente;
        }

        //public int InserisciUtente(Utente utente)
        //{

        //}
       
        public Utente RecuperaUtente(string nomeUtente)
        {
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            if (string.IsNullOrWhiteSpace(nomeUtente))
                throw new ArgumentNullException("nomeUtente");

            sseo = new SqlServerExecuteObject();

             

            sseo.CommandText = String.Format(_selectBase1, "NomeUtente", "nomeUtente", "TU.NomeUtente", "nomeUtente");
               // +String.Format(_selectBase2,"TU.NomeUtente", "nomeUtente");
            sseo.SqlParameters.AddWithValue("@nomeUtente", nomeUtente);

            dr = SqlProvider.ExecuteReaderObject(sseo);
            Utente utente = EseguiRecupero(dr);
            return utente;
        }

        public Utente RecuperaUtente(int utenteID)
        {
           
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = String.Format(_selectBase1, "UtenteID", "utenteId", "TU.UtenteID", "utenteId");
            sseo.SqlParameters.AddWithValue("@utenteId", utenteID);

            dr = SqlProvider.ExecuteReaderObject(sseo);
            Utente utente = EseguiRecupero(dr);
            return utente;
        }

        public Utente RecuperaUtenteDaEmail(string email)
        {
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
 
            sseo.CommandText = String.Format(_selectBase1, "Email", "email","TU.Email", "email");
            sseo.SqlParameters.AddWithValue("@email", email);

            dr = SqlProvider.ExecuteReaderObject(sseo);
            Utente utente = EseguiRecupero(dr);
            return utente;
        }

        public List<Utente> RecuperaUtenti()
        {
            List<Utente> listaUtenti = null;
            List<Tuple<int, int>> utenteRuoloUtente = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "listaUtenti";

            listaUtenti = cache[cacheKey] as List<Utente>;

            if (listaUtenti == null)
            {

                listaUtenti = new List<Utente>();
                sseo = new SqlServerExecuteObject();

                sseo.CommandText = @"SELECT UtenteID, Ruolo, NomeUtente, Abilitato, DataUltimoCambioPassword,
                                            DataUltimoLogin, Email, Nome, Cognome from TBL_Utenti;
                                     SELECT UtenteID, RuoloUtenteID FROM STG_UtentiRuoliUtente";

                dr = SqlProvider.ExecuteReaderObject(sseo);

                while (dr.Read())
                {
                    Utente utente = RiempiIstanzaUtente(dr);
                    listaUtenti.Add(utente);
                }

                if (listaUtenti.Count>0)
                {
                    utenteRuoloUtente = new List<Tuple<int, int>>();
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            int utenteID = dr.GetInt32(0);
                            int ruoloUtenteID = dr.GetInt32(1);
                            utenteRuoloUtente.Add(new Tuple<int,int>(utenteID, ruoloUtenteID));
                        }
                    }
                }

                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }

            }
            
             foreach(Utente u in listaUtenti)
             {
                 List<Tuple<int, int>> listaTuple = utenteRuoloUtente.FindAll(x => x.Item1 == u.ID);
                             
                 foreach (Tuple<int,int> tupla in listaTuple)
                 {
                     RuoloUtente ruolo = RuoliUtenteRepository.Instance.RecuperaRuoloUtente(tupla.Item2);
                     u.ListaRuoli.Add(ruolo);
                 }
             }

            return listaUtenti;
        }

        public string RecuperaPassword(int utenteID)
        {
            string password = null;
            SqlServerExecuteObject sseo = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "SELECT [Pswd] FROM dbo.TBL_Utenti WHERE UtenteID = @UtenteID;";
            sseo.SqlParameters.AddWithValue("@UtenteID", utenteID);

            password = (string)SqlProvider.ExecuteScalarObject(sseo);

            return password;
        }

        public void AggiornaPassword(int utenteID, string password)
        {
            SqlServerExecuteObject sseo = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = @"UPDATE dbo.TBL_Utenti  SET Pswd=@Pswd, DataUltimoCambioPassword = @DataUltimoCambioPassword
                                WHERE UtenteID = @UtenteID;";
            sseo.SqlParameters.AddWithValue("@Pswd", password);
            sseo.SqlParameters.AddWithValue("@UtenteID", utenteID);
            sseo.SqlParameters.AddWithValue("@DataUltimoCambioPassword", DateTime.Now);
            SqlProvider.ExecuteNonQueryObject(sseo);
        }

        public void AssegnaPassword(int utenteID, string password)
        {
            SqlServerExecuteObject sseo = null;

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password non può essere null o stringa vuota", "password");

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = @"UPDATE dbo.TBL_Utenti  SET Pswd=@Pswd
                                WHERE UtenteID = @UtenteID;";
            sseo.SqlParameters.AddWithValue("@Pswd", password);
            sseo.SqlParameters.AddWithValue("@UtenteID", utenteID);
            SqlProvider.ExecuteNonQueryObject(sseo);
        }

        public void ResetPassword(int utenteID)
        {
            SqlServerExecuteObject sseo = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = @"UPDATE dbo.TBL_Utenti  
                               SET Pswd= NULL,
                                  DataUltimoCambioPassword = NULL
                                WHERE UtenteID = @UtenteID;";

            sseo.SqlParameters.AddWithValue("@UtenteID", utenteID);
            SqlProvider.ExecuteNonQueryObject(sseo);
        }
 

        public  int SalvaUtente(Utente utente)
        {
            SqlServerExecuteObject sseo = null;
            int id = 0;
            if (utente == null)
                throw new ArgumentNullException("utente");
            sseo = new SqlServerExecuteObject();
            if (utente.ID < 1) 
            {   
                sseo.CommandText = @"INSERT INTO dbo.TBL_UTENTI (Ruolo, NomeUtente, Abilitato, DataUltimoLogin, Email, Nome, Cognome) 
                                    OUTPUT INSERTED.UtenteID VALUES (0, @NomeUtente, @Abilitato, @DataUltimoLogin, @Email, @Nome, @Cognome)";

            }
            else
            {
                sseo.CommandText = @"UPDATE dbo.TBL_Utenti
                                    SET 
                                      
                                        Email = @Email,
                                        Nome = @Nome,
                                        Cognome = @Cognome,
                                        Abilitato = @Abilitato,
                                        DataUltimoLogin = @DataUltimoLogin
                                        WHERE UtenteID = @UtenteID;";

                sseo.SqlParameters.AddWithValue("@UtenteID", utente.ID);

            }

            sseo.SqlParameters.AddWithValue("@NomeUtente", utente.NomeUtente);
            sseo.SqlParameters.AddWithValue("@Nome", utente.Nome);
            sseo.SqlParameters.AddWithValue("@Cognome", utente.Cognome);
            sseo.SqlParameters.AddWithValue("@Email", utente.Email);
            sseo.SqlParameters.AddWithValue("@Abilitato", utente.Abilitato);
            sseo.SqlParameters.AddWithValue("@DataUltimoLogin", utente.DataUltimoLogin==null ? (object)DBNull.Value : utente.DataUltimoLogin);

            if (utente.ID < 1)
            {
                id = Convert.ToInt32(SqlProvider.ExecuteScalarObject(sseo));
            }
            else
            {
                SqlProvider.ExecuteNonQueryObject(sseo);
                id = utente.ID;
            }

            return id;
        }

        public List<int> RecuperaRuoliUtente(int utenteID)
        {
            List<int> ruoliUtente = null;
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "ruoliID";

            ruoliUtente = cache[cacheKey] as List<int>;

            if (ruoliUtente == null)
            {
                sseo = new SqlServerExecuteObject();
                sseo.CommandText = @"SELECT UtenteID, RuoloUtenteID FROM STG_UtentiRuoliUtente WHERE UtenteID=@utenteID";
                sseo.SqlParameters.AddWithValue("@utenteID", utenteID);
                dr = SqlProvider.ExecuteReaderObject(sseo);
                while (dr.Read())
                {
                    ruoliUtente = new List<int>();
                    ruoliUtente.Add(dr.GetInt32(0));
                }

                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
            return ruoliUtente;
        }

        public void AssegnaRuoloUtente(int utenteID, int ruoloUtenteID)
        {
            SqlServerExecuteObject sseo = null;            
            ElogToolkit.Data.SqlServer.SqlServerProvider.SqlServerTransactionObject tran = SqlProvider.CreateTransactionObject();

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = @"INSERT INTO dbo.STG_UtentiRuoliUtente (UtenteID, RuoloUtenteID) VALUES (@UtenteID, @RuoloUtenteID)";
            sseo.SqlParameters.AddWithValue("@utenteID", utenteID);
            sseo.SqlParameters.AddWithValue("@RuoloUtenteID", ruoloUtenteID);

            try
            {
                tran.Begin();
                tran.ExecuteNonQueryObject(sseo);
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                sseo = null;
                tran.Dispose();
            }
        }

        public void EliminaRuoloUtente(int utenteID, int ruoloUtenteID)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.STG_UtentiRuoliUtente WHERE UtenteID = @UtenteID AND RuoloUtenteID = @RuoloUtenteID";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@UtenteID", utenteID);
            sseo.SqlParameters.AddWithValue("@RuoloUtenteID", ruoloUtenteID);
            SqlProvider.ExecuteNonQueryObject(sseo);
        }

        


    }
}

 