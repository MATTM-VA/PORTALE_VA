using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class FormatoImmagineRepository : Repository
    {
        private static readonly FormatoImmagineRepository _instance = new FormatoImmagineRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "FormatiImmagine";

        private FormatoImmagineRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static FormatoImmagineRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<FormatoImmagine> RecuperaFormatiImmagine()
        {
            IEnumerable<FormatoImmagine> formatiImmagine = null;

            formatiImmagine = this.CacheGet(_webCacheKey) as List<FormatoImmagine>;

            if (formatiImmagine == null)
            {
                formatiImmagine = RecuperaFormatiImmaginePrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, formatiImmagine, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, formatiImmagine, TimeSpan.FromMinutes(15));
            }

            return formatiImmagine;
        }

        public FormatoImmagine RecuperaFormatoImmagine(int id)
        {
            return RecuperaFormatiImmagine().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<FormatoImmagine> RecuperaFormatiImmaginePrivate()
        {
            List<FormatoImmagine> formatiImmagine = new List<FormatoImmagine>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT FormatoImmagineID, Nome, AltezzaMax, AltezzaMin, LarghezzaMax, LarghezzaMin, Abilitato FROM dbo.TBL_FormatiImmagine;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                FormatoImmagine formatoImmagine = RiempiIstanza(dr);

                formatiImmagine.Add(formatoImmagine);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return formatiImmagine;
        }

        private FormatoImmagine RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            FormatoImmagine formatoImmagine = new FormatoImmagine();

            formatoImmagine.ID = dr.GetInt32(0);
            formatoImmagine.Nome = dr.GetString(1);
            formatoImmagine.AltezzaMax = dr.GetInt32(2);
            formatoImmagine.AltezzaMin = dr.GetInt32(3);
            formatoImmagine.LarghezzaMax = dr.GetInt32(4);
            formatoImmagine.LarghezzaMin = dr.GetInt32(5);
            formatoImmagine.Enum = (FormatoImmagineEnum)dr.GetInt32(0);
            formatoImmagine.Abilitato = dr.GetBoolean(6);

            return formatoImmagine;
        }

    }
}
