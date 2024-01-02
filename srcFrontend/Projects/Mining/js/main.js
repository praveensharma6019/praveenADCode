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
// Analytics title
let indexdata;
var x = document.getElementsByClassName('analytic-title');
for (var i = 0; i < x.length; i++) {
    indexdata = x[i].textContent;
}

// Handling Form Distrotion issue after Submission fail - start
var intervalId;

function HandleLabelFloating() {
    var errorElement = document.querySelector('.validation-summary-errors');
    if (errorElement) {
		var termsAndConditions = document.querySelectorAll('.termsLabel');
		for (var i = 0; i < termsAndConditions.length; i++) {
			var termsAndCondition = termsAndConditions[i];
			var childNodes = termsAndCondition.childNodes;

			// Check if there are at least 3 child nodes
			if (childNodes.length >= 3) {
				childNodes[2].nodeValue = "";
			}
		} 
		$('.termsnconditions').html('By submitting, you agree to our <a href="/terms-and-conditions" class="label-bold">T&Cs</a> and <a href="/privacy-policy" class="label-bold">Privacy Policy</a>.');

        var floatLabelElements = errorElement.parentElement.querySelectorAll('.float-label');
        floatLabelElements.forEach(function (ele) {
            var inputFieldContext = ele.parentElement.querySelector(".form-control-custom");
            var isFocused = document.activeElement === inputFieldContext;
            var crossIcon = ele.parentElement.querySelector(".clear-icon");
            if (inputFieldContext.value.length > 0 || isFocused) {
                ele.classList.add('form-float'); //adding float functionality

                if (inputFieldContext.value.length > 0 && crossIcon != null) {
                    crossIcon.classList.add('d-block'); //added cross icon
                    //added close functionality to cross icon
                    crossIcon.setAttribute("onclick", "clearInput(this)");
                }
            }
            else {
                ele.classList.remove('form-float');
                crossIcon.classList.remove('d-block');
                crossIcon.removeAttribute("onclick", "clearInput(this)");
            }
        });
    }
}

function clearInput(closeContext) {
    $(closeContext).siblings('.form-control-custom').val('');
    $(closeContext).removeClass('d-block');
    $(closeContext).siblings('.float-label').removeClass('form-float');
}
//end


const scrollToTop = () => {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    });
};

// View More Logic web
const elements = document.querySelectorAll('.show-large.card-box.d-none');
for (let i = 0; i < elements.length; i++) {
    if (i < 6) {
        elements[i].classList.remove('d-none');
    }
}

$('#ViewMoreBtn').on('click', function () {
    const viewMoreBtn = document.getElementById('ViewMoreBtn');
    const buttonText = viewMoreBtn.innerText;
    if (buttonText === 'View More') {
        var remainingCardElement = document.querySelectorAll(".show-large.card-box.d-none");
        for (let i = 0; i < remainingCardElement.length; i++) {
            remainingCardElement[i].classList.remove('d-none');
        }
        viewMoreBtn.innerText = 'View Less';
    }
    if (buttonText === 'View Less') {
        var CardElement = document.querySelectorAll(".show-large.card-box");
        for (let i = 6; i < CardElement.length; i++) {
            CardElement[i].classList.add('d-none');
        }
        viewMoreBtn.innerText = 'View More';
    }
});

// View More Logic Mobile
const MobElements = document.querySelectorAll('.show-mob.card-box.d-none');
for (let i = 0; i < MobElements.length; i++) {
    if (i < 3) {
        MobElements[i].classList.remove('d-none');
    }
}

$('#MobViewMoreBtn').on('click', function () {
    const viewMoreBtnMob = document.getElementById('MobViewMoreBtn');
    const buttonTextMob = viewMoreBtnMob.innerText;
    if (buttonTextMob === 'View More') {
        var remainingCardElementMob = document.querySelectorAll(".show-mob.card-box.d-none");
        for (let i = 0; i < remainingCardElementMob.length; i++) {
            remainingCardElementMob[i].classList.remove('d-none');
        }
        viewMoreBtnMob.innerText = 'View Less';
    }
    if (buttonTextMob === 'View Less') {
        var CardElementMob = document.querySelectorAll(".show-mob.card-box");
        for (let i = 3; i < CardElementMob.length; i++) {
            CardElementMob[i].classList.add('d-none');
        }
        viewMoreBtnMob.innerText = 'View More';
    }
});

function downloadPdf(pdfUrl) {
    const link = document.createElement('a');
    link.href = pdfUrl;
    link.download = 'file.pdf';
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

$('#nav-last-btn').on('click', function () {
    var urlredirectfield = document.getElementsByClassName('url-redirect')[0];
    localStorage.setItem("redirectField", window.location.href + urlredirectfield.value)
});

$('#mobnav-last-btn').on('click', function () {
    var urlredirectfield = document.getElementsByClassName('url-redirect')[0];
    localStorage.setItem("redirectField", window.location.href + urlredirectfield.value)
});


$('#discover-download-btn').on('click', function () {
    var urlDiscoverredirectfield = document.getElementsByClassName('url-redirect')[1];
    localStorage.setItem("redirectField", window.location.href + urlDiscoverredirectfield.value)
});
$('#dwnldbrochurebtn').on('click', function () {
    var urlDiscoverredirectfield = document.getElementsByClassName('url-redirect')[1];
    localStorage.setItem("redirectField", window.location.href + urlDiscoverredirectfield.value)
});

// $('#btnexampleModal').on('click', function() {
// var subscribeUrlredirectfield = document.getElementsByClassName('url-redirect')[5];
// localStorage.setItem("redirectField", window.location.href + subscribeUrlredirectfield.value)
// });


//dropdownfield

let dropdown = '';

$('.dropdown-analytic-field').on('change', function (e) {
    dropdown = $(this).val();
    localStorage.setItem("dropdown", dropdown);

});

window.onhashchange = function () {
    var subscribePopUpURL = window.location.hash;
    if (subscribePopUpURL == "#popup") {
        var redirectURl = localStorage.getItem("redirectField");
        window.location.href = redirectURl;
    }
    if (subscribePopUpURL == "#homeexamplemodal" || subscribePopUpURL == "#enquiryexamplemodal" || subscribePopUpURL == "#downloadbroschureexamplemodal") {
        var URL = window.location.hash;
        var itemID = URL.substring(1);
        var id = document.getElementById(itemID);
        if (id != null) {
            if (subscribePopUpURL == "#downloadbroschureexamplemodal") {
                downloadPdf('/-/media/Project/Mining/Download-Brochure/MTCS-Corporate-Brochure');
            }
        }

    }
    else {

        var URL = window.location.hash;
        var itemID = URL.substring(1);
        if (itemID != null && itemID.includes("offcanvasServiceRight")) {
            if (window.innerWidth < 1024) {
                itemID = itemID.replace("Right", "Bottom");
            }
            openNav(itemID);
        }
        else {
            var id = document.getElementById(itemID);
            if (id != null) {
                clearInterval(intervalId); // Stopping the execution of function which is help in handling form distortion on fail
                id.classList.toggle("show");
                id.style.display = 'block';

                formStatus = 'success';

                const submitEventMeta = 'form_submit';
                const submitCategoryMeta = 'mining';
                const bannerCategoryMeta = '';
                const submitsubCategoryMeta = 'form_interaction';
                const submitlabelMeta = 'submit';
                const pageType = getPageType();
                const selectedOptionType = dropdown;
                const submitIndexMeta = indexdata;

                if (submitEventMeta != null) {
                    events = submitEventMeta;
                }
                if (bannerCategoryMeta != null) {
                    bannerCategory = bannerCategoryMeta;
                }

                if (submitCategoryMeta != null) {
                    category = submitCategoryMeta;
                }
                if (submitsubCategoryMeta != null) {
                    subCategory = submitsubCategoryMeta;
                }

                if (submitlabelMeta != null) {
                    label = submitlabelMeta;
                }
                if (selectedOptionType != null) {
                    type = selectedOptionType;
                }

                if (submitIndexMeta != null) {
                    index = submitIndexMeta;
                }

                sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, label, index, type, formStatus);

                var removePopups = document.getElementsByClassName('sidenav myWidth-add');

                for (var i = 0; i < removePopups.length; i++) {
                    removePopups[i].classList.remove('myWidth-add');
                }


                var overlays = document.getElementsByClassName('thankyouoverlay');


                for (var i = 0; i < overlays.length; i++) {
                    overlays[i].style.display = 'block';
                }


            }
        }
    }

}

function openNav(id) {
    clearInterval(intervalId); // Stopping the execution of function which is help in handling form distortion on fail
    var sideModal = document.getElementById(id);
    if (sideModal == null) {
        sideModal = document.getElementById(id.id);
    }
    if (sideModal != null) {
        sideModal.classList.add("myWidth-add");
        var overlayElement = sideModal.querySelector(".overlay");
        if (overlayElement) {
            overlayElement.style.display = "block";

            var removeFloatingOnClick = document.querySelector('.noCommonValidation');
            if (removeFloatingOnClick) {

                var floatElements = removeFloatingOnClick.parentElement.querySelectorAll('.float-label.form-float');
                floatElements.forEach(function (elem) {
                    var inputContext = elem.parentElement.querySelector(".form-control-custom");
                    var Focused = document.activeElement === inputContext;
                    var crossIcons = elem.parentElement.querySelector(".clear-icon");
                    if (inputContext.value.length > 0 || Focused) {
                        elem.classList.remove('form-float'); //adding float functionality

                        if (inputContext.value.length > 0 && crossIcons != null) {
                            crossIcons.classList.add('d-block'); //added cross icon
                            //added close functionality to cross icon
                            crossIcons.setAttribute("onclick", "clearInput(this)");
                        }
                    } else {
                        elem.classList.remove('form-float');
                        crossIcons.classList.remove('d-block');
                        crossIcons.removeAttribute("onclick", "clearInput(this)");
                    }
                });
            }
            document.body.style.overflow = 'hidden';
        }
    }
}


function closeNav(closeButton) {
    clearInterval(intervalId); // Stopping the execution of function which is help in handling form distortion on fail
    const parentDiv = closeButton.closest('.sidenav.myWidth-add');
    if (parentDiv) {
        //closing Modal
        parentDiv.classList.remove("myWidth-add");
        document.body.style.overflow = 'scroll';
        //removing overlay
        var overlayElement = parentDiv.querySelector(".overlay");
        if (overlayElement) {
            overlayElement.style.display = "none";
        }

        //set URL to original
        var currentURL = window.location.href;
        var baseURL = currentURL.split('#')[0];
        window.history.pushState(null, null, baseURL);
        const pageType = getPageType();
        if (pageType == 'homepage') {
            window.location.href = baseURL;
        }
        else {
            window.location.href = baseURL;
        }


        //removing overflow:hidden from Body tag
        var bodyStyle = window.getComputedStyle(document.body);
        if (bodyStyle.getPropertyValue('overflow') === 'hidden') {
            var bodyElement = document.body;
            var currentStyle = bodyElement.getAttribute('style');
            var modifiedStyle = currentStyle.replace(/overflow:\s*hidden;\s*/i, '');
            bodyElement.setAttribute('style', modifiedStyle);
        }
    }
}
function enableFloating(context) {
    context.classList.add('form-float');
    const inField = context.closest('.input-container');
    var focusField = inField.querySelector(".form-control-custom");
    focusField.focus();
}

// const $userCards = document.querySelectorAll(".main-skeleton");

// $userCards.forEach($el => {
//     setTimeout(() => {
//         $el.classList.remove("skeleton");
//         $el
//             .querySelectorAll(".hide-text")
//             .forEach((el) => el.classList.remove("hide-text"));
//     }, 2000);
// });


//File upload error message

let truncatedErrorMessage = '';

function truncateErrorMessage() {
    waitForElm('.field-validation-error').then((elm) => {

        let errorMessage = $(elm).text();

        let truncatedMessage = errorMessage.slice(0, 50);

        $(elm).text(truncatedMessage);
    });
}

// Call the function to truncate the error message initially
truncateErrorMessage();

$('.file-upload-field').change(function (e) {
    waitForElm('.field-validation-error').then((elm) => {

        let errorMessage = $(elm).text();

        truncatedErrorMessage = errorMessage.slice(0, 50);

        $(elm).text(truncatedErrorMessage);
    });
});

$(document).ready(function () {
	
	//TermsAndCondition Checkbox error
    var termsAndConditions = document.querySelectorAll('.termsLabel');
	for (var i = 0; i < termsAndConditions.length; i++) {
		var termsAndCondition = termsAndConditions[i];
		var childNodes = termsAndCondition.childNodes;

		// Check if there are at least 3 child nodes
		if (childNodes.length >= 3) {
			childNodes[2].nodeValue = "";
		}
	} 

    // To handle cases where the user clicks outside the form field
    $(document).on('click', function (e) {
        if ($(e.target).closest('.file-upload-field').length === 0) {
            $('.field-validation-error').text(truncatedErrorMessage);
        }
    });

    // Periodically check for error messages and truncate them if found.
    setInterval(truncateErrorMessage, 10);

    // Onclick on labels making input field editable

    var floatLabelElements = document.querySelectorAll(".float-label");

    floatLabelElements.forEach(function (element) {

        element.setAttribute("onclick", "enableFloating(this)");

    });

    // Analytics title
    // var x = document.getElementsByClassName('analytic-title');
    // for (var i = 0; i < x.length; i++) {
    // var index = x[i].textContent;
    // }

    // Dropdown null Check

    var drpdwnlocalstorage = localStorage.getItem("dropdown");
    if (drpdwnlocalstorage == null) {
        localStorage.setItem("dropdown", '');
    }

    // Clear icon for form fields
    const clearIcons = document.querySelectorAll(".clear-icon");
    clearIcons.forEach((elem) => {
        elem.addEventListener("click", () => {
            elem.classList.remove("d-block");
            var parentItem = elem.parentElement;
            let children = parentItem.querySelectorAll('.form-control-custom');
            children[0].value = "";
            let children1 = parentItem.querySelectorAll('.float-label');
            children1[0].classList.remove('form-float');
        });
    });
    /*Thankyou Pop UP */
    var formStatus = 'fail';
    var CurrentUrlHashValue = window.location.hash;
    if (CurrentUrlHashValue == "#popup") {
        var redirectURl = localStorage.getItem("redirectField");
        window.location.href = redirectURl;
    }

    // set Local storage value for popup redirection
    var urlRedirectElement = document.getElementsByClassName('url-redirect')[2];
    var LocalStorage_RedirectURL = localStorage.getItem("redirectField");
    if (urlRedirectElement) {
        var CurrentURLwithHashValue = window.location.href.split('#')[0] + urlRedirectElement.value;
    } else {

        CurrentURLwithHashValue = window.location.href.split('#')[0];
    }

    if (LocalStorage_RedirectURL != null) {
        localStorage.setItem("redirectField", CurrentURLwithHashValue);
    }

    /*Set URL back to Original state after popup close*/
    var currentURL = window.location.href;
    var baseURL = window.location.origin;
    if (currentURL.startsWith(baseURL)) {
        var newURL = currentURL.replace(baseURL, '');
        window.history.pushState(null, null, newURL);
    }



    //Show popup
    var URL = window.location.hash;
    var itemID = URL.substring(1);
    if (itemID != null && itemID.includes("offcanvasServiceRight")) {
        if (window.innerWidth < 1024) {
            itemID = itemID.replace("Right", "Bottom");

        }
        openNav(itemID);
    } else {
        var id = document.getElementById(itemID);
        if (id != null) {
            clearInterval(intervalId); // Stopping the execution of function which is help in handling form distortion on fail
            id.classList.toggle("show");
            id.style.display = 'block';

            // Status form Submission
            formStatus = 'success';

            const submitEventMeta = 'form_submit';
            const bannerCategoryMeta = '';
            const submitCategoryMeta = 'mining';
            const submitsubCategoryMeta = 'form_interaction';
            const submitlabelMeta = 'submit';
            const pageType = getPageType();
            const selectedOptionType = localStorage.getItem("dropdown");
            const submitIndexMeta = indexdata;

            if (submitEventMeta != null) {
                events = submitEventMeta;
            }
            if (bannerCategoryMeta != null) {
                bannerCategory = bannerCategoryMeta;
            }

            if (submitCategoryMeta != null) {
                category = submitCategoryMeta;
            }
            if (submitsubCategoryMeta != null) {
                subCategory = submitsubCategoryMeta;
            }

            if (submitlabelMeta != null) {
                label = submitlabelMeta;
            }
            if (selectedOptionType != null) {
                type = selectedOptionType;
            }

            if (submitIndexMeta != null) {
                index = submitIndexMeta;
            }

            sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, label, index, type, formStatus);


            var overlays = document.getElementsByClassName('thankyouoverlay');


            for (var i = 0; i < overlays.length; i++) {
                overlays[i].style.display = 'block';
            }

            if (URL == '#downloadbroschureexamplemodal') {
                downloadPdf('/-/media/Project/Mining/Download-Brochure/MTCS-Corporate-Brochure');
            }
        }


    }


    $('.btn-close').on('click', function (ele) {
        const pageType = getPageType();
        const currentURL = window.location.href;
        const baseurl = currentURL.split('#')[0];
        history.pushState(null, null, baseurl);
        if (pageType == 'homepage') {
            window.location.href = baseurl;
        }
        // var overlayFreeze = document.getElementsByClassName('offcanvas-backdrop fade show');
        // if(overlayFreeze != null)
        // {
        // for (var i = 0; i < overlayFreeze.length; i++) {
        // overlayFreeze[i].classList.remove('show');
        // }
        // }
        $("#fxb_977b995b-75e8-448c-ac9f-814b2df0e4bc_Fields_cd49af71-972b-4f5b-90df-cba0086ea71d__Value").val('')
        $("#fxb_977b995b-75e8-448c-ac9f-814b2df0e4bc_Fields_bec93fc3-15f9-422f-8d0c-0ed8f8114f6b__Value").val('')
        $("#fxb_977b995b-75e8-448c-ac9f-814b2df0e4bc_Fields_64c46ffd-84a6-4e32-a164-1efa9c3dda0e__Value").val('')
        $("#fxb_977b995b-75e8-448c-ac9f-814b2df0e4bc_Fields_b0895209-fd86-4a5f-b917-0bd9dec4a692__Value").val('')
        $("#fxb_977b995b-75e8-448c-ac9f-814b2df0e4bc_Fields_672dcc4e-6b44-404b-9951-2128a066f4d2__Value").val('')
        $("#fxb_977b995b-75e8-448c-ac9f-814b2df0e4bc_Fields_5e557fa6-8bc3-4db2-96d4-c70ad2e01233__Value").val('')
        $("#fxb_6526b839-2fed-4870-8244-1f42b855cca7_Fields_5c3f479d-f1db-4afc-9edc-c7538dcd5fab__Value").val('')
        $("#fxb_6526b839-2fed-4870-8244-1f42b855cca7_Fields_c4405548-5b8f-4c88-ad4a-915ccffa1c0b__Value").val('')
        $("#fxb_6526b839-2fed-4870-8244-1f42b855cca7_Fields_ce0e10a5-b495-47a3-b4e5-fd3395c9db2e__Value").val('')
        $("#fxb_6526b839-2fed-4870-8244-1f42b855cca7_Fields_1b95bbc6-ecb2-4e78-a693-c7c53019bb9f__Value").val('')

    });
    $('.download-submit').on('click', function (ele) {
        $('.termsnconditions').html('By submitting, you agree to our <a href="/terms-and-conditions" class="label-bold underline-text">T&Cs</a> and <a href="/privacy-policy" class="label-bold underline-text">Privacy Policy</a>.');

    });

    if (window.innerWidth < 768) {
        if ($('#projectlistingexamplemodal, #contactusexamplemodal').hasClass('show')) {
            $('.modal.show .modal-dialog').css('transform', 'translateY(24%)');
        }
    };


    var DownloadBtnContextId = 'downloadBtnExampleModal';
    var DownloadBtnContext = document.querySelector(".btn.btn-primary.show-large.download-submit");
    DownloadBtnContext.id = DownloadBtnContextId;


    // Check if it's the career page based on the page URL or some identifier
    var isCareerPage = window.location.pathname.includes('careers'); // Adjust the condition based on your website's URL structure or use any other identifier

    if (isCareerPage) {
        // Execute this code only if it's the career page
        var CareerContextId = 'fileInput';
        var CareerBtnContext = document.querySelector(".file-upload-field");
        CareerBtnContext.id = CareerContextId;

        var CareerUploadTextId = 'fileName';
        var CareerUploadContext = document.querySelector(".fileUploadShowText");
        CareerUploadContext.id = CareerUploadTextId;
    }


    // Add an event listener to the file input element
    $('#fileInput').change(function () {
        const fileNameDisplay = $('#fileName');

        // Check if a file is selected
        if (this.files.length > 0) {
            const fileName = this.files[0].name;
            fileNameDisplay.text('Selected File: ' + fileName);
            $('.hide-label').css('display', 'none');
        } else {
            fileNameDisplay.text('No file selected');
        }
    });

    $('.btn-close').click(function () {
        $('.offcanvas').removeClass('show');

    })


    $(".termsnconditions").ready(function () {
        $('.termsnconditions').html('By submitting, you agree to our <a href="/terms-and-conditions" class="label-bold underline-text">T&Cs</a> and <a href="/privacy-policy" class="label-bold underline-text">Privacy Policy</a>.');
    });

    $(".btn-primary").ready(function () {
        $('.btn-primary').data('bs-target', '#exampleModel');
    });
	// Other field disable
	$(document).on('change', '.dropdown-analytic-field', function () {
		if ($(this).val() == 'Other') {
			$('.other').removeClass('d-none');
		} else {
			$('.other').addClass('d-none');
		}
	});
	$(document).on('change', '.dropdown-analytic-field', function () {
		if ($(this).val() == 'Other') {
			$('.others').removeClass('d-none');
		} else {
			$('.others').addClass('d-none');
		}
	});

    $('.btn-close').click(function () {
        $('.offcanvas').removeClass('show');
    })

    

    // Download broschure button terms and conditions freeze
    $('.download-submit').on('click', function (ele) {
        waitForElm('.validation-summary-errors').then((elm) => {
            $('.termsnconditions').html('By submitting, you agree to our <a href="/terms-and-conditions" class="label-bold underline-text">T&Cs</a> and <a href="/privacy-policy" class="label-bold underline-text">Privacy Policy</a>.');
        });
    });

    $('.mobile-trigger').on('click', function (e) {
        e.stopPropagation();

        $('body').toggleClass('menu-open');
        $('html').toggleClass('cm-menu-open');

    });

    $('.hide-service li a').on('click', function (e) {
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



    $('.form-control-custom').focusin(function () {
        //$(this).next('.float-label').addClass('form-float');
        $(this).siblings('.float-label').addClass('form-float');
    });
    $('.form-control-custom').focusout(function () {
        if (this.value.length > 0) {
            //has input
            $(this).siblings('.float-label').addClass('form-float');
        } else {
            //input is blank
            $(this).siblings('.float-label').removeClass('form-float');
        }
    });
    $('.form-control-custom').keyup(function () {
        if (this.value.length > 0) {
            $(this).siblings('.clear-icon').addClass('d-block');
        } else {
            $(this).siblings('.clear-icon').removeClass('d-block');
        }
    });
    $('.btn-close').on('click', function () {
        $(this).siblings('.form-control-custom').val('');
        $(this).removeClass('d-block');
        //$(this).siblings('.form-control-custom').css({ 'transform': 'scale(1) translateY(0);' });
        $(this).siblings('.float-label').removeClass('form-float');
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

    const mtcsOtherTeam = $('.mob-para-8');
    const mtcsOtherreadMoreTeam = $('.read-more-8');

    mtcsOtherreadMoreTeam.on('click', function () {
        mtcsOtherTeam.toggleClass('expanded-8');
        mtcsOtherreadMoreTeam.text(mtcsOtherTeam.hasClass('expanded-8') ? 'Read Less' : 'Read More');
    });

    const mtcsOtherTeamMw = $('.mob-para-9');
    const mtcsOtherreadMoreTeamMw = $('.read-more-9');

    mtcsOtherreadMoreTeamMw.on('click', function () {
        mtcsOtherTeamMw.toggleClass('expanded-9');
        mtcsOtherreadMoreTeamMw.text(mtcsOtherTeamMw.hasClass('expanded-9') ? 'Read Less' : 'Read More');
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



    // $('.hamb-slide>a').click(function () {
    //     $(this).parent('li').siblings('li').find('ul').slideUp(250);
    //     $(this).parent('li').siblings('li').find('a.active').removeClass('active');
    //     $(this).next('ul').slideToggle(250);
    //     $(".menu_item i.icon").toggleClass("active");
    // });

    $('.hamb-slide > a').click(function (event) {
        event.preventDefault(); // Prevent the default link behavior

        // Close other open submenus
        $(this)
            .closest('li')
            .siblings('li')
            .find('ul:visible')
            .slideUp(250);

        // Remove "active" class from other menu items
        $(this)
            .closest('li')
            .siblings('li')
            .find('a.active')
            .removeClass('active');

        // Toggle the submenu of the clicked menu item
        $(this)
            .next('ul')
            .slideToggle(250);

        // Toggle the "active" class for the icon
        $(this)
            .find('i.icon')
            .toggleClass('active');
    });

    //Thank you model 
    var modal = $("#projectlistingexamplemodal, #downloadbroschureexamplemodal, #contactusexamplemodal, #careerexamplemodal, #enquiryexamplemodal");
    var span = $(".btn-close");

    span.click(function () {
        modal.css("display", "none");
    });

    $(window).click(function (event) {
        if (event.target == modal[0]) {
            modal.css("display", "none");
        }
    });



    $('footer').after('<div class="backToTop" id="btn-back-to-top" onclick="scrollToTop()" style="display: none;"><div class= "inner"><i class="i-arrow-u"><img src="https://sitecorecm.uat.adanirealty.com/-/media/Project/Mining/Icons/Arrow-Open.svg" /></i><span>Back to Top</span></div></div> ');
    var value1 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);

    $('.header-nav .menu-section>ul li a').each(function () {
        var url = $(this).attr('href');
        var lastSegment = url?.split('/').pop();
        var urlRedirectionValue = localStorage.getItem("redirectField");

        if (lastSegment == value1 && lastSegment != "") {
            $(this).parent().addClass('active-link');
        }
        var pageName = getPageType();
        if (urlRedirectionValue == null) {
            var urlredirectID = document.getElementsByClassName('url-redirect')[0];
            localStorage.setItem("redirectField", window.location.href + urlredirectID.value);
        }

        // if (lastSegment == "" && urlRedirectionValue == null) {
        // var urlredirectID = document.getElementsByClassName('url-redirect')[0];
        // localStorage.setItem("redirectField", window.location.href + urlredirectID.value);
        // }		
    });


    const toggleVisible = () => {
        const scrolled = document.body.scrollTop || document.documentElement.scrollTop;
        const classdata = document.getElementsByClassName("backToTop")[0];

        footerElement = document.querySelector("footer");
        var footerStyle = footerElement.currentStyle || window.getComputedStyle(footerElement);
        footerTopMargin = Number(footerStyle.marginTop.slice(0, -2));

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

    window.addEventListener("scroll", toggleVisible);

});


// Get page type

function getPageType() {
    const path = window.location.pathname;
    const segments = path.split('/');

    if (path === '/') {
        return 'homepage';
    } else {
        return segments[1] + '_page';
    }
}

const pageType = getPageType();

// form  analytics download certificates
const downloadCertificateElements = document.getElementsByClassName('download-submit-btn');

for (const downloadCertificateElement of downloadCertificateElements) {
    downloadCertificateElement.addEventListener('click', function (event) {
        const submitClick = event.target;

        const submitEventMeta = 'download_certificate';
        const bannerCategoryMeta = '';
        const submitCategoryMeta = 'mining';
        const submitsubCategoryMeta = 'our_accreditation';
        const pageType = getPageType();
        const submitIndexMeta = 'download_certificate';


        if (submitEventMeta != null) {
            events = submitEventMeta;
        }
        if (bannerCategoryMeta != null) {
            bannerCategory = bannerCategoryMeta;
        }

        if (submitCategoryMeta != null) {
            category = submitCategoryMeta;
        }
        if (submitsubCategoryMeta != null) {
            subCategory = submitsubCategoryMeta;
        }


        if (submitIndexMeta != null) {
            index = submitIndexMeta;
        }

        sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, index);

    });
}
// forms analytics

// const elementstags = document.getElementsByClassName('submit-analytics');

// for (const elementstag of elementstags) {
// elementstag.addEventListener('click', function (event) {
// const submitClick = event.target.getAttribute('type');
// if (submitClick == 'submit') {

// }

// });
// }

// Analytics Tags

const elementsitems = document.getElementsByClassName('analytics-tags');

for (const elementsitem of elementsitems) {
    elementsitem.addEventListener('click', function (event) {
        const ExploreClick = event.target;

        const eventMeta = ExploreClick.querySelector('meta[name="Event"]');
        const bannerCategoryMeta = ExploreClick.querySelector('meta[name="Banner_Category"]');
        const categoryMeta = ExploreClick.querySelector('meta[name="Category"]');
        const subCategoryMeta = ExploreClick.querySelector('meta[name="Sub_category"]');
        const pageType = getPageType();
        const labelMeta = ExploreClick.querySelector('meta[name="Label"]');
        const indexMeta = ExploreClick.querySelector('meta[name="Index"]');


        if (eventMeta != null) {
            events = eventMeta.getAttribute('content');
        }
        if (bannerCategoryMeta != null) {
            bannerCategory = bannerCategoryMeta.getAttribute('content');
        }
        if (categoryMeta != null) {
            category = categoryMeta.getAttribute('content');
        }
        if (subCategoryMeta != null) {
            subCategory = subCategoryMeta.getAttribute('content');
        }

        if (labelMeta != null) {
            label = labelMeta.getAttribute('content');
        }
        if (indexMeta != null) {
            index = indexMeta.getAttribute('content');
        }

        sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, label, index);
    });
}

// Logo analytics 
const tagsElements = document.getElementsByClassName('logo-analytics-tags');

for (const tagElement of tagsElements) {
    tagElement.addEventListener('click', function (event) {
        const ExploreClick = event.currentTarget;

        const eventMeta = ExploreClick.querySelector('meta[name="Event"]');
        const bannerCategoryMeta = ExploreClick.querySelector('meta[name="Banner_Category"]');
        const categoryMeta = ExploreClick.querySelector('meta[name="Category"]');
        const subCategoryMeta = ExploreClick.querySelector('meta[name="Sub_category"]');
        const pageType = getPageType();
        const labelMeta = ExploreClick.querySelector('meta[name="Label"]');
        const indexData = ExploreClick.querySelector('meta[name="Index"]');

        if (eventMeta != null) {
            events = eventMeta.getAttribute('content');
        }
        if (bannerCategoryMeta != null) {
            bannerCategory = bannerCategoryMeta.getAttribute('content');
        }

        if (categoryMeta != null) {
            category = categoryMeta.getAttribute('content');
        }
        if (subCategoryMeta != null) {
            subCategory = subCategoryMeta.getAttribute('content');
        }

        if (labelMeta != null) {
            label = labelMeta.getAttribute('content');
        }
        if (indexData != null) {
            index = indexData.getAttribute('content');
        }

        sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, label, index);
    });
}


function sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, label, index, type, formStatus, errorText) {
    const ExploreData = {
        Event: events,
        Category: category,
        BannerCategory: bannerCategory,
        Sub_category: subCategory,
        Page_type: pageType,
        Label: label,
        Index: index,
        Type: type,
        Status: formStatus,
        Error_Text: errorText
    };

    // Send data to datalayer
    dataLayer.push(ExploreData);
}

// $('.file-upload-field').change(function (e) {

//     waitForElm('.field-validation-error').then((elm) => {

//         $('#fileInput-error').css({

//             'display': '-webkit-box',
//             '-webkit-line-clamp': '1',
//             '-webkit-box-orient': 'vertical',
//             'overflow': 'hidden'

//         });

//     });

// });

$('.file-upload-field').change(function (e) {

    waitForElm('.field-validation-error').then((elm) => {

        $('#fileInput-error').slice(0, 50);

    });

});

$('.submit-analytics, .download-submit').click(function (e) {
    intervalId = setInterval(HandleLabelFloating, 100);  //Handling form submission fail
    waitForElm('.validation-summary-errors').then((elm) => {
        var floatLabelElements = document.querySelectorAll(".float-label");
        floatLabelElements.forEach(function (element) {
            element.classList.add('form-float');
        });

        formStatus = 'Fail';
        const submitEventMeta = 'form_submit';
        const submitCategoryMeta = 'mining';
        const bannerCategoryMeta = '';
        const submitsubCategoryMeta = 'form_interaction';
        const submitlabelMeta = 'submit';
        const pageType = getPageType();
        const selectedOptionType = dropdown;
        const submitIndexMeta = index;

        const ulContext = document.querySelector('.validation-summary-errors ul');
        var ulElement = ulContext.children;
        var errorString = '';
        for (let i = 0; i < ulElement.length; i++) {
            errorString = errorString + " " + ulElement[i].textContent;
        }
        const errorText = errorString;

        if (submitEventMeta != null) {
            events = submitEventMeta;
        }



        if (submitCategoryMeta != null) {
            category = submitCategoryMeta;
        }
        if (bannerCategoryMeta != null) {
            bannerCategory = bannerCategoryMeta;
        }
        if (submitsubCategoryMeta != null) {
            subCategory = submitsubCategoryMeta;
        }



        if (submitlabelMeta != null) {
            label = submitlabelMeta;
        }
        if (selectedOptionType != null) {
            type = selectedOptionType;
        }



        if (submitIndexMeta != null) {
            index = submitIndexMeta;
        }
	
        $('.termsnconditions').html('By submitting, you agree to our <a href="/terms-and-conditions" class="label-bold">T&Cs</a> and <a href="/privacy-policy" class="label-bold">Privacy Policy</a>.');

        sendAnalyticsData(events, category, bannerCategory, subCategory, pageType, label, index, type, formStatus, errorText);
    });
});


$(".carousel").slick({
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    dots: true,
    autoplay: true,
    prevArrow: '<span class="i-arrow-r slick-prev"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path></svg></span>',
    nextArrow: '<span class="i-arrow-r slick-next"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path></svg></span>',
    responsive: [{
        breakpoint: 600,
        settings: {
            autoplay: false,
        }
    }]
});

$(".our-service .btm").slick({
    arrows: true,
    dots: false,
    infinite: false,
    slidesToScroll: 1,
    slidesToShow: 4,
    prevArrow: $(".arr-left"),
    nextArrow: $(".arr-right"),
    responsive: [{
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

$(".mtcs-mining-leader").slick({
    arrows: true,
    dots: true,
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
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                dots: true
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
    responsive: [{
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

// $(".leadership .btm").slick({
//     arrows: true,
//     dots: false,
//     infinite: false,
//     slidesToScroll: 1,
//     slidesToShow: 3,
//     prevArrow: $(".arr-left"),
//     nextArrow: $(".arr-right"),
//     responsive: [{
//         breakpoint: 1200,
//         settings: {
//             slidesToShow: 3,
//             slidesToScroll: 1,
//         },
//     },
//     {
//         breakpoint: 991,
//         settings: {
//             slidesToShow: 2,
//             slidesToScroll: 1,
//         },
//     },
//     {
//         breakpoint: 600,
//         settings: {
//             slidesToShow: 1,
//             // slidesToShow: 1.2,
//             slidesToScroll: 1,
//             arrows: false
//         },
//     },
//     ],
// });

$(".project-two .btm").slick({
    arrows: true,
    dots: false,
    infinite: false,
    slidesToScroll: 1,
    slidesToShow: 2,
    prevArrow: $(".arr-left-1"),
    nextArrow: $(".arr-right-1"),
    responsive: [{
        breakpoint: 1200,
        settings: {
            slidesToShow: 2,
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