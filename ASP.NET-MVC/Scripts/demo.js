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
	para.getSpResultAsInt(sp_mem_add, { name: 'Daniel', phone: '714-225-1009' });
	para.getSpResultAsInt(sp_mem_add, { name: 'Gloria', phone: '408-436-2008' });
	para.getSpResultAsInt(sp_mem_add, { name: 'Jeremi', phone: '640-747-3007' });
	this.lastId = para.getSpResultAsInt(sp_mem_add, { name: 'Gianna', phone: '712-858-4006' });
};

Demo.prototype.delRecord = function () {
	para.getSpResultAsInt(sp_mem_del, { name: 'Daniel' });
	para.getSpResultAsInt(sp_mem_del, { name: 'Gloria' });
	para.getSpResultAsInt(sp_mem_del, { name: 'Jeremi' });
	para.getSpResultAsInt(sp_mem_del, { name: 'Gianna' });
};

Demo.prototype.getList = function () {
	var builder = "";

	var data = para.getSpResultAsJson(sp_mem_get_tbl);

	if (data != null) {
		for (var i = 0, ln = data.length; i < ln; i++) {
			builder += "<ul><li>" + data[i].id + "</li><li>" + data[i].name + "</li><li>" + data[i].phone + "</li></ul>"
		}
	}

	$("#board").append(builder);
};

Demo.prototype.getData = function () {
	var phone = para.getSpResultAsStr(sp_mem_get_phone, { name: "Gianna" });
	$("#phone").html(phone);
};

Demo.prototype.setData = function (phoneNum) {
	var param = {
		id: this.lastId,
		name: 'Gianna',
		phone: phoneNum
	};
	para.getSpResultAsInt(sp_mem_set, param);
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