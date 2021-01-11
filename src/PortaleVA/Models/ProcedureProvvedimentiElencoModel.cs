using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureProvvedimentiElencoModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public string DataDa { get; set; }

        public string DataA { get; set; }

        public List<Provvedimento> Risorse { get; set; }

        public TipoProvvedimento TipoProvvedimento { get; set; }

        public string NomeTipoProvvedimento { get; set; }

        public int VoceMenuID { get; set; }

        public string Azione { get; set; }

        public IEnumerable<SelectListItem> ProcedureSelectList { get; set; }

        public IEnumerable<SelectListItem> CategorieInstallazioneSelectList { get; set; }
        
    }
}