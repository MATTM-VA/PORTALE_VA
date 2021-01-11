using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class OggettoEventiModel
    {
        public OggettoEventiModel()
        {
            Eventi = new List<EventoModel>();
        }
        public List<EventoModel> Eventi { get; set; }
    }
}