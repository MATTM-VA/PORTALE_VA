using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.UI
{
    public class VoceDizionario
    {
        public int ID { get; internal set; }

        public string Sezione { get; internal set; }

        public string Nome { get; internal set; }

        internal string _valore_IT { get; set; }

        internal string _valore_EN { get; set; }

        public string GetValore()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetValore(codiceLingua);
        }

        public string GetValore(string codiceLingua)
        {
            string valore = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    valore = _valore_IT;
                    break;
                case "en":
                    valore = string.IsNullOrWhiteSpace(_valore_EN) ? _valore_IT : _valore_EN;
                    break;
                default:
                    valore = _valore_IT;
                    break;
            }

            return valore;
        }

    }
}
