using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VAPortale
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Legacy
            routes.MapRoute(
                name: "Legacy_Default",
                url: "Default.aspx",
                defaults: new { controller = "Legacy", action = "Index" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_InfoVia",
                url: "Ricerca/SchedaProgetto.aspx",
                defaults: new { controller = "Legacy", action = "InfoVia" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_InfoVas",
                url: "Ricerca/SchedaPianoProgramma.aspx",
                defaults: new { controller = "Legacy", action = "InfoVas" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_DocVia",
                url: "Ricerca/DettaglioProgetto.aspx",
                defaults: new { controller = "Legacy", action = "DocumentazioneVia" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_DocVas",
                url: "Ricerca/DettaglioPianoProgramma.aspx",
                defaults: new { controller = "Legacy", action = "DocumentazioneVas" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_MetadatoDocumentoVia",
                url: "Ricerca/DettaglioDocumentoVIA.aspx",
                defaults: new { controller = "Legacy", action = "MetadatoDocumentoVia" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_MetadatoDocumentoVas",
                url: "Ricerca/DettaglioDocumentoVAS.aspx",
                defaults: new { controller = "Legacy", action = "MetadatoDocumentoVas" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_File_MediaUmbraco",
                url: "Media/{cartella}/{nomeFile}",
                defaults: new { controller = "File", action = "DocumentoMedia" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "File_Ext",
                url: "File/{action}/{id}.{ext}",
                defaults: new { controller = "File", ext = UrlParameter.Optional },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "File_Default",
                url: "File/{action}/{id}",
                defaults: new { controller = "File", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Legacy_File_Media",
                url: "File/DocumentoMedia/{cartella}/{nomeFile}",
                defaults: new { controller = "File", action = "DocumentoMedia" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            // Home
            routes.MapRoute(
               name: "Home_RootContent_Robots",
               url: "robots.txt",
               defaults: new { controller = "Home", action = "Robots" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Home_Index",
                url: "{lang}",
                defaults: new { controller = "Home", action = "Index", lang = UrlParameter.Optional },
                namespaces: new[] { "VAPortale.Controllers" }
            );

          

            routes.MapRoute(
                name: "PaginaStatica",
                url: "{lang}/ps/{nomeSezione}/{nomeVoce}",
                defaults: new { controller = "Pagine", action = "Voce", lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            
            routes.MapRoute(
                name: "ProcedureInCorsoVia",
                url: "{lang}/Procedure/ViaElenco/{id}/{parametro}",
                defaults: new { controller = "Procedure", action = "ViaElenco", id = 0, oggettoProceduraID = 0, lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "ProcedureInCorsoVas",
                url: "{lang}/Procedure/VasElenco/{id}/{parametro}",
                defaults: new { controller = "Procedure", action = "VasElenco", id = 0, parametro = 0, lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "ProcedureInCorsoAia",
                url: "{lang}/Procedure/AiaElenco/{id}/{parametro}",
                defaults: new { controller = "Procedure", action = "AiaElenco", id = 0, parametro = 0, lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );


            routes.MapRoute(
                name: "ProcedureConsultazioniTransfrontaliere",
                url: "{lang}/Procedure/ConsultazioniTransfrontaliere/{NomeAttributo}",
                defaults: new { controller = "Procedure", action = "ConsultazioniTransfrontaliere", NomeAttributo = "VAS", lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
               name: "ProcedureProcedureIntegrateECoordinate",
               url: "{lang}/Procedure/ProcedureIntegrateECoordinate/{NomeAttributo}",
               defaults: new { controller = "Procedure", action = "ProcedureIntegrateECoordinate", NomeAttributo = "VAS-Valutazione incidenza", lang = "it-IT" },
               namespaces: new[] { "VAPortale.Controllers" }
           );


            routes.MapRoute(
               name: "ProcedureAvvisiAlPubblico",
               url: "{lang}/Procedure/AvvisiAlPubblico/{NomeAttributo}",
               defaults: new { controller = "Procedure", action = "AvvisiAlPubblico", NomeAttributo = "VIA", lang = "it-IT" },
               namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Documentazione",
                url: "{lang}/Oggetti/Documentazione/{id}/{oggettoProceduraID}",
                defaults: new { controller = "Oggetti", action = "Documentazione", id = 0, oggettoProceduraID = 0, lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
               name: "ElencoNotizieDaFeed",
               url: "{lang}/Comunicazione/ElencoNotizieFeed/{categoria}",
               defaults: new { controller = "Comunicazione", action = "ElencoNotizieFeed", categoria = "", lang = "it-IT" },
               namespaces: new[] { "VAPortale.Controllers" }
           );

            routes.MapRoute(
                name: "DettaglioNotiziaDaFeed",
                url: "{lang}/Comunicazione/NotiziaFeed/{categoria}/{id}",
                defaults: new { controller = "Comunicazione", action = "DettaglioNotizieFeed", id = 0, categoria = "", lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

         
            routes.MapRoute(
                name: "Error",
                url: "Error/{action}/{id}",
                defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional, lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, lang = "it-IT" },
                namespaces: new[] { "VAPortale.Controllers" }
            );


        }
    }
}