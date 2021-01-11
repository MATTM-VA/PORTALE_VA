using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class OggettiDatiAmministrativiModel
    {
        public OggettiDatiAmministrativiModel()
        {
            ProcedureCollegate = new List<ProceduraCollegata>();
            DatiAmministrativi = new List<ValoreDatoAmministrativo>();
        }

        public int OggettoID { get; set; }

        public List<ProceduraCollegata> ProcedureCollegate { get; set; }

        public List<ValoreDatoAmministrativo> DatiAmministrativi { get; set; }
    }
}