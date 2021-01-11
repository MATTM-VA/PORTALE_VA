using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Repositories.Contenuti;
using VALib.Domain.Entities.DatiAmbientali;
using VALib.Domain.Repositories.DatiAmbientali;
using System.Data;
using VALib.Helpers;
using VALib.Domain.Entities.UI;
using System.Globalization;
using System.IO;

namespace VAPortale.Code
{
    public static class ModelUtils
    {
        public static IEnumerable<SelectListItem> CreaProceduraSelectList(MacroTipoOggettoEnum macroTipoOggetto, bool insetEmptyElement)
        {
            List<SelectListItem> booleanSelectList = new List<SelectListItem>();
            List<Procedura> procedure = ProceduraRepository.Instance.RecuperaProcedure(macroTipoOggetto);

            string tuttoText = "[tutte]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insetEmptyElement)
                booleanSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "0" });

            foreach (Procedura procedura in procedure)
            {
                booleanSelectList.Add(new SelectListItem() { Text = procedura.GetNome(), Value = procedura.ID.ToString() });
            }

            return booleanSelectList;
        }

        public static IEnumerable<SelectListItem> CreaTipologiaSelectList(bool insetEmptyElement)
        {
            List<SelectListItem> tipologiaSelectList = new List<SelectListItem>();
            List<Tipologia> tipologie = TipologiaRepository.Instance.RecuperaTipologie();

            string tuttoText = "[tutte]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insetEmptyElement)
                tipologiaSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });

            foreach (Tipologia tipologia in tipologie)
            {
                tipologiaSelectList.Add(new SelectListItem() { Text = tipologia.GetNome(), Value = tipologia.ID.ToString() });
            }

            return tipologiaSelectList;
        }

        public static IEnumerable<SelectListItem> CreaSettoreSelectList(bool insetEmptyElement)
        {
            List<SelectListItem> settoreSelectList = new List<SelectListItem>();
            List<Settore> settori = SettoreRepository.Instance.RecuperaSettori();

            string tuttoText = "[tutti]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insetEmptyElement)
                settoreSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });

            foreach (Settore settore in settori)
            {
                settoreSelectList.Add(new SelectListItem() { Text = settore.GetNome(), Value = settore.ID.ToString() });
            }

            return settoreSelectList;
        }

        public static IEnumerable<SelectListItem> CreaTipologiaTerritorioSelectList(bool insetEmptyElement)
        {
            List<SelectListItem> tipologiaTerritorioSelectList = new List<SelectListItem>();
            List<TipologiaTerritorio> tipologieTerritorio = TipologiaTerritorioRepository.Instance.RecuperaTipologieTerritorio();

            string tuttoText = "[tutte]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insetEmptyElement)
                tipologiaTerritorioSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });

            foreach (TipologiaTerritorio tipologiaTerritorio in tipologieTerritorio)
            {
                if (tipologiaTerritorio.MostraRicerca)
                    tipologiaTerritorioSelectList.Add(new SelectListItem() { Text = tipologiaTerritorio.GetNome(), Value = tipologiaTerritorio.ID.ToString() });
            }

            return tipologiaTerritorioSelectList;
        }

        public static IEnumerable<SelectListItem> CreaBooleanSelectList()
        {
            List<SelectListItem> booleanSelectList = new List<SelectListItem>();

            booleanSelectList.Add(new SelectListItem() { Text = "", Value = "" });
            booleanSelectList.Add(new SelectListItem() { Text = "Sì", Value = "True" });
            booleanSelectList.Add(new SelectListItem() { Text = "No", Value = "False" });

            return booleanSelectList;
        }

        public static IEnumerable<SelectListItem> CreaCategoriaNotiziaSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> categoriaSelectList = new List<SelectListItem>();
            IEnumerable<CategoriaNotizia> categorie = CategoriaNotiziaRepository.Instance.RecuperaCategorieNotizie();

            if (insertEmptyElement)
                categoriaSelectList.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (CategoriaNotizia categoria in categorie)
            {
                categoriaSelectList.Add(new SelectListItem() { Text = categoria.GetNome("it"), Value = categoria.ID.ToString() });
            }

            return categoriaSelectList;
        }

        public static IEnumerable<SelectListItem> CreaVociMenuSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> vociMenuSelectList = new List<SelectListItem>();
            IEnumerable<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();

            if (insertEmptyElement)
                vociMenuSelectList.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (VoceMenu voceMenu in vociMenu)
            {
                vociMenuSelectList.Add(new SelectListItem()
                {
                    Text = String.Format("{0} | {1}", voceMenu.Sezione, voceMenu.GetNome("it")),
                    Value = voceMenu.ID.ToString()
                });
            }

            return vociMenuSelectList;
        }


        public static IEnumerable<SelectListItem> CreaStatoNotiziaSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> statoNotiziaSelectList = new List<SelectListItem>();

            if (insertEmptyElement)
                statoNotiziaSelectList.Add(new SelectListItem() { Text = "", Value = "" });

            statoNotiziaSelectList.Add(new SelectListItem() { Text = "Bozza", Value = StatoNotiziaEnum.Bozza.ToString() });
            statoNotiziaSelectList.Add(new SelectListItem() { Text = "Pubblicabile", Value = StatoNotiziaEnum.Pubblicabile.ToString() });

            return statoNotiziaSelectList;
        }

        public static IEnumerable<SelectListItem> CreaImmaginiSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> immaginiSelectList = new List<SelectListItem>();
            int rows = 0;
            List<Immagine> immagini = ImmagineRepository.Instance.RecuperaImmagini("", 0, int.MaxValue, out rows);

            if (insertEmptyElement)
                immaginiSelectList.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (Immagine immagine in immagini)
            {
                immaginiSelectList.Add(new SelectListItem() { Text = immagine.GetNome("it"), Value = immagine.ID.ToString() });
            }

            return immaginiSelectList;
        }

        public static IEnumerable<SelectListItem> CreaImmaginiCaroselloSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> immaginiSelectList = new List<SelectListItem>();
            List<Immagine> immagini = ImmagineRepository.Instance.RecuperaImmaginiPerCarosello();

            if (insertEmptyElement)
                immaginiSelectList.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (Immagine immagine in immagini)
            {
                immaginiSelectList.Add(new SelectListItem() { Text = immagine.GetNome("it"), Value = immagine.ID.ToString() });
            }

            return immaginiSelectList;
        }

        public static IEnumerable<SelectListItem> CreaImmaginiDatoAmbientaleHomeSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> immaginiSelectList = new List<SelectListItem>();
            List<Immagine> immagini = ImmagineRepository.Instance.RecuperaImmaginiPerDatoAmbientaleHome();

            if (insertEmptyElement)
                immaginiSelectList.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (Immagine immagine in immagini)
            {
                immaginiSelectList.Add(new SelectListItem() { Text = immagine.GetNome("it"), Value = immagine.ID.ToString() });
            }

            return immaginiSelectList;
        }

        public static IEnumerable<SelectListItem> CreaTemaSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> temaSelectList = new List<SelectListItem>();
            List<Tema> temi = TemaRepository.Instance.RecuperaTemi();

            string tuttoText = "[tutti]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insertEmptyElement)
                temaSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });

            foreach (Tema tema in temi)
            {
                temaSelectList.Add(new SelectListItem() { Text = tema.Nome, Value = tema.ID.ToString() });
            }

            return temaSelectList;
        }

        public static IEnumerable<SelectListItem> CreaTipoWidgetSelectList(bool insertEmptyElement)
        {
            List<SelectListItem> temaSelectList = new List<SelectListItem>();

            string tuttoText = "[tutti]";

            if (insertEmptyElement)

                temaSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });
            temaSelectList.Add(new SelectListItem() { Text = "Notizie", Value = TipoWidget.Notizie.ToString() });
            temaSelectList.Add(new SelectListItem() { Text = "Embed", Value = TipoWidget.Embed.ToString() });
            temaSelectList.Add(new SelectListItem()
            {
                Text = "In Evidenza",
                Value = TipoWidget.InEvidenza.ToString(),
                Selected = true
            });
            temaSelectList.Add(new SelectListItem() { Text = "Sezione", Value = TipoWidget.Sezione.ToString() });

            return temaSelectList;
        }

        public static List<String> CreaIconeList()
        {
            List<String> icone = new List<String>();
            String pathIcone = HttpContext.Current.Server.MapPath("~/Content/images/iconeSezioneHP");
            foreach (String filename in Directory.GetFiles(pathIcone, "*.png"))
            {
                icone.Add(filename.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length).Replace("\\", "/"));
            }
            return icone;
        }


        public static IEnumerable<SelectListItem> CreaAnnoSelectList(int maggioreUgualeA, bool insetEmptyElement)
        {
            List<SelectListItem> anniSelectList = new List<SelectListItem>();

            string tuttoText = "[tutto]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insetEmptyElement)
                anniSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });
            int i = DateTime.Now.Year;

            while (i >= maggioreUgualeA)
            {
                anniSelectList.Add(new SelectListItem() { Text = i.ToString(CultureInfo.InvariantCulture), Value = i.ToString(CultureInfo.InvariantCulture) });
                i--;
            }

            return anniSelectList;
        }

        public static void AggiungiRiga(DataTable dt, string chiave, string valore)
        {
            DataRow dr = dt.NewRow();
            dr[0] = chiave;
            dr[1] = valore;

            dt.Rows.Add(dr);
        }

        public static IEnumerable<SelectListItem> CreaNotiziaSelectList(int idCategoria = 0)
        {
            List<SelectListItem> notiziaSelectList = new List<SelectListItem>();
            //IEnumerable<CategoriaNotizia> categorie = NotiziaRepository.Instance.RecuperaNotizie()

            int risultato = 100;

            List<Notizia> notizie = NotiziaRepository.Instance.RecuperaNotizie(CultureHelper.GetCurrentCultureShortName(),
                            null, null, null, idCategoria, true, StatoNotiziaEnum.Pubblicabile,
                            0, 100, out risultato);

            foreach (Notizia notizia in notizie)
            {
                //int caratteri = (notizia.Titolo_IT.Length <= 150 ? notizia.Titolo_IT.Length : 150);                
                //notiziaSelectList.Add(new SelectListItem() { Text = notizia.Titolo_IT.Substring(0, caratteri), Value = notizia.ID.ToString() });

                int caratteri = (notizia.TitoloBreve_IT.Length <= 150 ? notizia.TitoloBreve_IT.Length : 150);
                notiziaSelectList.Add(new SelectListItem() { Text = String.Format("{0:dd/MM/yyyy}", notizia.DataInserimento) + " - " + notizia.TitoloBreve_IT.Substring(0, caratteri), Value = notizia.ID.ToString() });
            }

            return notiziaSelectList;
        }

        public static List<Notizia> NotizieByCategoria(int idCategoria)
        {
            int risultato = 100;

            List<Notizia> notizie = NotiziaRepository.Instance.RecuperaNotizie(CultureHelper.GetCurrentCultureShortName(),
                            null, null, null, idCategoria, true, StatoNotiziaEnum.Pubblicabile,
                            0, 100, out risultato);

            return notizie;
        }

        public static IEnumerable<SelectListItem> CreaCategoriaSelectList(bool insetEmptyElement)
        {
            List<SelectListItem> categoriaSelectList = new List<SelectListItem>();
            List<CategoriaImpianto> categorie = CategoriaImpiantoRepository.Instance.RecuperaCategorie();

            string tuttoText = "[tutte]";

            if (CultureHelper.GetCurrentCultureShortName().Equals("en", StringComparison.InvariantCultureIgnoreCase))
                tuttoText = "[all]";

            if (insetEmptyElement)
                categoriaSelectList.Add(new SelectListItem() { Text = tuttoText, Value = "" });

            foreach (CategoriaImpianto categoria in categorie)
            {
                categoriaSelectList.Add(new SelectListItem() { Text = categoria.GetNome(), Value = categoria.ID.ToString() });
            }

            return categoriaSelectList;
        }

    }
}