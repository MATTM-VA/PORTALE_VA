using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureAttributiPartialTabModel
    {
        public IEnumerable<Attributo> Attributi { get; set; }

        public Attributo Attributo { get; set; }

        public string Voce { get; set; }
    }
}