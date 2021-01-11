using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using System.Threading;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace VAPortale.Models
{
    public class RicercaViaSpazialeModel : PaginazioneModel
    {
        public string XMax { get; set; }
        public string YMax { get; set; }
        public string XMin { get; set; }
        public string YMin { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MapZoom { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MapCenter { get; set; }
        
        public List<OggettoElenco> Oggetti { get; set; }

    }
}