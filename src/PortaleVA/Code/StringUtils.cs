using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;

namespace VAPortale.Code
{
    public static class StringUtils
    {
        public static string GetNomeElenco(TipoElencoEnum tipoElenco)
        {
            string result = "";

            switch (tipoElenco)
            {
                case TipoElencoEnum.NonImpostato:
                    break;
                case TipoElencoEnum.HomeOggettiVia:
                    result = ProceduraRepository.Instance.RecuperaProcedura(3).GetNome();
                    break;
                case TipoElencoEnum.HomeOggettiAssoggettabilitaVia:
                    result = ProceduraRepository.Instance.RecuperaProcedura(5).GetNome();
                    break;
                case TipoElencoEnum.HomeOggettiVas:
                    result = ProceduraRepository.Instance.RecuperaProcedura(102).GetNome();
                    break;
                case TipoElencoEnum.HomeProvvedimentiVia:
                    result = ProceduraRepository.Instance.RecuperaProcedura(3).GetNome();
                    break;
                case TipoElencoEnum.HomeProvvedimentiAssoggettabilitaVia:
                    result = ProceduraRepository.Instance.RecuperaProcedura(5).GetNome();
                    break;
                case TipoElencoEnum.HomeProvvedimentiVas:
                    result = ProceduraRepository.Instance.RecuperaProcedura(102).GetNome();
                    break;
                case TipoElencoEnum.HomeOggettiAia:
                    result = MacroTipoOggettoRepository.Instance.RecuperaMacroTipoOggetto(3).GetNome();
                    break;
                default:
                    break;
            }

            return result;
        }

    }
}