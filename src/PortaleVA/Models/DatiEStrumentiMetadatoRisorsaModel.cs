using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.DatiAmbientali;
using System.Data;

namespace VAPortale.Models
{
    public class DatiEStrumentiMetadatoRisorsaModel : PaginaDinamicaModel
    {
        public DatiEStrumentiMetadatoRisorsaModel()
        {
            InformazioniGenerali = new DataTable();
            InformazioniGenerali.Columns.Add("Chiave");
            InformazioniGenerali.Columns.Add("Valore");

            Entita = new DataTable();
            Entita.Columns.Add("Chiave");
            Entita.Columns.Add("Valore");

            InformazioniRicerca = new DataTable();
            InformazioniRicerca.Columns.Add("Chiave");
            InformazioniRicerca.Columns.Add("Valore");

            Date = new DataTable();
            Date.Columns.Add("Chiave");
            Date.Columns.Add("Valore");
        }
        
        public DataTable InformazioniGenerali { get; set; }

        public DataTable Entita { get; set; }

        public DataTable InformazioniRicerca { get; set; }

        public DataTable Date { get; set; }

        public Risorsa Risorsa { get; set; }

    }
}