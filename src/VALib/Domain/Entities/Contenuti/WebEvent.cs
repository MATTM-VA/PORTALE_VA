using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class WebEvent
    {
        public Guid EventID { get; internal set; }
        public DateTime EventTime { get; internal set; }
        public String RequestUrl  { get; internal set; }
        public String UserHostAddress { get; internal set; }
        public String ExceptionMessage { get; internal set; }
        
    }
}
