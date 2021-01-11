using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class TerritorioRepository : Repository
    {
        private static readonly TerritorioRepository _instance = new TerritorioRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private TerritorioRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static TerritorioRepository Instance
        {
            get { return _instance; }
        }

        public List<Territorio> RecuperaTerritoriRaggruppatiPerTerritorio(MacroTipoOggettoEnum macrotipoOggetto, string testo, int criterio, int? tipologiaTerritorioID)
        {
            List<Territorio> territori = new List<Territorio>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";


            switch (macrotipoOggetto)
            {
                case MacroTipoOggettoEnum.Via:
                    sSql = "SP_RecuperaTerritoriVia_R_Territorio";
                    break;
                case MacroTipoOggettoEnum.Vas:
                    sSql = "SP_RecuperaTerritoriVas_R_Territorio";
                    break;
            }

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@testo", testo);
            sseo.SqlParameters.AddWithValue("@criterio", criterio);
            sseo.SqlParameters.AddWithValue("@tipologia", tipologiaTerritorioID.HasValue ? (object)tipologiaTerritorioID.Value : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Territorio documentoPortale = RiempiIstanza(dr);
                territori.Add(documentoPortale);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return territori;
        }

        public Territorio RecuperaTerritorio(int id)
        {
            Territorio Territorio = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT D.TerritorioID, D.TipoFileID, D.Nome_IT, D.Nome_EN, D.NomeFileOriginale, D.DataInserimento, D.DataUltimaModifica, D.Dimensione FROM dbo.TBL_Territori AS D WHERE TerritorioID = @TerritorioID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@TerritorioID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Territorio = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return Territorio;
        }

        private Territorio RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Territorio territorio = new Territorio();

            territorio.ID = dr.GetGuid(0);
            territorio.Tipologia = TipologiaTerritorioRepository.Instance.RecuperaTipologiaTerritorio(dr.GetInt32(1));
            territorio.GenitoreID = dr.IsDBNull(2) ? null : (Guid?)dr.GetGuid(2);
            territorio.Nome = dr.GetString(3);
            territorio.CodiceIstat = dr.IsDBNull(4) ? "" : dr.GetString(4);
            territorio.Selezionato = dr.GetInt32(5) > 0 ? true : false;

            return territorio;
        }


        public Territorio RecuperaTerritorioByOggettoID(int id)
        {
            Territorio Territorio = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @" SELECT T.TerritorioID, T.GenitoreID, T.TipologiaTerritorioID,
                            T.Nome, T.CodiceIstat
                            FROM dbo.TBL_Territori AS T
                            INNER JOIN dbo.STG_OggettiTerritori AS SOT ON SOT.TerritorioID = T.TerritorioID        
                            INNER JOIN TBL_TipologieTerritorio tp on T.TipologiaTerritorioID = tp.TipologiaTerritorioID
                            WHERE SOT.OggettoID = @OggettoID";


            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Territorio = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return Territorio;
        }
       
    }
}
