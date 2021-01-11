using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using System.Xml.Linq;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Entities.UI
{
    public class Widget : Entity
    {
        public TipoWidget Tipo { get; set; }

        public string Nome_IT { get; set; }

        public string Nome_EN { get; set; }

        public CategoriaNotizia Categoria { get; set; }

        public int? NumeroElementi { get; set; }

        public string Contenuto_IT { get; set; }

        public string Contenuto_EN { get; set; }

        public bool MostraTitolo { get; set; }

        public DateTime DataInserimento { get; internal set; }

        public DateTime DataUltimaModifica { get; internal set; }

        public string GetNome()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetNome(codiceLingua);
        }

        public string GetNome(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = Nome_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(Nome_EN) ? Nome_IT : Nome_EN;
                    break;
                default:
                    result = Nome_IT;
                    break;
            }

            return result;
        }

        public string GetContenuto()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetContenuto(codiceLingua);
        }

        public string GetContenuto(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = Contenuto_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(Contenuto_EN) ? Contenuto_IT : Contenuto_EN;
                    break;
                default:
                    result = Contenuto_IT;
                    break;
            }

            return result;
        }

        public int? VoceMenuID { get; set; }

        public VoceMenu VoceMenu { get; internal set; }

        internal bool IsNew
        {
            get
            {
                return ID < 1;
            }
        }

        public int? NotiziaID { get; set; }
    }
}
