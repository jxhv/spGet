# paradasc
The Stored Procedure To Script Model Framework.

If you are using ASP.NET MVC Framework and MS-SQL Server, there is an easiest way to make database web application.
No more Model, No more Entity Framework. Just call your stored procedures from JavaSript.
You can call stored procedure from your JavaScript without any model, controller, and route.

##Ready to Use
There are three simple steps to ready

###1. Define your database connection
Web.config
```html
<connectionStrings>
	<add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Demo.mdf;Initial Catalog=aspnet-WebApplication2-20160213110200;Integrated Security=True"
	  providerName="System.Data.SqlClient" />
</connectionStrings>
```
###2. Copy files

#####1)Place ParaScript folder into your Scripts folder
#####2)Place ParaModel folder into your Models folder
#####3)Place ParadascController.cs into your Controllers folder

###5. Add a MapRoute for Paradasc into your RouteConfig.cs
```C#
 routes.MapRoute(
     name: "Paradasc",
     url: "Paradasc/{action}",
     defaults: new { controller = "Paradasc", action = "" }
 );
```

##Write an Application
```html
<!DOCTYPE html>
<html>
<head>
   <title>Paradasc - StordProcedure to Script Framework</title>
	<style>
		li { display: inline-block; width: 120px; border-bottom: 1px solid #808080; padding: 7px; }
	</style>
	@* Include JQuery library *@
	<script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.0.min.js"></script>
	@* Include the Paradasc script file *@
	<script src="~/Scripts/ParaScript/paradasc.js"></script>
	<script>
		// define Paradasc
		var para = new Paradasc();

		$(function () {
			// delete all records
			para.getSpResultAsInt("mem_del_all");

			// add a record
			para.getSpResultAsInt("mem_add", { name: 'Daniel', phone: '714-100-0000' });

			// list a table
			var data = para.getSpResultAsJson("mem_get_tbl");
			for (var i = 0, ln = data.length; i < ln; i++) {
				$("#board").append("<ul><li>" + data[i].id + "</li><li>" + data[i].name + "</li><li>" + data[i].phone + "</li></ul>");
			}
		});

	</script>
</head>
<body>
	@* Define an AntyForgeryForm for script security *@
	<form id="__ajaxAntiForgeryForm" action="#" method="post">@Html.AntiForgeryToken()</form>
	<h1>Paradasc - Member List</h1>
	<div id="board"></div>
</body>
</html>
```

##Methods
```javascript
getSpResultAsJson(sp_name, param): Get returns from SP as Json table(s) 
getSpResultAsStr(sp_name, param): Get returns from SP as string via OUTPUT parameter
getSpResultAsInt(sp_name, param): Get returns from SP as integer via RETURN
```
param is json type parameter object. The structure of parameter is the same with the structure of stored procedure parameter

You don't need to care about the type of each parameter. JavaScript data types will be automatically converted to the SQL data types.
Now this version of Paradasc supports automatic type conversion as follow.

```javascript
JavasSript Type     <=>     SQL Type
----------------------------------------------------------------
string                      char, varchar, nchar, nvarchar, text, (datetime)
integer                     int, (smallint, tinyint)
decimal                     float, real, numeric, decimal
bool                        bit
```

##License

Paradasc Framework is licensed under MIT http://www.opensource.org/licenses/MIT 

##Credit

Paradasc uses Newtonsoft Json.Net
http://www.newtonsoft.com/json
