$(document).ready(function () {
	$('#js-mng-contests').click(function() {
		$(this).addClass('active');
		$('#js-mng-challenges').removeClass('active');
		$('#js-statistics').removeClass('active');

		$('.mng-contests-modal').removeClass('hidden');
		$('.mng-challenges-modal').addClass('hidden');
		$('.statistics-modal').addClass('hidden');
	});

	$('#js-mng-challenges').click(function() {
		$(this).addClass('active');
		$('#js-mng-contests').removeClass('active');
		$('#js-statistics').removeClass('active');

		$('.mng-contests-modal').addClass('hidden');
		$('.mng-challenges-modal').removeClass('hidden');
		$('.statistics-modal').addClass('hidden');
	});

	$('#js-statistics').click(function() {
		$(this).addClass('active');
		$('#js-mng-contests').removeClass('active');
		$('#js-mng-challenges').removeClass('active');

		$('.mng-contests-modal').addClass('hidden');
		$('.mng-challenges-modal').addClass('hidden');
		$('.statistics-modal').removeClass('hidden');
	});

	
});