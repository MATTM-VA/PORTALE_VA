using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class PaginaIndexModel
    {
        public List<Tuple<VoceMenu, int?, string, PaginaStatica>> VociMenu { get; set; }
    }
}