using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedMenuNavigazioneMobileModel
    {
        public SharedMenuNavigazioneMobileModel()
        {
            VociMenu = new List<VoceMenu>();
            VociMenuServizio = new List<VoceMenu>();
            NomeSezione = "Home";
        }

        public List<VoceMenu> VociMenu { get; set; }

        public List<VoceMenu> VociMenuServizio { get; set; }

        public string NomeSezione { get; set; }

        public string NomeVoce { get; set; }
    }
}