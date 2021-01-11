using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VAPortale.Models.Common;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class HomeIndexModel
    {
        public List<OggettoHome> Oggetti { get; set; }

        public List<OggettoHome> OggettiVAV { get; set; }

        public List<OggettoHome> OggettiVAS { get; set; }
      
        public List<WidgetCorrelato> Widget { get; set; }
    }
}