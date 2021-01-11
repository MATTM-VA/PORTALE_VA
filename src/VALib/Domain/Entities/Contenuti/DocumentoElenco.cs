using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class DocumentoElenco : MultilingualEntity
    {
        public Raggruppamento Raggruppamento { get; internal set; }

        public TipoFile TipoFile { get; internal set; }

        public string Titolo { get; internal set; }

        public string CodiceElaborato { get; internal set; }

        public string Scala { get; internal set; }

        public int Dimensione { get; internal set; }

        public DateTime Data { get; internal set; }

        public DateTime? DataScadenzaPresentazioneOsservazioni { get; internal set; }

        public int OggettoID { get; internal set; }

        public int OggettoProceduraID { get; internal set; }
    }
}
