using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class EventoModel
    {
        public GM_Evento Evento { get; set; }
        public GM_TipoEvento TipoEvento { get; set; }
    }
}