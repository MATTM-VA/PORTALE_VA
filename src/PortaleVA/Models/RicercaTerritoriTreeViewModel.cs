using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class RicercaTerritoriTreeViewModel
    {
        public RicercaTerritoriTreeViewModel()
        {
            Territori = new List<Territorio>();
        }
        
        public Guid? TerritorioID { get; set; }

        public List<Territorio> Territori { get; set; }

        public MacroTipoOggettoEnum MacroTipoOggetto { get; set; }
    }
}