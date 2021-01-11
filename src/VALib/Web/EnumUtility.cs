using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Services;

namespace VALib.Web
{
    public static class EnumUtility
    {
        public static string CategoriaNotiziaToString(CategoriaNotiziaEnum valore)
        {
            string result = "";

            switch (valore)
            {
                case CategoriaNotiziaEnum.Nessuna:
                    result = DizionarioService.SITO_CategoriaNotiziaNessuna;
                    break;
                case CategoriaNotiziaEnum.EventiENotizie:
                    result = DizionarioService.SITO_CategoriaNotiziaEventiENotizie;
                    break;
                case CategoriaNotiziaEnum.LaDirezioneInforma:
                    result = DizionarioService.SITO_CategoriaNotiziaLaDirezioneInforma;
                    break;
                case CategoriaNotiziaEnum.AreaGiuridica:
                    result = DizionarioService.SITO_CategoriaNotiziaAreaGiuridica;
                    break;
                case CategoriaNotiziaEnum.UltimiProvvedimenti:
                    result = DizionarioService.SITO_CategoriaNotiziaUltimiProvvedimenti;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
