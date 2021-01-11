using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class AttivitaIPPC : MultilingualEntity
    {
        internal AttivitaIPPC()
        {

        }

        internal AttivitaIPPC(int id, string codice, string nome_IT, string nome_EN)
        {
            this.ID = id;            
            this._nome_IT = nome_IT;
            this._nome_EN = nome_EN;
            this.Codice = codice;        
        }

        public string Codice { get; internal set; }
    }
}
