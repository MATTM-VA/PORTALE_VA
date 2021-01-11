using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti.Base
{
    public class OggettoDocumentazioneBase : OggettoBase
    {
        public OggettoDocumentazioneBase()
        {
            //EntitaCollegate = new List<EntitaCollegata>();
            //ProcedureCollegate = new List<ProceduraCollegata>();
            DatiAmministrativi = new List<ValoreDatoAmministrativo>();
        }
        
        //public DateTime? ScadenzaPresentazioneOsservazioni { get; internal set; }

        //public List<EntitaCollegata> EntitaCollegate { get; internal set; }

        //public List<ProceduraCollegata> ProcedureCollegate { get; internal set; }

        public int OggettoProceduraID { get; internal set; }

        public ProceduraCollegata ProceduraCollegata { get; internal set; }

        public List<ValoreDatoAmministrativo> DatiAmministrativi { get; internal set; }
    }
}
