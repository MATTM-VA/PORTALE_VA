using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class OggettiInfoVasModel : OggettiInfoBaseModel
    {
        public OggettoInfoVas Oggetto { get; set; }

        public EntitaCollegata AutoritaProcedente { get; set; }

        public EntitaCollegata AutoritaCompetente { get; set; }

        public EntitaCollegata Proponente { get; set; }
    }
}