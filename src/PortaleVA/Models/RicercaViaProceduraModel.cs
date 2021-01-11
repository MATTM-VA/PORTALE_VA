using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class RicercaViaProceduraModel : PaginazioneModel
    {
        public IEnumerable<SelectListItem> ProcedureSelectList { get; set; }

        public int ProceduraID { get; set; }

        public string Testo { get; set; }

        public List<OggettoElenco> Oggetti { get; set; }

    }
}