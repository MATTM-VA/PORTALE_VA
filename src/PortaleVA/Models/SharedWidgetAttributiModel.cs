using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class SharedWidgetAttributiModel
    {
        public SharedWidgetAttributiModel()
        {
            TipiAttributi = new List<TipoAttributo>();
            Attributi = new List<Attributo>();
            Oggetti = new List<OggettoWidgetAttributo>();
        }

        public List<TipoAttributo> TipiAttributi { get; set; }

        public List<Attributo> Attributi { get; set; }

        public List<OggettoWidgetAttributo> Oggetti { get; set; }
    }
}