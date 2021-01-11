using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class DatoAmministrativo : MultilingualEntity
    {
        internal DatoAmministrativo()
        {

        }
        
        internal DatoAmministrativo(int id, string nome_IT, string nome_EN, string tipoDati)
        {
            this.ID = id;
            this._nome_IT = nome_IT;
            this._nome_EN = nome_EN;
            this.TipoDati = tipoDati;
        }
        
        public string TipoDati { get; internal set; }

        public int Ordine { get; internal set; }
    }
}
