function escapeHTML(unsafe_str) {
    if (unsafe_str != undefined) {
        return unsafe_str
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/\"/g, '&quot;')
            .replace(/\'/g, '&#39;')
    }
    else {
        return unsafe_str;
    }
}

$(document).ready(function (e) {
    if ($(".about-us-wrapper").find(".approch-wrapper").length > 0) {
        $(".approch-wrapper").attr("id", "responsibility")
    }
    if ($(".about-us-wrapper").find(".manegar-wrap").length > 0) {
        $(".manegar-wrap").attr("id", "managingdirector")
    }

    var pageName = escapeHTML(window.location.pathname.toLowerCase());
    if (escapeHTML(pageName) == "/asset-portfolio-details") {
        $(".comman-breadcum").addClass("p-0");
        var sectionID = escapeHTML(window.location.href.split('#')[1]);
        if (escapeHTML(sectionID) != "" && escapeHTML(sectionID) != null && escapeHTML(sectionID) != "undefined") {
            var tabID = escapeHTML("#" + sectionID);
            /*side tabs*/
            $("#tabs-nav li").removeClass("active");
            $('#tabs-nav li a[href="#' + escapeHTML(sectionID) + '"]').parent().addClass("active");

            /*Asset Portfolio Content*/
            $("#tabs-content div").removeAttr("style");
            $('#tabs-content').children("div").css('display', 'none');
            $('#tabs-content').children('div[id="' + sectionID + '"]').css('display', 'block');
        }



        $('.dropdown-item').click(function () {
            var thisUrl = escapeHTML(this.href);
            var checkpart = escapeHTML("/asset-portfolio-details");
            if (thisUrl.indexOf(checkpart) != -1) {
                var sectionID = escapeHTML(this.href.split('#')[1]);
                if (escapeHTML(sectionID) != "" && escapeHTML(sectionID) != null && escapeHTML(sectionID) != "undefined") {
                    var tabID = escapeHTML("#" + sectionID);
                    /*side tabs*/
                    $("#tabs-nav li").removeClass("active");
                    $('#tabs-nav li a[href="#' + escapeHTML(sectionID) + '"]').parent().addClass("active");

                    /*Asset Portfolio Content*/
                    $("#tabs-content div").removeAttr("style");
                    $('#tabs-content').children("div").css('display', 'none');
                    $('#tabs-content').children('div[id="' + escapeHTML(sectionID) + '"]').css('display', 'block');
                }
            }
        });
        $('.field-navigationtitle').click(function () {
            var thisUrl = this.href;
            var checkpart = "/asset-portfolio-details";
            if (thisUrl.indexOf(checkpart) != -1) {
                var sectionID = escapeHTML(this.href.split('#')[1]);
                if (escapeHTML(sectionID) != "" && escapeHTML(sectionID) != null && escapeHTML(sectionID) != "undefined") {
                    var tabID = escapeHTML("#" + sectionID);
                    /*side tabs*/
                    $("#tabs-nav li").removeClass("active");
                    $('#tabs-nav li a[href="#' + escapeHTML(sectionID) + '"]').parent().addClass("active");

                    /*Asset Portfolio Content*/
                    $("#tabs-content div").removeAttr("style");
                    $('#tabs-content').children("div").css('display', 'none');
                    $('#tabs-content').children('div[id="' + escapeHTML(sectionID) + '"]').css('display', 'block');
                }
            }
        });
    }

    if (pageName == "/about-us" || pageName == "/investor/investor-download/") {
        var sectionID = window.location.href.split('#')[1];
        if (sectionID != "" && sectionID != null && sectionID != "undefined") {
            window.location = escapeHTML(window.location.href);
            $('html, body').animate({
                scrollTop: $("#" + escapeHTML(sectionID)).offset().top - 100
            }, '600');
        }
    }

    /*below code is to adding class to header according to page name*/
    var path = escapeHTML(window.location.pathname);
    var page = escapeHTML(path).split("/").pop();
    if (path == "/") {
        document.getElementById("header-head").classList.add('main-home-header');

    }
    else if (path.replace(/\/+$/, '') == "/asset-portfolio" || path.replace(/\/+$/, '') == "/about-us") {
        document.getElementById("header-head").classList.add('main-home-header');
    }
    else {
        document.getElementById("header-head").classList.add('inner-header');
    }


    var dataAttr = $('.stickytab').attr("data-variantitemid");
    if (escapeHTML(dataAttr) == "{863B9FEE-A8EC-4AB4-BD0A-442419EFCB8B}") {
        var stickyImg = $("<img>").attr("src", "/-/media/Project/Adani-Welspun/Adani-Welspun/Images/call.png");
        if ($('.stickytab').find('img').length == 0) {
            $('.stickytab').prepend(stickyImg);
        }
    }

    var customHeaderButton = escapeHTML($('.customContactUsbutton').attr("data-variantitemid"));
    if (customHeaderButton == "{F0769C1A-CA20-4D76-8227-7AD8114A5679}") {
        $('.customContactUsbutton[data-variantitemid="' + escapeHTML(customHeaderButton) + '"]').attr("data-bs-toggle", "modal");
        $('.customContactUsbutton[data-variantitemid="' + escapeHTML(customHeaderButton) + '"]').attr("data-bs-target", "#staticBackdrop");
    }

    $.each($(".services-boxes").find("li").find(".icon"), function () {

        var count = $(this).find("img").length;
        if (count == 0) {
            $(this).parents("li").remove();
        }
    });
    // removing  map's  2nd li 
    if ($('.box .des-top li').length > 0) {
        $('.box .des-top li')[1].remove();
    }

    var pageName = window.location.pathname;
    if (escapeHTML(pageName) == "/asset-portfolio-details") {
        var windowsize = $(window).width();
        if (windowsize <= 991) {
            $('.dropdown-item').click(function () {
                $('.navbar-toggler').click();
            }
            )
        }
    }

});

$(document).ready(function () {
    $('.moreless-button').click(function () {
        $('.moretext').slideToggle();
        if ($('.moreless-button').text() == "Read more") {
            $(this).text("Read less")
        } else {
            $(this).text("Read more")
        }
    });
});


// $(document).ready(function () {
// // Configure/customize these variables.
// var showChar = 100;  // How many characters are shown by default
// var ellipsestext = "...";
// var moretext = "Read more";
// var lesstext = "Read less";


// $('.more').each(function () {
// if ($(this).attr("data-showchar") != undefined) {
// showChar = $(this).attr("data-showchar");
// }
// var content = $(this).html();

// if (content.length > showChar) {

// var c = content.substr(0, showChar);
// var h = content.substr(showChar, content.length - showChar);

// var html = c + '<span class="morecontent"><span>' + h + '</span><a href="" class="morelink">' + moretext + '</a></span>';

// $(this).html(html);
// }

// });

// $(".morelink").click(function () {
// if ($(this).hasClass("less")) {
// $(this).removeClass("less");
// $(this).html(moretext);
// } else {
// $(this).addClass("less");
// $(this).html(lesstext);
// }
// $(this).parent().prev().toggle();
// $(this).prev().toggle();
// return false;
// });
// });

$('.field-link a').on('click', function () {
    var myinterval = setInterval(function () {
        if ($('.overlayFullWidth.component-content').length > 0) {
            $('.overlayFullWidth.component-content').addClass("contact-form");
            clearInterval(myinterval)
        }
        console.log('interval')

    }, 200)
});

/*Function to find element*/
function waitForElm(selector) {
    return new Promise(resolve => {
        if (document.querySelector(selector)) {
            return resolve(document.querySelector(selector));
        }

        const observer = new MutationObserver(mutations => {
            if (document.querySelector(selector)) {
                resolve(document.querySelector(selector));
                observer.disconnect();
            }
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    });
}



$('.evince-popup').click(function (e) {

    removeOverlayElements();
    var refrence = escapeHTML($(this).closest('ul').find('.evince-refrenceno').attr("data-refrence"));
    $('.evince-popup[data-variantitemid="' + $(this).attr("data-variantitemid") + '"]').attr("data-name", $(this).closest('ul').find('.evince-desc').attr("data-desc"));
    waitForElm('.eventname').then((elm) => {

        $(".eventname").attr('readonly', 'true');

        document.getElementsByClassName("eventname")[0].value = $(this).closest('ul').find('.evince-desc').attr("data-desc");
        setTimeout(function (objthis) {
            document.getElementsByClassName("refrence-title-info")[0].innerText = "Ref No - " + $(objthis).closest('ul').find('.evince-refrenceno').attr("data-refrence") + " - " + $(objthis).closest('ul').find('.evince-desc').attr("data-desc");
        }, 100, this);

        $(".messagesection").on("keypress", function (e) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code == 13) {
                $(this).next().hide();
            }
            else if ($(this).val().length > 38) {
                $(this).next().hide()
            }
            else if ($(this).val().indexOf("\n") > 0) {

                $(this).next().hide()
            }
            else {
                $(this).next().show()
            }


        });

        $(".messagesection").on("keyup", function (e) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code == 13) {
                $(this).next().hide();
            }
            else if ($(this).val().length > 38) {
                $(this).next().hide()
            }
            else if ($(this).val().indexOf("\n") > 0) {

                $(this).next().hide()
            }
            else {
                $(this).next().show()
            }


        });

        $("form").on("change", ".file-upload-field", function () {
            $(this).parent(".file-upload-wrapper").attr("data-text", $(this).val().replace(/.*(\/|\\)/, ''));
            var uploadedFileName = $(this).parent(".file-upload-wrapper").attr("data-text");
            if (uploadedFileName.length > 30) {
                $(this).parent(".file-upload-wrapper").attr("data-text", uploadedFileName.substr(0, 30) + "...");
            }
        });



    });

    if (refrence != "" || refrence != null) {
        waitForElm('.evincerefrence').then((elm) => {
            $(".evincerefrence").attr('readonly', 'true');
            document.getElementsByClassName("evincerefrence")[0].value = $(this).closest('ul').find('.evince-refrenceno').attr("data-refrence");
        });

    }

    waitForElm('.contact-form').then((elm) => {
        removeOverlayElements();
    });


    waitForElm('.thankyoumessage').then((elm) => {

        /*Remove default overlay pop compoment classes and styles*/
        $(".overlay").click(function () {
            location.reload(true);
        });
    });

    /*  }*/

    waitForElm('.thankyoumessage').then((elm) => {

        /*Remove default overlay pop compoment classes and styles*/
        $(".overlay").click(function () {
            location.reload(true);
        });
    });

});

$('.cntct-popup').click(function (e) {

    removeOverlayElements();

    waitForElm('.formwrapper').then((elm) => {
        $(".messagesection").on("keypress", function (e) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code == 13) {
                $(this).next().hide();
            }
            else if ($(this).val().length > 38) {
                $(this).next().hide()
            }
            else if ($(this).val().indexOf("\n") > 0) {

                $(this).next().hide()
            }
            else {
                $(this).next().show()
            }
        });

        $(".messagesection").on("keyup", function (e) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code == 13) {
                $(this).next().hide();
            }
            else if ($(this).val().length > 38) {
                $(this).next().hide()
            }
            else if ($(this).val().indexOf("\n") > 0) {

                $(this).next().hide()
            }
            else {
                $(this).next().show()
            }
        });

        $("form").on("change", ".file-upload-field", function () {
            $(this).parent(".file-upload-wrapper").attr("data-text", $(this).val().replace(/.*(\/|\\)/, ''));
            var uploadedFileName = $(this).parent(".file-upload-wrapper").attr("data-text");
            if (uploadedFileName.length > 30) {
                $(this).parent(".file-upload-wrapper").attr("data-text", uploadedFileName.substr(0, 30) + "...");
            }
        });

    });
    waitForElm('.contact-form').then((elm) => {

        /*Remove default overlay pop compoment classes and styles*/
        removeOverlayElements();
    });


    waitForElm('.thankyoumessage').then((elm) => {

        /*Remove default overlay pop compoment classes and styles*/
        $(".overlay").click(function () {
            location.reload(true);
        });
    });

});

function removeOverlayElements() {
    $('.contact-form').removeClass("component-content overlayFullWidth");
    $('.contact-form').removeAttr("style");
    $('.contact-form').find('.overlay-close').remove();

}



$('.field-navigationtitle').click(function () {
    var pageName = escapeHTML(window.location.pathname);
    var sectionID = escapeHTML(this.href.split('#')[1]);
    if (pageName == "/about-us") {
        if (escapeHTML(sectionID) != "" && escapeHTML(sectionID) != null && escapeHTML(sectionID) != "undefined") {
            window.location = escapeHTML(window.location.href);
            $('html, body').animate({
                scrollTop: $("#" + escapeHTML(sectionID)).offset().top - 100
            }, '600');
        }
    }
});


$('.dropdown-item, .field-navigationtitle').click(function () {
    var pageName = escapeHTML(window.location.pathname);
    var sectionID = escapeHTML(this.href.split('#')[1]);
    if (pageName == "/asset-portfolio-details") {
        if (escapeHTML(sectionID) != "" && escapeHTML(sectionID) != null && escapeHTML(sectionID) != "undefined") {
            window.location = escapeHTML(window.location.href);
            $('html, body').animate({
                scrollTop: $("#" + escapeHTML(sectionID)).offset().top - 100
            }, '600');
        }
    }
});


