using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Entita : Entity
    {
        internal Entita(int id, string nome, string codicefiscale, string indirizzo, string cap, string citta, string provincia, string sitoweb)
        {
            this.ID = id;
            this.Nome = nome;
            this.CodiceFiscale = codicefiscale;
            this.Indirizzo = indirizzo;
            this.Cap = cap;
            this.Citta = citta;
            this.Provincia = provincia;
            this.SitoWeb = sitoweb;
        }
        
        public string Nome { get; internal set; }
        public string CodiceFiscale { get; internal set; }
        public string Indirizzo  { get; internal set; }
        public string Cap  { get; internal set; }
        public string Citta { get; internal set; }
        public string Provincia { get; internal set; }
        public string SitoWeb { get; internal set; }
    
    }
}
