using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElogToolkit.Net.Mail;
using VALib.Configuration;
using VALib.Domain.Entities.Membership;
using System.Threading.Tasks;
using System.Net.Mail;

namespace VALib.Domain.Services
{
    public static class EmailService
    {
        public static void InvioEmail(string from, string body, string tipo)
        {
            string oggetto = "";

            switch (tipo.ToLower())
            {
                case "proponente":
                    oggetto = "Messaggio dal sito Valutazioni Ambientali - PROPONENTE";
                    break;
                case "cittadino":
                    oggetto = "Messaggio dal sito Valutazioni Ambientali - CITTADINO";
                    break;
                default:
                    break;
            }

            MailClient.Send(from, Settings.DestinatariEmail, oggetto, body, false);
        }

        public static void InvioToken(string urlCreazioneToken, Utente utente, TipoEmail tipo)
        {
            StringBuilder bodyMail = new StringBuilder();
            string oggettoMail = "Creazione utente per il portale delle valutazioni ambientali";

            switch (tipo)
            {
                case TipoEmail.Registrazione:
                    bodyMail.AppendFormat("<p>L'amministrazione VAS-VIA ha creato un account per lei</p><p>Nome Utente : {0} </p>", utente.NomeUtente);
                    bodyMail.AppendFormat("<p> Concludi la registrazione facendo il primo accesso e creando la tua password al link sottostante</p>");
                    bodyMail.AppendFormat("<a href={0}>{0}</a>", urlCreazioneToken);
                    break;
                case TipoEmail.ResetPassword:
                    bodyMail.AppendFormat("<p>L'amministrazione VAS-VIA ha effettuato il reset della sua password </p><p>Nome Utente : {0} </p>", utente.NomeUtente);
                    bodyMail.AppendFormat("<p> </p>");
                    bodyMail.AppendFormat("<a href={0}>{0}</a>", urlCreazioneToken);
                    break;
            }


            MailClient.Send("webmaster.dva@minambiente.it", utente.Email, oggettoMail, bodyMail.ToString(), true);

        }

       
       
    }

    public enum TipoEmail
    {
        ResetPassword = 1,
        Registrazione = 2
    }
}
