/*
 * Paradasc v0.7 Stored Procedure to Script Model Framework
 * Sources, Docs, and License: https://github.com/jxhv/paradasc/
 * MIT licensed
 * (c) 2015-2016 Daniel Yu (jxhv@live.com)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Paradasc
{
   public class Security
   {
      // You need to change this key for your security
      private const string DES_KEY = "SOI98d828OLlkmsdoiasj32l4k089lkkl09ll";

      public static string Encrypt(string value)
      {
         try {
            var des = get_DESAlgorithm();
            var ct = des.CreateEncryptor();
            var input = Encoding.Unicode.GetBytes(value);
            var result = ct.TransformFinalBlock(input, 0, input.Length);

            return "#" + Convert.ToBase64String(result);
         } catch {
            return string.Empty;
         }
      }

      public static string Decrypt(string value)
      {
         try {
            value = value.Substring(1);
            var source = Convert.FromBase64String(value);
            var des = get_DESAlgorithm();
            var ct = des.CreateDecryptor();
            var result = ct.TransformFinalBlock(source, 0, source.Length);

            return Encoding.Unicode.GetString(result);
         } catch {
            return string.Empty;
         }
      }

      private static TripleDES get_DESAlgorithm()
      {
         MD5 md5 = new MD5CryptoServiceProvider();

         TripleDES des = new TripleDESCryptoServiceProvider();
         des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(DES_KEY));
         des.IV = new byte[des.BlockSize / 8];

         return des;
      }
   }
}
