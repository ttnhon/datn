$(document).ready(function () {
	$('#js-mng-contests').click(function() {
		$(this).addClass('active');
		$('#js-mng-challenges').removeClass('active');
		$('#js-statistics').removeClass('active');

		$('.mng-contests-modal').removeAttr('hidden');
		$('.mng-challenges-modal').attr('hidden','');
		$('.statistics-modal').attr('hidden','');
	});

	$('#js-mng-challenges').click(function() {
		$(this).addClass('active');
		$('#js-mng-contests').removeClass('active');
		$('#js-statistics').removeClass('active');

		$('.mng-contests-modal').attr('hidden','');
		$('.mng-challenges-modal').removeAttr('hidden');
		$('.statistics-modal').attr('hidden','');
	});

	$('#js-statistics').click(function() {
		$(this).addClass('active');
		$('#js-mng-contests').removeClass('active');
		$('#js-mng-challenges').removeClass('active');

		$('.mng-contests-modal').attr('hidden','');
		$('.mng-challenges-modal').attr('hidden','');
		$('.statistics-modal').removeAttr('hidden');
	});

	
});