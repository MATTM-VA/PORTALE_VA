using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Web;
using VALib.Domain.Entities.Contenuti;
using System.IO;
using System.Globalization;
using VALib.Domain.Repositories.Contenuti;
using System.Web.Caching;
using VALib.Domain.Repositories.DatiAmbientali;
using VALib.Domain.Entities.DatiAmbientali;
using Ionic.Zip;

namespace VAPortale.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/

        public ActionResult ImmagineLocalizzazione(int id = 0)
        {
            ActionResult result = null;
            string filepath = FileUtility.VAImmagine(FormatoImmagineEnum.Localizzazione, FileUtility.ImmagineLocalizzazioneNomeFile(id));

            if (!string.IsNullOrWhiteSpace(filepath) && System.IO.File.Exists(filepath))
            {
                string contenttype = ContentTypeByExtension(Path.GetExtension(filepath));

                result = File(filepath, contenttype);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult LocalizzazioneWmc(int id = 0)
        {
            ActionResult result = null;
            string filepath = FileUtility.VAImmagine(FormatoImmagineEnum.Localizzazione, FileUtility.LocalizzazioneWmcNomeFile(id));

            if (!string.IsNullOrWhiteSpace(filepath) && System.IO.File.Exists(filepath))
            {
                string contenttype = ContentTypeByExtension(Path.GetExtension(filepath));

                result = File(filepath, contenttype);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult LocalizzazionePdf(int id = 0)
        {
            ActionResult result = null;
            string filepath = FileUtility.VAImmagine(FormatoImmagineEnum.Localizzazione, FileUtility.LocalizzazionePdfNomeFile(id));

            if (!string.IsNullOrWhiteSpace(filepath) && System.IO.File.Exists(filepath))
            {
                string contenttype = ContentTypeByExtension(Path.GetExtension(filepath));

                result = File(filepath, contenttype);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult ImmagineDatiAmbientaliEvidenza(Guid? id)
        {
            ActionResult result = null;
            string filepath = "";

            if (id.HasValue)
            {
                filepath = FileUtility.VAImmagine(FormatoImmagineEnum.Localizzazione, string.Format("{0}.jpg", id));
            }

            if (!string.IsNullOrWhiteSpace(filepath) && System.IO.File.Exists(filepath))
            {
                string contenttype = ContentTypeByExtension(Path.GetExtension(filepath));

                result = File(filepath, contenttype);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        [OutputCache(CacheProfile = "FileImmagine")]
        public ActionResult Immagine(int id = 0)
        {
            ActionResult result = null;
            string cacheKey = "Foto" + id.ToString(CultureInfo.InvariantCulture);
            string filepath = ControllerContext.HttpContext.Cache[cacheKey] as string;

            if (filepath == null)
            {
                Immagine immagine = ImmagineRepository.Instance.RecuperaImmagine(id);

                if (immagine != null)
                {
                    filepath = FileUtility.VAImmagine(immagine.FormatoImmagine.Enum, immagine.NomeFile);

                    ControllerContext.HttpContext.Cache.Insert(cacheKey, filepath, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
                }
                else
                {
                    filepath = string.Empty;
                }
            }

            if (filepath != string.Empty && System.IO.File.Exists(filepath))
            {
                string contenttype = ContentTypeByExtension(Path.GetExtension(filepath));

                result = File(filepath, contenttype);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult ImmaginePerFormato(int immagineMasterID, int formatoImmagineID)
        {
            ActionResult result = null;
            Immagine immagine = null;

            immagine = ImmagineRepository.Instance.RecuperaImmaginiFiglio(immagineMasterID).FirstOrDefault(x => x.FormatoImmagine.ID == formatoImmagineID);

            if (immagine != null)
            {
                string filepath = null;
                string contenttype = null;
                string ext = Path.GetExtension(immagine.NomeFile).ToLowerInvariant();

                contenttype = ContentTypeByExtension(ext);

                filepath = FileUtility.VAImmagine(immagine.FormatoImmagine.Enum, immagine.NomeFile);

                if (System.IO.File.Exists(filepath))
                    result = File(filepath, contenttype);
                else
                    result = HttpNotFound();
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult ImmagineLavoro(int id = 0)
        {
            ActionResult result = null;
            Immagine immagine = ImmagineRepository.Instance.RecuperaImmagine(id);

            if (immagine != null && immagine.FormatoImmagine.Enum == FormatoImmagineEnum.Master)
            {
                string filepath = null;
                string contenttype = null;
                string ext = Path.GetExtension(immagine.NomeFile).ToLowerInvariant();

                contenttype = ContentTypeByExtension(ext);

                filepath = FileUtility.VAImmagineLavoro(immagine.NomeFile);

                if (System.IO.File.Exists(filepath))
                    result = File(filepath, contenttype);
                else
                    result = HttpNotFound();
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult DocumentoPortale(int id = 0)
        {
            ActionResult result = null;
            DocumentoPortale documentoPortale = DocumentoPortaleRepository.Instance.RecuperaDocumentoPortale(id);

            if (documentoPortale != null)
            {
                string filepath = null;
                string contenttype = null;
                string ext = Path.GetExtension(documentoPortale.NomeFileOriginale).ToLowerInvariant();

                contenttype = ContentTypeByExtension(ext);

                filepath = FileUtility.VADocumentoPortale(string.Format("{0}{1}", documentoPortale.ID, ext));

                if (System.IO.File.Exists(filepath))
                {
                    result = File(filepath, contenttype, documentoPortale.NomeFileOriginale);
                    if (!Request.Browser.Crawler)
                        new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, id, VAWebEventTypeEnum.DownloadDocumentoPortale).Raise();
                }
                else
                    result = HttpNotFound();
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult DocumentoCondivisione(Guid? id)
        {
            ActionResult result = null;
            DocumentoCondivisione documento = null;

            if (id.HasValue)
            {
                documento = DocumentoCondivisioneRepository.Instance.RecuperaDocumentoCondivisione(id.Value);
            }

            if (documento != null && documento.TipoContenutoRisorsa.ID != 12)
            {
                string filepath = null;
                string contenttype = null;
                string ext = Path.GetExtension(documento.Url).ToLowerInvariant();

                contenttype = ContentTypeByExtension(ext);

                filepath = FileUtility.VADocumentoCondivisione(documento.Url);

                if (System.IO.File.Exists(filepath))
                {
                    result = File(filepath, contenttype, documento.Url);

                    if (!Request.Browser.Crawler)
                        new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, id, VAWebEventTypeEnum.DownloadDocumentoCondivisione).Raise();
                }
                else
                    result = HttpNotFound();
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult Documento(int id = 0)
        {
            ActionResult result = null;
            DocumentoDownload documento = DocumentoRepository.Instance.RecuperaDocumentoDownload(id);

            if (documento != null)
            {
                string filepath = null;
                string contenttype = null;
                string ext = documento.Estensione;
                string nomeFile = documento.PercorsoFile.Substring(documento.PercorsoFile.LastIndexOf("/") + 1);

                contenttype = ContentTypeByExtension(string.Format(".{0}", ext));
                if (documento.MacroTipoOggettoID == (int)MacroTipoOggettoEnum.Aia)
                    filepath = FileUtility.VADocumentoAia(string.Format("I{0}_P{1}/{2}.{3}", documento.OggettoID, documento.OggettoProceduraID, documento.PercorsoFile, ext));
                else
                    filepath = FileUtility.VADocumentoViaVas(string.Format("ID_Prog_{0}/{1}.{2}", documento.OggettoID, documento.PercorsoFile, ext));

                if (System.IO.File.Exists(filepath))
                {
                    result = File(filepath, contenttype, string.Format("{0}.{1}", nomeFile, ext));
                    if (!Request.Browser.Crawler)
                        new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, id, VAWebEventTypeEnum.DownloadDocumentoOggetto).Raise();
                }
                else
                    result = HttpNotFound();
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult Provvedimento(int id = 0)
        {
            ActionResult result = null;
            IEnumerable<DocumentoDownload> documenti = DocumentoRepository.Instance.RecuperaDocumentiDownloadPerProvvedimento(id);

            if (documenti.Count() > 0)
            {
                ZipFile zip = new ZipFile();
                MemoryStream mss = new MemoryStream();

                foreach (DocumentoDownload d in documenti)
                {
                    string filepath = null;
                    string ext = d.Estensione;

                    if (d.MacroTipoOggettoID == (int)MacroTipoOggettoEnum.Aia)
                        filepath = FileUtility.VADocumentoAia(string.Format("I{0}_P{1}/{2}.{3}", d.OggettoID, d.OggettoProceduraID, d.PercorsoFile, ext));
                    else
                        filepath = FileUtility.VADocumentoViaVas(string.Format("ID_Prog_{0}/{1}.{2}", d.OggettoID, d.PercorsoFile, ext));

                    if (System.IO.File.Exists(filepath))
                        zip.AddFile(filepath, "");
                }

                zip.Save(mss);
                zip.Dispose();

                mss.Seek(0, SeekOrigin.Begin);

                result = File(mss, ContentTypeByExtension(".zip"), string.Format("P_{0}.zip", id));
                if (!Request.Browser.Crawler)
                    new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, id, VAWebEventTypeEnum.DownloadDocumentiProvvedimento).Raise();

            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        public ActionResult ProvvedimentoAiaRegionale(int id = 0)
        {
            ActionResult result = null;
            IEnumerable<DocumentoDownload> documenti = DocumentoRepository.Instance.RecuperaDocumentiDownloadPerProvvedimento(id);

            if (documenti.Count() > 0)
            {
                ZipFile zip = new ZipFile();
                MemoryStream mss = new MemoryStream();

                foreach (DocumentoDownload d in documenti)
                {
                    string filepath = null;
                    string ext = d.Estensione;
                    
                    filepath = FileUtility.VADocumentoAiaRegionale(string.Format("I{0}_P{1}/{2}.{3}", d.OggettoID, d.OggettoProceduraID, d.PercorsoFile, ext));
                    
                    if (System.IO.File.Exists(filepath))
                        zip.AddFile(filepath, "");
                }

                zip.Save(mss);
                zip.Dispose();

                mss.Seek(0, SeekOrigin.Begin);

                result = File(mss, ContentTypeByExtension(".zip"), string.Format("P_{0}.zip", id));
                if (!Request.Browser.Crawler)
                    new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, id, VAWebEventTypeEnum.DownloadDocumentiProvvedimento).Raise();

            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

      
        public ActionResult DocumentoMedia(string cartella, string nomeFile)
        {
            ActionResult result = null;

            string filepath = null;
            string contenttype = null;
            string ext = Path.GetExtension(nomeFile).ToLowerInvariant();

            contenttype = ContentTypeByExtension(ext);

            filepath = FileUtility.VADocumentoMedia(cartella, nomeFile);

            if (System.IO.File.Exists(filepath))
            {
                result = File(filepath, contenttype, nomeFile);
                if (!Request.Browser.Crawler)
                    new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, 0, VAWebEventTypeEnum.DownloadDocumentoMedia).Raise();
            }
            else
                result = HttpNotFound();

            return result;
        }

        
        private string ContentTypeByExtension(string ext)
        {
            string contenttype = null;

            ext = ext.ToLowerInvariant();

            switch (ext)
            {
                case ".mp3":
                    contenttype = "audio/mpeg";
                    break;
                case ".mp4":
                    contenttype = "video/mp4";
                    break;
                case ".flv":
                    contenttype = "video/x-flv";
                    break;
                case ".jpg":
                case ".jpeg":
                    contenttype = "image/jpeg";
                    break;
                case ".gif":
                    contenttype = "image/gif";
                    break;
                case ".png":
                    contenttype = "image/png";
                    break;
                case ".swf":
                    contenttype = "application/x-shockwave-flash";
                    break;
                case ".txt":
                    contenttype = "text/plain";
                    break;
                case ".pdf":
                    contenttype = "application/pdf";
                    break;
                case ".doc":
                    contenttype = "application/msword";
                    break;
                case ".docx":
                    contenttype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                default:
                    contenttype = "application/octet-stream";
                    break;
            }

            return contenttype;
        }

    }
}
