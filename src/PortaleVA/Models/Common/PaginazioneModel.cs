using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VAPortale.Models.Common
{
    public abstract class PaginazioneModel : PaginaDinamicaModel
    {
        public PaginazioneModel()
        {
            Pagina = 1;
            DimensionePagina = 10;
            TotaleRisultati = 0;
        }

        public int Pagina { get; set; }

        public int TotaleRisultati { get; set; }

        public int DimensionePagina { get; set; }

        public int IndiceInizio
        {
            get
            {
                if (Pagina > 0)
                    return DimensionePagina * (Pagina - 1);
                else
                    return 0;
            }
        }

        public string Mode { get; set; }
    }
}