using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Helpers;

namespace VALib.Domain.Entities.Contenuti
{
    public class ReportProcedura : MultilingualEntity
    {
        public MacroTipoOggettoEnum MacroTipoOggetto { get; internal set; }

        public int OggettoProceduraID { get; internal set; }

        public string IDVIP { get; internal set; }

        public string Proponente { get; internal set; }

        public DateTime DataAvvio { get; internal set; }

        public StatoProcedura StatoProcedura { get; internal set; }

        public DateTime? DataProvvedimento { get; internal set; }

        public string NumeroProvvedimento { get; internal set; }

        public string Esito { get; internal set; }

        public Settore Settore { get; internal set; }

        public Tipologia Tipologia { get; internal set; }

        public CategoriaImpianto CategoriaImpianto { get; internal set; }

        public string GetValore(string chiave)
        {
            string result = "";

            if (chiave.Equals("IDVIP"))
            {
                if (this.IDVIP == null)
                    result = "-";
                else
                    //result = this.IDVIP.Value.ToString(CultureInfo.InvariantCulture);
                    result = this.IDVIP.ToString(CultureInfo.InvariantCulture);
            }
            else if (chiave.Equals("NomeVas") || chiave.Equals("NomeVia") || chiave.Equals("NomeAia"))
                result = this.GetNome();
            else if (chiave.Equals("Proponente") || chiave.Equals("Gestore"))
                result = this.Proponente;
            else if (chiave.Equals("NomeSettore"))
                result = this.Settore.GetNome();
            else if (chiave.Equals("TipologiaOpera"))
                result = this.Tipologia.GetNome();
            else if (chiave.Equals("DataAvvio"))
                result = this.DataAvvio.ToString(CultureHelper.GetDateFormat());
            else if (chiave.Equals("StatoProcedura"))
                result = this.StatoProcedura.GetNome();
            else if (chiave.Equals("Provvedimento"))
            {
                if (this.DataProvvedimento != null)
                    result += this.DataProvvedimento.Value.ToString(CultureHelper.GetDateFormat());

                if (!string.IsNullOrWhiteSpace(this.NumeroProvvedimento))
                {
                    if (!string.IsNullOrWhiteSpace(result))
                        result += " - ";

                    result += this.NumeroProvvedimento;
                }
            }
            else if (chiave.Equals("EsitoProvvedimento"))
            { 
                result = this.Esito;
            }
            else if (chiave.Equals("CategoriaImpianto"))
            {                
                result = this.CategoriaImpianto.GetNome();                
            }

            if (string.IsNullOrWhiteSpace(result))
                result = "-";

            return result;
        }
    }
}
