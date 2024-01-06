
$(document).ready(function () {
    if ($("#selectOffice").length > 0) {
        $("#selectOffice")[0].selectedIndex = 0;
    }

    $("#selectOffice").change(function () {
        var obj = JSON.parse($(this).attr('attr-data'));

        let val = $('#selectOffice :selected').text();
        let selecteditem = obj.filter(x => x.name == val);
        let img = $('#id0 img');
        selecteditem = selecteditem[0];
        $('.contact-address').html(selecteditem.RichText);
        img.attr('src', selecteditem.ImageSrc);
        img.attr('alt', selecteditem.ImageAlt);
    });
    if (document.getElementById('cookies_offcanvas')) {
        var myOffcanvas = document.getElementById('cookies_offcanvas')
        myOffcanvas.addEventListener('hidden.bs.offcanvas', function () {
            cookieHandler();
        })
    }
})
const setCookie = (name) => {
    document.cookie = name + "=accepted; path=/";
};
const getCookie = (name) => {
    return document.cookie.match("(^|;)\\s*" + name + "\\s*=\\s*([^;]+)");
};
const isCookieExist = getCookie("WTCookies");
const setIsCookie = (flag) => {
    if (window.innerWidth >= 992) {
        if (document.getElementById("cookiesModal")) {
            if (flag) {
                jQuery("#cookiesModal").removeClass('d-none');
            } else {
                jQuery("#cookiesModal").addClass('d-none');
            }
        }
    } else {
        if (document.getElementById("cookies_offcanvas")) {
            var $cookiesOffcanvas = document.getElementById("cookies_offcanvas");
            var cookiesOffcanvas = new bootstrap.Offcanvas($cookiesOffcanvas);
            if (flag) {
                cookiesOffcanvas.show();
            } else {
                cookiesOffcanvas.hide();
            }
        }
    }
}

if (isCookieExist) {
    setIsCookie(false);
} else {
    setIsCookie(true);
}

function closeCookie() {
    cookieHandler();
    setIsCookie(false);
}

const cookieHandler = () => {
    setCookie("WTCookies");
};
$('.tabLink').click(function () {
	  $('.tab-pane').removeClass('active').removeClass('in');
	 $($(this).attr('href')).addClass('active').addClass('in');
  });