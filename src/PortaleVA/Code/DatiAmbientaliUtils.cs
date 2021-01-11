using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Services;

namespace VAPortale.Code
{
    public static class DatiAmbientaliUtils
    {
        public static string CssClassForIDTipoContenutoRisorsa(int idTipoContenutoRisorsa)
        {
            string result = "";

            switch (idTipoContenutoRisorsa)
            {
                case 1:
                    result = "icona-generica";
                    break;
                case 2:
                    result = "icona-doc";
                    break;
                case 3:
                    result = "icona-mdb";
                    break;
                case 4:
                    result = "icona-jpg";
                    break;
                case 5:
                    result = "icona-generica";
                    break;
                case 6:
                    result = "icona-pdf";
                    break;
                case 7:
                    result = "icona-excel";
                    break;
                case 8:
                    result = "icona-generica";
                    break;
                case 9:
                    result = "icona-zip";
                    break;
                case 10:
                    result = "icona-generica";
                    break;
                case 11:
                    result = "icona-generica";
                    break;
                case 12:
                    result = "icona-web";
                    break;
                case 13:
                    result = "icona-dwf";
                    break;
                case 14:
                    result = "icona-localizzatore";
                    break;
                default:
                    result = "icona-mappa-interattiva";
                    break;
            }

            return result;
        }

        public static string VisualizzaRisorsaToolTip(int idTipoContenutoRisorsa)
        {
            string result = "";

            switch (idTipoContenutoRisorsa)
            {
                case 999:
                case 14:
                    result = DizionarioService.DATIAMBIENTALI_LabelMappaInterattiva;
                    break;
                case 12:
                    result = DizionarioService.DATIAMBIENTALI_LabelRisorsa;
                    break;
                default:
                    result = DizionarioService.DATIAMBIENTALI_LabelDocumento;
                    break;
            }

            return result;
        }

        public static string VisualizzaRisorsaTesto(int idTipoContenutoRisorsa)
        {
            string result = "";

            switch (idTipoContenutoRisorsa)
            {
                case 999:
                case 14:
                    result = DizionarioService.DATIAMBIENTALI_LabelMappaInterattiva;
                    break;
                case 12:
                    result = DizionarioService.DATIAMBIENTALI_LabelRisorsaOnline;
                    break;
                default:
                    result = "Download";
                    break;
            }

            return result;
        }

    }
}