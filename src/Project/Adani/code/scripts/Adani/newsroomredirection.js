$(document).ready(function () {
    if (window.location.href.indexOf("?title") > -1) {
        var title = GetParameterValues('title');
        window.location("https://www.adani.com/Newsroom/Media-Release/" + title);
    }
   
    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }
}); 
