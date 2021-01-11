using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class ComunicazioneCittadinoModel : PaginaDinamicaModel
    {
        public List<VoceMenu> Links { get; set; }
    }
}