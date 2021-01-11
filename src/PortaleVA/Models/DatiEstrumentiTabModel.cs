using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.DatiAmbientali;

namespace VAPortale.Models
{
    public class DatiEstrumentiTabModel : DatiEStrumentiTabBaseModel
    {
        public List<DocumentoCondivisione> Risorse { get; set; }
    }
}