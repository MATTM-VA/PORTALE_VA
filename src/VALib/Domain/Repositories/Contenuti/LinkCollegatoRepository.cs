using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class LinkCollegatoRepository : Repository
    {
        private static readonly LinkCollegatoRepository _instance = new LinkCollegatoRepository(Settings.VAConnectionString);

        private LinkCollegatoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static LinkCollegatoRepository Instance
        {
            get { return _instance; }
        }

        public LinkCollegato RecuperaLinkCollegatiPerOggetto(int oggettoID, TipoLinkEnum tipoLink)
        {
            LinkCollegato linkCollegato = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT L.LinkID, L.Nome, L.Descrizione, L.Indirizzo, SOL.TipoLinkID
	                        FROM dbo.TBL_Link AS L INNER JOIN
		                        dbo.STG_OggettiLink AS SOL ON SOL.LinkID = L.LinkID
	                        WHERE SOL.OggettoID = @OggettoID AND SOL.TipoLinkID = @TipoLinkID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@OggettoID", oggettoID);
            sseo.SqlParameters.AddWithValue("@TipoLinkID", (int)tipoLink);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                linkCollegato = new LinkCollegato();

                linkCollegato.Link = new Link(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetString(3));
                linkCollegato.Tipo = TipoLinkRepository.Instance.RecuperaTipiLink().Single(x => x.ID == dr.GetInt32(4));
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return linkCollegato;
        }

    }
}
