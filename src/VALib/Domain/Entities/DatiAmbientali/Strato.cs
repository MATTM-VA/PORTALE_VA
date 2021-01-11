using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.DatiAmbientali
{
    public class Strato : GuidEntity
    {
        public Strato()
        {
            Identificazione = new List<Tuple<string, string>>();
            Classificazione = new List<Tuple<string, string>>();
            ParoleChiave = new List<Tuple<string, string>>();
            Localizzazione = new List<Tuple<string, string>>();
            RiferimentoTemporale = new List<Tuple<string, string>>();
            Qualita = new List<Tuple<string, string>>();
            SistemaRiferimento = new List<Tuple<string, string>>();
            Conformita = new List<Tuple<string, string>>();
            Vincoli = new List<Tuple<string, string>>();
            Responsabili = new List<Tuple<string, string>>();
            Distribuzione = new List<Tuple<string, string>>();
            Metadati = new List<Tuple<string, string>>();
            Links = new List<StratoMetadatiLink>();
        }
        
        public List<Tuple<string, string>> Identificazione { get; internal set; }

        public List<Tuple<string, string>> Classificazione { get; internal set; }

        public List<Tuple<string, string>> ParoleChiave { get; internal set; }

        public List<Tuple<string, string>> Localizzazione { get; internal set; }

        public List<Tuple<string, string>> RiferimentoTemporale { get; internal set; }

        public List<Tuple<string, string>> Qualita { get; internal set; }

        public List<Tuple<string, string>> SistemaRiferimento { get; internal set; }

        public List<Tuple<string, string>> Conformita { get; internal set; }

        public List<Tuple<string, string>> Vincoli { get; internal set; }

        public List<Tuple<string, string>> Responsabili { get; internal set; }

        public List<Tuple<string, string>> Distribuzione { get; internal set; }

        public List<Tuple<string, string>> Metadati { get; internal set; }

        public List<StratoMetadatiLink> Links { get; internal set; }

        public string Titolo { get; internal set; }
    }
}
