using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VAPortale.Areas.Admin.Models
{
    public abstract class PaginazioneModel
    {
        public PaginazioneModel()
        {
            Pagina = 1;
            DimensionePagina = 25;
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
    }
}