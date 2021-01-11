using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Repositories.UI;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Entities.UI;
using VAPortale.Code;
using VALib.Domain.Services;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using VALib.Web;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;
using System.Text.RegularExpressions;
using VALib.Helpers;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreWidget)]
    public class WidgetController : Controller
    {
        //
        // GET: /Admin/Widget/

        public ActionResult Index(WidgetIndexModel model)
        {
            int totale = 0;
            model.Widget = WidgetRepository.Instance.RecuperaWidget(model.Testo, model.TipoWidget, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);

            model.TipoWidgetSelectList = ModelUtils.CreaTipoWidgetSelectList(false);
            model.TotaleRisultati = totale;

            if (model.TipoWidget == TipoWidget.Notizie)
                model.EditaActionName = "Edita";
            else if (model.TipoWidget == TipoWidget.Embed)
                model.EditaActionName = "EditaEmbed";
            else if (model.TipoWidget == TipoWidget.InEvidenza)
                model.EditaActionName = "EditaInEvidenza";
            else if (model.TipoWidget == TipoWidget.Sezione)
                model.EditaActionName = "EditaSezione";

            return View(model);
        }

        public ActionResult Crea()
        {
            WidgetNotiziaEditaModel model = new WidgetNotiziaEditaModel();
            model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);

            return View("Edita", model);
        }

        public ActionResult CreaEmbed()
        {
            WidgetEmbedEditaModel model = new WidgetEmbedEditaModel();
            model.BooleanSelectList = ModelUtils.CreaBooleanSelectList();

            return View("EditaEmbed", model);
        }

        public ActionResult CreaInEvidenza()
        {
            WidgetInEvidenzaEditaModel model = new WidgetInEvidenzaEditaModel();
            model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(false);
            model.NotizieSelectList = ModelUtils.CreaNotiziaSelectList();
            //model.EditaCategoriaNotiziaID = 1;
            //model.InEvidenzaList = InEvidenzaRepository.Instance.RecuperaAllInEvidenza();

            return View("EditaInEvidenza", model);
        }

        public ActionResult CreaSezione()
        {
            WidgetSezioneEditaModel model = new WidgetSezioneEditaModel();
            model.VociMenuSelectList = ModelUtils.CreaVociMenuSelectList(true).OrderBy(x => x.Text);
            model.SelezioneLinkVoce = "voce";
            model.IconeList = ModelUtils.CreaIconeList();

            return View("EditaSezione", model);
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            WidgetNotiziaEditaModel model = new WidgetNotiziaEditaModel();
            Widget widget = null;

            widget = WidgetRepository.Instance.RecuperaWidget(id);

            if (widget != null)
            {
                model.Widget = widget;

                model.ID = id;
                model.EditaNome = widget.Nome_IT;
                model.EditaCategoriaNotiziaID = widget.Categoria.ID;
                model.EditaMax = widget.NumeroElementi.Value;

                model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);
                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditaEmbed(int id)
        {
            ActionResult result = null;
            WidgetEmbedEditaModel model = new WidgetEmbedEditaModel();
            Widget widget = null;

            widget = WidgetRepository.Instance.RecuperaWidget(id);

            if (widget != null)
            {
                model.Widget = widget;

                model.ID = id;
                model.EditaNome_IT = widget.Nome_IT;
                model.EditaNome_EN = widget.Nome_EN;
                model.EditaContenuto_IT = UrlUtility.VAHtmlReplacePseudoUrls(widget.Contenuto_IT);
                model.EditaContenuto_EN = UrlUtility.VAHtmlReplacePseudoUrls(widget.Contenuto_EN);
                model.MostraTitolo = widget.MostraTitolo;
                model.BooleanSelectList = ModelUtils.CreaBooleanSelectList();


                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditaInEvidenza(int id)
        {
            ActionResult result = null;
            WidgetInEvidenzaEditaModel model = new WidgetInEvidenzaEditaModel();
            Widget widget = null;

            widget = WidgetRepository.Instance.RecuperaWidget(id);

            if (widget != null)
            {
                model.Widget = widget;

                model.ID = id;
                model.EditaNome = widget.Nome_IT;
                model.EditaCategoriaNotiziaID = widget.Categoria.ID;

                model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(false);
                model.NotizieSelectList = ModelUtils.CreaNotiziaSelectList(widget.Categoria.ID);

                model.EditaNotiziaInEvidenza = widget.NotiziaID;
                if (widget.NotiziaID != null)
                {
                    Notizia notizia = NotiziaRepository.Instance.RecuperaNotizia((int)widget.NotiziaID);
                    if (notizia != null) model.ImmagineID = notizia.ImmagineID;
                }

                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult EditaSezione(int id)
        {
            ActionResult result = null;
            WidgetSezioneEditaModel model = new WidgetSezioneEditaModel();
            Widget widget = null;

            widget = WidgetRepository.Instance.RecuperaWidget(id);

            if (widget != null)
            {
                model.Widget = widget;

                model.ID = id;
                model.EditaNome_IT = widget.Nome_IT;
                model.EditaNome_EN = widget.Nome_EN;

                if (!String.IsNullOrEmpty(widget.Contenuto_IT))
                {
                    if (widget.VoceMenuID == null)
                        foreach (Match match in new Regex(@"<a.*?href=(""|')(?<href>.*?)(""|').*?>(?<value>.*?)</a>").Matches(widget.Contenuto_IT))
                            model.EditaLinkIT = match.Groups["href"].Value;
                    foreach (Match match in new Regex(@"<img.*?src=(""|')(?<src>.*?)(""|').*?").Matches(widget.Contenuto_IT))
                        model.EditaIcona = match.Groups["src"].Value;
                }
                if (!String.IsNullOrEmpty(widget.Contenuto_EN))
                {
                    if (widget.VoceMenuID == null)
                        foreach (Match match in new Regex(@"<a.*?href=(""|')(?<href>.*?)(""|').*?>(?<value>.*?)</a>").Matches(widget.Contenuto_IT))
                            model.EditaLinkEN = match.Groups["href"].Value;
                    foreach (Match match in new Regex(@"<img.*?src=(""|')(?<src>.*?)(""|').*?").Matches(widget.Contenuto_IT))
                        model.EditaIcona = match.Groups["src"].Value;
                }
                model.SelezioneLinkVoce = widget.VoceMenuID != null ? "voce" : "link";
                model.EditaVoceMenuID = widget.VoceMenuID;
                model.VociMenuSelectList = ModelUtils.CreaVociMenuSelectList(true).OrderBy(x => x.Text);
                model.IconeList = ModelUtils.CreaIconeList();
                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edita(WidgetNotiziaEditaModel model)
        {
            ActionResult result = null;
            Widget widget = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                CategoriaNotizia categoria = null;
                ContenutoService cs = new ContenutoService();

                categoria = CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(model.EditaCategoriaNotiziaID.Value);

                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    widget.Nome_IT = model.EditaNome.Trim();
                    widget.Nome_EN = model.EditaNome.Trim();
                }
                else
                {
                    widget = cs.CreaWidget(model.EditaNome.Trim(), TipoWidget.Notizie);
                    widget.Nome_EN = widget.Nome_IT;
                }

                widget.Categoria = categoria;
                widget.NumeroElementi = model.EditaMax;

                switch (widget.Categoria.Enum)
                {
                    case CategoriaNotiziaEnum.Nessuna:
                        break;
                    case CategoriaNotiziaEnum.EventiENotizie:
                        widget.VoceMenuID = 44;
                        break;
                    case CategoriaNotiziaEnum.LaDirezioneInforma:
                        widget.VoceMenuID = 45;
                        break;
                    case CategoriaNotiziaEnum.AreaGiuridica:
                        widget.VoceMenuID = 46;
                        break;
                    default:
                        break;
                }

                id = cs.SalvaWidget(widget);

                result = RedirectToAction("Edita", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    model.Widget = widget;
                }

                model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditaEmbed(WidgetEmbedEditaModel model)
        {
            ActionResult result = null;
            Widget widget = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                ContenutoService cs = new ContenutoService();

                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    widget.Nome_IT = model.EditaNome_IT.Trim();

                }
                else
                {
                    widget = cs.CreaWidget(model.EditaNome_IT.Trim(), TipoWidget.Embed);
                }

                widget.Nome_EN = model.EditaNome_EN.Trim();
                widget.Contenuto_IT = UrlUtility.VAHtmlReplaceRealUrls(model.EditaContenuto_IT);
                widget.Contenuto_EN = UrlUtility.VAHtmlReplaceRealUrls(model.EditaContenuto_EN);
                widget.MostraTitolo = model.MostraTitolo.Value;

                id = cs.SalvaWidget(widget);

                result = RedirectToAction("EditaEmbed", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    model.Widget = widget;
                }
                model.BooleanSelectList = ModelUtils.CreaBooleanSelectList();

                result = View(model);
            }

            return result;
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditaSezione(WidgetSezioneEditaModel model)
        {
            ActionResult result = null;
            Widget widget = null;


            if (model.SelezioneLinkVoce.Equals("link") && string.IsNullOrEmpty(model.EditaLinkIT))
                ModelState.AddModelError("EditaLinkIT", "Link diretto IT obbligatorio");
            if (model.SelezioneLinkVoce.Equals("link") && string.IsNullOrEmpty(model.EditaLinkEN))
                ModelState.AddModelError("EditaLinkEN", "Link diretto EN obbligatorio");
            if (model.SelezioneLinkVoce.Equals("voce") && model.EditaVoceMenuID == null)
                ModelState.AddModelError("EditaVoceMenuID", "Voce menu obbligatoria");

            if (ModelState.IsValid)
            {
                int id = 0;
                ContenutoService cs = new ContenutoService();

                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                }
                else
                {
                    widget = cs.CreaWidget(model.EditaNome_IT.Trim(), TipoWidget.Sezione);
                }

                widget.Nome_IT = model.EditaNome_IT.Trim();
                widget.Nome_EN = model.EditaNome_EN.Trim();


                String linkIT = "";
                String linkEN = "";

                if (model.SelezioneLinkVoce.Equals("link"))
                {
                    widget.VoceMenuID = null;
                    linkIT = model.EditaLinkIT.Trim();
                    linkEN = model.EditaLinkEN.Trim();
                }
                else if (model.SelezioneLinkVoce.Equals("voce"))
                {
                    widget.VoceMenuID = model.EditaVoceMenuID;
                    VoceMenu VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu((int)model.EditaVoceMenuID);

                    linkIT = VoceMenu.Editabile ?
                        Url.RouteUrl("PaginaStatica", new { nomeSezione = VoceMenu.Sezione, nomeVoce = VoceMenu.Voce, lang = CultureHelper._it }) :
                        Url.RouteUrl("Default", new { controller = VoceMenu.Sezione, action = VoceMenu.Voce, lang = CultureHelper._it });
                    linkEN = VoceMenu.Editabile ?
                        Url.RouteUrl("PaginaStatica", new { nomeSezione = VoceMenu.Sezione, nomeVoce = VoceMenu.Voce, lang = CultureHelper._en }) :
                        Url.RouteUrl("Default", new { controller = VoceMenu.Sezione, action = VoceMenu.Voce, lang = CultureHelper._en });
                }

                String htmlTemplate = "" +
                        "<img src=\"{0}\" alt=\"{2}\">" +
                        "<a href=\"{1}\">{2}</a>";
                widget.Contenuto_IT = String.Format(htmlTemplate, model.EditaIcona, linkIT.TrimStart('/'), widget.Nome_IT);
                widget.Contenuto_EN = String.Format(htmlTemplate, model.EditaIcona, linkEN.TrimStart('/'), widget.Nome_EN);

                id = cs.SalvaWidget(widget);

                result = RedirectToAction("EditaSezione", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    model.Widget = widget;
                }
                model.VociMenuSelectList = ModelUtils.CreaVociMenuSelectList(true).OrderBy(x => x.Text);
                model.IconeList = ModelUtils.CreaIconeList();

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditaInEvidenza(WidgetInEvidenzaEditaModel model)
        {
            ActionResult result = null;
            Widget widget = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                CategoriaNotizia categoria = null;
                ContenutoService cs = new ContenutoService();

                categoria = CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(model.EditaCategoriaNotiziaID.Value);

                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    widget.Nome_IT = model.EditaNome.Trim();
                    widget.Nome_EN = model.EditaNome.Trim();
                }
                else
                {
                    widget = cs.CreaWidget(model.EditaNome.Trim(), TipoWidget.InEvidenza);
                    widget.Nome_EN = widget.Nome_IT;
                }

                widget.Categoria = categoria;

                switch (widget.Categoria.Enum)
                {
                    case CategoriaNotiziaEnum.Nessuna:
                        break;
                    case CategoriaNotiziaEnum.EventiENotizie:
                        widget.VoceMenuID = 44;
                        break;
                    case CategoriaNotiziaEnum.LaDirezioneInforma:
                        widget.VoceMenuID = 45;
                        break;
                    case CategoriaNotiziaEnum.AreaGiuridica:
                        widget.VoceMenuID = 46;
                        break;
                    default:
                        break;
                }

                widget.NotiziaID = model.EditaNotiziaInEvidenza;

                id = cs.SalvaWidget(widget);

                result = RedirectToAction("EditaInEvidenza", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    widget = WidgetRepository.Instance.RecuperaWidget(model.ID);
                    model.Widget = widget;
                }

                model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(false);
                model.NotizieSelectList = ModelUtils.CreaNotiziaSelectList(widget?.Categoria?.ID ?? 0);

                model.EditaNotiziaInEvidenza = widget?.NotiziaID;

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public ActionResult Elimina(int id)
        {
            ActionResult result = null;

            Widget widget = WidgetRepository.Instance.RecuperaWidget(id);

            if (widget != null)
                WidgetRepository.Instance.Elimina(id);

            result = RedirectToAction("Index", new { tipoWidget = widget.Tipo.ToString() });

            return result;
        }


        public ActionResult GetNotizieByCategoriaId(int id)
        {
            return Json(ModelUtils.CreaNotiziaSelectList(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EliminaCache()
        {
            ActionResult result = null;

            ContenutoService cs = new ContenutoService();

            cs.EliminaCache();

            result = RedirectToAction("Index", "Home");

            return result;
        }

    }
}
