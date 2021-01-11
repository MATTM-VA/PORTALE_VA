using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Provvedimento : Entity
    {
        public int OggettoID { get; internal set; }

        public int OggettoProceduraID { get; internal set; }

        public TipoProvvedimento Tipo { get; internal set; }

        public TipoOggetto TipoOggetto { get; internal set; }

        public DateTime? Data { get; internal set; }

        public string NumeroProtocollo { get; internal set; }

        internal string _nomeProgetto_IT { get; set; }

        internal string _nomeProgetto_EN { get; set; }

        public string GetNomeProgetto()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetNomeProgetto(codiceLingua);
        }

        public string GetNomeProgetto(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = _nomeProgetto_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(_nomeProgetto_EN) ? _nomeProgetto_IT : _nomeProgetto_EN;
                    break;
                default:
                    result = _nomeProgetto_IT;
                    break;
            }

            //if (!string.IsNullOrWhiteSpace(this.GetOggetto()))
            //    result = result + " - " + this.GetOggetto();

            return result;
        }

        public string Proponente { get; internal set; }

        public string Esito { get; internal set; }

        internal string _oggetto_IT { get; set; }

        internal string _oggetto_EN { get; set; }

        private string GetOggetto()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetOggetto(codiceLingua);
        }

        private string GetOggetto(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = _oggetto_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(_oggetto_EN) ? "" : _oggetto_EN;
                    break;
                default:
                    result = _oggetto_IT;
                    break;
            }

            return result;
        }

        public CategoriaImpianto CategoriaImpianto { get; set; }

        public Procedura Procedura { get; set; }

        public string Prov { get; set; }
    }
}
