function StickySideNav() {
    /* if (document.body.dataset.page == "home") {
    $("#main").addClass("not-floating");
  } else {
    $("#main").addClass("not-floating");
  } */
    $(".custom-hamburger").click(function () {
        $("body").toggleClass("nav-expand");
    });
    $('body').click(function() {
        $('body').removeClass('nav-expand');
    });
    $('.icon-bar-wrapper').click(function(e) {
        e.stopPropagation();
    });
}
export { StickySideNav };
