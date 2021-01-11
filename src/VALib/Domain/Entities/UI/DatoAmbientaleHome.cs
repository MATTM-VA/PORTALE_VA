using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Entities.UI
{
    public class DatoAmbientaleHome : Entity
    {
        public int ImmagineID { get; set; }

        public bool Pubblicato { get; set; }

        public string Titolo_IT { get; set; }

        public string Titolo_EN { get; set; }

        public string Link { get; set; }

        public string GetTitolo()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetTitolo(codiceLingua);
        }

        public string GetTitolo(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = Titolo_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(Titolo_EN) ? Titolo_IT : Titolo_EN;
                    break;
                default:
                    result = Titolo_IT;
                    break;
            }

            return result;
        }

        internal bool IsNew
        {
            get
            {
                return ID < 1;
            }
        }
        
        public DateTime DataInserimento { get; internal set; }

        public DateTime DataUltimaModifica { get; internal set; }

        public Immagine Immagine { get; internal set; }

    }
}
