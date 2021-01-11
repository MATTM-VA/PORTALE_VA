using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedBreadcrumbsModel
    {
        public IEnumerable<VoceMenu> Genitori { get; set; }

        public VoceMenu Voce { get; set; }
    }
}