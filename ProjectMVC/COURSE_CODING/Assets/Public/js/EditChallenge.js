$(document).ready(function () {
	$('#js-details').click(function() {
		$(this).addClass('active');
		$('#js-moderators').removeClass('active');
		$('#js-test-cases').removeClass('active');
		$('#js-code-stubs').removeClass('active');
		$('#js-languages').removeClass('active');
		$('#js-settings').removeClass('active');
		$('#js-editorial').removeClass('active');

		$('.details-modal').removeAttr('hidden');
		$('.moderators-modal').attr('hidden','');
		$('.test-cases-modal').attr('hidden','');
		$('.code-stubs-modal').attr('hidden','');
		$('.languages-modal').attr('hidden','');
		$('.settings-modal').attr('hidden','');
		$('.editorial-modal').attr('hidden','');
	});

	$('#js-moderators').click(function() {
		$(this).addClass('active');
		$('#js-details').removeClass('active');
		$('#js-test-cases').removeClass('active');
		$('#js-code-stubs').removeClass('active');
		$('#js-languages').removeClass('active');
		$('#js-settings').removeClass('active');
		$('#js-editorial').removeClass('active');

		$('.details-modal').attr('hidden','');
		$('.moderators-modal').removeAttr('hidden');
		$('.test-cases-modal').attr('hidden','');
		$('.code-stubs-modal').attr('hidden','');
		$('.languages-modal').attr('hidden','');
		$('.settings-modal').attr('hidden','');
		$('.editorial-modal').attr('hidden','');
	});

	$('#js-test-cases').click(function() {
		$(this).addClass('active');
		$('#js-details').removeClass('active');
		$('#js-moderators').removeClass('active');
		$('#js-code-stubs').removeClass('active');
		$('#js-languages').removeClass('active');
		$('#js-settings').removeClass('active');
		$('#js-editorial').removeClass('active');

		$('.details-modal').attr('hidden','');
		$('.moderators-modal').attr('hidden','');
		$('.test-cases-modal').removeAttr('hidden');
		$('.code-stubs-modal').attr('hidden','');
		$('.languages-modal').attr('hidden','');
		$('.settings-modal').attr('hidden','');
		$('.editorial-modal').attr('hidden','');
	});

	$('#js-code-stubs').click(function() {
		$(this).addClass('active');
		$('#js-details').removeClass('active');
		$('#js-moderators').removeClass('active');
		$('#js-test-cases').removeClass('active');
		$('#js-languages').removeClass('active');
		$('#js-settings').removeClass('active');
		$('#js-editorial').removeClass('active');

		$('.details-modal').attr('hidden','');
		$('.moderators-modal').attr('hidden','');
		$('.test-cases-modal').attr('hidden','');
		$('.code-stubs-modal').removeAttr('hidden');
		$('.languages-modal').attr('hidden','');
		$('.settings-modal').attr('hidden','');
		$('.editorial-modal').attr('hidden','');
	});

	$('#js-languages').click(function() {
		$(this).addClass('active');
		$('#js-details').removeClass('active');
		$('#js-moderators').removeClass('active');
		$('#js-test-cases').removeClass('active');
		$('#js-code-stubs').removeClass('active');
		$('#js-settings').removeClass('active');
		$('#js-editorial').removeClass('active');

		$('.details-modal').attr('hidden','');
		$('.moderators-modal').attr('hidden','');
		$('.test-cases-modal').attr('hidden','');
		$('.code-stubs-modal').attr('hidden','');
		$('.languages-modal').removeAttr('hidden');
		$('.settings-modal').attr('hidden','');
		$('.editorial-modal').attr('hidden','');
	});

	$('#js-settings').click(function() {
		$(this).addClass('active');
		$('#js-details').removeClass('active');
		$('#js-moderators').removeClass('active');
		$('#js-test-cases').removeClass('active');
		$('#js-code-stubs').removeClass('active');
		$('#js-languages').removeClass('active');
		$('#js-editorial').removeClass('active');

		$('.details-modal').attr('hidden','');
		$('.moderators-modal').attr('hidden','');
		$('.test-cases-modal').attr('hidden','');
		$('.code-stubs-modal').attr('hidden','');
		$('.languages-modal').attr('hidden','');
		$('.settings-modal').removeAttr('hidden');
		$('.editorial-modal').attr('hidden','');
	});

	$('#js-editorial').click(function() {
		$(this).addClass('active');
		$('#js-details').removeClass('active');
		$('#js-moderators').removeClass('active');
		$('#js-test-cases').removeClass('active');
		$('#js-code-stubs').removeClass('active');
		$('#js-languages').removeClass('active');
		$('#js-settings').removeClass('active');

		$('.details-modal').attr('hidden','');
		$('.moderators-modal').attr('hidden','');
		$('.test-cases-modal').attr('hidden','');
		$('.code-stubs-modal').attr('hidden','');
		$('.languages-modal').attr('hidden','');
		$('.settings-modal').attr('hidden','');
		$('.editorial-modal').removeAttr('hidden');
	});
	
	$('#js-moderator-add-btn').click(function() {
	});
	$('#js-save-test-case-btn').click(function() {
	});
	$('#js-save-changes-btn').click(function() {
	});

    
});