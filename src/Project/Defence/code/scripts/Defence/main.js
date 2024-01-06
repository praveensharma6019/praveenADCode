// headerHeight = document.querySelector("header").clientHeight;
// window.addEventListener("scroll", function () {
//     const header = document.querySelector("header"),
//         scroll = window.pageYOffset | document.body.scrollTop;

//     if (scroll > headerHeight) {
//         header.className = "header sticky";
//     } else if (scroll <= headerHeight) {
//         header.className = "header transparent";
//     }
// });
$(document).ready(function () {
    $('form').attr('autocomplete','off');
    $('.custom-header-wraper .sidebar-navigation,.custom-header-wraper .sidebar-navigation+.overLay').insertBefore('.custom-header-wraper');
});
document
    .querySelector(".mobile-trigger")
    .addEventListener("click", function (e) {
        e.stopPropagation();
        document.querySelector("body").classList.toggle("menu-open");
        document.querySelector("html").classList.toggle("cm-menu-open");
    });
document.querySelector("body").addEventListener("click", function (ele) {
    this.classList.remove("menu-open");
    document.querySelector("html").classList.remove("cm-menu-open");
});

document
    .querySelector(".sidebar-navigation")
    .addEventListener("click", function (a) {
        a.stopPropagation();
    });
$(document).ready(function () {
    $('.terms-condition-wrapper .icon-button').click(function () {
        $(this).closest('.terms-condition-wrapper').toggleClass('collapse-item');
    });
    $(".carousel").slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        dots: true,
        autoplay: true,
        prevArrow:
            '<span class="i-arrow-r slick-prev"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path></svg></span>',
        nextArrow:
            '<span class="i-arrow-r slick-next"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path></svg></span>',
        responsive: [
            {
                breakpoint: 600,
                settings: {
                    autoplay: false,
                }
            }
        ]
    });

    $(".green-banner-section .wrap").slick({
        infinite: true,
        arrows: true,
        dots: true,
        autoplay: true,
        autoplaySpeed: 2000,
        fade: true,
        slidesToShow: 1,
        slidesToScroll: 1,
    });

    //$(".sub-menu-btn").click(function () {
    //    $(this).next(".collapse").slideToggle(250);
    //    $(this).toggleClass('active');
    //    setTimeout(function() {
    //        document.querySelector(".sidebar-navigation .hamburger_section.single_item")?.scrollIntoView();
    //    }, 300);
        
    //});

    $(".cm_detail_banner").slick({
        arrows: true,
        dots: true,
        infinite: false,
        slidesToScroll: 1,
        slidesToShow: 1,
    });

    $('.expand-btn').click(function () {
        $(this).closest('.member-details').toggleClass('active');
    });

    $('.about-btn').click(function () {
        $(this).closest('.about-info').toggleClass('active');
        $(this).closest('.club-content').toggleClass('active');
        $(this).closest('.hero-intro').toggleClass('active');
        $(this).closest('.story-problemstatement').toggleClass('active');

        // $(this).closest('.problem-desc').parent().toggleClass('active');
    });

    $(window).on('load resize', function () {
        if (window.innerWidth >= 601) {
            if ($(".green-action-bar .wrap.slick-slider").length === 0) {
                $(".green-action-bar .wrap").slick({
                    arrows: true,
                    infinite: false,
                    slidesToShow: 6,
                    slidesToScroll: 3,
                    dots: false,
                    prevArrow:
                        '<span class="i-arrow-r slick-prev"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path></svg></span>',
                    nextArrow:
                        '<span class="i-arrow-r slick-next"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path></svg></span>',
                    responsive: [
                        {
                            breakpoint: 991,
                            settings: {
                                slidesToShow: 4,
                                slidesToScroll: 4,
                            },
                        },
                    ],
                });
            }
        } else {
            if ($(".green-action-bar .wrap.slick-slider").length > 0) {
                setTimeout(function () {
                    $(".green-action-bar .wrap").slick('unslick');
                }, 200);
            }
        }
    });

    if (window.innerWidth >= 768) {
        $(".business-update-section .btm").slick({
            arrows: true,
            dots: false,
            infinite: false,
            slidesToScroll: 1,
            slidesToShow: 4,
            prevArrow: $(".business_arr_left"),
            nextArrow: $(".business_arr_right"),
            responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 1.25,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
        $(".details_awards .btm").slick({
            arrows: true,
            dots: false,
            infinite: false,
            slidesToScroll: 1,
            slidesToShow: 3,
            prevArrow: $(".award_arr_left"),
            nextArrow: $(".award_arr_right"),
            responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 1.25,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
        $(".green-stories-section .btm").slick({
            arrows: true,
            dots: false,
            infinite: false,
            slidesToScroll: 1,
            slidesToShow: 4,
            prevArrow: $(".arr-left"),
            nextArrow: $(".arr-right"),
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
        $(".green-events .slides").slick({
            arrows: true,
            dots: true,
            infinite: false,
            slidesToScroll: 1,
            slidesToShow: 1,
            prevArrow: $(".event-arr-left"),
            nextArrow: $(".event-arr-right"),
        });
    }
    $(".input-search input").click(function () {
        $(".input-search input").click(function () {
            $(".search-suggestions").show();
        });
        $(".search-suggestions ul li").click(function () {
            $(".search-suggestions").hide();
        });

        $(".select-wrapper>label").click(function () {
            $(this).toggleClass("active");
            $(this).next(".select-drpodown").slideToggle(250);
        });
    });

    $(".view-all .btn").click(function () {
        $(this).closest(".action-bottom-section").toggleClass("active");
        let btn_text = $(".view-all .btn");
        if (btn_text.text() == "View All") {
            btn_text.text("View Less");
        } else {
            btn_text.text("View All");
        }
    });

    var header_height = $("header").outerHeight();
    var stickyBarHeight = $(".details_top_nav").outerHeight();

    $(".details_top_nav").css("top", header_height);
    $(window).on("resize scroll", function () {
        var header_height2 = $("header").outerHeight();
        $(".details_top_nav").css("top", header_height2);
    });

    function scrollSpy() {
        var sections = [
            "problem_statement",
            "solutions",
            "metrics",
            "awards",
            "team",
            "impact-mission",
            "our-vision",
            "advisory-board",
            "mentoring-club",
            "leadership",
        ];
        var current;
        for (var i = 0; i < sections.length; i++) {
            var top =
                ($("#" + sections[i]).offset() || { top: NaN }).top -
                (header_height + stickyBarHeight);
            if (top <= $(window).scrollTop()) {
                current = sections[i];
            }
        }
        $(".details_top_nav ul li a[href='#" + current + "']")
            .parent()
            .addClass("active");
        $(".details_top_nav ul li a")
            .not("a[href='#" + current + "']")
            .parent()
            .removeClass("active");
    }
    // smooth scrolling navigation
    // smooth scrolling navigation
    $(".details_top_nav ul li a").click(function () {
        var target = $(this).attr("href");
        /* $("body, html").animate({
              scrollTop: $(target).offset().top - (header_height + stickyBarHeight - 1)
            }, 500); */
        var customScrollTop =
            $(target).offset().top - (header_height + stickyBarHeight - 30);
        scrollTo(0, customScrollTop);
        return false;
    });

    scrollSpy();
    $(window).scroll(function () {
        scrollSpy();
    });

    // $(".tab-list>li").click(function () {
    //   $(this).siblings().removeClass("active");
    //   $(".tab-description").css("display", "none");
    //   var tabLink = $(this).attr("id");
    //   $("#b" + tabLink).css("display", "block");
    //   $(this).addClass("active");
    // });


    $(".service-description h3").click(function (e) {
        e.stopPropagation();
        $(this).closest(".body-container-wrapper").toggleClass("service-menu-open");
    });
    $(".service-menu").click(function (e) {
        e.stopPropagation();
    });

    $(document).click(function (e) {
        $(".body-container-wrapper").removeClass("service-menu-open");
    });
    $(".service-menu .close-btn").click(function (e) {
        $(".body-container-wrapper").removeClass("service-menu-open");
    });

    setTimeout(function () {
        var urlPath = window.location.pathname;
        urlPath.match(/\/(.*?)(\+|$)/)[1].toLowerCase();
        urlPath = urlPath.replace(/\//g, '-').slice(1);
        urlPath = urlPath.replace(/\.[^/.]+$/, "");
        $('body').addClass(urlPath);
        $('body:not([class])').addClass('home');
    }, 50);

    $('.x-col-btn').click(function(){
        $(this).parent('.inner-desc').toggleClass('active');
    });
    $('.filter-wrap .sort').click(function(){
        $(this).parent().toggleClass('show');
        $('body').toggleClass('active-filter');
        // $('.filter-wrap').removeClass('show');
    });
    $('.filter-wrap ul li').click(function() {    
        let data_name = $(this).attr('data-title');
        let data_text = $(this).text();
    
        $(this).closest('.heading').next('.cm-story-wrap').find('.story-cards').hide();
        $(this).closest('.heading').next('.cm-story-wrap').find(`.story-cards[data-name=${data_name}]`).show();
    
        $(this).closest('.filter-wrap').find('.sort span').text(data_text);
        $(this).closest('.filter-wrap').removeClass('show');

        $(this).siblings().removeClass('active');
        $(this).addClass('active');
        
        $('body').toggleClass('active-filter');
    });
    $('.filter-overLay, .filter-wrap ul svg').click(function(){
        $('body').toggleClass('active-filter');
        $('.filter-wrap').removeClass('show');
    });
});

document.querySelector(".view-all")?.addEventListener("click", function (e) {
    e.stopPropagation();
    document.querySelector(".goals-modal").classList.add("show-modal");
    document.querySelector("body").classList.add("ovf-hid");
    document.querySelector(".modal-overlay").classList.add("overlay-active");
});

document.querySelector(".close-bar")?.addEventListener("click", function (a) {
    a.stopPropagation();
    document.querySelector(".goals-modal").classList.remove("show-modal");
    document.querySelector("body").classList.remove("ovf-hid");
    document.querySelector(".modal-overlay").classList.remove("overlay-active");
});
document.querySelector(".goals-modal")?.addEventListener("click", function (a) {
    a.stopPropagation();
});

document.querySelector("body").addEventListener("click", function (ele) {
    document.querySelector(".goals-modal")?.classList.remove("show-modal");
    document.querySelector("body").classList.remove("ovf-hid");
    document.querySelector(".modal-overlay")?.classList.remove("overlay-active");
});



window.onload = function () {
    if (document.querySelectorAll('.tab-nav').length > 0) {
        document.body.scrollTop = 0;
        document.documentElement.scrollTop = 0;
        setTimeout(() => {
            var navlinks = document.getElementsByClassName("tab-nav");
            for (var i = 0; i < navlinks.length; i++) {
                navlinks[i].classList.remove('active');
                if (i === 0) {
                    navlinks[i].classList.add('active');
                }
            }
            document.body.scrollTop = 0; // For Safari
            document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera  
        }, 200);
    }
}

window.onscroll = function () {
    try {
        let horizontaltabHeader = document.getElementById("navbar-tabs");

        if (window.pageYOffset < horizontaltabHeader.offsetTop && document.querySelector('#navbar-tabs .tab-nav.active') == null) {
            document.getElementsByClassName("tab-nav")[0].classList.add('active');
        }
    } catch (error) {
    }
};

$(document).ready(function () {
    $('.hint-txt .tips a').click(function (abb) {
        abb.stopPropagation();
        $(this).closest('.formrow').toggleClass('tips-open');
        $('body').toggleClass('speak-open');
    });

    $(".hover-text").click(function (abb) {
        abb.stopPropagation();
    });

    $(document).click(function (abb) {
        $('body').removeClass('speak-open');
        $('.formrow').removeClass('tips-open');
    });
    $(".close-bar").click(function (abb) {
        $('body').removeClass('speak-open');
        $('.formrow').removeClass('tips-open');
    });


    /* =========== Form validation | START =========== */
    $('form').attr('novalidate', '');
    const config = {
        formFloating: '.form-floating',
        errorHtmClass: '.custom-field-validation-error',
        errorClassname: 'error',
        errorHtm: function (name) {
            return `<span class="custom-field-validation-error text-danger">Please ${name.toLowerCase().indexOf('upload') > -1 ? '' : 'enter valid'} ${name}</span>`
        },
        errorHtmSelect: function (name) {
            return `<span class="custom-field-validation-error text-danger">Please select ${name}</span>`
        },
        errorHtmUpload: function (name) {
            return `<span class="custom-field-validation-error text-danger">Please ${name}</span>`
        }
    }
    function errorCommon(ele) {
        var customVal = $(ele).val();
        if (customVal === '' || customVal === '-1') {
            $(ele).addClass(config.errorClassname);
            if ($(ele).parent(config.formFloating).find(config.errorHtmClass).length === 0) {
                var name = $(ele).parent().find('label').text().toLowerCase();
                // $(ele).parent(config.formFloating).append(config.errorHtm(name));
                if (($(ele).attr('type') && $(ele).attr('type').match(/text|number/)) || ($(ele).prop('tagName') === 'TEXTAREA')) {
                    // textfield
                    $(ele).parent(config.formFloating).append(config.errorHtm(name));
                }
                if ($(ele).prop('tagName') === 'SELECT') {
                    // selectbox
                    $(ele).parent(config.formFloating).append(config.errorHtmSelect(name));
                }
            }
        } else {
            $(ele).removeClass(config.errorClassname);
            $(ele).parent(config.formFloating).find(config.errorHtmClass).remove();
        }
    }
    function errorFileupload(ele) {
        var customVal = $(ele).val();
        if (customVal === '') {
            $(ele).parent('.inputfilebox').addClass(config.errorClassname);
            if ($(ele).closest('.formrow').find(config.errorHtmClass).length === 0) {
                var name = $(ele).parent().find('label').text().toLowerCase();
                // fileupload
                $(ele).closest('.formrow').append(config.errorHtmUpload(name));
            }
        } else {
            $(ele).parent('.inputfilebox').removeClass(config.errorClassname);
            $(ele).closest('.formrow').find(config.errorHtmClass).remove();
        }
    }
    function errorEmail(ele) {
        var customVal = $(ele).val();
        var emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (customVal.match(emailRegex)) {
            $(ele).removeClass(config.errorClassname);
            $(ele).parent(config.formFloating).find(config.errorHtmClass).remove();
        } else {
            $(ele).addClass(config.errorClassname);
            if ($(ele).parent(config.formFloating).find(config.errorHtmClass).length === 0) {
                var name = $(ele).parent().find('label').text().toLowerCase();
                $(ele).parent(config.formFloating).append(config.errorHtm(name));
            }
        }
    }
    function errorCheckbox(ele) {
        if ($(ele).is(':checked') === false) {
            $(ele).addClass(config.errorClassname);
        } else {
            $(ele).removeClass(config.errorClassname);
        }
    }
    function clearIcon_addHtm(ele) {
        var fieldType = $(ele).attr('type');
        if (fieldType !== undefined) {
            var textField = fieldType.match(/text|email|number/);
            var iconNotExist = $(ele).parent('.form-floating').find('.clearIcon').length === 0;
            if (textField && iconNotExist) {
                var clearIconHtm = '<div style="display: none;" class="clearIcon" role="presentation"><svg viewBox="0 0 16 16"><path d="M11.7789771,3.15363708 C12.0185699,2.9140443 12.4474409,2.95956693 12.743937,3.25606299 C13.0404331,3.55255905 13.0859557,3.98143013 12.8463629,4.2210229 L12.8463629,4.2210229 L9.068,8 L12.8463629,11.7789771 C13.0619964,11.9946106 13.0466864,12.3635595 12.8253225,12.6513386 L12.743937,12.743937 C12.4474409,13.0404331 12.0185699,13.0859557 11.7789771,12.8463629 L11.7789771,12.8463629 L8,9.068 L4.2210229,12.8463629 C4.0053894,13.0619964 3.63644049,13.0466864 3.34866141,12.8253225 L3.25606299,12.743937 C2.95956693,12.4474409 2.9140443,12.0185699 3.15363708,11.7789771 L3.15363708,11.7789771 L6.932,8 L3.15363708,4.2210229 C2.93800358,4.0053894 2.95331356,3.63644049 3.17467752,3.34866141 L3.25606299,3.25606299 C3.55255905,2.95956693 3.98143013,2.9140443 4.2210229,3.15363708 L4.2210229,3.15363708 L8,6.932 Z" id="Combined-Shape"></path></svg></div>';
                $(ele).before(clearIconHtm);
            }
        }
    }
    function clearIcon_hideShow_on_focus_and_typing(ele) {
        var emptyField = $(ele).val() === '';
        if (emptyField) {
            $(ele).parent(config.formFloating).find('.clearIcon').hide();
        } else {
            $(ele).parent(config.formFloating).find('.clearIcon').show();
        }
    }
    function addErrorHtm_onClearIconClick(ele) {
        $(ele).next('.form-control').val('');
        $(ele).next('.form-control').addClass(config.errorClassname);
        if ($(ele).parent(config.formFloating).find(config.errorHtmClass).length === 0) {
            var name = $(ele).parent().find('label').text().toLowerCase();
            $(ele).parent(config.formFloating).append(config.errorHtm(name));
        }
    }
    function addErrorHtm_fileUpload_onClearIconClick(ele) {
        $(ele).closest('.formrow').find('.form-control').val('');
        $(ele).closest('.formrow').find('.inputfilebox').addClass(config.errorClassname);
        if ($(ele).closest('.formrow').find(config.errorHtmClass).length === 0) {
            var name = $(ele).closest('.formrow').find('label').text().toLowerCase();
            $(ele).closest('.formrow').append(config.errorHtm(name));
        }
    }
    $('form').submit(function (e) {
        e.preventDefault();
        // let noValidation=false;
        // if($(this).hasClass('noCommonValidation'))
        //     noValidation=true;
        // if(!noValidation)
        // {
        $('form .form-control').each(function () {
            // text
            if ($(this).attr('type') && $(this).attr('type').match(/text|number/)) {
                errorCommon(this);
            }
            // textarea
            if ($(this).prop('tagName') === 'TEXTAREA') {
                errorCommon(this);
            }
            // email
            if ($(this).attr('type') === 'email') {
                errorEmail(this);
            }
            // file
            if ($(this).attr('type') === 'file') {
                errorFileupload(this);
            }
        });
        // select
        $('form .form-select').each(function () {
            if ($(this).prop('tagName') === 'SELECT') {
                errorCommon(this);
            }
        });
        // checkbox
        $('form #check-consent').each(function () {
            errorCheckbox(this);
        });
        // scroll to first error field
        $('form .form-control, form .form-select').each(function () {
            if ($(this).closest('.nominee').length === 0) {
                if ($(this).val() === '' || (($(this).val() === '-1' || $(this).val() === '') && $(this).prop('tagName') === 'SELECT')) {
                    var getTop = $(this).offset().top;
                    var headerHeight = $('.header').outerHeight();
                    var finalScroll = getTop - headerHeight - 20;   
                    $('html, body').animate({ scrollTop: finalScroll });
                    return false
                }
            }            
        });
        var waitThankyou = setInterval(function() {
            if ($('.active-popup .thankyou-popup').length > 0) {
              clearInterval(waitThankyou);
              $('.uploaded-file').hide();
              $('.uploaded-file span').html('');
              $('input[type=checkbox]').prop('checked', false);
              document.querySelector("form").reset();
            }
            setTimeout(function() {
              clearInterval(waitThankyou);
            }, 10000);
          }, 100);
        // }
    });
    // add clear icon html in input field
    $('form .form-control').each(function () {
        clearIcon_addHtm(this);
    });
    // input field focus
    $('form .form-control').focus(function () {
        clearIcon_hideShow_on_focus_and_typing(this);
    });
    // input field oninput, keyup
    $('form .form-control').on('input keyup keypress', function (event) {
        clearIcon_hideShow_on_focus_and_typing(this);
        // text
        if ($(this).attr('type') && $(this).attr('type').match(/text|number/)) {
            errorCommon(this);
        }
        // textarea
        if ($(this).prop('tagName') === 'TEXTAREA') {
            errorCommon(this);
        }
        // email
        if ($(this).attr('type') === 'email') {
            errorEmail(this);
        }
        // alphabet only
        if ($(this).hasClass('alphaOnly')) {
            var pattern = /[`!₹@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~\d]*$/g;
            if (pattern.test(this.value)) {
                this.value = this.value.replace(pattern, '');
            }
            /* var pattern = /^[A-Za-z\s]*$/;
            var value = this.value;
            !pattern.test(value) && (this.value = value = '');
            var currentValue = this.value;
            if(currentValue && !pattern.test(currentValue)) this.value = value;
            else value = currentValue; */
            /* var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            } */
        }
        // first digit should be 7,8,9
        if ($(this).hasClass('numOnly')) {
            var pattern = /^[6-9][0-9]{0,10}$/;
            var value = this.value;
            !pattern.test(value) && (this.value = value = '');
            var currentValue = this.value;
            if(currentValue && !pattern.test(currentValue)) this.value = value;
            else value = currentValue;
        }
    });

    // input field blur
    $('form .form-control').blur(function () {
        var $this = $(this)
        setTimeout(function () {
            $this.parent(config.formFloating).find('.clearIcon').hide();
        }, 200);
    });
    // Select box change function
    $('form .form-select').change(function () {
        errorCommon(this);
    });
    // checkbox change function
    // $('form .form-check-input').on('input', function () {
    //     errorCheckbox(this);
    // });
    $('form #check-consent').on('input', function () {
        errorCheckbox(this);
    });
    // add html to show uploaded file name
    $('.inputfilebox').each(function () {
        if ($(this).find('.uploaded-file').length === 0) {
            var clearIconHtm = '<div class="clearIcon" role="presentation"><svg viewBox="0 0 16 16"><path d="M11.7789771,3.15363708 C12.0185699,2.9140443 12.4474409,2.95956693 12.743937,3.25606299 C13.0404331,3.55255905 13.0859557,3.98143013 12.8463629,4.2210229 L12.8463629,4.2210229 L9.068,8 L12.8463629,11.7789771 C13.0619964,11.9946106 13.0466864,12.3635595 12.8253225,12.6513386 L12.743937,12.743937 C12.4474409,13.0404331 12.0185699,13.0859557 11.7789771,12.8463629 L11.7789771,12.8463629 L8,9.068 L4.2210229,12.8463629 C4.0053894,13.0619964 3.63644049,13.0466864 3.34866141,12.8253225 L3.25606299,12.743937 C2.95956693,12.4474409 2.9140443,12.0185699 3.15363708,11.7789771 L3.15363708,11.7789771 L6.932,8 L3.15363708,4.2210229 C2.93800358,4.0053894 2.95331356,3.63644049 3.17467752,3.34866141 L3.25606299,3.25606299 C3.55255905,2.95956693 3.98143013,2.9140443 4.2210229,3.15363708 L4.2210229,3.15363708 L8,6.932 Z" id="Combined-Shape"></path></svg></div>';
            var uploadedFileHtm = `<div style="display: none;" class="uploaded-file"><div class="uploaded-file-inn"><span></span>${clearIconHtm}</div></div>`;
            $(this).append(uploadedFileHtm);
        }
    });
    // file upload field change function
    $('.inputfilebox input[type="file"]').change(function (e) {
        /* var fileVal = e.target.files[0].name;
        $(this).next('.uploaded-file').find('span').text(fileVal);
        $(this).next('.uploaded-file').show();
        errorFileupload(this); */
        // var id = $(this).attr("id").toLowerCase();
        var fileVal = e.target.files[0].name;
        var checkPdf = $(this).attr('accept').indexOf('pdf') > -1 && fileVal.split('.').pop().match(/pdf/);
        var checkImage = $(this).attr('accept').indexOf('image') > -1 && fileVal.split('.').pop().match(/jpg|jpeg|png|gif/);
        var wrongPdf = $(this).attr('accept').indexOf('pdf') > -1 && fileVal.indexOf('.pdf') === -1;
        var wrongImage = $(this).attr('accept').indexOf('image') > -1 && fileVal.split('.').pop().match(/jpg|jpeg|png|gif/) === null;
        var fsize = e.target.files[0].size;
        fsize = Math.round(Math.max((fsize / 1024)));
        var checksize = fsize <= 10240
        if ((checkPdf && checksize) || (checkImage && checksize) ) {
            $(this).next('.uploaded-file').find('span').text(fileVal);
            $(this).next('.uploaded-file').show();
        } else {
            $(this).val('');
        }
        if (wrongPdf) {
            alert('Please select correct file format, Valid file format is PDF');
        }
        else if (wrongImage) {
            alert('Please select correct file format, Valid file formats are JPG, JPEG, PNG, GIF');
        }
        else if (!checksize) {
            alert('File size should be less than 10 MB');
           
        }





        errorFileupload(this);
    });
    // clear icon click | text field
    $('.form-floating .clearIcon').click(function () {
        $(this).next('.form-control').val('');
        $(this).hide();
        addErrorHtm_onClearIconClick(this);
    });
    // clear icon click | upload field
    $('.inputfilebox .clearIcon').click(function () {
        $(this).closest('.inputfilebox').find('.form-control').val('');
        $(this).closest('.uploaded-file').hide();
        addErrorHtm_fileUpload_onClearIconClick(this);
    });
    // mobile number field change function : numeric value only
    /* $('.numOnly').on('input', function () {
      if (/\D/g.test(this.value)) {
        this.value = this.value.replace(/\D/g, '');
      }
    }); */

    // Help page | search field cross icon 
    $('.input-search .clearIcon').click(function () {
        $(this).closest('.input-search').find('.form-control').val('');
        $(this).hide();
        window.location.reload();
    });
    $('.input-search .form-control').on('input focus', function () {
        if ($(this).val() !== '') {
            $(this).closest('.input-search').find('.clearIcon').show();
        } else {
            $(this).closest('.input-search').find('.clearIcon').hide();
        }
    });
    $('.input-search .form-control').on('blur', function () {
        var $this = $(this);
        setTimeout(function () {
            $this.closest('.input-search').find('.clearIcon').hide();
        }, 200);
    });
    /* =========== Form validation | END =========== */



    //video code | START

    $('.video-thumb-item').click(function () {
        var vidUrl = $(this).attr('vidSrc');
        $('.video-main iframe').attr('src', vidUrl + '?autoplay=1');

        var vidTitle = $(this).attr('title');
        $('.video-main-content h3').text(vidTitle);

        var vidDesc = $(this).attr('desc');
        $('.video-main-content p').text(vidDesc);
    });

    // video code | END

    $('footer').after('<div class="backToTop" onclick="scrollToTop()" style="display: none;"><div class="inner"><i class="i-arrow-u"></i><span>Back to Top</span></div></div>');
    var value1 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
    $('.header-nav .menu-section>ul li a').each(function () {
        var url = $(this).attr('href'); var lastSegment = url.split('/').pop();
        if (lastSegment == value1) {
            $(this).parent().addClass('active-link');
        }
    });


    var value2 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1); 
    $('.sidebar-navigation .hamburger_section.single_item .submenu>li>a').each(function(){ 
        var url2 = $(this).attr('href'); 
        var lastSegment2 = url2.split('/').pop(); 
        if (lastSegment2 == value2) { 
            $(this).parent().addClass('active');
        } 
    }); 
    var value3 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1); 
    $('.green-action-bar .item>a').each(function(){ 
        var url3 = $(this).attr('href'); 
        var lastSegment3 = url3.split('/').pop(); 
        if (lastSegment3 == value3) { 
            $(this).parent().addClass('remove-active');
        } 
    }); 


    $('.f-nav ul .f-nav-column>a').click(function () {
        // $(this).closest('.f-nav-column').find('ul').slideUp(250);
        $(this).parent('li').siblings('li').find('ul').slideUp(250);
        // $(this).closest('.f-nav-column').find('a').removeClass('active');
        $(this).parent('li').siblings('li').find('a.active').removeClass('active');
        $(this).next('ul').slideToggle(250);
        $(this).toggleClass('active');
    });

    $('.about-intro1 .about-btn').click(function () {
        $(this).closest('.about-intro1').toggleClass('desc-open');
    });
    $('.member-info .about-btn').click(function () {
        $(this).closest('.member-info').toggleClass('desc-open');
    });
    $('.custom-contribute-banner+ .content-form .form-wrapper .about-btn').click(function () {
        $(this).closest('.form-wrapper').toggleClass('desc-open');
    });

    $('.about-btn').each(function () {
        $(this).closest('.problem-desc').find('p,ul').wrapAll('<div class="custom-div"></div>');
        //var totalChar = $(this).closest('.problem-desc').find('.heading').text().trim().length;
        //var headingChar = $(this).closest('.problem-desc').find('.heading h2').text().trim().length;
        var finalChar = $(this).prev('.custom-div').text().trim().length;
        var pheight = $(this).closest('.problem-desc').find('p').height();
        var pChar = $(this).closest('.problem-desc').find('p').text().trim().length;
        //var finalChar = totalChar - headingChar;
        /* if (window.innerWidth > 600 && (finalChar <= 240 || (finalChar > 240 && pheight <= 72))){
          // desk
          $(this).hide();
          if ($(this).closest('.problem-desc').find('p').height() <= 72) {
            $(this).closest('.problem-desc').find('.custom-div').addClass('showUL');
          }
        } */
        var ulHeight = $(this).closest('.custom-div').find('ul').height();

        if (window.innerWidth > 600 && (pheight === 24 || pheight === 48)) {
            if (ulHeight <= 48) {
                $(this).hide();
            }
            else {
                $(this).closest('.problem-desc').find('.custom-div').addClass('p-with-ul')
                $('.about-btn').click(function () {
                    $(this).closest('.story-problemstatement').find('.custom-div').toggleClass('p-with-ul');
                });
            }
        }
        if (window.innerWidth > 600 && finalChar <= 240) {
            // desk
            $(this).hide();
            if ($(this).closest('.problem-desc').find('p').height() <= 72) {
                $(this).closest('.problem-desc').find('.custom-div').addClass('showUL');
            }
        }
        if (window.innerWidth > 600 && pChar > 240 && pheight <= 72) {
            // desk
            $(this).hide();
        }
        if (window.innerWidth <= 600 && finalChar < 147) {
            // mob
            $(this).hide();
            if ($(this).closest('.problem-desc').find('p').height() < 42) {
                $(this).closest('.problem-desc').find('.custom-div').addClass('showUL');
            }
        }

        if ($(this).closest('.problem-desc').find('p').length === 0) {
            $(this).closest('.problem-desc').find('.custom-div').addClass('onlyUL');
        }
    });

    $('.loader').wrapInner('<div class="cm-load-wrap"></div>');
    var restricted = [46];
    $('.numOnly').keypress(function (event) {
        if (restricted.indexOf(event.which) !== -1) {
            event.preventDefault();
        }
    });

});


const toggleVisible = () => {
    const scrolled = document.body.scrollTop || document.documentElement.scrollTop;
    const classdata = document.getElementsByClassName("backToTop")[0];

    footerElement = document.querySelector("footer");
    var footerStyle = footerElement.currentStyle || window.getComputedStyle(footerElement);
    footerTopMargin = Number(footerStyle.marginTop.slice(0, - 2));

    footeroffset = footerElement.offsetTop + footerTopMargin + document.querySelector(".footer-bottom").clientHeight - window.innerHeight;

    const {
        innerWidth: width,
        innerHeight: height
    } = window;
    if (scrolled > 300) {
        classdata && (classdata.style.display = "inline-block");
    } else if (scrolled <= 300) {
        classdata && (classdata.style.display = "none");
    }
    if (
        footeroffset < (document.body.scrollTop || document.documentElement.scrollTop)) {
        document.getElementsByClassName("backToTop")[0]?.classList.add("active");
    } else {
        document.getElementsByClassName("backToTop")[0]?.classList.remove("active");
    }
};

const scrollToTop = () => {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    });
};
window.addEventListener("scroll", toggleVisible);

$('.goals-list').on('scroll', function () {
    var scroll = $(this).scrollTop();

    //>=, not <=
    if (scroll >= 60) {
        //clearHeader, not clearheader - caps H
        $(".goals-modal").addClass("active");
    } else {
        $(".goals-modal").removeClass("active");
    }
});

window.textarea_height = () => {
    var textareaEle = document.querySelectorAll("textarea.form-control");
    function autoHeight(ele) {
        ele.style.minHeight = ele.scrollHeight + "px";
    }
    if (textareaEle.length > 0) {
        [].forEach.call(textareaEle, (ele) => {
            ele.addEventListener("input", function () {
                if (ele.scrollHeight <= 120 || ele.value.trim() === "" || ele.value.length <= 180) {
                    ele.style.minHeight = "120px";
                } else {
                    autoHeight(ele);
                }
            });
            ele.addEventListener("focusout", function () {
                if (ele.value.trim() === "") {
                    ele.style.minHeight = "120px";
                } else {
                    autoHeight(ele);
                }
            });
        });
    }
}
textarea_height();
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
// Adding this for whole page dated 7/13/2023 by Yashovardhan

const subMenuBtn = document.querySelector(".sub-menu-btn");

subMenuBtn.addEventListener("click", function () {

  this.classList.toggle("clicked");

});

// Carousal

        $('.banner-slider').slick({
            dots: true,
            infinite: true,
            loop: true,
            autoplay: false,
            arrows: false,
            autoplaySpeed: 1000,
            fade: true,
            speed: 500,
            cssEase: 'ease-in-out',
            touchThreshold: 100,
            slidesToShow: 1,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true,
                        autoplay: false,
                        speed: 300
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });

        $('.slider-capability').slick({
            dots: false,
            infinite: true,
            loop: true,
            autoplay: false,
            arrows: true,
            speed: 300,
            slidesToShow: 3,
            slidesToScroll: 1,
            prevArrow: '<div class="slider-arrow slider-prev fa fa-angle-left"></div>',
            nextArrow: '<div class="slider-arrow slider-next fa fa-angle-right"></div>',
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true,
                        autoplay: false,
                        speed: 300
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
 
        $('.slider-partner').slick({
            dots: true,
            infinite: true,
            loop: true,
            autoplay: false,
            arrows: false,
            speed: 300,
            slidesToShow: 4,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true,
                        autoplay: false,
                        speed: 300
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });

        $('.csr-slider').slick({
            dots: true,
            infinite: true,
            loop: true,
            autoplay: false,
            arrows: false,
            speed: 300,
            slidesToShow: 1,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true,
                        autoplay: false,
                        speed: 300
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
		
		
		        function myFunction() {
            var x = document.getElementById("myDIV");
            if (x.style.display === "block") {
                x.style.display = "none";
            } else {
                x.style.display = "block";
            }
        }

        $(function () {
            $('.ArrowScrollDown').on('click', function (e) {
                e.preventDefault();
                $('html, body').animate({ scrollTop: $($(this).toggleClass('.ArrowScrollDown')).offset().top }, 600, 'linear');
            });
        });

        $('.scroll_down a').on('click', function (e) {
            var href = $(this).attr('href');
            $('html, body').animate({
                scrollTop: $(href).offset().top - 70
            }, '600');
            e.preventDefault();
        });

        // Hide Header on scroll down
        var didScroll;
        var lastScrollTop = 0;
        var delta = 0;
        var navbarHeight = 0;

        $(window).scroll(function (event) {
            didScroll = true;
        });

        setInterval(function () {
            if (didScroll) {
                hasScrolled();
                didScroll = false;
            }
        }, 250);
        function hasScrolled() {
            var st = $(this).scrollTop();

            // Make sure they scroll more than delta
            if (Math.abs(lastScrollTop - st) <= delta)
                return;

            // If they scrolled down and are past the navbar, add class .nav-up.
            // This is necessary so you never see what is "behind" the navbar.
            $('#back-to-top').fadeIn();
            $('header.main_header').addClass('fixed-header');
            $('header.inner_header').addClass('box-shadow');
            $('header.main_header').removeClass('headerSec');
            if (st > lastScrollTop && st > navbarHeight) {
                // Scroll Down
                //$('.btm-floating').addClass('active');

            } else {
                // Scroll Up
                if (st + $(window).height() < $(document).height()) {

                }
            }
            if (st < 150) {

                $('#back-to-top').hide();
                $('header.main_header').removeClass('fixed-header');
                $('header.main_header').addClass('headerSec');
                $('header.inner_header').removeClass('box-shadow');
                //$('.btm-floating').removeClass('active');
            }
            lastScrollTop = st;
        }


        let mybutton = document.getElementById("btn-back-to-top");

        window.onscroll = function () {
            scrollFunction();
        };

        function scrollFunction() {
            if (
                document.body.scrollTop > 100 ||
                document.documentElement.scrollTop > 1000
            ) {
                mybutton.style.display = "block";
            } else {
                mybutton.style.display = "none";
            }
        }

        mybutton.addEventListener("click", backToTop);

        function backToTop() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }

        $("a.explore--open").click(function () {
            if ($(".news-box").hasClass("show-more-height")) {
                $(this).text("Explore Less");
            } else {
                $(this).text("Explore More");
            }

            $(".news-box").toggleClass("show-more-height");
        });
		
		
		// let sliderContainer = document.querySelector(".slider-container");
        // let innerSlider = document.querySelector(".inner-slider");

        // let pressed = false;
        // let startX;
        // let x;
        // sliderContainer.addEventListener("mousedown", (e) => {
            // pressed = true;
            // startX = e.offsetX - innerSlider.offsetLeft;
            // sliderContainer.style.cursor = "grabbing";
        // });
        // sliderContainer.addEventListener("mouseenter", () => {
            // sliderContainer.style.cursor = "grab";
        // });
        // sliderContainer.addEventListener("mouseup", () => {
            // sliderContainer.style.cursor = "grab";
            // pressed = false;
        // });
        // sliderContainer.addEventListener("mousemove", (e) => {
            // if (!pressed) return;
            // e.preventDefault();

            // x = e.offsetX;

            // innerSlider.style.left = `${x - startX}px`;
        // });
		
		
		
		        // let sliderContainer = document.querySelector(".slider-container");
        // let innerSlider = document.querySelector(".inner-slider");

        // let pressed = false;
        // let startX;
        // let x;
        // sliderContainer.addEventListener("mousedown", (e) => {
            // pressed = true;
            // startX = e.offsetX - innerSlider.offsetLeft;
            // sliderContainer.style.cursor = "grabbing";
        // });
        // sliderContainer.addEventListener("mouseenter", () => {
            // sliderContainer.style.cursor = "grab";
        // });
        // sliderContainer.addEventListener("mouseup", () => {
            // sliderContainer.style.cursor = "grab";
            // pressed = false;
        // });
        // sliderContainer.addEventListener("mousemove", (e) => {
            // if (!pressed) return;
            // e.preventDefault();

            // x = e.offsetX;

            // innerSlider.style.left = `${x - startX}px`;
        // });
		
		$("#searchright").change(function (e) {
            var searchstring = $("#searchright").val();
            if (searchstring != "" && searchstring.length > 2) {
                window.location.href = "/DefenceSearch?s=" + searchstring;
            }
            else {
                alert("Please enter more than 3 characters");
            }



        });

        $("#searchright1").click(function (e) {
            var searchstring = $("#searchbox").val();
            if (searchstring != "" && searchstring.length > 2) {
                window.location.href = "/DefenceSearch?s=" + searchstring;
            }
            else {
                alert("Please enter more than 3 characters");
            }



        });
        $("#SearchLoad").click(function () {
            var lastIndex = parseInt($("#hidden").val());
            if (parseInt(localStorage.getItem('clickCount')) > 0) {
                $("#hidden").val(parseInt(lastIndex + 5));
            }
            LoadmoreInsights(0);
        });



        function LoadmoreInsights(hasCount) {
            var lastIndex = parseInt($("#hidden").val());
            $.ajax({
                type: "GET",
                url: "/api/sitecore/Defence/DefenceSearchLoadMore",
                contentType: "application/html; charset=utf-8",
                data: { s: location.search.split("=")[1], lastIndex: lastIndex },
                dataType: "html",
                success: function (result) {
                    $('#2023').append(result);
                    if (hasCount <= 0) {
                        if (parseInt(localStorage.getItem('clickCount')) >= 1) {
                            localStorage.setItem('clickCount', parseInt(localStorage.getItem('clickCount')) + 1);
                        }
                        else {
                            localStorage.setItem('clickCount', 1);
                        }
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }


            