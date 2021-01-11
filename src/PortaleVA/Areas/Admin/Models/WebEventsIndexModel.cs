using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class WebEventsIndexModel : PaginazioneModel
    {
        public WebEventsIndexModel()
        {
            ElencoWebEvents = new List<WebEvent>();
        }

        public List<WebEvent> ElencoWebEvents { get; set; }
    }
}