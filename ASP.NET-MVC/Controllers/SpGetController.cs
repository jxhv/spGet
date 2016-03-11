/*
 * SpGet v1.0 Stored Procedure to Script Model Framework
 * Sources, Docs, and License: https://github.com/jxhv/paradasc/
 * MIT licensed
 * (c) 2015-2016 Daniel Yu (jxhv@live.com)
*/

using System;
using System.Web.Mvc;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SpGet
{
   public class SpGetController : Controller
   {
      [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      [ValidateInput(false)]
      public JsonResult dbGetResultAsJson(string spName, string jsonParam, string logging = null)
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
               return Json(result[0], JsonRequestBehavior.AllowGet);
            else
               return Json(result, JsonRequestBehavior.AllowGet);

         } catch (Exception ex) {
            Response.AppendToLog(Error.GetMessage(
                this, ex, MethodBase.GetCurrentMethod().Name).Message);
            return Json(null);
         }
      }

      [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      [ValidateInput(false)]
      public ContentResult dbGetResultAsStr(string spName, string jsonParam, string logging = null)
      {
         spName = spName.Substring(0, 1) == "#" ? Security.Decrypt(spName) : spName;

         try {
            Dictionary<string, object> param = get_param(jsonParam);

            if (!String.IsNullOrWhiteSpace(logging))
               logging = logging + String.Format(" | {0} : {1}", spName, jsonParam);

            return Content(DB.GetResultAsStr(spName, param, logging, true));

         } catch (Exception ex) {
            Response.AppendToLog(Error.GetMessage(
                this, ex, MethodBase.GetCurrentMethod().Name).Message);
            return Content(null);
         }
      }

      [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      [ValidateInput(false)]
      public JsonResult dbGetResultAsInt(string spName, string jsonParam, string logging = null)
      {
         spName = spName.Substring(0, 1) == "#" ? Security.Decrypt(spName) : spName;

         try {
            Dictionary<string, object> param = get_param(jsonParam);

            if (!String.IsNullOrWhiteSpace(logging))
               logging = logging + String.Format(" | {0} : {1}", spName, jsonParam);

            return Json(DB.GetResultAsInt(spName, param, logging, true));

         } catch (Exception ex) {
            Response.AppendToLog(Error.GetMessage(
                this, ex, MethodBase.GetCurrentMethod().Name).Message);
            return Json(null);
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