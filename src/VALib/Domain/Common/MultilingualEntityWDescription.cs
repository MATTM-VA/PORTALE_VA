using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VALib.Domain.Common
{
    public abstract class MultilingualEntityWDescription : MultilingualEntity
    {
        internal string _descrizione_IT { get; set; }

        internal string _descrizione_EN { get; set; }

        public string GetDescrizione()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetDescrizione(codiceLingua);
        }

        public string GetDescrizione(string codiceLingua)
        {
            string descrizione = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    descrizione = _descrizione_IT;
                    break;
                case "en":
                    descrizione = string.IsNullOrWhiteSpace(_descrizione_EN) ? _descrizione_IT : _descrizione_EN;
                    break;
                default:
                    descrizione = _descrizione_IT;
                    break;
            }

            return descrizione;
        }
        
    }
}
