using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.UI
{
    public class PaginaStatica : Entity
    {
        public VoceMenu VoceMenu { get; internal set; }

        public DateTime DataInserimento { get; internal set; }

        public DateTime DataUltimaModifica { get; internal set; }

        public string Testo_IT { get; set; }

        public string Testo_EN { get; set; }

        public string GetTesto()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetTesto(codiceLingua);
        }

        public string GetTesto(string codiceLingua)
        {
            string testo = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    testo = Testo_IT;
                    break;
                case "en":
                    testo = string.IsNullOrWhiteSpace(Testo_EN) ? Testo_IT : Testo_EN;
                    break;
                default:
                    testo = Testo_IT;
                    break;
            }

            return testo;
        }

        public bool Visibile { get; internal set; }
        
        internal bool IsNew
        {
            get
            {
                return ID < 1;
            }
        }

    }
}
