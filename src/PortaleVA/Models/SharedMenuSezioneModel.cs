using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedMenuSezioneModel
    {
        public SharedMenuSezioneModel()
        {
            VociMenu = new List<VoceMenu>();
        }

        public IEnumerable<VoceMenu> VociMenu { get; set; }

        public string NomeSezione { get; set; }

        public string NomeVoce { get; set; }
    }
}