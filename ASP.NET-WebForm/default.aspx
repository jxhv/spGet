<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ASP.NET_WebForm._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<title>SpGet - StordProcedure to Script Framework</title>
	<style>
		* { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
		ul { margin: 0; padding: 0; }
		li { display: inline-block; width: 120px; border-bottom: 1px solid #808080; padding: 7px; }
	</style>
	<script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.0.min.js"></script>
	<script src="Scripts/spGet/SpGet.js"></script>
	<script src="Scripts/demo.js"></script>
	<script>
		// names of stored procedures
		// if you want to hide stored procedure names...
		var sp_mem_add = "<%=SpGet.Security.Encrypt("mem_add")%>";
		var sp_mem_del = "<%=SpGet.Security.Encrypt("mem_del")%>";
		var sp_mem_set = "<%=SpGet.Security.Encrypt("mem_set")%>";
		var sp_mem_get_tbl = "<%=SpGet.Security.Encrypt("mem_get_tbl")%>";
		var sp_mem_get_phone = "<%=SpGet.Security.Encrypt("mem_get_phone")%>";

		var spGet = new SpGet();
		var demo = new Demo();

		$(function () {
			demo.init();
		});

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<h1>spGet ASP.NET WebForm Example</h1>
		<h1>Member List</h1>
		<div id="board"></div>
		<h2>Edit Data</h2>
		<p>Gianna's new phone number:
			<input id="edPhone" type="text" /></p>
		<h2>Updated Data</h2>
		<p>Gianna's phone number: <span id="phone"></span></p>
	</form>
</body>
</html>
