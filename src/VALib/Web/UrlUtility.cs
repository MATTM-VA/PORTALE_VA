using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Configuration;
using System.IO;
using System.Globalization;
using VALib.Helpers;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Web
{
    public static class UrlUtility
    {
        public static string VADomain
        {
            get { return Settings.UrlBase; }
        }

        public static string VASite()
        {
            return VADomain;
        }

        public static string VASite(string relativePath)
        {
            return Combine(VASite(), relativePath);
        }

        public static string VAAdmin()
        {
            return Settings.UrlAdmin;
        }

        public static string VAAdmin(string relativePath)
        {
            return Combine(VAAdmin(), relativePath);
        }

        public static string VAContent(string filename)
        {
            string url = null;
            string basePath = VASite("/Content/");
            string ext = Path.GetExtension(filename.Split('?')[0]).ToLowerInvariant();

            if (filename.StartsWith("jquery-ui", StringComparison.InvariantCultureIgnoreCase))
            {
                url = basePath + "jquery-ui/" + filename;
            }
            else
            {
                switch (ext)
                {
                    case ".png":
                    case ".jpg":
                    case ".gif":
                        url = basePath + "Images/" + filename;
                        break;
                    default:
                        url = basePath + filename;
                        break;
                }
            }

            return url;
        }

        public static string VAScript(string filename)
        {
            string url = null;
            string basePath = VASite("/Scripts/");
            url = basePath + filename;

            return url;
        }

        public static string VAImmagine()
        {
            string basePath = VASite("/File/Immagine/");

            return basePath;
        }

        public static string VAImmagine(int immagineID)
        {
            string url = null;
            string basePath = VASite("/File/Immagine/");
            url = basePath + immagineID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VAImmagineLocalizzazione(int oggettoID)
        {
            string url = null;
            string basePath = VASite("/File/ImmagineLocalizzazione/");
            url = basePath + oggettoID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VALocalizzazioneWmc(int oggettoID)
        {
            string url = null;
            string basePath = VASite("/File/LocalizzazioneWmc/");
            url = basePath + oggettoID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VALocalizzazionePdf(int oggettoID)
        {
            string url = null;
            string basePath = VASite("/File/LocalizzazionePdf/");
            url = basePath + oggettoID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VAImmagineDatiAmbientaliEvidenza(Guid risorsaID)
        {
            string url = null;
            string basePath = VASite("/File/ImmagineDatiAmbientaliEvidenza/");
            url = basePath + risorsaID.ToString();

            return url;
        }

        public static string VAImmagine(int immagineMasterID, int formatoImmagineID)
        {
            string url = null;
            string basePath = VASite("/File/Immagine/");
            url = basePath + immagineMasterID.ToString(CultureInfo.InvariantCulture) + "/" + formatoImmagineID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VAImmagineLavoro(int immagineMasterID)
        {
            string url = null;
            string basePath = VASite("/File/ImmagineLavoro/");
            url = basePath + immagineMasterID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VADocumentoPortale()
        {
            string basePath = VASite("/File/DocumentoPortale/");

            return basePath;
        }

        public static string VADocumentoPortale(int id)
        {
            string url = null;
            string basePath = VASite("/File/DocumentoPortale/");
            url = basePath + id.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VADocumentoCondivisione(Guid id)
        {
            string url = null;
            string basePath = VASite("/File/DocumentoCondivisione/");
            url = basePath + id.ToString();

            return url;
        }

        public static string VADocumentoViaVas(int id)
        {
            string url = null;
            string basePath = VASite("/File/Documento/");
            url = basePath + id.ToString();

            return url;
        }

        public static string VAProvvedimento(int id)
        {
            string url = null;
            string basePath = VASite("/File/Provvedimento/");
            url = basePath + id.ToString();

            return url;
        }

        public static string VAProvvedimentoRegionale(int id)
        {
            string url = null;
            string basePath = VASite("/File/ProvvedimentoAiaRegionale/");
            url = basePath + id.ToString();

            return url;
        }

        public static string VADocumentoMedia()
        {
            string basePath = VASite("/File/DocumentoMedia/");

            return basePath;
        }

        public static string VADocumentoMedia(string path)
        {
            string url = null;
            string basePath = VASite("/File/DocumentoMedia/");
            url = basePath + path;

            return url;
        }

        public static string VAOggettoInfo(int id)
        {
            string url = null;
            string basePath = VASite(CultureHelper.GetCurrentCultureInfo() + "/Oggetti/Info/");
            url = basePath + id.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VAOggettoDocumentazione(int id, int oggettoProceduraID)
        {
            string url = null;
            string basePath = VASite(CultureHelper.GetCurrentCultureInfo() + "/Oggetti/Documentazione/");
            url = basePath + id.ToString(CultureInfo.InvariantCulture) + "/" + oggettoProceduraID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VANotizia(Notizia notizia)
        {
            return VANotizia(notizia.ID, notizia.Categoria.Enum);
        }

        public static string VANotizia(int notiziaID, CategoriaNotiziaEnum categoria)
        {
            string url = null;

            string action = "";

            switch (categoria)
            {
                case CategoriaNotiziaEnum.Nessuna:
                    break;
                case CategoriaNotiziaEnum.EventiENotizie:
                    action = "DettaglioNotizia";
                    break;
                case CategoriaNotiziaEnum.LaDirezioneInforma:
                    action = "DettaglioDirezione";
                    break;
                case CategoriaNotiziaEnum.AreaGiuridica:
                    action = "DettaglioAreaGiuridica";
                    break;
                case CategoriaNotiziaEnum.UltimiProvvedimenti:
                    action = "DettaglioUltimiProvvedimenti";
                    break;
                case CategoriaNotiziaEnum.OsservatorioILVA:
                    action = "OsservatorioILVA";
                    break;
                default:
                    break;
            }

            string basePath = VASite(CultureHelper.GetCurrentCultureInfo() + "/Comunicazione/" + action + "/");
            url = basePath + notiziaID.ToString(CultureInfo.InvariantCulture);

            return url;
        }

        public static string VAFileAssociato()
        {
            string basePath = VASite("/File/Associato/");

            return basePath;
        }

        public static string VAFileScarica()
        {
            string basePath = VASite("/File/Scarica/");

            return basePath;
        }

        public static string VAFileAssociato(int fileAssociatoID, string fileAllegato)
        {
            string basePath = VASite(string.Concat("/File/Associato/", fileAssociatoID, Path.GetExtension(fileAllegato)));

            return basePath;
        }

        public static string VAHtmlReplacePseudoUrls(string html)
        {
            string htmlResult = null;

            if (!string.IsNullOrWhiteSpace(html))
            {
                htmlResult = html.Replace("va://File/Immagine/", VAImmagine()).Replace("va://File/DocumentoPortale/", VADocumentoPortale()).Replace("va://File/DocumentoMedia/", VADocumentoMedia());
            }

            return htmlResult;
        }

        public static string VAHtmlReplaceRealUrls(string html)
        {
            string htmlResult = null;

            if (!string.IsNullOrWhiteSpace(html))
            {
                htmlResult = html.Replace(VAImmagine(), "va://File/Immagine/").Replace(VADocumentoPortale(), "va://File/DocumentoPortale/").Replace(VADocumentoMedia(), "va://File/DocumentoMedia/");
            }

            return htmlResult;
        }

       

        public static string VATransformSegment(string input)
        {
            string output = null;

            if (input != null)
            {
                StringBuilder sb = new StringBuilder(input);
                sb.Replace(' ', '-');
                sb.Replace('/', '-');
                sb.Replace("\\", "-");
                sb.Replace('%', '-');
                sb.Replace('&', '-');
                sb.Replace('+', '-');
                sb.Replace('*', '-');
                sb.Replace('<', '-');
                sb.Replace('>', '-');
                sb.Replace(',', '-');
                sb.Replace(':', '-');
                sb.Replace('?', '-');
                sb.Replace('!', '-');
                sb.Replace('.', '-');
                sb.Replace(';', '-');
                sb.Replace('(', '-');
                sb.Replace(')', '-');
                sb.Replace('"', '-');
                sb.Replace('“', '-');
                sb.Replace('”', '-');
                sb.Replace("'", "-");
                sb.Replace('’', '-');
                sb.Replace('#', '-');
                sb.Replace('°', '-');
                sb.Replace('à', 'a');
                sb.Replace('è', 'e');
                sb.Replace('ì', 'i');
                sb.Replace('ò', 'o');
                sb.Replace('ù', 'u');
                sb.Replace('À', 'A');
                sb.Replace('È', 'E');
                sb.Replace('Ì', 'I');
                sb.Replace('Ò', 'O');
                sb.Replace('Ù', 'U');
                sb.Replace('á', 'a');
                sb.Replace('é', 'e');
                sb.Replace('í', 'i');
                sb.Replace('ó', 'o');
                sb.Replace('ú', 'u');
                sb.Replace('Á', 'A');
                sb.Replace('É', 'E');
                sb.Replace('Í', 'I');
                sb.Replace('Ó', 'O');
                sb.Replace('Ú', 'U');
                sb.Replace("---", "-");
                sb.Replace("--", "-");
                sb.Replace("\r", "-");
                sb.Replace("\n", "-");
                sb.Replace("\t", "-");

                output = sb.ToString().Trim('-');
            }

            return output;
        }

       

        private static string Combine(string basePath, string relativePath)
        {
            bool basePathHasSlash, relativePathHasSlash;

            if (basePath == null)
                throw new ArgumentNullException("basePath");

            if (relativePath == null)
                throw new ArgumentNullException("relativePath");

            basePathHasSlash = basePath.EndsWith("/");
            relativePathHasSlash = relativePath.StartsWith("/");

            if (basePathHasSlash && relativePathHasSlash)
                return basePath + relativePath.Substring(1);
            else if (basePathHasSlash || relativePathHasSlash)
                return basePath + relativePath;
            else
                return basePath + "/" + relativePath;
        }

    }
}
