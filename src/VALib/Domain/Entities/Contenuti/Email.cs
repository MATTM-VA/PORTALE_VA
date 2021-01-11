using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Email : Entity
    {
        public string Testo { get; internal set; }

        public string IndirizzoEmail { get; internal set; }

        public string Tipo { get; internal set; }

        public string Data { get; internal set; }
    }
}
