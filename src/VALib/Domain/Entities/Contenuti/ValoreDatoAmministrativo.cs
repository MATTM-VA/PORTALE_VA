using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti;
using VALib.Helpers;

namespace VALib.Domain.Entities.Contenuti
{
    public class ValoreDatoAmministrativo
    {
        public DatoAmministrativo DatoAmministrativo { get; internal set; }
        
        public int OggettoProceduraID { get; internal set; }
        
        public Procedura Procedura { get; internal set; }
        
        internal bool? _vBool { get; set; }

        internal DateTime? _vDatetime { get; set; }

        internal double? _vDouble { get; set; }

        internal string _vString { get; set; }

        public string ViperaAiaID { get; set; }

        public string GetValore()
        {
            string result = "";
            
            switch (DatoAmministrativo.TipoDati.ToLower())
            {
                case "testo":
                    result = string.IsNullOrWhiteSpace(_vString) ? "" : _vString;
                    break;
                case "data":
                    result = _vDatetime.HasValue ? _vDatetime.Value.ToString(CultureHelper.GetDateFormat()) : "";
                    break;
                case "bit":
                    result = _vBool.HasValue ? _vBool.Value.ToString() : "";
                    break;
                case "numero":
                    result = _vDouble.HasValue ? _vDouble.ToString() : "";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }
    }
}
