using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class OggettiInfoModel
    {
        public Oggetto Oggetto { get; set; }

        public OggettiTerritoriModel TerritoriModel { get; set; }

        public OggettiDatiAmministrativiModel DatiAmministrativiModel { get; set; }
    }
}