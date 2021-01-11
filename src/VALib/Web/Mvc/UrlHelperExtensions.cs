using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string VASite(this UrlHelper urlHelper)
        {
            return UrlUtility.VASite();
        }

        public static string VASite(this UrlHelper urlHelper, string relativePath)
        {
            return UrlUtility.VASite(relativePath);
        }

        public static string VAAdmin(this UrlHelper urlHelper)
        {
            return UrlUtility.VAAdmin();
        }

        public static string VAAdmin(this UrlHelper urlHelper, string relativePath)
        {
            return UrlUtility.VAAdmin(relativePath);
        }

        public static string VAContent(this UrlHelper urlHelper, string filename)
        {
            return UrlUtility.VAContent(filename);
        }

        public static string VAScript(this UrlHelper urlHelper, string filename)
        {
            return UrlUtility.VAScript(filename);
        }

        public static string VAImmagine(this UrlHelper urlHelper, int immagineID)
        {
            return UrlUtility.VAImmagine(immagineID);
        }

        public static string VAImmagineLocalizzazione(this UrlHelper urlHelper, int oggettoID)
        {
            return UrlUtility.VAImmagineLocalizzazione(oggettoID);
        }

        public static string VAImmagineDatiAmbientaliEvidenza(this UrlHelper urlHelper, Guid risorsaID)
        {
            return UrlUtility.VAImmagineDatiAmbientaliEvidenza(risorsaID);
        }

        public static string VAImmagine(this UrlHelper urlHelper, int immagineMasterID, int formatoImmagineID)
        {
            return UrlUtility.VAImmagine(immagineMasterID, formatoImmagineID);
        }

        public static string VAImmagineLavoro(this UrlHelper urlHelper, int immagineMasterID)
        {
            return UrlUtility.VAImmagineLavoro(immagineMasterID);
        }

        public static string VADocumentoPortale(this UrlHelper urlHelper, int id)
        {
            return UrlUtility.VADocumentoPortale(id);
        }

        public static string VADocumentoCondivisione(this UrlHelper urlHelper, Guid id)
        {
            return UrlUtility.VADocumentoCondivisione(id);
        }

        public static string VADocumentoViaVas(this UrlHelper urlHelper, int id)
        {
            return UrlUtility.VADocumentoViaVas(id);
        }

        public static string VAProvvedimento(this UrlHelper urlHelper, int id)
        {
            return UrlUtility.VAProvvedimento(id);
        }
        
        public static string VAProvvedimentoRegionale(this UrlHelper urlHelper, int id)
        {
            return UrlUtility.VAProvvedimentoRegionale(id);
        }
        public static string VADocumentoMedia(this UrlHelper urlHelper, string path)
        {
            return UrlUtility.VADocumentoMedia(path);
        }

        public static string VAOggettoInfo(this UrlHelper urlHelper, int id)
        {
            return UrlUtility.VAOggettoInfo(id);
        }

        public static string VAOggettoDocumentazione(this UrlHelper urlHelper, int id, int oggettoProceduraID)
        {
            return UrlUtility.VAOggettoDocumentazione(id, oggettoProceduraID);
        }

        public static string VANotizia(this UrlHelper urlHelper, Notizia notizia)
        {
            return UrlUtility.VANotizia(notizia);
        }

        public static string VAFileAssociato(this UrlHelper urlHelper, int fileAssociatoID, string fileAllegato)
        {
            return UrlUtility.VAFileAssociato(fileAssociatoID, fileAllegato);
        }

        public static string VATransformSegment(this UrlHelper urlHelper, string input)
        {
            return UrlUtility.VATransformSegment(input);
        }

        //public static string VAContenuto(this UrlHelper urlHelper, ApplicazioneEnum applicazione, Contenuto contenuto)
        //{
        //    return UrlUtility.VAContenuto(applicazione, contenuto);
        //}

        //public static string VAContenuto(this UrlHelper urlHelper, ApplicazioneEnum applicazione, ContenutoInfo contenutoInfo)
        //{
        //    return UrlUtility.VAContenuto(applicazione, contenutoInfo);
        //}

        //public static string VATipoContenuto(this UrlHelper urlHelper, ApplicazioneEnum applicazione, TipoContenuto tipoContenuto)
        //{
        //    return UrlUtility.VATipoContenuto(applicazione, tipoContenuto);
        //}
    }
}
