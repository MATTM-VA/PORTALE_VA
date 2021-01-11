using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoWidgetAttributo : MultilingualEntity
    {
        public TipoAttributo TipoAttributo { get; internal set; }

        public Attributo Attributo { get; internal set; }
    }
}
