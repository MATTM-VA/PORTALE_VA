using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti.Base
{
    public class OggettoInfoBase : OggettoBase
    {
        public OggettoInfoBase()
        {
            EntitaCollegate = new List<EntitaCollegata>();
            LinkCollegati = new List<LinkCollegato>();
            Territori = new List<Territorio>();
            ProcedureCollegate = new List<ProceduraCollegata>();
            DatiAmministrativi = new List<ValoreDatoAmministrativo>();
        }
        
        public DateTime? ScadenzaPresentazioneOsservazioni { get; internal set; }

        public string LinkLocalizzazione { get; internal set; }

        public List<EntitaCollegata> EntitaCollegate { get; internal set; }

        public List<LinkCollegato> LinkCollegati { get; internal set; }

        public List<Territorio> Territori { get; internal set; }

        public List<ProceduraCollegata> ProcedureCollegate { get; internal set; }

        public List<ValoreDatoAmministrativo> DatiAmministrativi { get; internal set; }

        public int OggettoProceduraID { get; internal set; }
    }
}
