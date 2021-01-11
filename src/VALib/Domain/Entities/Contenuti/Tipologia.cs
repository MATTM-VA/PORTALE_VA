using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Tipologia : MultilingualEntity
    {
        public Macrotipologia Macrotipologia { get; internal set; }

        public string FileIcona { get; internal set; }
    }
}
