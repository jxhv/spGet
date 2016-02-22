using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace Paradasc
{
   using System;
   using System.Configuration;
   using System.Reflection;

   public class DB
   {
      private const string DB_CONN = "DefaultConnection";

      public static SqlDataReader GetTable(string pStoredProcedure, dynamic pParams = null,
          string pLogging = null, bool pb_UseDictionaryParam = false)
      {
         var localConn = get_DBConnection();

         var cmd = new SqlCommand(pStoredProcedure, localConn) {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 7200
         };

         set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

         if (localConn.State == ConnectionState.Closed) {
            localConn.Open();
         }
         var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

         return dr;
      }

      public static int GetResultAsInt(string pStoredProcedure, dynamic pParams = null,
          string pLogging = null, bool pb_UseDictionaryParam = false)
      {
         var localConn = get_DBConnection();

         var cmd = new SqlCommand(pStoredProcedure, localConn) {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 7200
         };

         cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

         set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

         if (localConn.State == ConnectionState.Closed) {
            localConn.Open();
         }
         cmd.ExecuteNonQuery();
         localConn.Close();

         return Convert.ToInt32(cmd.Parameters["RETURN_VALUE"].Value);
      }

      public static string GetResultAsStr(string pStoredProcedure, dynamic pParams = null,
          string pLogging = null, bool pb_UseDictionaryParam = false)
      {
         var localConn = get_DBConnection();

         const string OutName = "@OUTPUT";

         var cmd = new SqlCommand(pStoredProcedure, localConn) {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 7200
         };

         set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

         var OutVal = new SqlParameter(OutName, SqlDbType.NVarChar, 2147483647) {
            Direction = ParameterDirection.Output
         };

         cmd.Parameters.Add(OutVal);

         if (localConn.State == ConnectionState.Closed) {
            localConn.Open();
         }
         cmd.ExecuteNonQuery();
         localConn.Close();

         return OutVal.Value == null ? string.Empty : ((string)OutVal.Value).Trim();
      }

      public static IEnumerable<Dictionary<string, object>> GetJsonSerialize(SqlDataReader reader)
      {
         var results = new List<Dictionary<string, object>>();
         var cols = new List<string>();
         for (var i = 0; i < reader.FieldCount; i++)
            cols.Add(reader.GetName(i));

         while (reader.Read())
            results.Add(get_SerializeRow(cols, reader));

         return results;
      }
      private static Dictionary<string, object> get_SerializeRow(IEnumerable<string> cols,
                                                      SqlDataReader reader)
      {
         var result = new Dictionary<string, object>();
         foreach (var col in cols)
            result.Add(col, reader[col]);
         return result;
      }

      private static void set_SqlParam(SqlCommand cmd, dynamic pParams, string logging = null,
          bool pb_UseDictionaryParam = false)
      {
         if (pParams == null) {
            return;
         }
         if (!pb_UseDictionaryParam) {
            var paramArray = pParams.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo param in paramArray) {
               var fieldValue = param.GetValue(pParams, null);

               if (fieldValue != null) {
                  Type type = fieldValue.GetType();

                  if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                     cmd.Parameters.AddWithValue("@" + param.Name, convert_ToDataTable(fieldValue));
                  else
                     cmd.Parameters.AddWithValue("@" + param.Name, fieldValue);
               }
               else {
                  // if this param is an userdefined table, null will be passed as default (empty table)
                  cmd.Parameters.AddWithValue("@" + param.Name, null);
               }
            }
         }
         else {
            foreach (KeyValuePair<string, object> param in (IDictionary<string, object>)pParams) {
               cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
            }
         }

         if (!String.IsNullOrWhiteSpace(logging))
            cmd.Parameters.AddWithValue("@logging", logging);
      }

      private static SqlConnection get_DBConnection(SqlConnection userConnection = null)
      {
         if (userConnection == null) {
            userConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[DB_CONN].ConnectionString);
         }
         return (SqlConnection)((ICloneable)userConnection).Clone();
      }

      private static DataTable convert_ToDataTable<T>(List<T> items)
      {
         DataTable result = new DataTable(typeof(T).Name);

         PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
         foreach (PropertyInfo prop in Props) {
            result.Columns.Add(prop.Name, prop.PropertyType);
         }
         foreach (T item in items) {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++) {
               values[i] = Props[i].GetValue(item, null);
            }
            result.Rows.Add(values);
         }

         return result;
      }
   }
}
