using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public enum RuoloEntitaEnum
    {
        Nessuno = 0,
        Proponente = 1, 
        AutoritaCompetente = 2, 
        SoggettoPortatoreInteresse = 3,
        Autore = 4, 
        Redattore = 5, 
        ResponsabilePubblicazione = 6, 
        ResponsabileMetadato = 7,
        AutoritaProcedente = 8,
        Richiedente = 9,
        Gestore = 10
    }
}
