using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VALib.Configuration;
using VALib.Domain.Entities.Membership;
using VALib.Domain.Repositories.Membership;

namespace VALib.Domain.Services
{
    public class MembershipService
    {
        private const string _tokenDataFormatString = "dd/MM/yyyy HH:mm";

        private UtenteRepository _utenteRepos;

        public MembershipService()
        {
            _utenteRepos = UtenteRepository.Instance;
        }

        public List<Utente> RecuperaListaUtenti()
        {
            List<Utente> lista = UtenteRepository.Instance.RecuperaUtenti();
            return lista;
        }

        public Utente RecuperaUtente(int id)
        {
            Utente utente = null;
            utente = UtenteRepository.Instance.RecuperaUtente(id);
            return utente;
        }

        public Utente RecuperaUtente(string nomeUtente)
        {
            Utente utente = null;
            utente = UtenteRepository.Instance.RecuperaUtente(nomeUtente);
            return utente;
        }

        public List<RuoloUtente> RecuperaListaRuoliUtente()
        {
            List<RuoloUtente> lista = RuoliUtenteRepository.Instance.RecuperaTuttiRuoliUtente();
            return lista;
        }

        public void AssegnaRuoloUtente(int utenteId, int ruoloUtenteId)
        {
            _utenteRepos.AssegnaRuoloUtente(utenteId, ruoloUtenteId);
        }

        public void EliminaRuoloUtente(int utenteId, int ruoloUtenteId)
        {
            _utenteRepos.EliminaRuoloUtente(utenteId, ruoloUtenteId);
        }

        public bool EseguiLoginUtente(string nomeUtente, string password)
        {
            bool loginEseguito = false;
            Utente utente = null;

            if (string.IsNullOrWhiteSpace(nomeUtente))
                throw new ArgumentNullException("nomeUtente");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("password");

            utente = _utenteRepos.RecuperaUtente(nomeUtente);

            if (utente != null && utente.Abilitato)
            {
                string reposPassword = _utenteRepos.RecuperaPassword(utente.ID);
                if (EseguiValidazionePassword(reposPassword, password))
                {
                    loginEseguito = true;
                    utente.DataUltimoLogin = DateTime.Now;
                    _utenteRepos.SalvaUtente(utente);
                }
            }

            return loginEseguito;
        }

        public bool EseguiLoginUtenteToken(string token, out string nomeUtente)
        {
            bool loginEseguito = false;
            Utente utente = null;
          


            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            if (ValidaTokenPrimoLogin(token, out nomeUtente))
            {
                utente = _utenteRepos.RecuperaUtente(nomeUtente);

                if (utente != null && utente.DataUltimoCambioPassword == null && utente.Abilitato)
                {
                    loginEseguito = true;
                    utente.DataUltimoLogin = DateTime.Now;
                    _utenteRepos.SalvaUtente(utente);
                }
            }

            return loginEseguito;
        }

        public Utente CreaNuovoUtenteESalva(string nomeUtente, string nome, string cognome, bool abilitato, string email)
        {
            if (string.IsNullOrWhiteSpace(nomeUtente))
                throw new ArgumentException("nomeUtente null o whitespace", "nomeUtente");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("nome null o whitespace", "nome");

            if (string.IsNullOrWhiteSpace(nomeUtente))
                throw new ArgumentException("cognome null o whitespace", "cognome");

            if (string.IsNullOrWhiteSpace(nomeUtente))
                throw new ArgumentException("email null o whitespace", "email");

            int idUtente = 0;
            Utente nuovoUtente = null;
            
            string passwordHash = "";

            if (!String.IsNullOrWhiteSpace(nomeUtente) && !String.IsNullOrWhiteSpace(nomeUtente)
                && !String.IsNullOrWhiteSpace(email))
            {
                Utente utente = new Utente();
                utente.Abilitato = abilitato;
                utente.NomeUtente = nomeUtente;
                utente.Nome = nome;
                utente.Cognome = cognome;
                utente.Email = email;
                utente.DataUltimoCambioPassword = null;
                idUtente = _utenteRepos.SalvaUtente(utente);

                if (idUtente > 0)
                {
                    

                    passwordHash = GeneraPasswordCasuale();

                    _utenteRepos.AssegnaPassword(idUtente, passwordHash);
                }

            }

            nuovoUtente = RecuperaUtente(idUtente);

            return nuovoUtente;
        }

        private string GeneraPasswordCasuale()
        {
            string passwordGenerata = null;
            string passwordHash = null;

            ElogToolkit.Text.RandomStringGenerator strGen = new ElogToolkit.Text.RandomStringGenerator();
            passwordGenerata = strGen.Next(10);
            PasswordHasher hasher = new PasswordHasher();
            passwordHash = hasher.HashPassword(passwordGenerata);

            return passwordHash;
        }

        public string EseguiHashPassword(string password)
        {
            PasswordHasher passwordHasher = new PasswordHasher();
            string hashedPassword = passwordHasher.HashPassword(password);
            return hashedPassword;
        }

        public bool EseguiValidazionePassword(string hashedPassword, string password)
        {
            PasswordHasher passwordHasher = new PasswordHasher();
            return passwordHasher.ValidatePassword(password, hashedPassword);
        }

        public Utente TrovaEmailUguali(string email)
        {
            Utente utente = null;
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email");
            utente = UtenteRepository.Instance.RecuperaUtenteDaEmail(email);
            return utente;
        }

        public void ModificaUtente(Utente utente)
        {
            if (utente.ID < 1)
            {
                throw new ArgumentException("Utente non esistente", "utente");
            }
            else
            {
                UtenteRepository.Instance.SalvaUtente(utente);
            }
        }

        public void ResetPasswordUtente(Utente utente)
        {
            string password = null;
            if (utente == null)
                throw new ArgumentNullException("utente non può essere null", "utente");

            _utenteRepos.ResetPassword(utente.ID);

            password = GeneraPasswordCasuale();

            _utenteRepos.AssegnaPassword(utente.ID, password);
        }
 
        public string GeneraNomeUtente(string nome, string cognome)
        {
            MembershipService service = new MembershipService();
            string nomeUtente = "";
            Utente utenteTemp = null;
            int numeroProgress = 1;

            nome = nome.Trim().ToLower();
            cognome = cognome.Trim().ToLower();
            nomeUtente = nome + "." + cognome;
            nomeUtente.Replace(' ', '.');

            while (numeroProgress < 100)
            {
                utenteTemp = service.RecuperaUtente(nomeUtente);
                if (utenteTemp != null)
                {
                    nomeUtente = nome + "." + cognome + numeroProgress.ToString();
                    nomeUtente.Replace(' ', '.');
                }
                else
                {
                    break;
                }
                numeroProgress++;
            }

            return nomeUtente;
        }

        public static string GeneraTokenPrimoLogin(Utente utente)
        {
            string token = "";
            string chiaveCrypt = "";

            chiaveCrypt = Settings.ChiaveCriptazione;
            token = utente.NomeUtente + "_" + DateTime.Now.ToString(_tokenDataFormatString, CultureInfo.CurrentCulture);
            token = ElogToolkit.Security.Cryptography.CryptographyUtils.EncryptString(token, chiaveCrypt);

            return token;
        }

        public static bool ValidaTokenPrimoLogin(string token, out string nomeUtente)
        {
            string tokenPlain = "";
            DateTime dataToken = DateTime.MinValue;
            bool result = false;
            string[] campiToken;
            nomeUtente = "";

            tokenPlain = ElogToolkit.Security.Cryptography.CryptographyUtils.DecryptString(token, Settings.ChiaveCriptazione);

            campiToken = tokenPlain.Split(new[] { '_' });

            if (campiToken.Length == 2)
            {
                if (DateTime.TryParseExact(campiToken[1], _tokenDataFormatString, CultureInfo.CurrentCulture, DateTimeStyles.None, out dataToken))
                {
                    nomeUtente = campiToken[0];
                    dataToken = dataToken.AddHours(Settings.DurataTokenCreazioneUtente);

                    if (dataToken.CompareTo(DateTime.Now) > 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
