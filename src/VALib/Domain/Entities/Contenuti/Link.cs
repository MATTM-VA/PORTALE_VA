using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Link : Entity
    {
        internal Link(int id, string nome, string descrizione, string indirizzo)
        {
            this.ID = id;
            this.Nome = nome;
            this.Descrizione = descrizione;
            this.Indirizzo = indirizzo;
        }

        public string Nome { get; internal set; }

        public string Descrizione { get; internal set; }

        public string Indirizzo { get; internal set; }
    }
}
