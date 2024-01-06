function domExe() {
    document.querySelector(".mobile-trigger").addEventListener("click", function() {
        document.querySelector('body').classList.toggle('menu-open');
    });
    if (document.querySelectorAll('.advHome-banner').length === 0) {
        document.querySelector('body').classList.add('banner-is-not-here');
    }
}

document.addEventListener("DOMContentLoaded", function() {
    domExe();
});
$(function() { 
    AOS.init({
      easing: 'ease-out-back',
      duration: 1000
    });
	setTimeout(function() {

var urlParam = window.location.search;

if (urlParam.indexOf('?') > -1) {

var urlTabId = urlParam.replace('?', '');

$('li.tab-link[data-tab="'+urlTabId+'"]').click();

}

}, 100);
});