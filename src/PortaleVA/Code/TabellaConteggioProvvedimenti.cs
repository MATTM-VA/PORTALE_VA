using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Code
{
    public class TabellaConteggioProvvedimenti
    {
        public TabellaConteggioProvvedimenti()
        {
            Righe = new List<ConteggioTipoProvvedimento>();
        }
        
        public AreaTipoProvvedimento Area { get; set; }

        public List<ConteggioTipoProvvedimento> Righe { get; set; }

        public int Conteggio
        {
            get
            {
                return Righe.Sum(x => x.Conteggio);
            }


        }
    }
}