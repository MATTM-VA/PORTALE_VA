using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Web
{
    public class VAWebRequestInformation
    {
        internal VAWebRequestInformation() { }

        public int? UtenteID { get; internal set; }

        public string NomeUtente { get; internal set; }

        public string UserAgent { get; internal set; }

        public string UrlReferrer { get; internal set; }

        public int? IntEntityID { get; internal set; }

        public Guid? GuidEntityID { get; internal set; }

        public VAWebEventTypeEnum? EventTypeID { get; internal set; }
    }
}
