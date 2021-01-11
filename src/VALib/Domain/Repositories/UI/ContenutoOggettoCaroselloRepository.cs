using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.UI;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Repositories.UI
{
    public sealed class ContenutoOggettoCaroselloRepository : Repository
    {
        private static readonly ContenutoOggettoCaroselloRepository _instance = new ContenutoOggettoCaroselloRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = typeof(OggettoBaseRepository).FullName;

        private ContenutoOggettoCaroselloRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ContenutoOggettoCaroselloRepository Instance
        {
            get { return _instance; }
        }

        public ContenutoOggettoCarosello RecuperaContenutoOggettoCarosello(int id, ContenutoOggettoCaroselloTipo tipo)
        {
            ContenutoOggettoCarosello oggettoBase = new ContenutoOggettoCarosello();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();

            switch (tipo)
            {
                case ContenutoOggettoCaroselloTipo.Oggetto:
                    sseo.CommandText = @"SELECT O.OggettoID, O.Nome_IT, O.Nome_EN, TOG.Nome_IT, TOG.Nome_EN, 0 AS CategoriaNotiziaID 
                                         FROM dbo.TBL_Oggetti AS O INNER JOIN dbo.TBL_TipiOggetto AS TOG ON TOG.TipoOggettoID = O.TipoOggettoID
                                         WHERE O.OggettoID = @ID;";
                    break;
                case ContenutoOggettoCaroselloTipo.Notizia:
                    sseo.CommandText = @"SELECT N.NotiziaID, N.Titolo_IT, N.Titolo_EN, CN.Nome_IT, CN.Nome_EN, CN.CategoriaNotiziaID
                                         FROM dbo.TBL_Notizie AS N INNER JOIN dbo.TBL_CategorieNotiziE AS CN ON CN.CategoriaNotiziaID = N.CategoriaNotiziaID
                                         WHERE N.NotiziaID = @ID AND N.Pubblicata = 1;";
                    break;
                default:
                    break;
            }

            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@ID", id);

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

        private ContenutoOggettoCarosello RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            ContenutoOggettoCarosello oggetto = new ContenutoOggettoCarosello();

            oggetto.ID = dr.GetInt32(0);
            oggetto._nome_IT = dr.GetString(1);
            oggetto._nome_EN = dr.GetString(2);
            oggetto._descrizione_IT = dr.GetString(3);
            oggetto._descrizione_EN = dr.GetString(4);
            oggetto.CategoriaNotizia = (CategoriaNotiziaEnum)dr.GetInt32(5);

            return oggetto;
        }
    }
}
