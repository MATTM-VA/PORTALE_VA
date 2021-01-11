﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class DocumentoPortale : Entity
    {
        public TipoFile TipoFile { get; set; }

        public string Nome_IT { get; set; }

        public string Nome_EN { get; set; }

        public string NomeFileOriginale { get; set; }

        public DateTime DataInserimento { get; internal set; }

        public DateTime DataUltimaModifica { get; internal set; }

        public int Dimensione { get; set; }

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

        internal bool IsNew
        {
            get
            {
                return ID < 1;
            }
        }
        
    }
}
