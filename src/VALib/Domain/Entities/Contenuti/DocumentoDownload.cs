using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class DocumentoDownload : Entity
    {
        public int OggettoID { get; internal set; }

        public string PercorsoFile { get; internal set; }

        public string Estensione { get; internal set; }
        public int OggettoProceduraID { get; internal set; }
        public int MacroTipoOggettoID { get; set; }
    }
}
