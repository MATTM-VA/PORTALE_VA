using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class InEvidenza : Entity
    {
        public int idEvidenza { get; set; }
        public int ordine { get; set; }

        public int widgetID { get; set; }

        public int NotiziaID { get; set; }
    }

    public class ListaInEvidenza : Entity
    {
        public int ordine { get; set; }

        public int widgetID { get; set; }

        public string NomeWidget_IT { get; set; }

        public string NomeCategoria { get; set; }

        public string TitoloNotizia_IT { get; set; }

        public int ImmagineID { get; set; }

        public int NotiziaID { get; set; }

    }

}
