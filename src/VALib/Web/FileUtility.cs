using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VALib.Domain.Entities.Contenuti;
using VALib.Configuration;
using System.Web;
using System.Globalization;

namespace VALib.Web
{
    public static class FileUtility
    {
        private static string CartellaFormatoImmagine(FormatoImmagineEnum formatoImmagine)
        {
            string result = "";

            switch (formatoImmagine)
            {
                case FormatoImmagineEnum.Master:
                    result = "Master";
                    break;
                case FormatoImmagineEnum.CaroselloHome:
                    result = "Carosello";
                    break;
                case FormatoImmagineEnum.WidgetHome:
                    result = "Widgethome";
                    break;
                case FormatoImmagineEnum.ElencoNotizie:
                    result = "ElencoNotizie";
                    break;
                case FormatoImmagineEnum.DettaglioNotizie:
                    result = "DettaglioNotizia";
                    break;
                case FormatoImmagineEnum.Localizzazione:
                    result = "Localizzazione";
                    break;
                case FormatoImmagineEnum.Thumb:
                    result = "Thumb";
                    break;
                case FormatoImmagineEnum.Libero:
                    result = "Libero";
                    break;
                case FormatoImmagineEnum.WidgeHPEvidenza:
                    result = "WidgeHPEvidenza";
                    break;
                default:
                    break;
            }

            return result;
        }
        
        public static string VAImmagine(FormatoImmagineEnum formatoImmagine, string filename)
        {
            string filepath = null;

            if (formatoImmagine == null)
                throw new ArgumentNullException("formatoFoto");

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathImmagini, CartellaFormatoImmagine(formatoImmagine), filename);

            return filepath;
        }

       

        public static string VAImmagineLavoro(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathImmagini, "Master", filename);

            return filepath;
        }

        public static void VASalvaImmagine(FormatoImmagineEnum formatoImmagine, string filename, HttpPostedFileBase postedFile)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            if (postedFile == null)
                throw new ArgumentNullException("postedFile");

            filepath = VAImmagine(formatoImmagine, filename);

            postedFile.SaveAs(filepath);
        }

        public static void VAEliminaImmagine(FormatoImmagineEnum formatoImmagine, string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = VAImmagine(formatoImmagine, filename);

            if (File.Exists(filepath))
                File.Delete(filepath);
        }

        public static void VAEliminaImmagineLavoro(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = VAImmagineLavoro(filename);

            if (File.Exists(filepath))
                File.Delete(filepath);
        }

        public static bool EsisteImmagine(FormatoImmagineEnum formatoImmagine, string filename)
        {
            return System.IO.File.Exists(VAImmagine(formatoImmagine, filename));
        }

        public static bool EsisteImmagine(FormatoImmagineEnum formatoImmagine, int oggettoID)
        {
            return System.IO.File.Exists(VAImmagine(formatoImmagine, ImmagineLocalizzazioneNomeFile(oggettoID)));
        }

        public static string ImmagineLocalizzazioneNomeFile(int oggettoID)
        {
            return string.Format("{0}.jpg", oggettoID);
        }

        public static string LocalizzazioneWmcNomeFile(int oggettoID)
        {
            return string.Format("{0}.wmc", oggettoID);
        }

        public static string LocalizzazionePdfNomeFile(int oggettoID)
        {
            return string.Format("{0}.pdf", oggettoID);
        }

        public static string VADocumentoCondivisione(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumetiCondivisione, filename);

            return filepath;
        }

        public static string VADocumentoViaVas(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumetiViaVas, filename);

            return filepath;
        }

        public static string VADocumentoAia(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumetiAIA, filename);

            return filepath;
        }

        public static string VADocumentoAiaRegionale(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumetiAIARegionali, filename);

            return filepath;
        }

        public static string VADocumentoAiaEventi(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumetiAiaEventi, filename);

            return filepath;
        }

        public static string VADocumentoPortale(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumentiPortale, filename);

            return filepath;
        }

        public static string VADocumentoPortaleTemp(string filename)
        {
            string filepath = null;

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumentiPortale, Settings.PathDocumentiPortaleTemp, filename);

            return filepath;
        }

        public static bool VASalvaDocumentoPortaleTempChunk(string filePath, int chunk, int chunks, HttpPostedFileBase postedFileChunk)
        {
            bool fileCompleted = false;
            string fileDir = null;
            string tempFilePath = null;
            FileStream fs = null;

            fileDir = Path.GetDirectoryName(filePath);

            if (Directory.Exists(fileDir))
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                tempFilePath = filePath + ".part";

                if (chunk == 0 && File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
                if (chunk > 0 && !File.Exists(tempFilePath))
                {
                    throw new InvalidOperationException("Missing previous chunks: chunk " + chunk.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory(fileDir);
            }

            try
            {
                fs = new FileStream(filePath + ".part", FileMode.Append);

                postedFileChunk.InputStream.CopyTo(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs = null;
                }
            }

            if (chunks == 0 || chunk == chunks - 1)
            {
                File.Move(tempFilePath, filePath);
                fileCompleted = true;
            }

            return fileCompleted;

        }

        public static void VASpostaDocumentoPortaleTemp(int id, string filtempename)
        {
            string fileTempPath = "";
            string ext = "";
            string filePath = "";

            fileTempPath = VADocumentoPortaleTemp(filtempename);
            ext = Path.GetExtension(filtempename);
            filePath = VADocumentoPortale(id.ToString() + ext);

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Move(fileTempPath, filePath);
            File.Delete(fileTempPath);
        }

        public static void VAEliminaDocumentoPortale(string filename)
        {
            string filepath = "";

            filepath = VADocumentoPortale(filename);

            if (File.Exists(filepath))
                File.Delete(filepath);
        }

        public static string VADocumentoMedia(string cartella, string filename)
        {
            string filepath = null;

            if (filename == null)
                throw new ArgumentNullException("filename");

            filepath = Path.Combine(Settings.PathBase, Settings.PathDocumentiMedia, cartella, filename);

            return filepath;
        }

    }
}
