$(document).ready(function () {
    var owl = $("#groupWeb_carsol");

    owl.owlCarousel({
        items: 6, //10 items above 1000px browser width
        itemsDesktop: [1199, 6],
        itemsDesktopSmall: [979, 5],
        itemsTablet: [800, 4],
        itemsMobile: [640, 2],
        pagination: false,
        navigation: true,
        margin: 10,
        afterAction: funcABC
    });

    var owl = $(".homeBanner_main");

    owl.owlCarousel({
        items: 1, //10 items above 1000px browser width
        itemsDesktop: [1199, 1],
        itemsDesktopSmall: [979, 1],
        itemsTablet: [800, 1],
        itemsMobile: [640, 1],
        pagination: false,
        navigation: false,
        margin: 0,
        afterAction: funcABC
    });


    function changeCountry(val) {

        var activeImageId = "";
        try {
            $(".pinBox").remove();
        } catch (e) { }
        $(".mapImg img").each(function () {
            if ($(this).css("display") == "inline-block") {
                activeImageId = $(this).attr("id");
            }
        });
        $("#" + activeImageId).fadeOut();
        if (val == "-1") {
            setTimeout(function () {

                $("#backimg-worldmap").fadeIn();
                createCorporateOfficePin();
            }, 500);

        } else {
            setTimeout(function () { $("#backimg-" + val).fadeIn(); }, 500);
        }
        getCountryState(val);
        setTimeout(function () {
            makePins(val, "null", "null");
        }, 500);
    }

    function getCountryState(countryName) {
        stateCounter = 0;
        locationCounter = 0;
        if (countryName == -1) {
            $("#stateName").html("<option value='-1'> Select State </option>");
        } else {
            var allStates = "<option value='-1'> Select State </option>";
            for (var state in BusinessTypeState[countryName]) {
                allStates += "<option value='" + state + "'>" + state + "</option>";
                stateCounter++;

                stateVal = state;
            }
        }
        if (stateCounter > 1) {
            console.log("All States : " + allStates);
            $("#stateName").html(allStates);
        } else {
            for (var location in BusinessTypeState[countryName][stateVal]) {
                locationCounter++;
                locationVal = location;
            }


        }
        if (locationCounter == 1 && stateCounter == 1) {
            $("#location").html("<option value='" + locationVal + "'>" + locationVal + "</option>");
            $("#stateName").html("<option value='" + stateVal + "'>" + state + "</option>");

        } else {


            $("#stateName").html(allStates);
            $("#location").html("<option value='-1'> Select Location </option>");
        }


    }
    function businessChange(businessName) {

        console.log("Selected Business : " + businessName);
        var selectedCountry = $("#contactCountry option:selected").val();
        var seletedState = $("#stateName option:selected").val();

        $(".pinBox").css("display", "none");
        $(".pinBox.example-obtuse").removeClass("activeBubble");
        console.log(".pinBox." + selectedCountry + " ." + seletedState + " ." + businessName);
        if (seletedState != -1) {


            if (businessName != -1) {

                $(".pin." + selectedCountry + "." + seletedState + "." + businessName).parent().css("display", "block");
            }
            else {

                $(".pin." + selectedCountry + "." + seletedState).parent().css("display", "block");
            }
        }
        else if (businessName != -1) {
            console.log(".pin ." + selectedCountry + " ." + businessName);
            $(".pin." + selectedCountry + "." + businessName).parent().css("display", "block");
        } else {
            $(".pin." + selectedCountry).parent().css("display", "block");
        }
    }

    function getLocation(businessType, stateName) {

        if (businessType == -1) {
            $("#location").html("<option value='-1'> Select Location</option>");
        } else if (stateName == -1) {
            $("#location").html("<option value='-1'> Select Location</option>");

        } else {
            var allStates = "<option value='-1'> Select Location </option>";
            for (var location in BusinessTypeState[businessType][stateName]) {
                allStates += "<option value='" + location + "'>" + location + "</option>";
            }
            console.log("All States : " + allStates);
            $("#location").html(allStates);
        }

    }
    function stateChange(stateName) {


        var selectedbusiness = $("#contactCountry option:selected").val();
        var selectedState = $("#stateName option:selected").val();

        $(".pinBox").css("display", "none");
        $(".pinBox.example-obtuse").removeClass("activeBubble");

        if (selectedbusiness != -1) {

            if (selectedState != -1) {

                getLocation(selectedbusiness, selectedState);
                if (selectedbusiness.indexOf(" ") > -1) {

                    var tmp;
                    var ar = selectedbusiness.split(" ");
                    tmp = ar[0];
                    for (var count = 1; count < ar.length; count++)
                        tmp += ar[count];
                    selectedbusiness = tmp;
                }
                if (selectedState.indexOf(" ") > -1) {

                    var tmp;
                    var ar = selectedState.split(" ");
                    tmp = ar[0];
                    for (var count = 1; count < ar.length; count++)
                        tmp += ar[count];
                    selectedState = tmp;
                }


                $(".pin." + selectedbusiness + "." + selectedState).parent().css("display", "block");
            }
            else
                $(".pin." + selectedbusiness + "." + selectedState).parent().css("display", "block");
        } else if (stateName == -1 && selectedState == -1) {
            $(".pin." + selectedbusiness).parent().css("display", "block");
        }
        else {
            if (stateName.indexOf(" ") > -1) {
                var tmp;
                var ar = stateName.split(" ");
                tmp = ar[0];
                for (var count = 1; count < ar.length; count++)
                    tmp += ar[count];
                stateName = tmp;
            }
            $(".pin." + selectedbusiness + "." + stateName).parent().css("display", "block");
        }

        if (selectedState == -1) {
            if (selectedbusiness.indexOf(" ") > -1) {
                var tmp;
                var ar = selectedbusiness.split(" ");
                tmp = ar[0];
                for (var count = 1; count < ar.length; count++)
                    tmp += ar[count];
                selectedbusiness = tmp;
            }
            getLocation(selectedbusiness, selectedState);
            $(".pin." + selectedbusiness).parent().css("display", "block");
        }

    }

    function locationChange(stateName) {

        var selectedbusiness = $("#contactCountry option:selected").val();
        var selectedState = $("#stateName option:selected").val();
        var location = $("#location option:selected").val();

        $(".pinBox").css("display", "none");
        $(".pinBox.example-obtuse").removeClass("activeBubble");

        if (selectedbusiness != -1) {

            if (selectedState != -1) {
                if (selectedState.indexOf(" ") > -1) {

                    var tmp;
                    var ar = selectedState.split(" ");
                    tmp = ar[0];
                    for (var count = 1; count < ar.length; count++)
                        tmp += ar[count];
                    selectedState = tmp;
                }


                if (selectedbusiness.indexOf(" ") > -1) {

                    var tmp;
                    var ar = selectedbusiness.split(" ");
                    tmp = ar[0];
                    for (var count = 1; count < ar.length; count++)
                        tmp += ar[count];
                    selectedbusiness = tmp;
                }
                if (location.indexOf(" ") > -1) {

                    var tmp;
                    var ar = location.split(" ");
                    tmp = ar[0];
                    for (var count = 1; count < ar.length; count++)
                        tmp += ar[count];
                    location = tmp;
                }


                $(".pin." + selectedbusiness + "." + selectedState + "." + location).parent().css("display", "block");
                if (location == -1) {
                    $(".pin." + selectedbusiness + "." + selectedState).parent().css("display", "block");
                }
            }
            else {

                $(".pin." + selectedbusiness + "." + selectedState).parent().css("display", "block");
            }
        } else if (state == -1 && selectedState == -1) {

            $(".pin." + selectedbusiness).parent().css("display", "block");
        }
        else {
            if (stateName.indexOf(" ") > -1) {
                var tmp;
                var ar = stateName.split(" ");
                tmp = ar[0];
                for (var count = 1; count < ar.length; count++)
                    tmp += ar[count];
                stateName = tmp;
            }
            $(".pin." + selectedbusiness + "." + stateName).parent().css("display", "block");
        }

    }

    function pinClickBuble(el) {

        if ($(el).parent().find('.example-obtuse').hasClass('activeBubble')) {
            $(el).parent().find('.example-obtuse').removeClass('activeBubble')
            return;
        }

        $(".activeBubble").each(function (index) {
            $(this).removeClass('activeBubble');
        });
        //  $(el).parent().find('.example-obtuse').addClass('activeBubble');

        $(".pinBox").css({ 'z-index': '300' });
        $(".activeBubble").each(function (index) {
            $(this).removeClass('activeBubble');
            $(".pinBox").css({ 'z-index': '300' });
        });
        $(el).parent().find('.example-obtuse').addClass('activeBubble');
        $(el).parent().css({ 'z-index': '500' });




    }

    function calculateNewLocationOfPin(xaxis, yaxis) {
        var deviceWidth = $(window).width();
        var imageHeight = $(".mapImg").height();
        var fixWidth = 980;
        var fixHeight = 560;
        var newXaxis = xaxis;
        var newYaxis = yaxis;
        if (deviceWidth < fixWidth) {
            var widthDifferenceInPercentage = (fixWidth - deviceWidth) / (fixWidth * 0.01);
            var heightDifferenceInPercentage = (fixHeight - imageHeight) / (fixHeight * 0.01);
            console.log(widthDifferenceInPercentage + ", " + heightDifferenceInPercentage);
            newXaxis = newXaxis * ((100 - widthDifferenceInPercentage) / 100);
            newYaxis = newYaxis * ((100 - heightDifferenceInPercentage) / 100);
        }

        return {
            xaxis: newXaxis,
            yaxis: newYaxis
        };
    }

    BusinessTypeState["Marketing Office"]["Ahmedabad"] = {}; BusinessTypeState["Packhouse Facility"]["Shimla"] = {}; BusinessTypeState["Marketing Office"]["Shimla"] = {};

    if (typeof BusinessTypeState["Marketing Office"]["Ahmedabad"][""] == "undefined") {

        BusinessTypeState["Marketing Office"]["Ahmedabad"][""] = [];

    }

    BusinessTypeState["Marketing Office"]["Ahmedabad"][""].push({ "x-axis": "525.0", "y-axis": "176.0", "company": "Sanjay Shah (Sales, Imports)", "telephone": "+91-9909014971", "Address": "Fortune House, Nr. Mithakhali Circle, Navrangpura, Ahmedabad- 380 009", "mailid": "sanjay.shah2@adani.com" });


    console.log(JSON.stringify(BusinessTypeState)); if (typeof BusinessTypeState["Packhouse Facility"]["Shimla"][""] == "undefined") {

        BusinessTypeState["Packhouse Facility"]["Shimla"][""] = [];

    }

    BusinessTypeState["Packhouse Facility"]["Shimla"][""].push({ "x-axis": "568.5", "y-axis": "67.5", "company": "Mr. Pankaj Mishra <br><br> Adani Agri Fresh Ltd", "telephone": "+91-9816619356", "Address": "Village: Mehandli PO & Tehsil Rohru, District: Shimla, HP", "mailid": "" });


    console.log(JSON.stringify(BusinessTypeState)); if (typeof BusinessTypeState["Packhouse Facility"]["Shimla"][""] == "undefined") {

        BusinessTypeState["Packhouse Facility"]["Shimla"][""] = [];

    }

    BusinessTypeState["Packhouse Facility"]["Shimla"][""].push({ "x-axis": "577.5", "y-axis": "70.5", "company": "Mr. Manjeet Shillu <br><br> Adani Agri Fresh Ltd", "telephone": "+91-9816618366", "Address": "Village:Sainj, Po Sainj Tehsil Theog, District: Shimla, HP", "mailid": "" });


    console.log(JSON.stringify(BusinessTypeState)); if (typeof BusinessTypeState["Packhouse Facility"]["Shimla"][""] == "undefined") {

        BusinessTypeState["Packhouse Facility"]["Shimla"][""] = [];

    }

    BusinessTypeState["Packhouse Facility"]["Shimla"][""].push({ "x-axis": "575.0", "y-axis": "65.0", "company": "Mr. Dinesh Negi <br><br> Adani Agri Fresh Ltd", "telephone": "+91-9816007547", "Address": "Village:Rewali (Bithal) Tehsil Kumarsain, Rampur, District: Shimla, HP", "mailid": "" });



    console.log(JSON.stringify(BusinessTypeState)); if (typeof BusinessTypeState["Marketing Office"]["Shimla"][""] == "undefined") {

        BusinessTypeState["Marketing Office"]["Shimla"][""] = [];

    }

    BusinessTypeState["Marketing Office"]["Shimla"][""].push({ "x-axis": "572.5", "y-axis": "67.5", "company": "Mr. S.K.Mahajan", "telephone": "+91- 177-2624560,  +91-177-2623907", "Address": "Evergreen House, 1-New Road, Chotta Shimla, Shimla-171002", "mailid": "sanjay.mahajan@adani.com" });


    console.log(JSON.stringify(BusinessTypeState));

    function makePins(countryName, stateName, businessName) {

        console.log("Make Pin called");
        if (countryName != "null" && stateName == "null" && businessName == "null") { // only country is selected
            //$(".pin").display("none");
            for (var country in BusinessTypeState) {

                if (country == countryName) {

                    for (var state in BusinessTypeState[country]) {

                        for (var business in BusinessTypeState[country][state]) {

                            for (var i = 0; i < BusinessTypeState[country][state][business].length; i++) {
                                createAndPlacePin(country, state, business, BusinessTypeState[country][state][business][i]);
                            }
                        }
                    }
                }
            }
        } else if (countryName != "null" && stateName == "null" && businessName != "null") {
            $(".pinBox").css("display", "none");
            for (var country in BusinessTypeState) {
                if (country == countryName) {
                    for (var state in BusinessTypeState[country]) {
                        for (var business in BusinessTypeState[country][state]) {
                            for (var i = 0; i < BusinessTypeState[country][state][business].length; i++) {
                                console.log("Business : " + business + ", name : " + businessName);
                                if (business == businessName) {
                                    console.log("show business");
                                    createAndPlacePin(country, state, business, BusinessTypeState[country][state][business][i], "true");
                                }
                                else {
                                    console.log("hide business");
                                    createAndPlacePin(country, state, business, BusinessTypeState[country][state][business][i], "false");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    function createAndPlacePin(country, state, business, data, isVisible) {
        dhaval = data;
        if (country.indexOf(" ") > -1) {

            var tmp;
            var ar = country.split(" ");
            tmp = ar[0];
            for (var count = 1; count < ar.length; count++)
                tmp += ar[count];
            country = tmp;
        }
        if (state.indexOf(" ") > -1) {

            var tmp;
            var ar = state.split(" ");
            tmp = ar[0];
            for (var count = 1; count < ar.length; count++)
                tmp += ar[count];
            state = tmp;
        }
        if (business.indexOf(" ") > -1) {

            var tmp;
            var ar = business.split(" ");
            tmp = ar[0];
            for (var count = 1; count < ar.length; count++)
                tmp += ar[count];
            business = tmp;
        }



        var display = 'style = "display:block;"';
        if (typeof isVisible != 'undefined')
            display = (isVisible == "true") ? 'style = "display:block;"' : 'style = "display:none;"';
        console.log("Visible : " + isVisible + ", display : " + display);

        var singlePin = $('<div class="pinBox "' + display + '><div class="pin ' + country + ' ' + state + ' ' + business + '" onclick="pinClickBuble(this)" onmouseenter="pinClickBuble(this)" onmouseleave="pinClickBuble(this)"></div><div class="pulse"></div><blockquote class="example-obtuse ' + business + '"><p class="pinCompanyName">' + data.company + '</p><p class="pinAddress">' + data.Address + '</p><p class="pinTelephone">Tel : ' + data.telephone + '</p><p class="pinEmail">' + data.mailid + '</p></blockquote></div>');

        var axis = calculateNewLocationOfPin(data["x-axis"], data["y-axis"]);
        var xaxis = axis.xaxis;
        var yaxis = axis.yaxis;

        singlePin.css({
            position: 'absolute',
            left: xaxis + "px",
            top: yaxis + "px",
        });
        $("#overlay-dots").append(singlePin);
    }

    function createCorporateOfficePin() {

        var xaxis = 525;
        var yaxis = 176;
        var axis = calculateNewLocationOfPin(xaxis, yaxis);
        var xaxis = axis.xaxis;
        var yaxis = axis.yaxis;
        var div = $('<div class="pinBox" style="position: absolute; left: ' + xaxis + 'px; top: ' + yaxis + 'px;"><div class="pin " onclick="pinClickBuble(this)" onmouseenter="pinClickBuble(this)" onmouseleave="pinClickBuble(this)"></div><div class="pulse"></div><blockquote class="example-obtuse "><p class="pinCompanyName">Corporate Office</p><p class="pinAddress">Shantigram, Near Vaishnodevi Circle, S G Highway, Ahmedabad-382421, Gujarat, India.</p><p class="pinTelephone">Tel :+91-79-26565555 </p><a href="https://www.adani.com/wps/portal/wps/portal" target="blank" >www.adani.com</a></blockquote></div>')
        $("#overlay-dots").append(div);
    }

    $(document).ready(function () {

        $("#safetyLighthorticulture").lightGallery({
            loop: true,
            auto: false,
            caption: true,
            pause: 1000,
            cursor: false
        });

        var swiper = new Swiper('#imgBlk_horticulture', {
            //scrollbar: '.imgBlk_horticulture-scroll',
            scrollbarHide: false,
            slidesPerView: 'auto',
            /*  centeredSlides: false, */
            spaceBetween: 30,
            grabCursor: false,
            pagination: '.imgBlk_horticulture-scroll',
            paginationClickable: true,
            navigation: true,
            nextButton: '.swiper-button-next',
            prevButton: '.swiper-button-prev',
        }); $("#safetyLightfarmpik").lightGallery({
            loop: true,
            auto: false,
            caption: true,
            pause: 1000,
            cursor: false
        });

        var swiper = new Swiper('#imgBlk_farmpik', {
            //scrollbar: '.imgBlk_farmpik-scroll',
            scrollbarHide: false,
            slidesPerView: 'auto',
            /*  centeredSlides: false, */
            spaceBetween: 30,
            grabCursor: false,
            pagination: '.imgBlk_farmpik-scroll',
            paginationClickable: true,
            navigation: true,
            nextButton: '.swiper-button-next',
            prevButton: '.swiper-button-prev',
        }); $("#safetyLightmedical").lightGallery({
            loop: true,
            auto: false,
            caption: true,
            pause: 1000,
            cursor: false
        });

        var swiper = new Swiper('#imgBlk_medical', {
            //scrollbar: '.imgBlk_medical-scroll',
            scrollbarHide: false,
            slidesPerView: 'auto',
            /*  centeredSlides: false, */
            spaceBetween: 30,
            grabCursor: false,
            pagination: '.imgBlk_medical-scroll',
            paginationClickable: true,
            navigation: true,
            nextButton: '.swiper-button-next',
            prevButton: '.swiper-button-prev',
        });
    });


});
$(document).ready(function () {
    /*$('.overlay').css('visibility','visible');
    $('.close').click(function(){
      $('.overlay').fadeOut();
    });	*/

    $('.single-carousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                margin: 10
            }
        }
    })

    $('.bxslider').bxSlider({
        mode: 'fade',
        speed: 500,
        responsive: true,
        auto: true,
    });

    var owl = $("#groupWeb_carsol");

    owl.owlCarousel({
        items: 6, //10 items above 1000px browser width
        itemsDesktop: [1199, 6],
        itemsDesktopSmall: [979, 5],
        itemsTablet: [800, 4],
        itemsTabletSmall: [800, 3],
        itemsMobile: [640, 2],
        pagination: false,
        navigation: true,
    });


    // Parallax start

    $('li[data-menuanchor="sectionOne"]').addClass('active')
    $("a[href='#sectionOne']").click(function (event) {
        event.preventDefault();

        //$('li[data-menuanchor]').removeClass('active');
        //$('li[data-menuanchor="sectionOne"]').addClass('active')
        $("html, body").animate({
            scrollTop: $("#sectionOne").offset().top - 150
        }, "slow");

    });

    $("a[href='#sectionTwo']").click(function (event) {
        event.preventDefault();

        //$('li[data-menuanchor]').removeClass('active');
        //$('li[data-menuanchor="sectionTwo"]').addClass('active')
        if (!$('#mainContainer header').hasClass("headerDown")) {


            $("html, body").animate({
                scrollTop: $("#sectionTwo").offset().top - 140
            }, "slow");
        } else {

            $("html, body").animate({
                scrollTop: $("#sectionTwo").offset().top - 45
            }, "slow");


        }

    });

    $("a[href='#sectionThree']").click(function (event) {
        event.preventDefault();

        //$('li[data-menuanchor]').removeClass('active');
        //$('li[data-menuanchor="sectionThree"]').addClass('active')

        if (!$('#mainContainer header').hasClass("headerDown")) {

            $("html, body").animate({
                scrollTop: $("#sectionThree").offset().top - 150
            }, "slow");
        } else {

            $("html, body").animate({
                scrollTop: $("#sectionThree").offset().top - 55
            }, "slow");


        }

    });
    $("a[href='#sectionFour']").click(function (event) {
        event.preventDefault();

        //$('li[data-menuanchor]').removeClass('active');
        //$('li[data-menuanchor="sectionFour"]').addClass('active')

        if (!$('#mainContainer header').hasClass("headerDown")) {


            $("html, body").animate({
                scrollTop: $("#sectionFour").offset().top - 135
            }, "slow");
        } else {

            $("html, body").animate({
                scrollTop: $("#sectionFour").offset().top - 45
            }, "slow");


        }



    });
    $("a[href='#sectionFive']").click(function (event) {
        event.preventDefault();

        //$('li[data-menuanchor]').removeClass('active');
        //$('li[data-menuanchor="sectionFive"]').addClass('active')

        if (!$('#mainContainer header').hasClass("headerDown")) {


            $("html, body").animate({
                scrollTop: $("#sectionFive").offset().top - 135
            }, "slow");
        } else {

            $("html, body").animate({
                scrollTop: $("#sectionFive").offset().top - 45
            }, "slow");


        }



    });


    var currentSelection = "sectionOne"

    $(window).scroll(function () {

        var scrollHeight = $(document).scrollTop();
        console.log(scrollHeight);
        console.log(" Section -> " + currentSelection);
        if (scrollHeight < 470) {

            if (!$('li[data-menuanchor="sectionOne"]').hasClass("active")) {
                $('li[data-menuanchor]').removeClass('active');
                $('li[data-menuanchor="sectionOne"]').addClass('active')
                currentSelection = "sectionOne";
            }


        } else if (scrollHeight > 470 && scrollHeight < 950) {
            if (!$('li[data-menuanchor="sectionTwo"]').hasClass("active")) {

                $('li[data-menuanchor]').removeClass('active');
                $('li[data-menuanchor="sectionTwo"]').addClass('active')
                currentSelection = "sectionTwo";
            }



        } else if (scrollHeight > 949 && scrollHeight < 1200) {
            if (!$('li[data-menuanchor="sectionThree"]').hasClass("active")) {

                $('li[data-menuanchor]').removeClass('active');
                $('li[data-menuanchor="sectionThree"]').addClass('active')
                currentSelection = "sectionThree";
            }


        } else if (scrollHeight > 1569 && scrollHeight < 1970) {
            if (!$('li[data-menuanchor="sectionFour"]').hasClass("active")) {

                $('li[data-menuanchor]').removeClass('active');
                $('li[data-menuanchor="sectionFour"]').addClass('active')
                currentSelection = "sectionFour";
            }


        } else if (scrollHeight > 1969) {
            if (!$('li[data-menuanchor="sectionFive"]').hasClass("active")) {

                $('li[data-menuanchor]').removeClass('active');
                $('li[data-menuanchor="sectionFive"]').addClass('active')
                currentSelection = "sectionFive";
            }


        }



    });







    var sections = ['sectionOne', 'sectionTwo', 'sectionThree', 'sectionFour', 'sectionFive'];

    function nextElement(num) {
        var curIndex = $.inArray(num, sections);
        if (curIndex >= 0) {
            curIndex = curIndex + 1;
        }

        return sections[curIndex];
    }

    function prevElement(num) {
        return sections[($.inArray(num, sections) - 1 + sections.length) % sections.length];
    }


    $(document).keydown(function (e) {

        console.log("current Section " + currentSelection);
        console.log("Key Stroke is " + e.keyCode);
        if (e.keyCode == 38 || e.keyCode == 40) {
            var element = $("li[data-menuanchor]").find("active");
            var nextEl = "";
            if (e.keyCode == 38) {
                nextEl = prevElement(currentSelection)

            }
            if (e.keyCode == 40) {
                nextEl = nextElement(currentSelection);

            }
            //var element  = "a[href='/adani_farmPik/home#" + nextEl +"']";
            var element = "a[href='#" + nextEl + "']";
            console.log("Next Selection After Key Stroke: " + element);
            $(element).trigger("click");
        }

    });

});

document.addEventListener("DOMContentLoaded",
    function () {
        var div, n,
            v = document.getElementsByClassName("youtube-player");
        for (n = 0; n < v.length; n++) {
            div = document.createElement("div");
            div.setAttribute("data-id", v[n].dataset.id);
            div.setAttribute("id", v[n].dataset.id);
            div.setAttribute("onclick", "labnolIframe(this)");
            div.innerHTML = labnolThumb(v[n].dataset.id);
            v[n].appendChild(div);
            $(".playOver").remove();
        }
    });

function labnolThumb(id) {
    var thumb = '<img src="https://i.ytimg.com/vi/ID/hqdefault.jpg">',
        play = '<div class="play"></div>';
    console.log(thumb);
    return thumb.replace("ID", id) + play;
}

function labnolIframe(elem) {
    var iframe = document.createElement("iframe");
    var embed = "https://www.youtube.com/embed/ID?autoplay=1";
    iframe.setAttribute("src", embed.replace("ID", elem.id));
    iframe.setAttribute("frameborder", "0");
    iframe.setAttribute("allowfullscreen", "1");
    iframe.setAttribute("sandbox", "");
    elem.parentNode.replaceChild(iframe, elem);
}


$(document).ready(function () {

    $("#safetyLighthorticulture").lightGallery({
        loop: true,
        auto: false,
        caption: true,
        pause: 1000,
        cursor: false
    });

    var swiper = new Swiper('#imgBlk_horticulture', {
        //scrollbar: '.imgBlk_horticulture-scroll',
        scrollbarHide: false,
        slidesPerView: 'auto',
        /* centeredSlides: false, */
        spaceBetween: 30,
        grabCursor: false,
        pagination: '.imgBlk_horticulture-scroll',
        paginationClickable: true,
        navigation: true,
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
    }); $("#safetyLightfarmpik").lightGallery({
        loop: true,
        auto: false,
        caption: true,
        pause: 1000,
        cursor: false
    });

    var swiper = new Swiper('#imgBlk_farmpik', {
        //scrollbar: '.imgBlk_farmpik-scroll',
        scrollbarHide: false,
        slidesPerView: 'auto',
        /* centeredSlides: false, */
        spaceBetween: 30,
        grabCursor: false,
        pagination: '.imgBlk_farmpik-scroll',
        paginationClickable: true,
        navigation: true,
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
    }); $("#safetyLightmedical").lightGallery({
        loop: true,
        auto: false,
        caption: true,
        pause: 1000,
        cursor: false
    });

    var swiper = new Swiper('#imgBlk_medical', {
        //scrollbar: '.imgBlk_medical-scroll',
        scrollbarHide: false,
        slidesPerView: 'auto',
        /* centeredSlides: false, */
        spaceBetween: 30,
        grabCursor: false,
        pagination: '.imgBlk_medical-scroll',
        paginationClickable: true,
        navigation: true,
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
    });
});

jQuery(document).ready(function ($) {


    setInterval(function () {
        moveRight();
    }, 3000);
    var slideCount = $('#slider ul li').length;
    var slideWidth = $('#slider ul li').width();
    var slideHeight = $('#slider ul li').height();
    var sliderUlWidth = slideCount * slideWidth;

    $('#slider').css({ width: slideWidth, height: slideHeight });

    $('#slider ul').css({ width: sliderUlWidth, marginLeft: - slideWidth });

    $('#slider ul li:last-child').prependTo('#slider ul');

    function moveLeft() {
        $('#slider ul').animate({
            left: + slideWidth
        }, 400, function () {
            $('#slider ul li:last-child').prependTo('#slider ul');
            $('#slider ul').css('left', '');
        });
    };

    function moveRight() {
        $('#slider ul').animate({
            left: - slideWidth
        }, 400, function () {
            $('#slider ul li:first-child').appendTo('#slider ul');
            $('#slider ul').css('left', '');
        });
    };

    $('a.control_prev').click(function () {
        moveLeft();
    });

    $('a.control_next').click(function () {
        moveRight();
    });

});    
