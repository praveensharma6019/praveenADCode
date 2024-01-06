var Contactrecaptcha;
if (typeof sitekey !== 'undefined' && (sitekey != null && sitekey != '')) {
	var onloadCallback = function () {
		//Render the Contactrecaptcha on the element with ID "Contactrecaptcha"
		if ($('#recaptcha').length) {
			Contactrecaptcha = grecaptcha.render('Contactrecaptcha', {
				'sitekey': sitekey,
				'theme': 'light'
			});
		}
	}
}

function submitContactUsForm () {
	var response = grecaptcha.getResponse(Contactrecaptcha);
	grecaptcha.reset();
	$('#captchaResp').val(response);
	if (response.length == 0) {
		alert("Captcha required.");
		return false;
	}
};
