using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Services;

namespace VALib.Web.Mvc
{
    public static class HtmlHelperExtensions
    {

        public static MvcHtmlString BooleanToString(this HtmlHelper htmlHelper, bool value, string trueText, string falseText)
        {
            if (value)
                return new MvcHtmlString(trueText);
            else
                return new MvcHtmlString(falseText);
        }

        public static MvcHtmlString VaPaginatore(this HtmlHelper htmlHelper, int pagina, int dimensionePagina, int numeroTotale, string pageParam, Func<RouteValueDictionary, string> pageUrl)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            NameValueCollection qnv = HttpContext.Current.Request.QueryString;
            StringBuilder result = new StringBuilder();
            int numeroPagine = (int)Math.Ceiling((decimal)numeroTotale / dimensionePagina);

            foreach (string key in qnv)
            {
                rvd.Add(key, qnv[key] ?? "5");
            }

            if (!rvd.ContainsKey(pageParam))
                rvd.Add(pageParam, null);

            TagBuilder tagContainer = new TagBuilder("div");
            tagContainer.AddCssClass("paginatore");

            TagBuilder tagStatus = new TagBuilder("li");
            tagStatus.SetInnerText(string.Format(DizionarioService.GRIGLIA_PaginatorePagine, pagina, numeroPagine));
            tagStatus.AddCssClass("etichettaRicerca");

            TagBuilder tagPaginatore = new TagBuilder("ul");
            tagPaginatore.AddCssClass("pagination");

            tagPaginatore.InnerHtml += tagStatus.ToString();

            if (pagina > 1)
            {
                TagBuilder tagPagina = new TagBuilder("li");

                TagBuilder tagLink = new TagBuilder("a");
                rvd[pageParam] = 1;
                tagLink.MergeAttribute("href", pageUrl(rvd));
                tagLink.InnerHtml = DizionarioService.GRIGLIA_PaginatorePrimaPagina;

                tagPagina.InnerHtml = tagLink.ToString();

                tagPaginatore.InnerHtml += tagPagina.ToString();

                tagPagina = new TagBuilder("li");

                tagLink = new TagBuilder("a");
                rvd[pageParam] = pagina - 1;
                tagLink.MergeAttribute("href", pageUrl(rvd));
                tagLink.InnerHtml = "&laquo;";

                tagPagina.InnerHtml = tagLink.ToString();

                tagPaginatore.InnerHtml += tagPagina.ToString();
            }
            //else
            //{
            //    TagBuilder tag = new TagBuilder("li");
            //    tag.InnerHtml = DizionarioService.GRIGLIA_PaginatorePrimaPagina;

            //    tagPaginatore.InnerHtml += tag.ToString();

            //    tag = new TagBuilder("li");
            //    tag.InnerHtml = "&laquo;";

            //    tagPaginatore.InnerHtml += tag.ToString();
            //}

            int paginaInizio = pagina - (pagina % 10);

            if (paginaInizio == pagina)
                paginaInizio -= 9;
            else
                paginaInizio++;

            for (int i = paginaInizio; i <= numeroPagine && i < paginaInizio + 10; i++)
            {
                TagBuilder tagPaginaS = new TagBuilder("li");

                TagBuilder tagLinkS = new TagBuilder("a");
                rvd[pageParam] = i;
                tagLinkS.MergeAttribute("href", pageUrl(rvd));
                tagLinkS.SetInnerText(i.ToString());

                if (i == pagina)
                    tagPaginaS.AddCssClass("active");

                tagPaginaS.InnerHtml = tagLinkS.ToString();

                tagPaginatore.InnerHtml += tagPaginaS.ToString();
            }

            if (pagina < numeroPagine)
            {
                TagBuilder tagPaginaS2 = new TagBuilder("li");

                TagBuilder tagLinkS2 = new TagBuilder("a");
                rvd[pageParam] = pagina + 1;
                tagLinkS2.MergeAttribute("href", pageUrl(rvd));
                tagLinkS2.InnerHtml = "&raquo;";

                tagPaginaS2.InnerHtml = tagLinkS2.ToString();

                tagPaginatore.InnerHtml += tagPaginaS2.ToString();

                tagPaginaS2 = new TagBuilder("li");

                tagLinkS2 = new TagBuilder("a");
                rvd[pageParam] = numeroPagine;
                tagLinkS2.MergeAttribute("href", pageUrl(rvd));
                tagLinkS2.InnerHtml = DizionarioService.GRIGLIA_PaginatoreUltimaPagina;

                tagPaginaS2.InnerHtml = tagLinkS2.ToString();

                tagPaginatore.InnerHtml += tagPaginaS2.ToString();
            }
            //else
            //{
            //    TagBuilder tag = new TagBuilder("li");
            //    tag.InnerHtml = "&raquo;";
            //    tagPaginatore.InnerHtml += tag.ToString();

            //    tag = new TagBuilder("li");
            //    tag.InnerHtml = "ultima";

            //    tagPaginatore.InnerHtml += tag.ToString();
            //}

            //tagContainer.InnerHtml += tagStatus.ToString();
            tagContainer.InnerHtml += tagPaginatore.ToString();

            return MvcHtmlString.Create(tagContainer.ToString());
        }

        public static MvcHtmlString VaPaginatore(this HtmlHelper htmlHelper, int pagina, int dimensionePagina, int numeroTotale, string pageParam, string anchor, Func<RouteValueDictionary, string> pageUrl)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            NameValueCollection qnv = HttpContext.Current.Request.QueryString;
            StringBuilder result = new StringBuilder();
            int numeroPagine = (int)Math.Ceiling((decimal)numeroTotale / dimensionePagina);

            foreach (string key in qnv)
            {
                rvd.Add(key, qnv[key] ?? "5");
            }

            if (!rvd.ContainsKey(pageParam))
                rvd.Add(pageParam, null);

            TagBuilder tagContainer = new TagBuilder("div");
            tagContainer.AddCssClass("paginatore");

            TagBuilder tagStatus = new TagBuilder("li");
            tagStatus.SetInnerText(string.Format(DizionarioService.GRIGLIA_PaginatorePagine, pagina, numeroPagine));
            tagStatus.AddCssClass("etichettaRicerca");

            TagBuilder tagPaginatore = new TagBuilder("ul");
            tagPaginatore.AddCssClass("pagination");

            tagPaginatore.InnerHtml += tagStatus.ToString();

            if (pagina > 1)
            {
                TagBuilder tagPagina = new TagBuilder("li");

                TagBuilder tagLink = new TagBuilder("a");
                rvd[pageParam] = 1;

                if (string.IsNullOrWhiteSpace(anchor))
                    tagLink.MergeAttribute("href", pageUrl(rvd));
                else
                    tagLink.MergeAttribute("href", pageUrl(rvd) + "#" + anchor);

                tagLink.InnerHtml = DizionarioService.GRIGLIA_PaginatorePrimaPagina;

                tagPagina.InnerHtml = tagLink.ToString();

                tagPaginatore.InnerHtml += tagPagina.ToString();

                tagPagina = new TagBuilder("li");

                tagLink = new TagBuilder("a");
                rvd[pageParam] = pagina - 1;
                if (string.IsNullOrWhiteSpace(anchor))
                    tagLink.MergeAttribute("href", pageUrl(rvd));
                else
                    tagLink.MergeAttribute("href", pageUrl(rvd) + "#" + anchor);

                tagLink.InnerHtml = "&laquo;";

                tagPagina.InnerHtml = tagLink.ToString();

                tagPaginatore.InnerHtml += tagPagina.ToString();
            }
            //else
            //{
            //    TagBuilder tag = new TagBuilder("li");
            //    tag.InnerHtml = DizionarioService.GRIGLIA_PaginatorePrimaPagina;

            //    tagPaginatore.InnerHtml += tag.ToString();

            //    tag = new TagBuilder("li");
            //    tag.InnerHtml = "&laquo;";

            //    tagPaginatore.InnerHtml += tag.ToString();
            //}

            int paginaInizio = pagina - (pagina % 10);

            if (paginaInizio == pagina)
                paginaInizio -= 9;
            else
                paginaInizio++;

            for (int i = paginaInizio; i <= numeroPagine && i < paginaInizio + 10; i++)
            {
                TagBuilder tagPaginaS = new TagBuilder("li");

                TagBuilder tagLinkS = new TagBuilder("a");
                rvd[pageParam] = i;
                if (string.IsNullOrWhiteSpace(anchor))
                    tagLinkS.MergeAttribute("href", pageUrl(rvd));
                else
                    tagLinkS.MergeAttribute("href", pageUrl(rvd) + "#" + anchor);
                tagLinkS.SetInnerText(i.ToString());

                if (i == pagina)
                    tagPaginaS.AddCssClass("active");

                tagPaginaS.InnerHtml = tagLinkS.ToString();

                tagPaginatore.InnerHtml += tagPaginaS.ToString();
            }

            if (pagina < numeroPagine)
            {
                TagBuilder tagPaginaS2 = new TagBuilder("li");

                TagBuilder tagLinkS2 = new TagBuilder("a");
                rvd[pageParam] = pagina + 1;
                if (string.IsNullOrWhiteSpace(anchor))
                    tagLinkS2.MergeAttribute("href", pageUrl(rvd));
                else
                    tagLinkS2.MergeAttribute("href", pageUrl(rvd) + "#" + anchor);
                tagLinkS2.InnerHtml = "&raquo;";

                tagPaginaS2.InnerHtml = tagLinkS2.ToString();

                tagPaginatore.InnerHtml += tagPaginaS2.ToString();

                tagPaginaS2 = new TagBuilder("li");

                tagLinkS2 = new TagBuilder("a");
                rvd[pageParam] = numeroPagine;
                if (string.IsNullOrWhiteSpace(anchor))
                    tagLinkS2.MergeAttribute("href", pageUrl(rvd));
                else
                    tagLinkS2.MergeAttribute("href", pageUrl(rvd) + "#" + anchor);
                tagLinkS2.InnerHtml = DizionarioService.GRIGLIA_PaginatoreUltimaPagina;

                tagPaginaS2.InnerHtml = tagLinkS2.ToString();

                tagPaginatore.InnerHtml += tagPaginaS2.ToString();
            }
            //else
            //{
            //    TagBuilder tag = new TagBuilder("li");
            //    tag.InnerHtml = "&raquo;";
            //    tagPaginatore.InnerHtml += tag.ToString();

            //    tag = new TagBuilder("li");
            //    tag.InnerHtml = "ultima";

            //    tagPaginatore.InnerHtml += tag.ToString();
            //}

            //tagContainer.InnerHtml += tagStatus.ToString();
            tagContainer.InnerHtml += tagPaginatore.ToString();

            return MvcHtmlString.Create(tagContainer.ToString());
        }

        public static MvcHtmlString VaEsporta(this HtmlHelper htmlHelper, Func<RouteValueDictionary, string> pageUrl)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            NameValueCollection qnv = HttpContext.Current.Request.QueryString;
            StringBuilder result = new StringBuilder();

            foreach (string key in qnv)
            {
                rvd.Add(key, qnv[key] ?? "5");
            }

            if (!rvd.ContainsKey("mode"))
                rvd.Add("mode", "export");

            TagBuilder tagExport = new TagBuilder("a");
            tagExport.AddCssClass("button esportaButton external");
            tagExport.MergeAttribute("href", pageUrl(rvd));
            tagExport.InnerHtml = DizionarioService.RICERCA_Esporta;

            return MvcHtmlString.Create(tagExport.ToString());
        }

        public static string ActionWithFragment(this UrlHelper helper, string actionName, string fragment)
        {
            return string.Concat(helper.Action(actionName), "#", fragment);
        }

        public static string ActionWithFragment(this UrlHelper helper, string actionName, object routeValues, string fragment)
        {
            return string.Concat(helper.Action(actionName, routeValues), "#", fragment);
        }


        public static MvcHtmlString TruncateAtWord(this HtmlHelper htmlHelper, string input, int length)
        {
            if (input == null || input.Length <= length)
                return new MvcHtmlString(input);
            else
            {
                int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
                string text = string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
                return new MvcHtmlString(text);
            }

        }

    }
}
