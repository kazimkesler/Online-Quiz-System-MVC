function getFormData(form) { //Form bilgisini JSON formatında serialize eder. Buradaki amaç otomatik bind işlemini gerçekleştirecek bir formatı elde etmektir.
	var unindexed_array = $(form).serializeArray();
	var indexed_array = {};

	$.map(unindexed_array, function (n, i) {
		indexed_array[n['name']] = n['value'];
	});

	return indexed_array;
}