using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using System.ComponentModel.DataAnnotations;

namespace VAPortale.Models
{
    public class RicercaViaVasModel : PaginazioneModel
    {
        [Required]
        public string Testo { get; set; }

        public List<OggettoElenco> Oggetti { get; set; }
    }
}