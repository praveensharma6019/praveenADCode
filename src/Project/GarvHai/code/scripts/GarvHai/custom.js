$(document).ready(function () {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        dots:true,
        responsiveClass: true,
        navText: [
            '<i class="fa fa-arrow-left" aria-hidden="true"></i>',
            '<i class="fa fa-arrow-right" aria-hidden="true"></i>'
        ],
        autoplay: true,
        autoplaytimeout: 2000,
        slideSpeed: 1500,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });

    $('.mobile-menu-button span i').click(function () {
        if ($('.site-navigation.mobile-menu >div>ul').hasClass("mobileNav")) {
            $('.site-navigation.mobile-menu >div>ul').removeClass("mobileNav");
            $('.menu-primary-container').hide();
        }
        else {
            $('.site-navigation.mobile-menu >div>ul').addClass("mobileNav");
            $('.menu-primary-container').show();
        }
    });


    /*Scroll*/
    $(function () {
        $('.menucomm [href*=\\#]').on('click', function (e) {
            e.preventDefault();
            $('html, body').animate({ scrollTop: $($(this).attr('href')).offset().top }, 500, 'linear');
        });
    });



    $('.PersonalDetailsSection, .ParentsDetailsSection, .sportsBackgroundSection').removeClass("show");



    $(".BasicDetailsSectionHeader").on('click', function (event) {
        $('.PersonalDetailsSection').slideUp();
        $('.BasicDetailsSection').slideToggle("fast");
        $('.ParentsDetailsSection').slideUp();
        $('.sportsBackgroundSection').slideUp();
    });


    $(".btnNextBasicDetail, .PersonalDetailsSectionHeader").on('click', function (event) {
        $('.PersonalDetailsSection').slideToggle("fast");
        $('.BasicDetailsSection').slideUp();
        $('.ParentsDetailsSection').slideUp();
        $('.sportsBackgroundSection').slideUp();
    });

    $(".btnNextPersonalDetails, .ParentsDetailsSectionHeader").on('click', function (event) {
        $('.PersonalDetailsSection').slideUp();
        $('.BasicDetailsSection').slideUp();
        $('.ParentsDetailsSection').slideToggle("fast");
        $('.sportsBackgroundSection').slideUp();
    });


    $(".btnNextParentsDetails, .sportsBackgroundSectionHeader").on('click', function (event) {
        $('.PersonalDetailsSection').slideUp();
        $('.BasicDetailsSection').slideUp();
        $('.ParentsDetailsSection').slideUp();
        $('.sportsBackgroundSection').slideToggle("fast");
    });

    $('.contactusFormLastContent').html('<p class="email-txt text-center"> write us at <a href="mailto:communication@adani.com">communication@adani.com</a> </p> <p class="abt-social-txt text-uppercase text-center">follow us on</p> <div class="heros-social-links text-center"> <ul class="list-inline"> <li><a href="https://www.facebook.com/AdaniOnline/" class="social-icon-top" target="_blank"><img src="http://10.0.201.23/garvhai/assets/img/fb-o.png"></a></li> <li><a href="https://twitter.com/AdaniOnline" class="social-icon-top" target="_blank"><img src="http://10.0.201.23/garvhai/assets/img/tw-o.png"></a></li> </ul> </div>')


    $('.aboutus').attr('href', '/#aboutus');
    $('.contactus').attr('href', '/#contactus');
    $('.media').attr('href', '/#media');

    

});