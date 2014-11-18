/*
https://tapatalk.com/api.php
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Tapatalk.Handlers;
using Tapatalk.XmlRpc;

using Tapatalk_BasePlugin.Interfaces;

namespace ActiveForumsTapatalk.Implementation
{
    [XmlRpcService(Name = "Tapatalk", Description = "Tapatalk Service For ActiveForums for DNN", UseIntTag = true, AppendTimezoneOffset = true)]
    public partial class ActiveForumsTapatalk : TapatalkAPIHandler
    {
        public override string GetTapatalkApiKey()
        {
            return "";
        }

        public override string GetForumUrl()
        {
           return "";
        }

        public override ITapatalkModuleContext InitizalizeTapatalkModuleContext()
        {
            var moduleContext = ActiveForumsTapatalkModuleContext.Create(HttpContext.Current);
            return moduleContext;

        }
        public override void ExceptionLog(ITapatalkModuleContext context, Exception ex)
        {
            
        }

        
    

        private enum ProcessModes { Normal, TextOnly, Quote }
        
       
        #region Private Helper Methods

        public static string CreateExceptionString(Exception e)
        {
            var sb = new StringBuilder();
            CreateExceptionString(sb, e, string.Empty);

            return sb.ToString();
        }

        private static void CreateExceptionString(StringBuilder sb, Exception e, string indent)
        {
            if (indent == null)
            {
                indent = string.Empty;
            }
            else if (indent.Length > 0)
            {
                sb.AppendFormat("{0}Inner ", indent);
            }

            sb.AppendFormat("Exception Found:\n{0}Type: {1}", indent, e.GetType().FullName);
            sb.AppendFormat("\n{0}Message: {1}", indent, e.Message);
            sb.AppendFormat("\n{0}Source: {1}", indent, e.Source);
            sb.AppendFormat("\n{0}Stacktrace: {1}", indent, e.StackTrace);

            if (e.InnerException != null)
            {
                sb.Append("\n");
                CreateExceptionString(sb, e.InnerException, indent + "  ");
            }
        }


   

        private static string GetFileExt(string filename)
        {
             var dot = filename.LastIndexOf(".");
                if (dot == -1) return "";
                var extension = filename.Substring(dot + 1);

             return extension;
        }

   
     
      

        #endregion
     
    }
}