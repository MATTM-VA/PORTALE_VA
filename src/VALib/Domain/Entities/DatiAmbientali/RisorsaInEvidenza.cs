using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Web;

namespace VALib.Domain.Entities.DatiAmbientali
{
    public class RisorsaInEvidenza : GuidEntity
    {
        public string FileIcona { get; internal set; }

        internal string Url { get; set; }

        public string Titolo { get; internal set; }

        public string Tipo { get; internal set; }

        public string ServizioWMS { get; internal set; }

        public string ServizioWFS { get; internal set; }

        public string GoogleEarth { get; internal set; }

        public string Descrizione { get; internal set; }

        public int IDTipoContenutoRisorsa { get; internal set; }

        public string GetUrl()
        {
            string result = "";

            switch (FileIcona.ToLower())
            {
                case "icostrato.gif":
                case "progettocartografico.gif":
                case "interactiveresource.gif":
                    result = Url;
                    break;
                default:
                    result = UrlUtility.VADocumentoCondivisione(ID);
                    break;
            }

            return result;
        }
    }
}
