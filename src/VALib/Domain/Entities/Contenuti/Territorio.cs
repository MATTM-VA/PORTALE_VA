using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class Territorio
    {
        public Territorio()
        {
            Selezionato = false;
        }

        internal Territorio(Guid id, Guid? genitoreID, TipologiaTerritorio tipologia, string nome, string codiceIstat)
        {
            this.ID = id;
            this.GenitoreID = genitoreID;
            this.Tipologia = tipologia;
            this.Nome = nome;
            this.CodiceIstat = codiceIstat;
            this.Selezionato = false;
        }
        
        public Guid ID { get; internal set; }

        public Guid? GenitoreID { get; internal set; }

        public TipologiaTerritorio Tipologia { get; internal set; }

        public string Nome { get; internal set; }

        public string CodiceIstat { get; internal set; }

        public bool Selezionato { get; internal set; }
    }
}
