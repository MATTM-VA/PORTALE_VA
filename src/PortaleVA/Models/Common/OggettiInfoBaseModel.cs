using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VAPortale.Models.Common
{
    public class OggettiInfoBaseModel : PaginaDinamicaModel
    {
        public OggettiTerritoriModel TerritoriModel { get; set; }

        public OggettiDatiAmministrativiModel DatiAmministrativiModel { get; set; }

        public OggettoEventiModel EventiModel { get; set; }
    }
}