using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Entities.Contenuti
{
    public class ProvvedimentoHome : ElementoHomeBase
    {
        public int ProvvedimentoID { get; internal set; }

        public DateTime DataProvvedimento { get; internal set; }
    }
}
