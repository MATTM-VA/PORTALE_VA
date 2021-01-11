using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Services;

namespace VAPortale.Code
{
    public static class CsvUtils
    {
        // Tipo
        // 1 = in corso
        // 2 = avviate
        // 3 = concluse

        public static byte[] GeneraCsvFileBytesperReportProcedura(IEnumerable<ReportProcedura> righe, MacroTipoOggettoEnum macroTipoOggetto, AmbitoProcedura ambitoProcedura, int tipo)
        {
            byte[] fileBytes = null;
            IEnumerable<KeyValuePair<string, string>> headerCells = null;
            StringBuilder sb = null;
            int totalColumns = 0;
            int c = 1;
            string delimitedTextStringFormat = "{0};";
            string textStringFormat = "\"{0}\";";
            string delimitedTextStringFormatLast = "{0};";
            string textStringFormatLast = "\"{0}\";";


            headerCells = CreaColumnList(macroTipoOggetto, ambitoProcedura, tipo);

            if (headerCells != null)
            {
                totalColumns = headerCells.Count();
                sb = new StringBuilder();

                foreach(KeyValuePair<string, string> key in headerCells)
                {
                    if (c == totalColumns)
                        sb.AppendFormat(textStringFormatLast, key.Value);
                    else
                        sb.AppendFormat(textStringFormat, key.Value);

                    c++;
                }

                sb.AppendLine();

            }

            if (sb != null)
            {
                c = 1;                
                foreach(ReportProcedura riga in righe)
                {
                    foreach (KeyValuePair<string, string> key in headerCells)
                    {
                        string value = riga.GetValore(key.Key);

                        if (c == totalColumns)
                        {
                            if (value.Contains("\""))
                                sb.AppendFormat(delimitedTextStringFormatLast, value);
                            else
                                sb.AppendFormat(textStringFormatLast, value);
                        }
                        else
                        {
                            if (value.Contains("\""))
                                sb.AppendFormat(delimitedTextStringFormat, value);
                            else
                                sb.AppendFormat(textStringFormat, value);

                        }
                        c++;

                    }
                    sb.AppendLine();
                }
            }

            if (sb != null)
            {
                fileBytes = Encoding.BigEndianUnicode.GetBytes(sb.ToString());
                sb = null;
            }

            return fileBytes;
        }

        private static IEnumerable<KeyValuePair<string, string>> CreaColumnList(MacroTipoOggettoEnum macroTipoOggetto, AmbitoProcedura ambitoProcedura, int tipo)
        {
            List<KeyValuePair<string, string>> chiavi = new List<KeyValuePair<string, string>>();

            if (macroTipoOggetto == MacroTipoOggettoEnum.Aia)
            {
                chiavi.Add(new KeyValuePair<string, string>("IDVIP", "IDMATTM"));
            }
            else
            {
                chiavi.Add(new KeyValuePair<string, string>("IDVIP", "IDVIP"));
            }

            if (macroTipoOggetto == MacroTipoOggettoEnum.Vas) {
                chiavi.Add(new KeyValuePair<string, string>("NomeVas", DizionarioService.GRIGLIA_ColonnaOggettoVas));
                chiavi.Add(new KeyValuePair<string, string>("Proponente", DizionarioService.GRIGLIA_ColonnaProponente));
            }
            else if (macroTipoOggetto == MacroTipoOggettoEnum.Via) { 
                chiavi.Add(new KeyValuePair<string, string>("NomeVia", DizionarioService.GRIGLIA_ColonnaOggettoVia));
                chiavi.Add(new KeyValuePair<string, string>("Proponente", DizionarioService.GRIGLIA_ColonnaProponente));
            }
            else {
                chiavi.Add(new KeyValuePair<string, string>("NomeAia", DizionarioService.GRIGLIA_ColonnaOggettoAia));
                chiavi.Add(new KeyValuePair<string, string>("Gestore", DizionarioService.GRIGLIA_ColonnaGestore));
            }
            
            
            if (macroTipoOggetto == MacroTipoOggettoEnum.Vas)
                chiavi.Add(new KeyValuePair<string, string>("NomeSettore", DizionarioService.OGGETTO_LabelSettoreProgramma));
            else if (macroTipoOggetto == MacroTipoOggettoEnum.Via)            
                chiavi.Add(new KeyValuePair<string, string>("TipologiaOpera", DizionarioService.OGGETTO_LabelTipologiaOpera));
            else
                chiavi.Add(new KeyValuePair<string, string>("CategoriaImpianto", DizionarioService.OGGETTO_LabelTipologia));

            chiavi.Add(new KeyValuePair<string, string>("DataAvvio", DizionarioService.GRIGLIA_ColonnaDataAvvio));

            if (tipo == 1)
            {
                chiavi.Add(new KeyValuePair<string, string>("StatoProcedura", DizionarioService.GRIGLIA_ColonnaStatoProcedura));
            }
            else if (tipo == 2)
            {
                     chiavi.Add(new KeyValuePair<string, string>("StatoProcedura", DizionarioService.GRIGLIA_ColonnaStatoProcedura));
            }
            else if (tipo == 3)
            {
                if (ambitoProcedura.ID == 3)
                    chiavi.Add(new KeyValuePair<string, string>("Provvedimento", DizionarioService.GRIGLIA_ColonnaDataENumero));
                else
                    chiavi.Add(new KeyValuePair<string, string>("Provvedimento", DizionarioService.GRIGLIA_ColonnaDataENumeroProvvedimento));
                chiavi.Add(new KeyValuePair<string, string>("EsitoProvvedimento", DizionarioService.GRIGLIA_ColonnaEsito));
            }

            return chiavi.ToList();
        }

    }
}