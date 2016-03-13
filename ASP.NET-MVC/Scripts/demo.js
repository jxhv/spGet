function Demo()
{
	this.lastId = 0;
	this.eventHolder = null;
}

Demo.prototype.init = function () {
	var self = this;

	this.delRecord();
	this.addRecord();
	this.getList();
	this.getTypes();
	this.getData();

	$("#edPhone").val($("#phone").html());

	$("#edPhone").keydown(function () {
		self.delayrun(function () {
			self.setData($("#edPhone").val());
			self.getData();
		});
	});
};

Demo.prototype.addRecord = function () {
	spGet.getAsInt(sp_mem_add, { name: 'Daniel', phone: '714-225-1009' });
	spGet.getAsInt(sp_mem_add, { name: 'Gloria', phone: '408-436-2008' });
	spGet.getAsInt(sp_mem_add, { name: 'Jeremi', phone: '640-747-3007' });
	this.lastId = spGet.getAsInt(sp_mem_add, { name: 'Gianna', phone: '712-858-4006' });
};

Demo.prototype.delRecord = function () {
	spGet.getAsInt(sp_mem_del, { name: 'Daniel' });
	spGet.getAsInt(sp_mem_del, { name: 'Gloria' });
	spGet.getAsInt(sp_mem_del, { name: 'Jeremi' });
	spGet.getAsInt(sp_mem_del, { name: 'Gianna' });
};

Demo.prototype.getList = function () {
	var builder = "";

	var data = spGet.getAsJson(sp_mem_get_tbl);

	if (data != null) {
		for (var i = 0, ln = data.length; i < ln; i++) {
			builder += "<ul><li>" + data[i].id + "</li><li>" + data[i].name + "</li><li>" + data[i].phone + "</li></ul>"
		}
	}

	$("#board").append(builder);
};

Demo.prototype.getTypes = function () {
	var builder = "";

	var param = {
		param1: 1234,
		param2: "spGet Framework",
		param3 :24.98,
		param4 :0.9778,
		param5 :-12.33,
		param6 :304.05,
		param7 :false
	};

	var data = spGet.getAsJson("test", param);
	var result = "<tr><th>int</th><th>varchar</th><th>decimal</th><th>float</th><th>real</th><th>money</th><th>bit</th></tr>";

	if(data != null) {
		var rec = data[0];
		result += "\
		<tr>\
			<td>" + rec.int + "\
			<td>" + rec.varchar + "\
			<td>" + rec.decimal + "\
			<td>" + rec.float + "\
			<td>" + rec.real + "\
			<td>" + rec.money + "\
			<td>" + rec.bit + "\
		</tr>";
	}

	$("#types").html(result);
};

Demo.prototype.getData = function () {
	var phone = spGet.getAsStr(sp_mem_get_phone, { name: "Gianna" });
	$("#phone").html(phone);
};

Demo.prototype.setData = function (phoneNum) {
	var param = {
		id: this.lastId,
		name: 'Gianna',
		phone: phoneNum
	};
	spGet.getAsInt(sp_mem_set, param);
};

Demo.prototype.delayrun = function (function_module) {
	var self = this;

	clearTimeout(this.eventHolder);
	this.eventHolder = setTimeout(function () {
		try {
			function_module();
			clearTimeout(self.eventHolder);
		} catch (e) {
			alert(e.message);
		}
	}, 120);
};