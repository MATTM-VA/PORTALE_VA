using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using VALib.Domain.Services;
using System.Drawing;
using VALib.Web;
using System.Drawing.Imaging;
using VALib.Helpers;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreImmagine)]
    public class ImmagineController : Controller
    {
        [HttpGet]
        public ActionResult Index(ImmagineIndexModel model)
        {
            List<Immagine> immagineList = null;
            int numeroTotale = 0;

            string nome_IT = string.IsNullOrWhiteSpace(model.Testo) ? "" : model.Testo;

            immagineList = ImmagineRepository.Instance.RecuperaImmagini(nome_IT, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out numeroTotale);

            model.Immagini = immagineList;
            model.TotaleRisultati = numeroTotale;

            return View(model);
        }

        [HttpGet]
        public ActionResult Crea()
        {
            //ActionResult result = null;
            ImmagineEditaModel model = new ImmagineEditaModel();

            model.ID = 0;

            //result = View(model);

            return View("Edita", model);
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            ImmagineEditaModel model = new ImmagineEditaModel();
            model.MostraLinkInserimentoNotizia = User.IsInRole(RuoloUtenteCodici.GestoreNotizie);
            Immagine immagine = null;

            immagine = ImmagineRepository.Instance.RecuperaImmagine(id);

            if (immagine != null && immagine.FormatoImmagine.Enum == FormatoImmagineEnum.Master)
            {
                model.Immagine = immagine;

                model.ID = id;
                model.EditaNome_IT = immagine.Nome_IT;
                model.EditaNome_EN = immagine.Nome_EN;

                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edita(ImmagineEditaModel model)
        {
            ActionResult result = null;

            if (model.ID == 0 && ModelState.IsValidField("EditaFileImmagine"))
            {
                if (model.EditaFileImmagine == null || model.EditaFileImmagine.ContentLength == 0)
                    ModelState.AddModelError("EditaFileImmagine", "Immagine obbligatoria.");
            }

            if (ModelState.IsValid)
            {
                int id = 0;
                Immagine immagine = null;
                ContenutoService cs = new ContenutoService();

                if (model.ID != 0)
                {
                    if (model.EditaFileImmagine != null)
                    {
                        immagine = ImmagineRepository.Instance.RecuperaImmagine(model.ID);
                        string nomeFile = string.Format("{0}{1}", immagine.ID, System.IO.Path.GetExtension(model.EditaFileImmagine.FileName));
                        FileUtility.VASalvaImmagine(immagine.FormatoImmagine.Enum, nomeFile, model.EditaFileImmagine);
                        immagine.NomeFile = nomeFile;
                        immagine.Nome_IT = model.EditaNome_IT.Trim();
                        immagine.Nome_EN = model.EditaNome_EN.Trim();
                        Bitmap immagineMaster = new Bitmap(FileUtility.VAImmagine(immagine.FormatoImmagine.Enum, nomeFile));
                        immagine.Altezza = immagineMaster.Height;
                        immagine.Larghezza = immagineMaster.Width;
                        immagineMaster.Dispose();
                    }
                    else
                    {
                        immagine = ImmagineRepository.Instance.RecuperaImmagine(model.ID);
                        immagine.Nome_IT = model.EditaNome_IT.Trim();
                        immagine.Nome_EN = model.EditaNome_EN.Trim();
                    }
                }
                else
                {
                    immagine = cs.CreaImmagineMaster(model.EditaNome_IT.Trim(),
                                                     model.EditaNome_EN.Trim(), 
                                                    "temp.file");
                }

                id = cs.SalvaImmagine(immagine);
                immagine = ImmagineRepository.Instance.RecuperaImmagine(id);

                if (model.ID == 0)
                {
                    string nomeFile = string.Format("{0}{1}", immagine.ID, System.IO.Path.GetExtension(model.EditaFileImmagine.FileName));
                    FileUtility.VASalvaImmagine(immagine.FormatoImmagine.Enum, nomeFile, model.EditaFileImmagine);

                    Bitmap immagineMaster = new Bitmap(FileUtility.VAImmagine(immagine.FormatoImmagine.Enum, nomeFile));

                    //immagineMaster.Save(FileUtility.VAImmagineLavoro(nomeFile));

                    immagine.NomeFile = nomeFile;
                    immagine.Altezza = immagineMaster.Height;
                    immagine.Larghezza = immagineMaster.Width;

                    immagineMaster.Dispose();

                    cs.SalvaImmagine(immagine);
                }
                else
                {
                    List<Immagine> immaginiCollegate = ImmagineRepository.Instance.RecuperaImmaginiFiglio(immagine.ID);

                    foreach (Immagine immagineCollegata in immaginiCollegate)
                    {
                        immagineCollegata.Nome_IT = immagine.Nome_IT;
                        immagineCollegata.Nome_EN = immagine.Nome_EN;
                        cs.SalvaImmagine(immagineCollegata);
                    }
                }

                result = RedirectToAction("Edita", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    model.Immagine = ImmagineRepository.Instance.RecuperaImmagine(model.ID);
                }

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public ActionResult Elimina(int id)
        {
            ActionResult result = null;
            Immagine immagine = ImmagineRepository.Instance.RecuperaImmagine(id);

            if (immagine != null)
            {
                if (immagine.FormatoImmagine.Enum == FormatoImmagineEnum.Master)
                {
                    List<Immagine> immaginiCollegate = ImmagineRepository.Instance.RecuperaImmaginiFiglio(immagine.ID);

                    foreach (Immagine immagineCollegata in immaginiCollegate)
                    {
                        FileUtility.VAEliminaImmagine(immagineCollegata.FormatoImmagine.Enum, immagineCollegata.NomeFile);
                        ImmagineRepository.Instance.Elimina(immagineCollegata.ID);
                    }

                    FileUtility.VAEliminaImmagineLavoro(immagine.NomeFile);
                }

                FileUtility.VAEliminaImmagine(immagine.FormatoImmagine.Enum, immagine.NomeFile);
                ImmagineRepository.Instance.Elimina(id);
            }

            result = RedirectToAction("Index");

            return result;
        }

        [HttpGet]
        public ActionResult EditaFormati(int id, int? formatoImmagineID, int mod = 0)
        {
            ActionResult result = null;
            ImmagineEditaFormatiModel model = new ImmagineEditaFormatiModel();
            Immagine immagineMaster = null;

            immagineMaster = ImmagineRepository.Instance.RecuperaImmagine(id);

            if (immagineMaster != null && immagineMaster.FormatoImmagine.Enum == FormatoImmagineEnum.Master)
            {
                Dictionary<FormatoImmagine, Immagine> formatiImmagine = null;
                List<FormatoImmagine> formati = FormatoImmagineRepository.Instance.RecuperaFormatiImmagine().Where(x => x.Abilitato).ToList();
                List<Immagine> immaginiElenco = null;

                immaginiElenco = ImmagineRepository.Instance.RecuperaImmaginiFiglio(immagineMaster.ID);

                formatiImmagine = new Dictionary<FormatoImmagine, Immagine>();

                foreach (FormatoImmagine item in formati)
                {
                    Immagine immagine = immaginiElenco.SingleOrDefault(x => x.FormatoImmagine.Enum == item.Enum);

                    formatiImmagine.Add(item, immagine);
                }

                model.ID = id;
                model.ImmagineMaster = immagineMaster;
                model.FormatiImmagine = formatiImmagine;

                if (formatoImmagineID != null)
                {
                    model.FormatoImmagine = formati.SingleOrDefault(x => x.ID.Equals(formatoImmagineID.Value));

                    if (model.FormatoImmagine != null)
                        model.Immagine = immaginiElenco.SingleOrDefault(x => x.FormatoImmagine.Enum == model.FormatoImmagine.Enum);
                }

                model.Modalita = mod;

                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        [HttpPost]
        public ActionResult EditaFormatiTaglia(int id, int formatoImmagineID, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0, int w = 0, int h = 0)
        {
            ActionResult result = null;
            Immagine immagineMaster = null;

            immagineMaster = ImmagineRepository.Instance.RecuperaImmagine(id);

            if (immagineMaster != null)
            {
                FormatoImmagine formato = FormatoImmagineRepository.Instance.RecuperaFormatoImmagine(formatoImmagineID);
                Immagine immagine = null;
                immagine = ImmagineRepository.Instance.RecuperaImmaginiFiglio(immagineMaster.ID).FirstOrDefault(x => x.FormatoImmagine.ID == formato.ID);

                Bitmap immagineLavoro = null;
                ImageFormat format = null;
                Bitmap immagineRisultato = null;

                immagineLavoro = new Bitmap(FileUtility.VAImmagineLavoro(immagineMaster.NomeFile));
                format = immagineLavoro.RawFormat;
                immagineRisultato = ImageHelper.Ritaglia(immagineLavoro, x1, y1, x2, y2);

                if (immagineRisultato != null)
                {
                    immagineLavoro.Dispose();
                    immagineLavoro = immagineRisultato;
                }

                immagineRisultato = ImageHelper.Ridimensiona(immagineLavoro, w, h);

                if (immagineRisultato != null)
                {
                    immagineLavoro.Dispose();
                }
                else
                {
                    immagineRisultato = immagineLavoro;
                }

                immagineLavoro = null;

                string ext = System.IO.Path.GetExtension(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile)).ToLower();

                switch (ext)
                {
                    case ".jpeg":
                    case ".jpg":
                        immagineRisultato.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), format);
                        break;
                    case ".gif":
                        immagineRisultato.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), format);
                        break;
                    case ".png":
                        immagineRisultato.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), format);
                        break;
                    case ".bmp":
                        immagineRisultato.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), format);
                        break;
                    default:
                        throw new ApplicationException("Formato immagine non supportato: " + ext);

                }

                ContenutoService cs = new ContenutoService();

                if (immagine != null)
                {
                    immagine.NomeFile = immagineMaster.NomeFile;
                }
                else
                {
                    immagine = cs.CreaImmagine(immagineMaster, formato, immagineMaster.NomeFile);
                }

                immagine.Larghezza = immagineRisultato.Width;
                immagine.Altezza = immagineRisultato.Height;

                immagineRisultato.Dispose();

                cs.SalvaImmagine(immagine);

                result = RedirectToAction("EditaFormati", new { id = immagineMaster.ID, formatoImmagineID = formato.ID });
            }
            else
            {
                result = RedirectToAction("Index");
            }

            return result;
        }

        [HttpPost]
        public ActionResult EditaFormatiRidimensiona(int id, int formatoImmagineID, int w, int h)
        {
            ActionResult result = null;
            Immagine immagineMaster = null;

            immagineMaster = ImmagineRepository.Instance.RecuperaImmagine(id);

            if (immagineMaster != null)
            {
                FormatoImmagine formato = FormatoImmagineRepository.Instance.RecuperaFormatoImmagine(formatoImmagineID);
                Immagine immagine = null;

                immagine = ImmagineRepository.Instance.RecuperaImmaginiFiglio(immagineMaster.ID).FirstOrDefault(x => x.FormatoImmagine.ID == formato.ID);

                Bitmap immagineLavoro = null;
                Bitmap immagineRidimensionata = null;
                Graphics grpImmagineRidimensionata = null;
                Rectangle rettangoloRidimensionata;

                immagineLavoro = new Bitmap(FileUtility.VAImmagineLavoro(immagineMaster.NomeFile));

                rettangoloRidimensionata = new Rectangle(0, 0, w, h);

                immagineRidimensionata = new Bitmap(w, h);

                grpImmagineRidimensionata = Graphics.FromImage((System.Drawing.Image)immagineRidimensionata);

                grpImmagineRidimensionata.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                grpImmagineRidimensionata.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                grpImmagineRidimensionata.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                grpImmagineRidimensionata.DrawImage((System.Drawing.Image)immagineLavoro, rettangoloRidimensionata);
                grpImmagineRidimensionata.Dispose();

                string ext = System.IO.Path.GetExtension(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile)).ToLower();

                switch (ext)
                {
                    case ".jpeg":
                    case ".jpg":
                        immagineRidimensionata.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), immagineLavoro.RawFormat);
                        break;
                    case ".gif":
                        immagineRidimensionata.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), immagineLavoro.RawFormat);
                        break;
                    case ".png":
                        immagineRidimensionata.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), immagineLavoro.RawFormat);
                        break;
                    case ".bmp":
                        immagineRidimensionata.Save(FileUtility.VAImmagine(formato.Enum, immagineMaster.NomeFile), immagineLavoro.RawFormat);
                        break;
                    default:
                        throw new ApplicationException("Formato immagine non supportato: " + ext);

                }

                immagineLavoro.Dispose();

                ContenutoService cs = new ContenutoService();

                if (immagine != null)
                {
                    immagine.NomeFile = immagineMaster.NomeFile;
                }
                else
                {
                    immagine = cs.CreaImmagine(immagineMaster, formato, immagineMaster.NomeFile);
                }

                immagine.Larghezza = immagineRidimensionata.Width;
                immagine.Altezza = immagineRidimensionata.Height;

                immagineRidimensionata.Dispose();

                cs.SalvaImmagine(immagine);

                result = RedirectToAction("EditaFormati", new { id = immagineMaster.ID, formatoImmagineID = formato.ID });
            }
            else
            {
                result = RedirectToAction("Index");
            }

            return result;
        }
        
    }
}
