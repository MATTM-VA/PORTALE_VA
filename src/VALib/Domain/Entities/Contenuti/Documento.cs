using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Documento : Entity
    {
        public Documento()
        {
            Entita = new List<EntitaCollegata>();
            Raggruppamenti = new List<Raggruppamento>();
            Argomenti = new List<Argomento>();
        }

        public TipoFile TipoFile { get; internal set; }

        public Procedura Procedura { get; internal set; }

        public TipoOggetto TipoOggetto { get; internal set; }

        public List<EntitaCollegata> Entita { get; internal set; }

        public List<Raggruppamento> Raggruppamenti { get; internal set; }

        public List<Argomento> Argomenti { get; internal set; }

        public string CodiceElaborato { get; internal set; }

        public string Titolo { get; internal set; }

        public string Descrizione { get; internal set; }

        public string Tipologia { get; internal set; }

        public string Scala { get; internal set; }

        public string Diritti { get; internal set; }

        public string LinguaDocumento { get; internal set; }

        public int Dimensione { get; internal set; }

        public int OggettoID { get; internal set; }

        public string NomeOggetto_IT { get; internal set; }

        public string NomeOggetto_EN { get; internal set; }

        public string Riferimenti { get; internal set; }

        public string Origine { get; internal set; }

        public string Copertura { get; internal set; }

        public DateTime DataPubblicazione { get; internal set; }

        public DateTime DataStesura { get; internal set; }

        public string GetNomeOggetto()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetNomeOggetto(codiceLingua);
        }

        public string GetNomeOggetto(string codiceLingua)
        {
            string result = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    result = NomeOggetto_IT;
                    break;
                case "en":
                    result = string.IsNullOrWhiteSpace(NomeOggetto_EN) ? NomeOggetto_IT : NomeOggetto_EN;
                    break;
                default:
                    result = NomeOggetto_IT;
                    break;
            }

            return result;
        }

        public string GetRaggruppamenti()
        {
            string result = "";

            foreach (Raggruppamento r in Raggruppamenti)
            {
                result += string.Format("{0}/", r.GetNome());
            }

            if (result.Length > 0)
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        public string GetArgomenti()
        {
            string result = "";

            foreach (Argomento a in Argomenti)
            {
                result += string.Format("{0}, ", a.GetNome());
            }

            if (result.Length > 0)
                result = result.Substring(0, result.Length - 2);

            return result;
        }
    }
}
