/*
 * SpGet v1.0 Stored Procedure to Script Model Framework
 * Sources, Docs, and License: https://github.com/jxhv/spGet/
 * MIT licensed
 * (c) 2015-2016 Daniel Yu (jxhv@live.com)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
using SpGet;
using System.Reflection;

namespace ASP.NET_WebForm.Services
{
   [WebService(Namespace = "http://tempuri.org/")]
   [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
   [System.ComponentModel.ToolboxItem(false)]
   [System.Web.Script.Services.ScriptService]
   public class spGet : System.Web.Services.WebService
   {
      [WebMethod(true)]
      [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
      public object dbGetResultAsJson(string spName, string jsonParam, string logging = null)
      {
         spName = spName.Substring(0, 1) == "#" ? Security.Decrypt(spName) : spName;

         List<object> result = new List<object>();

         try {
            Dictionary<string, object> param = get_param(jsonParam);

            if (!String.IsNullOrWhiteSpace(logging))
               logging = logging + String.Format(" | {0} : {1}", spName, jsonParam);

            using (SqlDataReader dr = DB.GetTable(spName, param, logging, true)) {

               while (dr.HasRows) {
                  result.Add(DB.GetJsonSerialize(dr));
                  dr.NextResult();
               }
            }

            if (result.Count == 1)
               return result[0];
            else
               return result;

         } catch (Exception ex) {
            HttpContext.Current.Response.AppendToLog(Error.GetMessage(
                   this, ex, MethodBase.GetCurrentMethod().Name).Message);
            return null;
         }
      }

      [WebMethod]
      [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
      public string dbGetResultAsStr(string spName, string jsonParam, string logging = null)
      {
         spName = spName.Substring(0, 1) == "#" ? Security.Decrypt(spName) : spName;

         try {
            Dictionary<string, object> param = get_param(jsonParam);

            if (!String.IsNullOrWhiteSpace(logging))
               logging = logging + String.Format(" | {0} : {1}", spName, jsonParam);

            string result = DB.GetResultAsStr(spName, param, logging, true);

            return result;

         } catch (Exception ex) {
            HttpContext.Current.Response.AppendToLog(Error.GetMessage(
                this, ex, MethodBase.GetCurrentMethod().Name).Message);
            return null;
         }
      }

      [WebMethod]
      [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
      public int? dbGetResultAsInt(string spName, string jsonParam, string logging = null)
      {
         spName = spName.Substring(0, 1) == "#" ? Security.Decrypt(spName) : spName;

         try {
            Dictionary<string, object> param = get_param(jsonParam);

            if (!String.IsNullOrWhiteSpace(logging))
               logging = logging + String.Format(" | {0} : {1}", spName, jsonParam);

            return DB.GetResultAsInt(spName, param, logging, true);

         } catch (Exception ex) {
            HttpContext.Current.Response.AppendToLog(Error.GetMessage(
                this, ex, MethodBase.GetCurrentMethod().Name).Message);
            return null;
         }
      }

      private Dictionary<string, object> get_param(string jsonParam)
      {
         if (String.IsNullOrWhiteSpace(jsonParam))
            return null;
         else
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonParam);
      }
   }

   public class Error
   {
      public static Exception GetMessage(Object self, Exception ex, String method)
      {
         return new Exception(string.Format("Problems at {0}/{1} :: {2}", self.GetType(), method, ex.Message));
      }
   }
}
