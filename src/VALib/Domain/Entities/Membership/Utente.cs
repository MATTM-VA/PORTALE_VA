using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using System.Collections.ObjectModel;

namespace VALib.Domain.Entities.Membership
{
    public class Utente : Entity
    {
        internal Utente()
        {
            ListaRuoli = new List<RuoloUtente>();
            RuoliUtente = new ReadOnlyCollection<RuoloUtente>(ListaRuoli);
        }

        //public string Nominativo { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        public string Email { get; set; }

        public string NomeUtente { get; set; }

        public int Ruolo { get; internal set; }

        public bool Abilitato { get; set; }

        public ReadOnlyCollection<RuoloUtente> RuoliUtente { get; private set; }

        internal List<RuoloUtente> ListaRuoli { get; set; }

        public DateTime? DataUltimoCambioPassword { get; internal set; }

        public DateTime? DataUltimoLogin { get; internal set; }
    }
}
