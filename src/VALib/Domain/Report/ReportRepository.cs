using ElogToolkit.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using VALib.Configuration;
using VALib.Domain.Common;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;

namespace VALib.Domain.Report
{
    public sealed class ReportRepository : Repository
    {
        private static readonly object _lockReportTipologia = new object();
        private static readonly object _lockReportProvvedimento = new object();
        private static readonly object _lockReportEsitoVIA = new object();
        private static readonly object _lockReportEsitoVIALO = new object();
        private static readonly string _connectionString = Settings.VAConnectionString;


        private static readonly ReportRepository _instance = new ReportRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = typeof(OggettoElencoRepository).FullName;

        private ReportRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ReportRepository Instance
        {
            get { return _instance; }
        }

        //private static readonly string _webCacheKey = "CategorieNotizie";

        private static ReportTipologiaItem RetrieveReportTipologiaFromReader(SqlDataReader reader)
        {
            ReportTipologiaItem retrievedReportTipologia = new ReportTipologiaItem();
            retrievedReportTipologia.Tipologia = TipologiaRepository.Instance.RecuperaTipologia(reader.GetInt32(1));
            retrievedReportTipologia.TotaliTipi.Add(1, reader.GetInt32(2));
            retrievedReportTipologia.TotaliTipi.Add(2,reader.GetInt32(3));
            retrievedReportTipologia.TotaliTipi.Add(9, reader.GetInt32(4));
            return retrievedReportTipologia;
        }

        private static ReportTipoProvvedimentoItem RetrieveReportTipoProvvedimentoFromReader(SqlDataReader reader)
        {
            ReportTipoProvvedimentoItem retrievedReportTipoProvvedimento = null;
            retrievedReportTipoProvvedimento = new ReportTipoProvvedimentoItem();
            retrievedReportTipoProvvedimento.Anno = reader.GetInt32(0);

            //retrievedReportTipoProvvedimento.TipoProvvedimento.Add(1, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(1));
            //retrievedReportTipoProvvedimento.TipoProvvedimento.Add(2, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(2));
            //retrievedReportTipoProvvedimento.TipoProvvedimento.Add(9, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(9));
            //retrievedReportTipoProvvedimento.Totale = reader.GetInt32(2);

            retrievedReportTipoProvvedimento.TotaliTipi.Add(1, reader.GetInt32(1));
            retrievedReportTipoProvvedimento.TotaliTipi.Add(2, reader.GetInt32(2));
            retrievedReportTipoProvvedimento.TotaliTipi.Add(9, reader.GetInt32(3));
            return retrievedReportTipoProvvedimento;
        }

        private static ReportEsitoItem RetrieveReportEsitoFromReader(SqlDataReader reader)
        {
            ReportEsitoItem reItem = new ReportEsitoItem();
            reItem.Esito = reader.GetString(0);
            reItem.Percentuale = reader.GetInt32(1);
            return reItem;
        }

        public ReportTipologia RecuperaReportTipologia()
        {
            ReportTipologia reportTipologia = null;
            List<ReportTipologiaItem> reportTipologieItems = null;
            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "reportTipologia";

            reportTipologia = cache[cacheKey] as ReportTipologia;

            if (reportTipologia == null)
            {
                lock (_lockReportTipologia)
                {
                    reportTipologia = cache[cacheKey] as ReportTipologia;
                    if (reportTipologia == null)
                    {
                        reportTipologia = new ReportTipologia();

                        //SqlConnection connection = null;
                        //SqlCommand command = null;
                        //SqlDataReader reader = null;

                        SqlServerExecuteObject sseo = null;
                        SqlDataReader reader = null;
                        //rows = 0;

                        sseo = new SqlServerExecuteObject();
                        sseo.CommandText = "dbo.SP_RecuperaReportGrafici";
                        sseo.CommandType = CommandType.StoredProcedure;
                        sseo.SqlParameters.AddWithValue("@TipoReport",1);
                         reader = SqlProvider.ExecuteReaderObject(sseo);
                        reportTipologieItems = new List<ReportTipologiaItem>();
                         while (reader.Read())
                        {
                            ReportTipologiaItem retrievedReportTipologiaItem = RetrieveReportTipologiaFromReader(reader);
                            reportTipologia.RtpItem.Add(retrievedReportTipologiaItem);
                        }
                        if (reader != null)
                        {
                         reader.Close();
                         reader.Dispose();
                        }
                        reportTipologia.TipoProvvedimento.Add(1, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(1));
                        reportTipologia.TipoProvvedimento.Add(2, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(2));
                        reportTipologia.TipoProvvedimento.Add(9, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(9));

                        foreach (ReportTipologiaItem rtp in reportTipologia.RtpItem)
                        {
                            if (rtp.Tipologia.Macrotipologia.ID == 1)
                            {
                                reportTipologia.ReportMT.MT1Via = reportTipologia.ReportMT.MT1Via + rtp.TotaliTipi[1] + rtp.TotaliTipi[1];
                                reportTipologia.ReportMT.MT1ViaLO = reportTipologia.ReportMT.MT1ViaLO + rtp.TotaliTipi[9];
                            }
                            else if (rtp.Tipologia.Macrotipologia.ID == 2)
                            {
                                reportTipologia.ReportMT.MT2Via = reportTipologia.ReportMT.MT2Via + rtp.TotaliTipi[2] + rtp.TotaliTipi[2];
                                reportTipologia.ReportMT.MT2ViaLO = reportTipologia.ReportMT.MT2ViaLO + rtp.TotaliTipi[9];
                            }
                        }

                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                        cache.Add(cacheKey, reportTipologia, policy);
                    }
                }

            }

             

            return reportTipologia;
        }



        public ReportTipoProvvedimento RecuperaReportTipoProvvedimento()
        {
            ReportTipoProvvedimento reportTipoProvvedimento = null ;
            List<ReportTipoProvvedimentoItem> reportTipoProvvedimentoItem = null;

            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "reportTipoProvvedimento";

            reportTipoProvvedimento = cache[cacheKey] as ReportTipoProvvedimento;

            if (reportTipoProvvedimento == null)
            {
                lock (_lockReportProvvedimento)
                {
                    reportTipoProvvedimento = cache[cacheKey] as ReportTipoProvvedimento;
                    if (reportTipoProvvedimento == null)
                    {
                        reportTipoProvvedimento = new ReportTipoProvvedimento();
                        SqlServerExecuteObject sseo = null;
                        SqlDataReader reader = null;
                        //rows = 0;

                        sseo = new SqlServerExecuteObject();
                        sseo.CommandText = "dbo.SP_RecuperaReportGrafici";
                        sseo.CommandType = CommandType.StoredProcedure;
                        sseo.SqlParameters.AddWithValue("@TipoReport", 2);
                        reader = SqlProvider.ExecuteReaderObject(sseo);
                        reportTipoProvvedimentoItem = new List<ReportTipoProvvedimentoItem>();
                        while (reader.Read())
                        {
                            ReportTipoProvvedimentoItem retrievedReportTipoProvvedimentoItem = RetrieveReportTipoProvvedimentoFromReader(reader);
                            reportTipoProvvedimento.RtpItem.Add(retrievedReportTipoProvvedimentoItem);
                        }
                        if (reader != null)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                        reportTipoProvvedimento.TipoProvvedimento.Add(1, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(1));
                        reportTipoProvvedimento.TipoProvvedimento.Add(2, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(2));
                        reportTipoProvvedimento.TipoProvvedimento.Add(9, TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(9));

                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                        cache.Add(cacheKey, reportTipoProvvedimento, policy);
                    }
                }
            }

            return reportTipoProvvedimento;
        }



        public ReportEsito RecuperaReportEsitoVIA()
        {
            ReportEsito reportEsito = null;
            List<ReportEsitoItem> reportEsitoItems = null;

            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "reportEsitoVIA";
            reportEsito = cache[cacheKey] as ReportEsito;

            if (reportEsito == null)
            {
                lock (_lockReportEsitoVIA)
                {
                    reportEsito = cache[cacheKey] as ReportEsito;
                    if (reportEsito == null)
                    {
                        reportEsito = new ReportEsito();

                        SqlServerExecuteObject sseo = null;
                        SqlDataReader reader = null;
                        //rows = 0;

                        sseo = new SqlServerExecuteObject();
                        sseo.CommandText = "dbo.SP_RecuperaReportGrafici";
                        sseo.CommandType = CommandType.StoredProcedure;
                        sseo.SqlParameters.AddWithValue("@TipoReport", 3);
                        reader = SqlProvider.ExecuteReaderObject(sseo);
                        reportEsitoItems = new List<ReportEsitoItem>();
                        while (reader.Read())
                        {
                            ReportEsitoItem retrievedReportEsito = RetrieveReportEsitoFromReader(reader);
                            reportEsito.ListaEsiti.Add(retrievedReportEsito);
                        }
                        if (reader != null)
                        {
                            reader.Close();
                            reader.Dispose();
                        }

                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                        cache.Add(cacheKey, reportEsito, policy);
                    }
                }
            }
            return reportEsito;
        }

        public ReportEsito RecuperaReportEsitoVIALO()
        {
            ReportEsito reportEsito = null;
            List<ReportEsitoItem> reportEsitoItems = null;

            MemoryCache cache = MemoryCache.Default;
            String cacheKey = "reportEsitoVIALO";
            reportEsito = cache[cacheKey] as ReportEsito;

            if (reportEsito == null)
            {
                lock (_lockReportEsitoVIALO)
                {
                    reportEsito = cache[cacheKey] as ReportEsito;
                    if (reportEsito == null)
                    {
                        reportEsito = new ReportEsito();

                        SqlServerExecuteObject sseo = null;
                        SqlDataReader reader = null;
                        //rows = 0;

                        sseo = new SqlServerExecuteObject();
                        sseo.CommandText = "dbo.SP_RecuperaReportGrafici";
                        sseo.CommandType = CommandType.StoredProcedure;
                        sseo.SqlParameters.AddWithValue("@TipoReport", 4);
                        reader = SqlProvider.ExecuteReaderObject(sseo);
                        reportEsitoItems = new List<ReportEsitoItem>();
                        while (reader.Read())
                        {
                            ReportEsitoItem retrievedReportEsito = RetrieveReportEsitoFromReader(reader);
                            reportEsito.ListaEsiti.Add(retrievedReportEsito);
                        }
                        if (reader != null)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                        cache.Add(cacheKey, reportEsito, policy);
                    }
                }
            }
            return reportEsito;
        }




    }
}
