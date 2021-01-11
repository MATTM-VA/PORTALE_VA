using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;
using System.ComponentModel;
using System.Web.Mvc;

namespace VAPortale.Areas.Admin.Models
{
    public class DatoAmbientaleHomeIndexModel : PaginazioneModel
    {
        [DisplayName("Testo")]
        public string Testo { get; set; }

        [DisplayName("Pubblicato")]
        public bool? Pubblicato { get; set; }

        public IEnumerable<SelectListItem> BooleanSelectList { get; set; }

        public List<DatoAmbientaleHome> DatiAmbientaliHome { get; set; }
    }
}