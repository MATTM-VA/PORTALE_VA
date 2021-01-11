using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VALib.Domain.Common
{
    public abstract class GuidMultilingualEntity : GuidEntity
    {
        internal string _nome_IT { get; set; }

        internal string _nome_EN { get; set; }

        public string Nome
        {
            get
            {
                string nome = "";

                switch (System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower())
                {
                    case "it":
                        nome = _nome_IT;
                        break;
                    case "en":
                        nome = string.IsNullOrWhiteSpace(_nome_EN) ? _nome_IT : _nome_EN;
                        break;
                    default:
                        nome = _nome_IT;
                        break;
                }

                return nome;
            }
        }

        public string GetNome()
        {
            string codiceLingua = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            return GetNome(codiceLingua);
        }

        public string GetNome(string codiceLingua)
        {
            string nome = "";

            switch (codiceLingua.ToLower())
            {
                case "it":
                    nome = _nome_IT;
                    break;
                case "en":
                    nome = string.IsNullOrWhiteSpace(_nome_EN) ? _nome_IT : _nome_EN;
                    break;
                default:
                    nome = _nome_IT;
                    break;
            }

            return nome;
        }

    }
}
