using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.Contenuti.Base;
using VALib.Web;

namespace VALib.Domain.Entities.UI
{
    public class OggettoCarosello : Entity
    {
        public ContenutoOggettoCaroselloTipo TipoContenuto { get; set; }

        public int ContenutoID { get; set; }

        public int ImmagineID { get; set; }

        public DateTime Data { get; set; }

        public string Nome_IT { get; set; }

        public string Nome_EN { get; set; }

        public string Descrizione_IT { get; set; }

        public string Descrizione_EN { get; set; }

        public string LinkContenuto
        {
            get
            {
                string url = "";

                switch (this.TipoContenuto)
                {
                    case ContenutoOggettoCaroselloTipo.Oggetto:
                        url = UrlUtility.VAOggettoInfo(this.ContenutoID);
                        break;
                    case ContenutoOggettoCaroselloTipo.Notizia:
                        url = UrlUtility.VANotizia(this.ContenutoID, this.Contenuto.CategoriaNotizia);
                        break;
                    default:
                        break;
                }
                return url;
            }
        }

        public string LinkProgettoCartografico { get; set; }

        public bool Pubblicato { get; set; }

        public DateTime DataInserimento { get; internal set; }

        public DateTime DataUltimaModifica { get; internal set; }

        internal bool IsNew
        {
            get
            {
                return ID < 1;
            }
        }

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

        public string GetDescrizione()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetDescrizione(codiceLingua);
        }

        public string GetDescrizione(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = Descrizione_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(Descrizione_EN) ? Descrizione_IT : Descrizione_EN;
                    break;
                default:
                    result = Descrizione_IT;
                    break;
            }

            return result;
        }

        public ContenutoOggettoCarosello Contenuto { get; set; }

    }
}
