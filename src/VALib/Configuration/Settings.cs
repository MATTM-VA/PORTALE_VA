using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ElogToolkit;
using VALib.Domain.Entities.Membership;
using System.IO;

namespace VALib.Configuration
{
    internal static class Settings
    {
        static Settings()
        {
            VAConnectionStringWebEvents = "";
            VAConnectionString = "";
            DivaWebConnectionString = "";

            UrlBase = "";
            UrlAdmin = "";

            PathBase = "";
            PathImmagini = "";
            PathDocumentiPortale = "";
            PathDocumentiPortaleTemp = "";
            PathDocumentiMedia = "";
            PathDocumetiCondivisione = "";
            PathDocumetiViaVas = "";
            PathDocumetiAIA = "";
            PathDocumetiAIARegionali = "";

            DurataCacheSecondi = 0;

            SumOggettoID = 0;
            SumDocumentoID = 0;
            DefaultImmagineID = 0;

            DestinatariEmail = "";

            ChiaveCriptazione = "sdaVWghvv820l#dsavs£";
            DurataTokenCreazioneUtente = 2;

            MailSmtpServer = "";
            MailSmtpServerPort = 0;
            MailFrom = "";
            MailTo = "";

            ReadSettings();
        }

        internal static string VAConnectionStringWebEvents { get; private set; }
        internal static string VAConnectionString { get; private set; }
        internal static string DivaWebConnectionString { get; private set; }

        internal static string UrlBase { get; private set; }
        internal static string UrlAdmin { get; private set; }

        internal static string PathBase { get; private set; }
        internal static string PathImmagini { get; private set; }
        internal static string PathDocumentiPortale { get; private set; }
        internal static string PathDocumentiPortaleTemp { get; private set; }
        internal static string PathDocumentiMedia { get; private set; }
        internal static string PathDocumetiCondivisione { get; private set; }
        internal static string PathDocumetiViaVas { get; private set; }
        internal static string PathDocumetiAIA { get; private set; }
        internal static string PathConfermaRegistrazione { get; private set; }
        internal static string PathDocumetiAIARegionali { get; private set; }
        internal static string PathDocumetiAiaEventi { get; private set; }
        internal static int DurataCacheSecondi { get; private set; }

        internal static int SumOggettoID { get; private set; }
        internal static int SumDocumentoID { get; private set; }
        internal static int DefaultImmagineID { get; private set; }

        internal static string DestinatariEmail { get; private set; }

        internal static string ChiaveCriptazione { get; private set; }
        internal static int DurataTokenCreazioneUtente { get; private set; }

        internal static string MailSmtpServer { get; private set; }
        internal static int MailSmtpServerPort { get; private set; }
        internal static string MailFrom { get; private set; }
        internal static string MailTo { get; private set; }
  

        private static void ReadSettings()
        {
            VAConnectionStringWebEvents = ConfigurationManager.ConnectionStrings["VAConnectionStringWebEvents"].ConnectionString;
            VAConnectionString = ConfigurationManager.ConnectionStrings["VAConnectionString"].ConnectionString;
            DivaWebConnectionString = ConfigurationManager.ConnectionStrings["DivaWebConnectionString"].ConnectionString;

            UrlBase = ConfigurationManager.AppSettings["UrlBase"];
            UrlAdmin = ConfigurationManager.AppSettings["UrlAdmin"];

            PathBase = ConfigurationManager.AppSettings["PathBase"];
            PathImmagini = ConfigurationManager.AppSettings["PathImmagini"];
            PathDocumentiPortale = ConfigurationManager.AppSettings["PathDocumentiPortale"];
            PathDocumentiPortaleTemp = ConfigurationManager.AppSettings["PathDocumentiPortaleTemp"];
            PathDocumentiMedia = ConfigurationManager.AppSettings["PathDocumentiMedia"];
            PathDocumetiCondivisione = ConfigurationManager.AppSettings["PathDocumetiCondivisione"];
            PathDocumetiViaVas = ConfigurationManager.AppSettings["PathDocumetiViaVas"];
            PathDocumetiAIA = ConfigurationManager.AppSettings["PathDocumetiAIA"];
            PathDocumetiAIARegionali = ConfigurationManager.AppSettings["PathDocumetiAiaRegionali"];
            PathDocumetiAiaEventi = ConfigurationManager.AppSettings["PathDocumetiAiaEventi"];

            String[] paths = new String[] { PathImmagini, PathDocumentiPortale, PathDocumentiPortaleTemp, PathDocumentiMedia,
                                            PathDocumetiCondivisione,PathDocumetiViaVas,PathDocumetiAIA,PathDocumetiAIARegionali,PathDocumetiAiaEventi};
           
            DurataCacheSecondi = Parse.ToInt32OrDefault(ConfigurationManager.AppSettings["DurataCacheSecondi"], 10);

            SumOggettoID = Parse.ToInt32OrDefault(ConfigurationManager.AppSettings["SumOggettoID"], 0);
            SumDocumentoID = Parse.ToInt32OrDefault(ConfigurationManager.AppSettings["SumDocumentoID"], 0);
            DefaultImmagineID = Parse.ToInt32OrDefault(ConfigurationManager.AppSettings["DefaultImmagineID"], 0);

            DestinatariEmail = ConfigurationManager.AppSettings["DestinatariEmail"];

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ChiaveCriptazione"]))
                ChiaveCriptazione = ConfigurationManager.AppSettings["ChiaveCriptazione"];

            DurataTokenCreazioneUtente = Parse.ToInt32OrDefault(ConfigurationManager.AppSettings["DurataTokenCreazioneUtente"], 2);

            MailSmtpServer = ConfigurationManager.AppSettings["MailSmtpServer"]; 
            MailSmtpServerPort = Parse.ToInt32OrDefault(ConfigurationManager.AppSettings["MailSmtpServerPort"], 25);
            MailFrom = ConfigurationManager.AppSettings["MailFrom"];
            MailTo = ConfigurationManager.AppSettings["MailTo"];
        }
    }
}
