using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using System.Threading;

namespace VALib.Domain.Entities.Contenuti
{
    public class Raggruppamento : MultilingualEntity
    {
        internal Raggruppamento(int id, int genitoreID, string nome_it, string nome_en, int ordine)
        {
            ID = id;
            GenitoreID = genitoreID;
            _nome_IT = nome_it;
            _nome_EN = nome_en;
            Ordine = ordine;
        }
        
        public int GenitoreID { get; internal set; }

        public int LivelloVisibilita { get; internal set; }

        public int Ordine { get; internal set; }

        internal int Figli { get; set; }

        public bool HaFigli
        {
            get
            {
                return Figli > 0;
            }
        }
    }
}
