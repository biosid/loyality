define('Feedback/thread', ['Shared/captcha', 'Shared/file-upload'], function (captcha, fileUpload) {
	return function(options) {

		captcha(options.captcha);
		fileUpload(options.fileUpload);

	};
});