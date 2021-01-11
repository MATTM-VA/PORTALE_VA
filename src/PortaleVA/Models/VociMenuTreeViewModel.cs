using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class VociMenuTreeViewModel
    {
        public VociMenuTreeViewModel()
        {
            VociMenu = new List<VoceMenu>();
        }
        
        public int VoceMenuID { get; set; }

        public List<VoceMenu> VociMenu { get; set; }
    }
}