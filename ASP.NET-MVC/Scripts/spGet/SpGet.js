/*
 * SpGet v1.0 Stored Procedure to Script Model Framework
 * Sources, Docs, and License: https://github.com/jxhv/spGet/
 * MIT licensed
 * (c) 2015-2016 Daniel Yu (jxhv@live.com)
 */

/* ========================================================================== */
/* === Extensions =========================================================== */
/* ========================================================================== */

String.prototype.format = function() {
	var a = this;
	for ( var b in arguments) {
		a = a.replace(/%[a-z]/, arguments[b]);
	}
	return a; // Make chainable
};

/* ========================================================================== */
/* === Helper Object ======================================================== */
/* ========================================================================== */

function SpGet() {
	this.isEmpty = function(val) {
		return (typeof (val) == "undefined") || (val == null);
	};

	this.isEmptyStr = function(val) {
		return (typeof (val) == "undefined") || (val == null) || (val == "");
	};

	this.isSafeInt = function(val) { 
		return (typeof (val) != "undefined") && (val != null) && (!isNaN(parseInt(val, 10)));
	};
	
	this.getSafeInt = function(val, def) {
		def = this.isSafeInt(def) ? def : 0;
		return this.isSafeInt(val) ? parseInt(val, 10) : def;
	};

	this.getSafeStr = function(val, def) {
		def = this.isEmptyStr(def) ? "" : def
		return this.isEmptyStr(val) ? def : val;
	};

	this.getSafeBoolean = function(val, def) {
		def = this.isEmpty(def) ? false : def
		return this.isEmpty(val) ? def : val;
	};

	this.ajaxCallCore = function(returnType, method, param, requestMethod, debugError) {
		var result = null;
		requestMethod = this.getSafeStr(requestMethod, "POST"),
		debugError = this.getSafeBoolean(debugError, true);

		$.ajax({ url : method,
			cache : false,
			async : false,
			type: requestMethod,
			dataType: returnType,
			data: this.addAntiForgeryToken(param),
		}).done(function (data) {
			result = data;
		}).fail(function (jqXHR, textStatus) {
			if (debugError) {
				$(window).html(
					"<h1>%s</h1><h1> AJAX CALL ERROR from : %s</h1><h3> Parameters: %s</h3><hr />%s"
					.format("SpGet", method, JSON.stringify(param), jqXHR.responseText)
				);
			}
			else {
				alert("%s : %s".format("There is an error occured while ajax process is running", method));
			}
		});
		
		return result;
	};

	this.getAsJson = function (sp, params, log) {
		log = this.isEmpty(log) ? null : log;
		var param = { spName: sp, jsonParam: JSON.stringify(params), logging: log };
		var result = this.ajaxCallCore("json", "/SpGet/dbGetResultAsJson", param, "POST", true);
		return result;
	}

	this.getAsStr = function (sp, params, log) {
		log = this.isEmpty(log) ? null : log;
		var param = { spName: sp, jsonParam: JSON.stringify(params), logging: log };
		var result = this.ajaxCallCore("text", "/SpGet/dbGetResultAsStr", param, "POST", true);
		return this.getSafeStr(result);
	}

	this.getAsInt = function (sp, params, log) {
		log = this.isEmpty(log) ? null : log;
		var param = { spName: sp, jsonParam: JSON.stringify(params), logging: log };
		var result = this.ajaxCallCore("json", "/SpGet/dbGetResultAsInt", param, "POST", true);
		return this.getSafeInt(result);
	}

	this.addAntiForgeryToken = function (params) {
		if (this.isEmpty(params))
			params = {};

		params.__RequestVerificationToken = $('#__ajaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
		return params;
	};
}