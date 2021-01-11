using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Notizia : Entity
    {
        public CategoriaNotizia Categoria { get; set; }

        public int ImmagineID { get; set; }

        public DateTime Data { get; set; }

        public bool Pubblicata { get; set; }

        public StatoNotiziaEnum Stato { get; set; }

        public string Titolo_IT { get; set; }

        public string Titolo_EN { get; set; }

        public string TitoloBreve_IT { get; set; }

        public string TitoloBreve_EN { get; set; }

        public string Abstract_IT { get; set; }

        public string Abstract_EN { get; set; }

        public string Testo_IT { get; set; }

        public string Testo_EN { get; set; }

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

        public string GetTitoloBreve()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetTitoloBreve(codiceLingua);
        }

        public string GetTitoloBreve(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = TitoloBreve_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(TitoloBreve_EN) ? TitoloBreve_IT : TitoloBreve_EN;
                    break;
                default:
                    result = TitoloBreve_IT;
                    break;
            }

            return result;
        }

        public string GetAbstract()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetAbstract(codiceLingua);
        }

        public string GetAbstract(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = Abstract_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(Abstract_EN) ? Abstract_IT : Abstract_EN;
                    break;
                default:
                    result = Abstract_IT;
                    break;
            }

            return result;
        }

        public string GetTesto()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetTesto(codiceLingua);
        }

        public string GetTesto(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = Testo_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(Testo_EN) ? Testo_IT : Testo_EN;
                    break;
                default:
                    result = Testo_IT;
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

        public List<Immagine> Immagini { get; internal set; }
        
    }
}
