using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using VALib.Domain.Entities.DatiAmbientali;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class ElencoRepository : Repository
    {
        private static readonly ElencoRepository _instance = new ElencoRepository(Settings.DivaWebConnectionString);
        private static readonly string _webCacheKey = "Elenchi";

        private ElencoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ElencoRepository Instance
        {
            get { return _instance; }
        }

        public List<Elenco> RecuperaElenchi()
        {
            List<Elenco> elenchi = new List<Elenco>();

            elenchi = this.CacheGet(_webCacheKey) as List<Elenco>;

            if (elenchi == null)
            {
                elenchi = RecuperaElenchiPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, elenchi, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));
                this.CacheInsert(_webCacheKey, elenchi, TimeSpan.FromMinutes(15));
            }

            return elenchi;
        }

        public List<Elenco> RecuperaElenchiDatiEStrumenti(string elencoElenchi)
        {
            List<Elenco> elenchi = new List<Elenco>();

            switch (elencoElenchi.ToLower())
            {
                case "normativa":
                    elenchi = RecuperaElenchiNormativa();
                    break;
                case "studieindaginidisettore":
                    elenchi = RecuperaElenchiStudiEIndaginiDiSettore();
                    break;
                case "datiambientali":
                     
                    elenchi = null;
                    break;
                default:
                    break;
            }

            return elenchi;
        }

        private List<Elenco> RecuperaElenchiNormativa()
        {
            List<Elenco> elenchi = new List<Elenco>();

            elenchi.Add(RecuperaElenco(1));
            elenchi.Add(RecuperaElenco(2));
            elenchi.Add(RecuperaElenco(3));

            elenchi.Add(RecuperaElenco(7));
            elenchi.Add(RecuperaElenco(8));

            return elenchi;
        }

        private List<Elenco> RecuperaElenchiStudiEIndaginiDiSettore()
        {
            List<Elenco> elenchi = new List<Elenco>();

            elenchi.Add(RecuperaElenco(4));
            elenchi.Add(RecuperaElenco(5));
            elenchi.Add(RecuperaElenco(9));
            elenchi.Add(RecuperaElenco(10));

            return elenchi;
        }

        private List<Elenco> RecuperaElenchiDatiAmbientali()
        {
            List<Elenco> elenchi = new List<Elenco>();

            elenchi.Add(RecuperaElenco(6));
            elenchi.Add(RecuperaElenco(7));

            return elenchi;
        }


        public Elenco RecuperaElenco(int id)
        {
            return RecuperaElenchi().FirstOrDefault(x => x.ID == id);
        }

        public Elenco RecuperaElenco(string nome)
        {
            return RecuperaElenchi().FirstOrDefault(x => x._nome_IT.Equals(nome, StringComparison.CurrentCultureIgnoreCase));         
        }

        public string RecuperaElencoStudiIndagini(string nome)
        {
            List<string> lista = new List<string>();

            foreach (Elenco elenco in RecuperaElenchi().Where(x => nome.Contains(x._nome_IT)))
            {
                lista.Add(elenco.ID.ToString());
            }
            return string.Join(",", lista);
        }

        private List<Elenco> RecuperaElenchiPrivate()
        {
            List<Elenco> elenchi = new List<Elenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT IDElenco, Elenco, Nome_EN FROM dbo.TBLElenchi;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Elenco elenco = RiempiIstanza(dr);

                elenchi.Add(elenco);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            //elenchi.Add(new Elenco() { ID = 11, _nome_IT = "Ricerca nel catalogo", _nome_EN = "Search the catalog" });

            return elenchi;
        }

        private Elenco RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Elenco elenco = new Elenco();

            elenco.ID = dr.GetInt32(0);
            elenco._nome_IT = dr.IsDBNull(1) ? "" : dr.GetString(1).Trim();
            elenco._nome_EN = dr.IsDBNull(2) ? "" : dr.GetString(2).Trim();

            return elenco;
        }

    }
}
