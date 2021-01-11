using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Caching;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class OggettoWidgetAttributoRepository : Repository
    {
        private static readonly OggettoWidgetAttributoRepository _instance = new OggettoWidgetAttributoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "OggettiWidgetAttributi";

        private OggettoWidgetAttributoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static OggettoWidgetAttributoRepository Instance
        {
            get { return _instance; }
        }

        public List<OggettoWidgetAttributo> RecuperaOggettiWidgetAttributi()
        {
            List<OggettoWidgetAttributo> oggettiWidgetAttributi = new List<OggettoWidgetAttributo>();

            oggettiWidgetAttributi = this.CacheGet(_webCacheKey) as List<OggettoWidgetAttributo>;

            if (oggettiWidgetAttributi == null)
            {
                oggettiWidgetAttributi = RecuperaOggettiWidgetAttributiPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, oggettiWidgetAttributi, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, oggettiWidgetAttributi, TimeSpan.FromMinutes(15));
            }

            return oggettiWidgetAttributi;
        }

        private List<OggettoWidgetAttributo> RecuperaOggettiWidgetAttributiPrivate()
        {
            List<OggettoWidgetAttributo> oggettiWidgetAttributi = new List<OggettoWidgetAttributo>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT TA.TipoAttributoID, A.AttributoID, O.OggettoID, O.Nome_IT, O.Nome_EN
                            FROM dbo.TBL_TipiAttributi AS TA INNER JOIN
	                            dbo.TBL_Attributi AS A ON A.TipoAttributoID = TA.TipoAttributoID INNER JOIN
	                            dbo.STG_OggettiProcedureAttributi AS OPA ON OPA.AttributoID = A.AttributoID INNER JOIN
	                            dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = OPA.OggettoProceduraID INNER JOIN
	                            dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID
                            WHERE OPA.Widget = 1
                            ORDER BY TA.Ordine, A.Ordine;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoWidgetAttributo oggettoWidgetAttributo = RiempiIstanza(dr);
                oggettiWidgetAttributi.Add(oggettoWidgetAttributo);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiWidgetAttributi;
        }

        private OggettoWidgetAttributo RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoWidgetAttributo oggettoWidgetAttributo = new OggettoWidgetAttributo();

            oggettoWidgetAttributo.TipoAttributo = TipoAttributoRepository.Instance.RecuperaTipoAttributo(dr.GetInt32(0));
            oggettoWidgetAttributo.Attributo = AttributoRepository.Instance.RecuperaAttributo(dr.GetInt32(1));
            oggettoWidgetAttributo.ID = dr.GetInt32(2);
            oggettoWidgetAttributo._nome_IT = dr.GetString(3);
            oggettoWidgetAttributo._nome_EN = dr.GetString(4);

            return oggettoWidgetAttributo;
        }
    }
}
