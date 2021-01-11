using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Oggetto : MultilingualEntityWDescription
    {
        public Oggetto()
        {
            EntitaCollegate = new List<EntitaCollegata>();
            LinkCollegati = new List<LinkCollegato>();
            Territori = new List<Territorio>();
            DatiAmministrativi = new List<ValoreDatoAmministrativo>();
        }
        
        public DateTime? ScadenzaPresentazioneOsservazioni { get; internal set; }

        public string LinkLocalizzazione { get; internal set; }

        public TipoOggetto TipoOggetto { get; internal set; }

        public List<EntitaCollegata> EntitaCollegate { get; internal set; }

        public List<LinkCollegato> LinkCollegati { get; internal set; }

        public List<Territorio> Territori { get; internal set; }

        public List<ValoreDatoAmministrativo> DatiAmministrativi { get; internal set; }
    }
}
