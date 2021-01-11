using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Management;
using VALib.Configuration;

namespace VALib.Web
{
    public sealed class SqlWebEventProvider : BufferedWebEventProvider
    {
        private string _providerName = null;
        private string _connectionString = null;
        private string _connectionStringWebEvents = null;
        private DataTable _dataTable = null;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            _providerName = name;

            InitConnectionString();
            InitDataTable();
        }

        public override void ProcessEvent(WebBaseEvent eventRaised)
        {
            if (eventRaised != null)
            {
                if (UseBuffering)
                    base.ProcessEvent(eventRaised);
                else
                {

                    if (((System.Web.Management.WebBaseErrorEvent)eventRaised).ErrorException != null) {
                        
                        if (Convert.ToBoolean(WebConfigurationManager.AppSettings["WriteErrorToDb"])) {
                            InsertEvent(eventRaised);
                            SaveEvents();
                        }
                    }                    
                    else
                    {
                        InsertEvent(eventRaised);
                        SaveEvents();
                    }                    
                }
            }

        }

        public override void ProcessEventFlush(WebEventBufferFlushInfo flushInfo)
        {
            foreach (WebBaseEvent eventToFlush in flushInfo.Events)
            {
                InsertEvent(eventToFlush);
            }

            SaveEvents();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        private void InitConnectionString()
        {
            _connectionString = Settings.VAConnectionString;
            _connectionStringWebEvents = Settings.VAConnectionStringWebEvents;
        }

        private void InitDataTable()
        {
            _dataTable = new DataTable("T_Evento");
            _dataTable.Columns.Add("EventID", Type.GetType("System.Guid"));
            _dataTable.Columns.Add("EventTimeUtc", Type.GetType("System.DateTime"));
            _dataTable.Columns.Add("EventTime", Type.GetType("System.DateTime"));
            _dataTable.Columns.Add("EventType", Type.GetType("System.String"));
            _dataTable.Columns.Add("EventSequence", Type.GetType("System.Int64"));
            _dataTable.Columns.Add("EventOccurrence", Type.GetType("System.Int64"));
            _dataTable.Columns.Add("EventCode", Type.GetType("System.Int32"));
            _dataTable.Columns.Add("EventDetailCode", Type.GetType("System.Int32"));
            _dataTable.Columns.Add("EventMessage", Type.GetType("System.String"));
            _dataTable.Columns.Add("MachineName", Type.GetType("System.String"));
            _dataTable.Columns.Add("ApplicationPath", Type.GetType("System.String"));
            _dataTable.Columns.Add("ApplicationVirtualPath", Type.GetType("System.String"));
            _dataTable.Columns.Add("RequestUrl", Type.GetType("System.String"));
            _dataTable.Columns.Add("RequestUserAgent", Type.GetType("System.String"));
            _dataTable.Columns.Add("RequestUrlReferrer", Type.GetType("System.String"));
            _dataTable.Columns.Add("PrincipalIdentityIsAuthenticated", Type.GetType("System.Boolean"));
            _dataTable.Columns.Add("PrincipalIdentityName", Type.GetType("System.String"));
            _dataTable.Columns.Add("UserHostAddress", Type.GetType("System.String"));
            _dataTable.Columns.Add("ExceptionType", Type.GetType("System.String"));
            _dataTable.Columns.Add("ExceptionMessage", Type.GetType("System.String"));
            _dataTable.Columns.Add("UtenteID", Type.GetType("System.Int32"));
            _dataTable.Columns.Add("UtenteNomeUtente", Type.GetType("System.String"));
            _dataTable.Columns.Add("IntEntityID", Type.GetType("System.Int32"));
            _dataTable.Columns.Add("GuidEntityID", Type.GetType("System.Guid"));
            _dataTable.Columns.Add("WebEventTypeID", Type.GetType("System.Int32"));
            _dataTable.Columns.Add("Details", Type.GetType("System.String"));
        }

        private void InsertEvent(WebBaseEvent eventToInsert)
        {
            WebRequestInformation requestInfo = null;
            Exception exception = null;
            VAWebRequestInformation vaRequestInfo = null;
            DataRow row = _dataTable.NewRow();
            row["EventID"] = eventToInsert.EventID;
            row["EventTimeUtc"] = eventToInsert.EventTimeUtc;
            row["EventTime"] = eventToInsert.EventTime;
            row["EventType"] = eventToInsert.GetType().ToString();
            row["EventSequence"] = eventToInsert.EventSequence;
            row["EventOccurrence"] = eventToInsert.EventOccurrence;
            row["EventCode"] = eventToInsert.EventCode;
            row["EventDetailCode"] = eventToInsert.EventDetailCode;
            row["EventMessage"] = eventToInsert.Message;
            row["MachineName"] = WebBaseEvent.ApplicationInformation.MachineName;
            row["ApplicationPath"] = WebBaseEvent.ApplicationInformation.ApplicationPath;
            row["ApplicationVirtualPath"] = WebBaseEvent.ApplicationInformation.ApplicationVirtualPath;

            if (eventToInsert is WebRequestEvent)
            {
                requestInfo = ((WebRequestEvent)eventToInsert).RequestInformation;
            }
            else if (eventToInsert is WebRequestErrorEvent)
            {
                requestInfo = ((WebRequestErrorEvent)eventToInsert).RequestInformation;
            }
            else if (eventToInsert is WebErrorEvent)
            {
                requestInfo = ((WebErrorEvent)eventToInsert).RequestInformation;
            }
            else if (eventToInsert is WebAuditEvent)
            {
                requestInfo = ((WebAuditEvent)eventToInsert).RequestInformation;
            }

            if (eventToInsert is WebBaseErrorEvent)
            {
                exception = ((WebBaseErrorEvent)eventToInsert).ErrorException;
            }
 
            if (eventToInsert is VAWebRequestErrorEvent)
            {
                vaRequestInfo = ((VAWebRequestErrorEvent)eventToInsert).VARequestInformation;
            }
            else if (eventToInsert is VAWebRequestDocumentoDownloadEvent)
            {
                vaRequestInfo = ((VAWebRequestDocumentoDownloadEvent)eventToInsert).VARequestInformation;
            }

            if (requestInfo != null)
            {
                row["RequestUrl"] = requestInfo.RequestUrl;
                row["UserHostAddress"] = requestInfo.UserHostAddress;
                row["PrincipalIdentityIsAuthenticated"] = requestInfo.Principal.Identity.IsAuthenticated;

                if (requestInfo.Principal.Identity.IsAuthenticated)
                    row["PrincipalIdentityName"] = requestInfo.Principal.Identity.Name;
            }

            if (exception != null)
            {
                row["ExceptionType"] = exception.GetType().ToString();
                row["ExceptionMessage"] = exception.Message;
                row["Details"] = eventToInsert.ToString(true, true);
                row["WebEventTypeID"] = VAWebEventTypeEnum.Errore;
                
            }

            if (vaRequestInfo != null)
            {
                if (vaRequestInfo.UtenteID != null)
                {
                    row["UtenteID"] = vaRequestInfo.UtenteID;
                    row["UtenteNomeUtente"] = vaRequestInfo.NomeUtente;
                }

                if (vaRequestInfo.UrlReferrer != null)
                    row["RequestUrlReferrer"] = vaRequestInfo.UrlReferrer;

                if (vaRequestInfo.UserAgent != null)
                    row["RequestUserAgent"] = vaRequestInfo.UserAgent;

                if (vaRequestInfo.IntEntityID != null)
                    row["IntEntityID"] = vaRequestInfo.IntEntityID;

                if (vaRequestInfo.GuidEntityID != null)
                    row["GuidEntityID"] = vaRequestInfo.GuidEntityID;

                if (vaRequestInfo.EventTypeID != null)
                    row["WebEventTypeID"] = vaRequestInfo.EventTypeID;

            }

            _dataTable.Rows.Add(row);
        }

        private void SaveEvents()
        {
            if (_dataTable.Rows.Count > 0)
            {
                SqlBulkCopy sbc = new SqlBulkCopy(_connectionStringWebEvents, SqlBulkCopyOptions.TableLock);
                sbc.DestinationTableName = "dbo.TBL_WebEvents";
                sbc.ColumnMappings.Add("EventID", "EventID");
                sbc.ColumnMappings.Add("EventTimeUtc", "EventTimeUtc");
                sbc.ColumnMappings.Add("EventTime", "EventTime");
                sbc.ColumnMappings.Add("EventType", "EventType");
                sbc.ColumnMappings.Add("EventSequence", "EventSequence");
                sbc.ColumnMappings.Add("EventOccurrence", "EventOccurrence");
                sbc.ColumnMappings.Add("EventCode", "EventCode");
                sbc.ColumnMappings.Add("EventDetailCode", "EventDetailCode");
                sbc.ColumnMappings.Add("EventMessage", "EventMessage");
                sbc.ColumnMappings.Add("MachineName", "MachineName");
                sbc.ColumnMappings.Add("ApplicationPath", "ApplicationPath");
                sbc.ColumnMappings.Add("ApplicationVirtualPath", "ApplicationVirtualPath");
                sbc.ColumnMappings.Add("RequestUrl", "RequestUrl");
                sbc.ColumnMappings.Add("RequestUserAgent", "RequestUserAgent");
                sbc.ColumnMappings.Add("RequestUrlReferrer", "RequestUrlReferrer");
                sbc.ColumnMappings.Add("UserHostAddress", "UserHostAddress");
                sbc.ColumnMappings.Add("PrincipalIdentityIsAuthenticated", "PrincipalIdentityIsAuthenticated");
                sbc.ColumnMappings.Add("PrincipalIdentityName", "PrincipalIdentityName");
                sbc.ColumnMappings.Add("ExceptionType", "ExceptionType");
                sbc.ColumnMappings.Add("ExceptionMessage", "ExceptionMessage");
                sbc.ColumnMappings.Add("UtenteID", "UtenteID");
                sbc.ColumnMappings.Add("UtenteNomeUtente", "UtenteNomeUtente");
                sbc.ColumnMappings.Add("IntEntityID", "IntEntityID");
                sbc.ColumnMappings.Add("GuidEntityID", "GuidEntityID");
                sbc.ColumnMappings.Add("WebEventTypeID", "WebEventTypeID");
                sbc.ColumnMappings.Add("Details", "Details");
                sbc.WriteToServer(_dataTable);
                sbc.Close();

                _dataTable.Rows.Clear();
            }
        }
    }
}
