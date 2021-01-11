using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace VAPortale.Code
{
    public class GetError
    {

     
        /// <summary>Genera la stringa Html da inviare per email</summary>
        ///     ''' <param name="Request">oggetto Request chiamata</param>
        ///     ''' <param name="Session">oggetto Session</param>
        ///     ''' <param name="exs">Lista di eccezioni</param>
        public static string getHtmlError(HttpRequest Request, HttpSessionState Session, Exception ex)
        {
            StringBuilder sb = new StringBuilder(10000);
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<style>");
            sb.Append("TABLE {font-size: xx-small;font-family: verdana, arial, helvetica, sans-serif;cell-spacing: 2px;background-color: #0000CC;}");
            sb.Append("TD {background-color: #FFFFFF;padding: 2px;}");
            sb.Append(".Label {text-align: left;color: black;padding: 5px;background-color: #CCDDFF;}");
            sb.Append(".Title {color: white; font-weight:bold; padding: 5px;text-align: left;background-color: #4444CC;}");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("<body>");

            if (Request!=null)
                DumpGeneralInfo(sb, Request.ServerVariables, "GeneralInfo");

            //foreach (Exception ex in exs)
                DumpException(sb, ex, "Exception");

            if (Request!=null)
                DumpCollection(sb, Request.Form, "Form");
            if (Request!=null)
                DumpCollection(sb, Request.QueryString, "QueryString");
            if (Session != null)
                DumpCollection(sb, Session, "Session");
            if (Request!=null)
                DumpCollection(sb, Request.ServerVariables, "ServerVariables");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        
            /// <summary>Restituisce la tabella  HTML di informazioni sulla chiamata</summary>
            ///     ''' <param name="sb">oggetto su cui accodare la tabella</param>
            ///     ''' <param name="SV">oggetto da cui prelevare i dati</param>
            ///     ''' <param name="cssName">nome della classe da appicare alla tabella</param>
            private static void DumpGeneralInfo(StringBuilder sb, NameValueCollection SV, string cssName = "")
            {
                sb.Append("<TABLE class=\"" + cssName + "\">");
                sb.Append("<TR><TD colspan=\"2\" class=\"Title\">" + cssName + "</TD></TR>");
                sb.Append("<TR><TD class=\"Label\">Target</TD><TD>" + SV.Get("SCRIPT_NAME") + "?" + SV.Get("QUERY_STRING") + "</TD></TR>");
                sb.Append("<TR><TD class=\"Label\">RemoteAddress</TD><TD>" + SV.Get("REMOTE_ADDR") + "</TD></TR>");
                sb.Append("</TABLE>");
            }

            /// <summary>Restituisce la tabella HTML di informazioni sull'eccezione</summary>
            ///     ''' <param name="sb">oggetto su cui accodare la tabella</param>
            ///     ''' <param name="obj">oggetto da cui prelevare i dati</param>
            ///     ''' <param name="cssName">nome della classe da appicare alla tabella</param>
            private static void DumpException(StringBuilder sb, Exception obj, string cssName = "")
            {
                sb.Append("<TABLE class=\"" + cssName + "\">");
                sb.Append("<TR><TD colspan=\"2\" class=\"Title\">" + cssName + "</TD></TR>");
                sb.Append("<TR><TD class=\"Label\">type</TD><TD>" + obj.GetType().ToString() + "</TD></TR>");
                sb.Append("<TR><TD class=\"Label\">message</TD><TD>" + obj.Message + "</TD></TR>");
                sb.Append("<TR><TD class=\"Label\">stacktrace</TD><TD>" + obj.StackTrace + "</TD></TR>");
                if (obj.InnerException != null)
                {
                    sb.Append("<TR><TD class=\"Label\">InnerException</TD><TD>");
                    DumpException(sb, obj.InnerException, "InnerException");
                    sb.Append("</TD></TR>");
                }
                sb.Append("</TABLE>");
            }

            /// <summary>Restituisce la tabella  HTML di informazioni sulla collection</summary>
            ///     ''' <param name="sb">oggetto su cui accodare la tabella</param>
            ///     ''' <param name="obj">oggetto da cui prelevare i dati</param>
            ///     ''' <param name="cssName">nome della classe da appicare alla tabella</param>
            private static void DumpCollection(StringBuilder sb, dynamic obj, string cssName = "")
            {
                if (obj!=null)
                {
                    sb.Append("<TABLE class=\"" + cssName + "\">");
                    sb.Append("<TR><TD colspan=\"2\" class=\"Title\">" + cssName + "</TD></TR>");

                    foreach (string key in obj)
                        sb.Append("<TR><TD class=\"Label\">" + key + "</TD><TD>" + obj[key].ToString() + "</TD></TR>");
                    sb.Append("</TABLE>");
                
                }
            }
       

    }
}