using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class VariabileIndexModel : PaginazioneModel
    {
        public List<Variabile> Variabili { get; set; }
    }
}