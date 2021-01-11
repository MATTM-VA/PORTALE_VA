using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class WidgetIndexModel : PaginazioneModel
    {
        public string Testo { get; set; }

        [DisplayName("Tipo widget")]
        public TipoWidget TipoWidget { get; set; }

        public IEnumerable<SelectListItem> TipoWidgetSelectList { get; set; }

        public List<Widget> Widget { get; set; }

        public string EditaActionName { get; set; }
    }
}