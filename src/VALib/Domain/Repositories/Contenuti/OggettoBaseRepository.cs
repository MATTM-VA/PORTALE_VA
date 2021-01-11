using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.Contenuti.Base;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class OggettoBaseRepository : Repository
    {
        private static readonly OggettoBaseRepository _instance = new OggettoBaseRepository(Settings.VAConnectionString);
       
        private OggettoBaseRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static OggettoBaseRepository Instance
        {
            get { return _instance; }
        }

 

        public OggettoBase RecuperaOggettoBase(int id)
        {
            OggettoBase oggettoBase = new OggettoBase();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();

            sseo.CommandText = @"SELECT O.OggettoID, O.TipoOggettoID, O.Nome_IT, O.Nome_EN, 
                                    O.Descrizione_IT, O.Descrizione_EN 
                                 FROM dbo.TBL_Oggetti AS O 
                                 WHERE O.OggettoID = @OggettoID;";
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                oggettoBase = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettoBase;
        }

        private OggettoBase RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoBase oggetto = new OggettoBase();

            oggetto.ID = dr.GetInt32(0);
            oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(1));
            oggetto._nome_IT = dr.GetString(2);
            oggetto._nome_EN = dr.GetString(3);
            oggetto._descrizione_IT = dr.GetString(4);
            oggetto._descrizione_EN = dr.GetString(5);

            return oggetto;
        }
    }
}
