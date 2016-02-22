﻿/*
 * Paradasc v0.7 Stored Procedure to Script Model Framework
 * Sources, Docs, and License: https://github.com/jxhv/paradasc/
 * MIT licensed
 * (c) 2015-2016 Daniel Yu (jxhv@live.com)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Demo
{
   public class RouteConfig
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute(
             name: "Paradasc",
             url: "Paradasc/{action}",
             defaults: new { controller = "Paradasc", action = "" }
         );

         routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}
