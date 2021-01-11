using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;
using System.Data;

namespace VAPortale.Models
{
    public class OggettiMetadatoDocumentoModel : PaginaDinamicaModel
    {
        public OggettiMetadatoDocumentoModel()
        {
            InformazioniGenerali = new DataTable();
            InformazioniGenerali.Columns.Add("Chiave");
            InformazioniGenerali.Columns.Add("Valore");

            InformazioniContenuto = new DataTable();
            InformazioniContenuto.Columns.Add("Chiave");
            InformazioniContenuto.Columns.Add("Valore");

            Date = new DataTable();
            Date.Columns.Add("Chiave");
            Date.Columns.Add("Valore");
        }
        
        public Documento Documento { get; set; }

        public DataTable InformazioniGenerali { get; set; }

        public DataTable InformazioniContenuto { get; set; }

        public DataTable Date { get; set; }
    }
}