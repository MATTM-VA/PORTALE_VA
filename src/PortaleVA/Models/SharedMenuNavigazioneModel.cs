using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedMenuNavigazioneModel
    {
        public SharedMenuNavigazioneModel()
        {
            VociMenu = new List<VoceMenu>();
            NomeSezione = "Home";
        }

        public List<VoceMenu> VociMenu { get; set; }

        public string NomeSezione { get; set; }

        public string NomeVoce { get; set; }
    }
}