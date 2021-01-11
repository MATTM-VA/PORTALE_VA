using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using System.Globalization;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using VALib.Domain.Services;
using VALib.Helpers;
using VALib.Domain.Entities.DatiAmbientali;

namespace VAPortale.Code
{
    public static class EsportazioneUtils
    {
        private static string[] GetHeaderCells(MacroTipoOggettoEnum? macroTipoOggetto, Griglia grigliaEsportazione)
        {
            string[] headerCells = null;

            switch (grigliaEsportazione)
            {
                case Griglia.OggettiRicerca:                    
                    
                    if (macroTipoOggetto == null)
                    {
                        headerCells = new string[4];
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoOrizzontale;
                        headerCells[1] = DizionarioService.GRIGLIA_ColonnaProponente +"/"+ DizionarioService.GRIGLIA_ColonnaGestore;
                        headerCells[2] = DizionarioService.GRIGLIA_ColonnaOggetto;
                        headerCells[3] = DizionarioService.GRIGLIA_ColonnaUltimaProcedura;
                    }
                    else {
                        headerCells = new string[3];
                        if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Vas)
                            headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVas;
                        else
                            headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVia;
                            headerCells[1] = DizionarioService.GRIGLIA_ColonnaProponente;
                            headerCells[2] = DizionarioService.GRIGLIA_ColonnaUltimaProcedura;
                    }
                    
                    break;
                case Griglia.DocumentiRicerca:
                    headerCells = new string[5];
                    if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Vas)
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVas;
                    else
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVia;

                    headerCells[1] = DizionarioService.GRIGLIA_ColonnaTitolo;
                    headerCells[2] = DizionarioService.GRIGLIA_ColonnaData;
                    headerCells[3] = DizionarioService.GRIGLIA_ColonnaScala;
                    headerCells[4] = DizionarioService.GRIGLIA_ColonnaDimensione;
                    break;
                case Griglia.OggettiProcedureInCorso:
                    headerCells = new string[4];
                    
                    if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Aia)
                    {
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoAia;
                        headerCells[1] = DizionarioService.GRIGLIA_ColonnaGestore;
                    }
                    else {
                        if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Vas)
                            headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVas;
                        else
                            headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVia;

                        headerCells[1] = DizionarioService.GRIGLIA_ColonnaProponente;
                    }
                    
                    headerCells[2] = DizionarioService.GRIGLIA_ColonnaDataAvvio;
                    headerCells[3] = DizionarioService.GRIGLIA_ColonnaStatoProcedura;
                    break;
                case Griglia.OggettiOsservatoriAmbientali:
                    headerCells = new string[2];
                    if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Vas)
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVas;
                    else
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVia;

                    headerCells[1] = DizionarioService.GRIGLIA_ColonnaProponente;
                    break;
                case Griglia.DatiAmbientaliRicerca:
                    headerCells = new string[5];
                    headerCells[0] = DizionarioService.GRIGLIA_ColonnaTitolo;
                    headerCells[1] = DizionarioService.GRIGLIA_ColonnaTema;
                    headerCells[2] = DizionarioService.GRIGLIA_ColonnaScala;
                    headerCells[3] = "wms/wfs";
                    headerCells[4] = "google earth";
                    break;
                case Griglia.DatiAmbientaliCondivisione:
                    headerCells = new string[3];
                    headerCells[0] = DizionarioService.GRIGLIA_ColonnaTitolo;
                    headerCells[1] = DizionarioService.GRIGLIA_ColonnaArgomento;
                    headerCells[2] = DizionarioService.GRIGLIA_ColonnaDimensione;
                    break;
                case Griglia.DocumentiDocumentazione:
                    headerCells = new string[6];
                    headerCells[0] = DizionarioService.GRIGLIA_ColonnaTitolo;
                    headerCells[1] = DizionarioService.GRIGLIA_ColonnaSezione;
                    headerCells[2] = DizionarioService.GRIGLIA_ColonnaCodiceElaborato;
                    headerCells[3] = DizionarioService.GRIGLIA_ColonnaData;
                    headerCells[4] = DizionarioService.GRIGLIA_ColonnaScala;
                    headerCells[5] = DizionarioService.GRIGLIA_ColonnaDimensione;
                    break;
                case Griglia.OggettiRicercaProcedura:
                    headerCells = new string[3];

                    if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Aia)
                    {
                        headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoAia;
                        headerCells[1] = DizionarioService.GRIGLIA_ColonnaGestore;
                    }
                    else
                    {
                        if (macroTipoOggetto.Value == MacroTipoOggettoEnum.Vas)
                            headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVas;
                        else
                            headerCells[0] = DizionarioService.GRIGLIA_ColonnaOggettoVia;

                        headerCells[1] = DizionarioService.GRIGLIA_ColonnaProponente;
                    }
                    headerCells[2] = DizionarioService.GRIGLIA_ColonnaProcedura;
                    break;
                default:
                    break;
            }

            return headerCells;
        }

        public static byte[] GeneraXlsxOggettiRicerca(IEnumerable<OggettoElenco> oggetti, MacroTipoOggettoEnum? macroTipoOggetto, bool ricercaProcedura)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;
            if (ricercaProcedura)
                headerCells = GetHeaderCells(macroTipoOggetto, Griglia.OggettiRicercaProcedura);
            else
                headerCells = GetHeaderCells(macroTipoOggetto, Griglia.OggettiRicerca);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }
                    
                r++;

                c = 0;

                foreach (OggettoElenco o in oggetti)
                {
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(o.GetNome());

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(o.Proponente);

                    if (macroTipoOggetto == null)
                    {
                        dataCell = dataRow.CreateCell(2);
                        dataCell.SetCellValue(o.TipoOggetto.GetNome());

                        dataCell = dataRow.CreateCell(3);
                        dataCell.SetCellValue(o.Procedura.GetNome());
                    }
                    else
                    {
                        dataCell = dataRow.CreateCell(2);
                        dataCell.SetCellValue(o.Procedura.GetNome());
                    }

                   
                    
                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
                mss.Dispose();
            }

            return fileBytes;
        }

        public static byte[] GeneraXlsxDocumentiRicerca(IEnumerable<DocumentoElenco> items, MacroTipoOggettoEnum macroTipoOggetto)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;

            headerCells = GetHeaderCells(macroTipoOggetto, Griglia.DocumentiRicerca);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }

                r++;

                c = 0;

                foreach (DocumentoElenco i in items)
                {
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(i.GetNome());

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(i.Titolo);

                    dataCell = dataRow.CreateCell(2);
                    dataCell.SetCellValue(i.Data.ToString(CultureHelper.GetDateFormat()));

                    dataCell = dataRow.CreateCell(3);
                    dataCell.SetCellValue(i.Scala);

                    dataCell = dataRow.CreateCell(4);
                    dataCell.SetCellValue(string.Format("{0} kB", i.Dimensione));

                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
            }

            return fileBytes;
        }

        public static byte[] GeneraXlsxOsservatoriAmbientali(List<OggettoElenco> oggetti, MacroTipoOggettoEnum macroTipoOggetto)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;

            headerCells = GetHeaderCells(macroTipoOggetto, Griglia.OggettiOsservatoriAmbientali);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }

                r++;

                c = 0;

                foreach (OggettoElenco o in oggetti)
                {
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(o.GetNome());

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(o.Proponente);

                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
            }

            return fileBytes;
        }

        public static byte[] GeneraXlsxProcedureInCorso(List<OggettoElencoProcedura> oggetti, MacroTipoOggettoEnum macroTipoOggetto)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;

            headerCells = GetHeaderCells(macroTipoOggetto, Griglia.OggettiProcedureInCorso);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }

                r++;

                c = 0;

                foreach (OggettoElencoProcedura o in oggetti)
                {
                    string statoProcedura = "-";

                    if (o.StatoProcedura != null)
                        statoProcedura = o.StatoProcedura.GetNome();
                    
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(o.GetNome());

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(o.Proponente);

                    dataCell = dataRow.CreateCell(2);
                    dataCell.SetCellValue(o.Data.ToString(CultureHelper.GetDateFormat()));

                    dataCell = dataRow.CreateCell(3);
                    dataCell.SetCellValue(statoProcedura);

                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
            }

            return fileBytes;
        }

        public static byte[] GeneraXlsxDatiAmbientaliRicerca(List<RisorsaElenco> risorse)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;

            headerCells = GetHeaderCells(null, Griglia.DatiAmbientaliRicerca);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }

                r++;

                c = 0;

                foreach (RisorsaElenco re in risorse)
                {
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(re.Titolo);

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(re.Tema.Nome);

                    dataCell = dataRow.CreateCell(2);
                    dataCell.SetCellValue(re.Scala);

                    dataCell = dataRow.CreateCell(3);
                    dataCell.SetCellValue(re.UrlWms);

                    dataCell = dataRow.CreateCell(4);
                    dataCell.SetCellValue(re.UrlGoogleEarth);

                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
            }

            return fileBytes;
        }

        public static byte[] GeneraXlsxDatiAmbientaliCondivisione(List<DocumentoCondivisione> risorse)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;

            headerCells = GetHeaderCells(null, Griglia.DatiAmbientaliCondivisione);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }

                r++;

                c = 0;

                foreach (DocumentoCondivisione dc in risorse)
                {
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(dc.Titolo);

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(dc.Soggetto);

                    dataCell = dataRow.CreateCell(2);
                    dataCell.SetCellValue(string.Format("{0} kB", dc.Dimensione));

                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
            }

            return fileBytes;
        }

        public static byte[] GeneraXlsxDocumentiDocumentazione(IEnumerable<DocumentoElenco> items, MacroTipoOggettoEnum macroTipoOggetto)
        {
            XSSFWorkbook workbook = null;

            byte[] fileBytes = null;
            string[] headerCells = null;

            int r = 0;
            int c = 0;

            headerCells = GetHeaderCells(macroTipoOggetto, Griglia.DocumentiDocumentazione);

            workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Export");

            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.LEFT;

            IFont headerCellFont = workbook.CreateFont();
            headerCellFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerCellStyle.SetFont(headerCellFont);

            try
            {
                IRow headerRow = sheet1.CreateRow(r);

                foreach (string s in headerCells)
                {
                    ICell dataCell = headerRow.CreateCell(c);
                    dataCell.SetCellValue(s);
                    dataCell.CellStyle = headerCellStyle;
                    c++;
                }

                r++;

                c = 0;

                foreach (DocumentoElenco i in items)
                {
                    IRow dataRow = sheet1.CreateRow(r);

                    ICell dataCell = dataRow.CreateCell(0);
                    dataCell.SetCellValue(i.Titolo);

                    dataCell = dataRow.CreateCell(1);
                    dataCell.SetCellValue(i.Raggruppamento.GetNome());

                    dataCell = dataRow.CreateCell(2);
                    dataCell.SetCellValue(i.CodiceElaborato);

                    dataCell = dataRow.CreateCell(3);
                    dataCell.SetCellValue(i.Data.ToString(CultureHelper.GetDateFormat()));

                    dataCell = dataRow.CreateCell(4);
                    dataCell.SetCellValue(i.Scala);

                    dataCell = dataRow.CreateCell(5);
                    dataCell.SetCellValue(string.Format("{0} kB", i.Dimensione));

                    r++;
                }
            }
            catch (Exception ex)
            {

            }

            MemoryStream mss = new MemoryStream();

            try
            {
                workbook.Write(mss);
                fileBytes = mss.ToArray();
            }
            finally
            {
                mss.Close();
            }

            return fileBytes;
        }
    }

    public enum Griglia
    {
        OggettiRicerca = 1, 
        DocumentiRicerca = 2, 
        OggettiProcedureInCorso = 3, 
        OggettiOsservatoriAmbientali = 4, 
        DatiAmbientaliRicerca = 5, 
        DatiAmbientaliCondivisione = 6, 
        DocumentiDocumentazione = 7, 
        OggettiRicercaProcedura = 8
    }
}