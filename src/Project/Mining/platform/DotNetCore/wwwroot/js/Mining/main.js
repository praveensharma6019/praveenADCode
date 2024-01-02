$(document).ready(function () {

    $('.mobile-trigger').on('click', function (e) {
        e.stopPropagation();

        $('body').toggleClass('menu-open');
        $('html').toggleClass('cm-menu-open');

    });

    $('body').on('click', function (ele) {

        $(this).removeClass('menu-open');

        $('html').removeClass('cm-menu-open');

    });

    $('.sidebar-navigation').on('click', function (e) {
        e.stopPropagation();
    });

    headerHeight = document.querySelector("header").clientHeight;
    window.addEventListener("scroll", function () {
        const header = document.querySelector("header"),
            scroll = window.pageYOffset | document.body.scrollTop;

        if (scroll > headerHeight) {
            header.className = "header sticky";
        } else if (scroll <= headerHeight) {
            header.className = "header transparent";
        }
    });

    var header_height = $("header").outerHeight();
    var stickyBarHeight = $(".details_top_nav").outerHeight();

    $(".details_top_nav").css("top", header_height);
    $(window).on("resize scroll", function () {
        var header_height2 = $("header").outerHeight();
        $(".details_top_nav").css("top", header_height2);
    });

    $(".details_top_nav ul li a").click(function () {
        var target = $(this).attr("href");

        var customScrollTop =
            $(target).offset().top - (header_height + stickyBarHeight - 30);
        scrollTo(0, customScrollTop);
        return false;
    });


    const aboutDescription = $('.mob-para');
    const aboutreadMoreBut = $('.read-more');

    aboutreadMoreBut.on('click', function () {
        aboutDescription.toggleClass('expanded');
        aboutreadMoreBut.text(aboutDescription.hasClass('expanded') ? 'Read Less' : 'Read More');
    });


    const whyMTCSDesc = $('.mob-para-1');
    const whyMTCSreadMoreBut = $('.read-more-1');

    whyMTCSreadMoreBut.on('click', function () {
        whyMTCSDesc.toggleClass('expanded-1');
        whyMTCSreadMoreBut.text(whyMTCSDesc.hasClass('expanded-1') ? 'Read Less' : 'Read More');
    });

    const accDesc = $('.mob-para-2');
    const accreadMoreBut = $('.read-more-2');

    accreadMoreBut.on('click', function () {
        accDesc.toggleClass('expanded-2');
        accreadMoreBut.text(accDesc.hasClass('expanded-2') ? 'Read Less' : 'Read More');
    });

    const leaderDesc = $('.mob-para-3');
    const leaderreadMoreBut = $('.read-more-3');

    leaderreadMoreBut.on('click', function () {
        leaderDesc.toggleClass('expanded-3');
        leaderreadMoreBut.text(leaderDesc.hasClass('expanded-3') ? 'Read Less' : 'Read More');
    });

    const mtcsProzDesc = $('.mob-para-4');
    const mtcsProzreadMoreBut = $('.read-more-4');

    mtcsProzreadMoreBut.on('click', function () {
        mtcsProzDesc.toggleClass('expanded-4');
        mtcsProzreadMoreBut.text(mtcsProzDesc.hasClass('expanded-4') ? 'Read Less' : 'Read More');
    });

    const oritServDesc = $('.mob-para-5');
    const oritServreadMoreBut = $('.read-more-5');

    oritServreadMoreBut.on('click', function () {
        oritServDesc.toggleClass('expanded-5');
        oritServreadMoreBut.text(oritServDesc.hasClass('expanded-5') ? 'Read Less' : 'Read More');
    });

    const closePlanDesc = $('.mob-para-6');
    const closePlanreadMoreBut = $('.read-more-6');

    closePlanreadMoreBut.on('click', function () {
        closePlanDesc.toggleClass('expanded-6');
        closePlanreadMoreBut.text(closePlanDesc.hasClass('expanded-6') ? 'Read Less' : 'Read More');
    });

    const mtcsOtherDesc = $('.mob-para-7');
    const mtcsOtherreadMoreBut = $('.read-more-7');

    mtcsOtherreadMoreBut.on('click', function () {
        mtcsOtherDesc.toggleClass('expanded-7');
        mtcsOtherreadMoreBut.text(mtcsOtherDesc.hasClass('expanded-7') ? 'Read Less' : 'Read More');
    });

    $("a.explore--open-1").click(function () {
        if ($(".news-box-1").hasClass("show-more-height-1")) {
            $(this).text("View Less");
        } else {
            $(this).text("View More");
        }

        $(".news-box-1").toggleClass("show-more-height-1");
    });


    $('.f-nav ul .f-nav-column>a').click(function () {
        // $(this).closest('.f-nav-column').find('ul').slideUp(250);
        $(this).parent('li').siblings('li').find('ul').slideUp(250);
        // $(this).closest('.f-nav-column').find('a').removeClass('active');
        $(this).parent('li').siblings('li').find('a.active').removeClass('active');
        $(this).next('ul').slideToggle(250);
        $(this).toggleClass('active');
    });

    var value1 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
    $('.header-nav .menu-section>ul li a').each(function () {
        var url = $(this).attr('href'); var lastSegment = url?.split('/').pop();
        if (lastSegment === value1 && lastSegment != "") {
            $(this).parent().addClass('active-link');
        }
    });

    $('.countryCode').on('click', function () {
        $('#countryid').toggleClass('d-block');
    })


    // $('footer').after('<div class="backToTop" onclick="scrollToTop()" style="display: none;">< div class= "inner" ><i class="fa fa-arrow-up"></i><span>Back to Top</span></div ></div > ');
    // var value1 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
    // $('.header-nav .menu-section>ul li a').each(function () {
    //     var url = $(this).attr('href');
    //     var lastSegment = url?.split('/').pop();
    //     if (lastSegment == value1) {
    //         $(this).parent().addClass('active-link');
    //     }
    // });


    // const toggleVisible = () => {
    //     const scrolled = document.body.scrollTop || document.documentElement.scrollTop;
    //     const classdata = document.getElementsByClassName("backToTop")[0];

    //     footerElement = document.querySelector("footer");
    //     var footerStyle = footerElement.currentStyle || window.getComputedStyle(footerElement);
    //     footerTopMargin = Number(footerStyle.marginTop.slice(0, - 2));

    //     footeroffset = footerElement.offsetTop + footerTopMargin + document.querySelector(".footer-bottom").clientHeight - window.innerHeight;

    //     const {
    //         innerWidth: width,
    //         innerHeight: height
    //     } = window;
    //     if (scrolled > 300) {
    //         classdata && (classdata.style.display = "inline-block");
    //     } else if (scrolled <= 300) {
    //         classdata && (classdata.style.display = "none");
    //     }
    //     if (
    //         footeroffset < (document.body.scrollTop || document.documentElement.scrollTop)) {
    //         document.getElementsByClassName("backToTop")[0]?.classList.add("active");
    //     } else {
    //         document.getElementsByClassName("backToTop")[0]?.classList.remove("active");
    //     }
    // };

    const scrollToTop = () => {
        window.scrollTo({
            top: 0,
            behavior: "smooth",
        });
    };
    // window.addEventListener("scroll", toggleVisible);

});


$(".carousel").slick({
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    dots: true,
    autoplay: false,
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

$(".our-service .btm").slick({
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
                // slidesToShow: 1,
                slidesToShow: 1.2,
                slidesToScroll: 1,
                arrows: false
            },
        },
    ],
});


$(".mtcs-projects .btm").slick({
    arrows: true,
    dots: false,
    infinite: false,
    slidesToScroll: 1,
    slidesToShow: 3,
    prevArrow: $(".arr-left-1"),
    nextArrow: $(".arr-right-1"),
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
                // slidesToShow: 1,
                slidesToShow: 1.2,
                slidesToScroll: 1,
                arrows: false
            },
        },
    ],
});

