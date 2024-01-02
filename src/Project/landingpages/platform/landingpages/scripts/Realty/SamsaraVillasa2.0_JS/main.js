$(document).ready(function () {
    //Active Tab 1
    var pTabItem = $(".prodNav .ptItem");
    $(pTabItem).click(function () {
        $(pTabItem).removeClass("active");
        $(this).addClass("active");
        var tabid = $(this).attr("id");
        $(".prodMain").removeClass("active");
        $("#" + tabid + "C").addClass("active");
        return false;
    });

    //Active Tab 2
    var psItem = $(".str_m .psItem");
    $(psItem).click(function () {
        $(psItem).removeClass("active");
        $(this).addClass("active");
        var tabid = $(this).attr("id");
        $(".str_ms").removeClass("active");
        $("#" + tabid + "s").addClass("active");
        return false;
    });

    var psItem_m = $(".str_d .psItem");
    $(psItem_m).click(function () {
        $(psItem_m).removeClass("active");
        $(this).addClass("active");
        var tabid = $(this).attr("id");
        $(".str_ds").removeClass("active");
        $("#" + tabid + "s").addClass("active");
        return false;
    });

    //Sticky Header
    initStickyHeader();
    function initStickyHeader() {
        "use strict";

        var win = $(window),
            stickyClass = "sticky";

        $("#header.sticky-header").each(function () {
            var header = $(this);
            var headerOffset = header.offset().top || 0;
            var flag = true;

            $(this).css("height", jQuery(this).innerHeight());

            function scrollHandler() {
                if (win.scrollTop() > headerOffset) {
                    if (flag) {
                        flag = false;
                        header.addClass(stickyClass);
                    }
                } else {
                    if (!flag) {
                        flag = true;
                        header.removeClass(stickyClass);
                    }
                }
            }

            scrollHandler();
            win.on("scroll resize orientationchange", scrollHandler);
        });
    }

    initbackTop();
    // Back Top
    function initbackTop() {
        "use strict";

        var jQuerybackToTop = jQuery("#back-top");
        jQuery(window).on("scroll", function () {
            if (jQuery(this).scrollTop() > 100) {
                jQuerybackToTop.addClass("active");
            } else {
                jQuerybackToTop.removeClass("active");
            }
        });
        jQuerybackToTop.on("click", function (e) {
            jQuery("html, body").animate({ scrollTop: 0 }, 500);
        });
    }

    window.addEventListener("scroll", function () {
        if ($(window).width() < 768) {
            var header = document.querySelector(".footer_btn_e");
            header.classList.toggle("active", scrollY > 1400);
        }
    });

    $(".footer_e").click(function () {
        $(".section_enquire").show();
    });

    $(".close-enquiry").click(function () {
        $(".section_enquire").hide();
        var element = document.getElementById("footer_form");
        element.reset();
        $(".formTop").removeClass("formTop");
        $("#footer_form small").innerHTML = "";
    });

    $(".btn-enquire").click(function () {
        $(".section_enquire").show();
        var element = document.getElementById("footer_form");
        element.reset();
        clearForm();
    });

    $(window).on("load", function () {
        setTimeout(function () {
            if ($("#exampleModals").hasClass("in")) {
                return;
            } else {
                $("#exampleModal").modal("show");
            }
        }, 3000);
    });
    $(".smooth").click(function () {
        $("html, body").animate(
            {
                scrollTop: $($(this).attr("href")).offset().top - 100,
            },
            300
        );
        return false;
    });

    var syncedSecondary = true;

    var item = $(".owl-carousel").owlCarousel({
        autoplay: true,
        autoplayTimeout: 5000,

        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1,
                margin: 10,
                loop: true,
                stagePadding: 50,
                nav: true,
            },
            768: {
                items: 1,
                margin: 10,
                nav: true,
                stagePadding: 200,
                loop: true,
            },
            1000: {
                items: 1,
                margin: 10,
                nav: true,
                stagePadding: 250,
                loop: true,
            },
            1200: {
                items: 1,
                margin: 30,
                nav: true,
                stagePadding: 300,
                loop: true,
            },
        },
    });
    $(".yBox").magnificPopup({
        type: "image",
        gallery: {
            enabled: true,
        },
    });

    var formInputs = $("div.form-item input");
    formInputs.focus(function () {
        $(this).parent().children("label.formLabel").addClass("formTop");
    });
    formInputs.focusout(function () {
        if ($.trim($(this).val()).length == 0) {
            $(this).parent().children("label.formLabel").removeClass("formTop");
        }
    });
    $("p.formLabel").click(function () {
        $(this).parent().children(".form-style").focus();
    });

    $("#crs_icon").click(function () {
        $(this).toggleClass("active_menu");
        $("body").toggleClass("overflow-hidden");
    });

    $("#mobile").on("keypress", function (key) {
        if (key.charCode < 48 || key.charCode > 57) return false;
    });

    $("#phone_number_fot").on("keypress", function (key) {
        if (key.charCode < 48 || key.charCode > 57) return false;
    });

    $("#b_mobile").on("keypress", function (key) {
        if (key.charCode < 48 || key.charCode > 57) return false;
    });
});

$(".video")
    .parent()
    .click(function () {
        if ($(this).children(".video").get(0).paused) {
            $(this).children(".video").get(0).play();
            $(this).children(".playpause").fadeOut();
        } else {
            $(this).children(".video").get(0).pause();
            $(this).children(".playpause").fadeIn();
        }
    });

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(document).scrollTop() > 200) {
            $("header").addClass("sticky");
        } else {
            $("header").removeClass("sticky");
        }
    });
});

$(".d_broucher").click(function (e) {
    $(".formTop").removeClass("formTop");

    clearForm();
});

const allowOnlyNumberInput = (ele) => {
    ele.value = ele.value.replace(/[^0-9]/g, "");
};

$("#crs_icon").click(function () {
    $(".section_enquire").css("display", "none");
});

$(".navbar-collapse .smooth").click(function () {
    $(".navbar-collapse").removeClass("in");
    $(".navbar-collapse").attr("aria-expanded", "false");
    $("#crs_icon").removeClass("active_menu");
    $("body").removeClass("overflow-hidden");
});

function clearForm() {
    const errorElements = document.querySelectorAll(".errorMsg");
    const formInputs = document.querySelectorAll("input");
    for (let i = 0; i < formInputs.length; i++) {
        formInputs[i].value = "";
    }
    for (let i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
}
const otpValidate = function (ele) {
    ele.value = ele.value.replace(/[^0-9]/g, "");
};

const tabChange = function (val) {
    const ele = document.querySelectorAll(".form-otp");
    if (ele[val - 1].value != "") {
        ele[val].focus();
    } else if (ele[val - 1].value == "") {
        ele[val - 2].focus();
    }
};
