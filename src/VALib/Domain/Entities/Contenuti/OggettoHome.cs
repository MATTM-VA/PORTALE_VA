using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoHome : ElementoHomeBase
    {   
        public int DocumentoID { get; internal set; }

        public DateTime DataScadenzaPresentazione { get; internal set; }

        /* SERIO - 27/11/17 */
        public TipoElencoEnum TipoElenco { get; internal set; }
        
    }
}
