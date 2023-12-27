const locationOrigin = window.location.origin;

function replaceUrlParam(url, paramName, paramValue) {
    if (paramValue == null) {
        paramValue = '';
    }
    let pattern = new RegExp('\\b(' + paramName + '=).*?(&|#|$)');
    if (url.search(pattern) >= 0) {
        return url.replace(pattern, '$1' + paramValue + '$2');
    }
    url = url.replace(/[?#]$/, '');
    return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
}

jQuery(document).ready(function () {
    let getPageUrl = window.location.search;
    let urlParams = new URLSearchParams(getPageUrl);
    let isapp = urlParams.get('isapp');
    if (isapp == 'true') {
        jQuery("a").each(function (index) {
            if (!jQuery(this).attr('href')?.includes("#") && jQuery(this).attr('href') != '') {
                jQuery(this).attr('href', replaceUrlParam(jQuery(this).attr('href'), 'isapp', 'true'))
            }
        });

        jQuery(".city-airport-slider .customslider .services-card__col a:not([data-airportcode='']), .city-airport-slider .customslider .services-card__col a:not([data-title=''])").on('click', function (event) {
            event.preventDefault();
            print = [];
            if (jQuery(this).attr('data-airportcode').trim() != '') {
                print.push("airportcode:" + jQuery(this).attr('data-airportcode').trim());
            }
            if (jQuery(this).attr('data-airportname').trim() != '') {
                print.push("airportname:" + jQuery(this).attr('data-airportname').trim());
            }
            if (jQuery(this).attr('data-title').trim() != '') {
                print.push("title:" + jQuery(this).attr('data-title').trim());
            }
            if (print.length) {
                //TODO - Print.postMessage(print.join(',')); //for temp till app disable the function 
            }
            window.location.href = jQuery(this).attr('href');
        })
    }
});

apiBasePath_array = {
    'https://adanicm.dev.local': 'https://api-dev.adanidigitallabs.com',
    'https://flightsearch.dev': 'https://api-dev.adanidigitallabs.com',
    'https://www.uat.adanione.com': 'https://www.uat.adanione.com/api',
    'https://www.adanione.com' : 'https://www.adanione.com/api',
    'https://www.adanione.cloud': 'https://www.adanione.cloud/api'
};
apiBasePath = apiBasePath_array[locationOrigin] || locationOrigin+'/api';

const apiVersionPath = {
    V1: {
        GETACTIVECART: "/Authenticator/api/MyBooking/GetActiveCart/v2",
        REFRESHTOKEN: "/Identity/api/v2/AuthenticateApiv2/RefreshToken",
        PointBalance: "/loyalty/Loyalty/PointBalanceV2?q=1",
        CARTADDON: "/pranaam/api/PranaamService/CartAddOn",
        ADDTOCARTPRNAAM: "/pranaam/api/PranaamService/RemoveCartItems",
        GETCARTDETAIL: "/Dutyfree/api/DutyFree/GetCartDetails",
        ADDTOCARTDUTYFREE: "/DutyFree/api/DutyFree/AddCart",
        DELETECART: "/authenticator/api/MyBooking/DeleteCart",
    },
    V2: {
        GETACTIVECART: "/authenticatorv2/api/MyBooking/getActiveCart/v2",
        REFRESHTOKEN: "/authenticatorv2/api/v2/AuthenticateApiv2/RefreshToken",
        PointBalance: "/loyaltyv2/point?balanceTypes=available&ref=web",
        CARTADDON: "/pranaamservicev2/api/PranaamService/v1/CartAddOn",
        ADDTOCARTPRNAAM: "/pranaamservicev2/api/PranaamService/V2/RemoveCartItems",
        GETCARTDETAIL: "/dutyfreeservicev2/api/DutyFree/GetCartDetails",
        ADDTOCARTDUTYFREE: "/dutyfreeservicev2/api/DutyFree/AddCart",
        DELETECART: "/authenticatorv2/api/MyBooking/DeleteCart",
    }
}

// Temporary Variable Settings
let environmentAPIVersion_temp = typeof environmentAPIVersion === 'undefined' || environmentAPIVersion == '' ? 'V1': environmentAPIVersion;
const orderRestructuringAPIVersion = apiVersionPath[environmentAPIVersion_temp];

var channelId = 'Web', traceId = 'ADL_WEB_APP', clientId = 'f3d35cce-de69-45bf-958c-4a8796f8ed37';
var deleteSVG = "<svg viewBox='0 0 20 20'><path d='M10.5,0 C12.1568542,0 13.5,1.34314575 13.5,3 L13.5,3 L13.5,4 L18,4 C18.5522847,4 19,4.44771525 19,5 C19,5.55228475 18.5522847,6 18,6 L16.5,6 L16.5,17 C16.5,18.6568542 15.1568542,20 13.5,20 L5.5,20 C3.84314575,20 2.5,18.6568542 2.5,17 L2.5,6 L1,6 C0.44771525,6 6.76353751e-17,5.55228475 0,5 C-6.76353751e-17,4.44771525 0.44771525,4 1,4 L5.5,4 L5.5,3 C5.5,1.40231912 6.74891996,0.0963391206 8.32372721,0.00509269341 L8.5,0 Z M14.5,6 L4.5,6 L4.5,17 C4.5,17.5128358 4.88604019,17.9355072 5.38337887,17.9932723 L5.5,18 L13.5,18 C14.0128358,18 14.4355072,17.6139598 14.4932723,17.1166211 L14.5,17 L14.5,6 Z M7.5,8 C8.05228475,8 8.5,8.44771525 8.5,9 L8.5,15 C8.5,15.5522847 8.05228475,16 7.5,16 C6.94771525,16 6.5,15.5522847 6.5,15 L6.5,9 C6.5,8.44771525 6.94771525,8 7.5,8 Z M11.5,8 C12.0522847,8 12.5,8.44771525 12.5,9 L12.5,15 C12.5,15.5522847 12.0522847,16 11.5,16 C10.9477153,16 10.5,15.5522847 10.5,15 L10.5,9 C10.5,8.44771525 10.9477153,8 11.5,8 Z M10.5,2 L8.5,2 L8.38337887,2.00672773 C7.88604019,2.06449284 7.5,2.48716416 7.5,3 L7.5,3 L7.5,4 L11.5,4 L11.5,3 L11.4932723,2.88337887 C11.4355072,2.38604019 11.0128358,2 10.5,2 L10.5,2 Z'></path></svg>";

const date = new Date();
date.setDate(date.getDate() + 30);

async function ValidateToken(val) {
    new Promise(function(resolve, reject) {
      try {
        if(loginStatus() == 1){
            var base64Url = window?.localStorage?.getItem("token")?.split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            if(moment(JSON.parse(jsonPayload)?.exp).diff(moment().unix()) < 0){
                RefreshToken();
                resolve("done")
                return true;
            } else {
                resolve("done")
                return true;
            }
  
        } else {
          resolve("done")
          return true;
        }
      } catch (error) {
        reject("some error Occurred")
      }
      
    });
}
  
async function RefreshToken() {
    
    if(loginStatus() == 1){
  
       await fetch(apiBasePath+orderRestructuringAPIVersion["REFRESHTOKEN"], {
            method: 'POST',
            headers: {
              "traceId": traceId,
              "channelId":channelId,
              "Access-Control-Allow-Methods":"",
              "Access-Control-Allow-Origin": "*",
              "Content-Type": "application/json",
              "clientId": clientId
            },
            body:JSON.stringify({
              AccessToken: window?.localStorage?.getItem("token"),
              RefreshToken : window?.localStorage?.getItem("rToken")
            })
        })
        .then((res) => res.json())
        .then((data) => {
            if(data?.status){
                localStorage.setItem("token", data?.data?.accessToken);
                localStorage.setItem("rToken", data?.data?.refreshToken);
                return true;
            } else {
                return true;
            }
        });
    } else {
        return true;
    }
}

finalSearchlist = [];

var _defaultBadgeURL = '', _defaultBadgePage = false;
if (document.querySelector("a[data-badgeurl]")) {
    _defaultBadgeURL = document.querySelector("a[data-badgeurl]").getAttribute('data-badgeurl');
    _defaultBadgePage = document.querySelector("a[data-badgeurl]").getAttribute('data-page') ? true : false;
}

function htmlSanitizer(str){
    return str.replace(new RegExp('<[^>]*>', 'g'), '').trim().replace(/[$&+,:;=?@#|'"<>.^*()%!-]+/g,"");
}

function isDeviceiPad() {
    return /Macintosh|iPad|iPhone/.test(navigator.userAgent) && 'ontouchend' in document;
}

function isTablet(){
    const userAgent = navigator.userAgent.toLowerCase();
    return /(ipad|tablet|(android(?!.*mobile))|(windows(?!.*phone)(.*touch))|kindle|playbook|silk|(puffin(?!.*(IP|AP|WP))))/.test(userAgent);
} 

if (isDeviceiPad()) {
    if (document.querySelector(".hero-slider")) {
        document.querySelector(".hero-slider").classList.add('touch');
    }
}

function _userName(){
    var userName;
    if(!localStorage.getItem("userName")){
        userName = "Hi there!";
    } else {
        userName = 'Hi '+htmlSanitizer((localStorage.getItem("userName").split(' '))[0])+'!';
    }
    return userName;
}

function loginStatus(){
    if(!localStorage.getItem("token") || localStorage.getItem("token") == 'Anonymous' || localStorage.getItem("token")==''){
      loginStatusVal = 0;
    } else {
      loginStatusVal = 1;
    }
    return loginStatusVal;
}

var _airportURLName = '';
function airportCodeSet() {
    if (!window?.localStorage?.getItem('userId')) {
        try {
            const fpPromise = import(locationOrigin+'/assets/airport/js/uuid.js').then((FingerprintJS) => FingerprintJS.load());
            fpPromise
                .then((fp) => fp.get())
                .then((result) => {
                    const visitorId = result.visitorId;
                    window?.localStorage?.setItem('userId', visitorId);
                });
        } catch (error) {
            console.log(error);
        }
    }
    localStorage.setItem("airportCode", document.querySelector('#airportCode') ? document.querySelector('#airportCode').value : '');
    setCookieVal('airportCode', document.querySelector('#airportCode') ? document.querySelector('#airportCode').value : '');
    localStorage.setItem("airportURLName", document.querySelector('#airportUrl') ? document.querySelector('#airportUrl').value : '');
    _airportURLName = document.querySelector('#airportUrl') ? document.querySelector('#airportUrl').value : '';
    localStorage.setItem("airportServiceItemPath", document.querySelector('#airportService_itemPath') ? document.querySelector('#airportService_itemPath').value : '');
}

airportCodeSet();

function displayListGroup() {
    var list = document.querySelector("#list-display");
    const selected1 = document.querySelector(".selected1");
    const optionList = document.querySelectorAll(".option");
    optionList.forEach((o) => {
        o.addEventListener("click", () => {
            selected1.src = o.querySelector("img").src;
            list.classList.add("hide");
        });
    });
}

function usernameSet() {
    if (document.querySelector('.usernameSet')) {
        UserNameset = document.querySelectorAll('.usernameSet');
        UserNameset.forEach((o) => {
            if (loginStatus() == 0 || !localStorage.getItem("userName")) {
                o.innerHTML = "there";
            } else {
                if(_userName()=='Hi there!') {
                  o.innerHTML = 'there';
                } else {
                  o.innerHTML = htmlSanitizer((localStorage.getItem("userName").split(' '))[0]);
                }
            }
        });
    }
}

usernameSet();

function getAirportListCanvas() {
    const { innerWidth: width, innerHeight: height } = window;
    if (width > 992) {
    } else {
        html = document.querySelector(".main-header .brandBox .selectLocation .locationLayer").innerHTML;
        document.querySelector("#airportListBody").innerHTML = html;
    }
}

floatingHeadFlag = false;
floatingHead = document.querySelector(".floating-header");
if (typeof (floatingHead) != 'undefined' && floatingHead != null) {
    floatingHeadFlag = true;
}

document.addEventListener("scroll", function () {
    headerHeight = document.querySelector(".main-header")?.clientHeight + parseInt(jQuery(".main-header")?.css("marginBottom")?.replace('px', ''));
    if (document.querySelector(".main-header") != null) {
        if (window.innerWidth > 992) {
            if ((document.body.scrollTop || document.documentElement.scrollTop) >= headerHeight) {
                document.querySelector("body").classList.add("sticky-header");
                document.querySelector(".main-header").classList.remove("floating-header");
                if (!floatingHeadFlag) {
                    document.querySelector("#header").style.height = headerHeight + 'px';
                }
            } else {
                document.querySelector("body").classList.remove("sticky-header");
                if (floatingHeadFlag) {
                    document.querySelector(".main-header").classList.add("floating-header");
                } else {
                    document.querySelector("#header").style.height = headerHeight + 'px';
                }
            }
        } else {
            if ((document.body.scrollTop || document.documentElement.scrollTop) >= 5) {
                document.querySelector("body").classList.add("sticky-header");
            } else {
                document.querySelector("body").classList.remove("sticky-header");
            }

            if (document.querySelector(".airportServiceListBar")) {
                document.querySelector("#header").classList.add("noSticky");
            } else {
                document.querySelector("#header").classList.remove("noSticky");
            }
        }

    }
    if (
        document.querySelector("#navbar-tabs") &&
        window.getComputedStyle(document.querySelector("#navbar-tabs")).display !=
        "none"
    ) {
        if (document.querySelector(".tab-pane.active #navbar-tabs")) {
            eleObj = document.querySelector(".tab-pane.active #navbar-tabs");
        } else {
            eleObj = document.querySelector("#navbar-tabs");
        }
        if (document.getElementById("header").clientHeight < (document.body.scrollTop || document.documentElement.scrollTop)) {
            if (
                eleObj.getBoundingClientRect().top - document.querySelector(".main-header .main-nav").clientHeight <= 0 &&
                eleObj.getBoundingClientRect().bottom - eleObj.clientHeight >= 0
            ) {
                document.querySelector("#header").classList.add("animatedHidden");
            } else {
                document.querySelector("#header").classList.remove("animatedHidden");
            }
        } else {
        }
    }
});

const toggleVisible = () => {
    const scrolled =
        document.body.scrollTop || document.documentElement.scrollTop;
    const classdata = document.getElementsByClassName("backToTop")[0];

    footerElement = document.querySelector(".footer_wrapper");
    var footerStyle =
        footerElement.currentStyle || window.getComputedStyle(footerElement);
    footerTopMargin = Number(footerStyle.marginTop.slice(0, -2));

    footeroffset =
        footerElement.offsetTop +
        footerTopMargin +
        document.querySelector(".bottomNav").clientHeight -
        window.innerHeight;

    const { innerWidth: width, innerHeight: height } = window;
    if (scrolled > 300) {
        classdata && (classdata.style.display = "inline-block");
    } else if (scrolled <= 300) {
        classdata && (classdata.style.display = "none");
    }
    if (
        footeroffset <
        (document.body.scrollTop || document.documentElement.scrollTop)
    ) {
        document.getElementsByClassName("backToTop")[0].classList.add("active");
    } else {
        document.getElementsByClassName("backToTop")[0].classList.remove("active");
    }
};

const scrollToTop = () => {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    });
};
window.addEventListener("scroll", toggleVisible);

function selectFooterMenu(obj, e) {
    [...obj.parentElement.children].forEach((sib) =>
        sib.classList.remove("active")
    );
    obj.classList.add("active");
}

if (document.getElementById('offcanvasBottom')) {
    var myOffcanvas = document.getElementById('offcanvasBottom')
    myOffcanvas.addEventListener('hidden.bs.offcanvas', function () {
        if (document.querySelector('.offcanvas-backdrop.show')) {
            document.querySelector('.offcanvas-backdrop.show').remove();
        }
    })
}

function createRipple(e, colorFlag = false) {
    const button = e.currentTarget;
    button.classList.add("ripple_root");
    var children = button.getElementsByClassName("ripple_waves");
    while (children.length > 0) {
        children[0].parentNode.removeChild(children[0]);
    }

    var circle = document.createElement("span");
    circle.style["position"] = "absolute";
    circle.style["pointer-events"] = "none";
    circle.style["border-radius"] = "inherit";
    circle.style["z-index"] = "0";
    circle.style["overflow"] = "hidden";
    button.appendChild(circle);

    var d = Math.max(button.clientWidth, button.clientHeight);
    var eRect = button.getBoundingClientRect();

    circle.style.width = circle.style.height = d + "px";
    circle.style.left = e.clientX - eRect.left - d / 2 + "px";
    circle.style.top = e.clientY - eRect.top - d / 2 + "px";
    circle.classList.add("ripple_waves");
    if (colorFlag == true) {
        circle.classList.add("primary_waves");
    }
}

const rippleList = document.querySelectorAll("#header.sticky .main-header .dropdown-toggle,#header:not(.sticky) .main-header .dropdown-toggle, .hamburger_section a.menu_item,#header .selectLocation .location-arrow, .slick-arrow, .accordion-button, .header-nav .nav-tabs .nav-link, .backToTop.active,.bottomNav a,.airport_main_wrap .hero-slider .banner_slide .banner-content .know-more-btn,.servicesPrev,.servicesNext,.lightRipple");
rippleList.forEach((o) => {
    o.addEventListener("click", () => {
        createRipple(event)
    });
});

const rippleList1 = document.querySelectorAll("#header:not(.sticky) .floating-header .dropdown-toggle, button.btn-outline-primary, button.btn-outline-dark:not(.know-more-btn), button.btn-dark, .primary_waves");
rippleList1.forEach((o) => {
    o.addEventListener("click", () => {
        createRipple(event, true)
    });
});

if (window.innerWidth < 992) {
    const rippleList2 = document.querySelectorAll(".footer_link_heading");
    rippleList2.forEach((o) => {
      o.addEventListener("click", () => {
        createRipple(event)
      });
    });
}

/* Custom slider */
function sliderNavigation(obj) {
    const { innerWidth: width, innerHeight: height } = window;
    if (width > 992) {
        elm = document.getElementById(obj);
        testimonials = elm.querySelector('.customslider');
        totalWidth = testimonials.querySelectorAll('.services-card .services-card__col').length * testimonials.querySelector('.services-card .services-card__col').clientWidth;
        if(testimonials.clientWidth >= totalWidth){
            elm.querySelector('.servicesPrev').style.setProperty('display', 'none', 'important');
            elm.querySelector('.servicesNext').style.setProperty('display', 'none', 'important');
        } else {
            elm.querySelector('.servicesPrev').style.setProperty('display', 'flex', 'important');
            elm.querySelector('.servicesNext').style.setProperty('display', 'flex', 'important');
        }
    }
}

function scrollToNextItem(obj) {
    testimonials = obj.closest('.customslider');
    scroller = testimonials.querySelector('.services-card');
    itemWidth = testimonials.querySelector('.services-card__col');
    scrollItemWidth = itemWidth.clientWidth * parseInt(testimonials.clientWidth / itemWidth.clientWidth);// * parseInt(testimonials.clientWidth/testimonials.querySelector('.services-card__col').clientWidth);
    scroller.scrollBy({ left: scrollItemWidth, top: 0, behavior: 'smooth' });
    if (scroller.scrollLeft + scroller.clientWidth + scrollItemWidth >= (scroller.scrollWidth)) {
        obj.classList.add("slick-disabled");
    } else {
        obj.classList.remove("slick-disabled");
    }
    testimonials.querySelector('.servicesPrev').classList.remove("slick-disabled");
}
function scrollToPrevItem(obj) {
    testimonials = obj.closest('.customslider');
    scroller = testimonials.querySelector('.services-card');
    itemWidth = testimonials.querySelector('.services-card__col');
    scrollItemWidth = itemWidth.clientWidth * parseInt(testimonials.clientWidth / itemWidth.clientWidth);// * parseInt(testimonials.clientWidth/testimonials.querySelector('.services-card__col').clientWidth);

    if (scroller.scrollLeft != 0) {
        scroller.scrollBy({ left: -scrollItemWidth, top: 0, behavior: 'smooth' });
        obj.classList.remove("slick-disabled");
        if (scroller.scrollLeft - scrollItemWidth <= 0) {
            obj.classList.add("slick-disabled");
        } else {
            obj.classList.remove("slick-disabled");
        }
    }
    else {
        obj.classList.add("slick-disabled");
    }
    testimonials.querySelector('.servicesNext').classList.remove("slick-disabled");
}

/* Custom slider */

/* For cart Detail */
cartNo = 0;
var addedCartQList = {};
function getCartNO(urlParam, headerParam, serviceName) {
    fetch(urlParam, {
        method: 'GET',
        headers: headerParam
    })
        .then((response) => response.json())
        .then((jsonValue) => {
            if (serviceName == 'Pranaam') {
            }
            if (serviceName == 'Dutyfree') {
                if ((jsonValue.data.itemDetails == null ? 0 : jsonValue.data.itemDetails.length) > 0) {
                    addedCartQList = {};
                    jsonValue.data.itemDetails.forEach(function (itemsDetail) {
                        addedCartQList['item_' + itemsDetail.skuCode+'_'+itemsDetail.storeType] = itemsDetail.quantity;
                    });
                    setAddToCartQuantity();
                }
            }
        })
        .catch((error) => {
            console.log('Error:', error);
        });
}

function getTotalBalance() {
    //const mob = localStorage.getItem("userId");
    if (loginStatus() == 1) {
        fetch(apiBasePath + orderRestructuringAPIVersion["PointBalance"], {
            method: 'GET',
            headers: {
                "Authorization": 'bearer ' + window?.localStorage?.getItem("token")
            }
        })
            .then((res) => res.json())
            .then((data) => {
                var ele = document.getElementById("reward_balance");
                var ele_hum = document.getElementById("reward_balance_hamburger");
                LoyaltyBalance = 0;
                if (data?.availableBalancePoints) {
                    if (data.availableBalancePoints) {
                        LoyaltyBalance = (data.availableBalancePoints).toLocaleString();
                    }
                }
                ele.innerHTML = LoyaltyBalance;
                ele_hum.innerHTML = LoyaltyBalance;

                rewardCoins = document.querySelectorAll(".rewardCoins");
                loyaltyCntr = document.querySelectorAll(".loyaltyCoins, #rewardsPopup .points");
                loyaltyCntr.forEach((o) => {
                    if (o.querySelector('.loader')) {
                        o.querySelector('.loader').classList.add('d-none');
                        if (o.querySelector('.loyaltyDetail')) {
                            o.querySelector('.loyaltyDetail').classList.remove('d-none');
                        }
                    }
                });
                if (loginStatus() == 1) {
                    rewardCoins.forEach((o) => {
                        o.classList.remove('d-none');
                    });
                } else {
                    rewardCoins.forEach((o) => {
                        o.classList.add('d-none');
                    });
                }
                var point = document.querySelector("#rewardsPopup .points");
                if (point) {
                    if (window?.localStorage?.getItem("showLoyaltyPopup") == 'true') {
                        point.querySelector('.amount').classList.remove('d-none');
                    }
                }
            })
            .catch((error) => {
                if (document.getElementById("reward_balance")) {
                    document.getElementById("reward_balance").innerHTML = 0;
                }
                if (document.getElementById("reward_balance_hamburger")) {
                    document.getElementById("reward_balance_hamburger").innerHTML = 0;
                }
                loyaltyCntr = document.querySelectorAll(".loyaltyCoins")
                loyaltyCntr.forEach((o) => {
                    if (o.querySelector('.loader')) {
                        o.querySelector('.loader').classList.add('d-none');
                        o.querySelector('.loyaltyDetail').classList.remove('d-none');
                    }
                });
                rewardCoins = document.querySelectorAll(".rewardCoins")
                if (loginStatus() == 1) {
                    rewardCoins.forEach((o) => {
                        o.classList.remove('d-none');
                    });
                } else {
                    rewardCoins.forEach((o) => {
                        o.classList.add('d-none');
                    });
                }
                console.log('Error:', error);

            });
    }
}

getTotalBalance();

previousBadgeVal = 0;
sparcleFlag = false;
function cardBadgeToggle(countValue) {
    var element = document.querySelector(".header-nav .badge")
    var footerElement = document.querySelector(".bottomNav .badge");
    if (typeof(element) != 'undefined' && element != null)
    {
      if(countValue!=0){
        element.classList.remove('d-none');
        element.querySelector('label').innerHTML = countValue ;
        footerElement.innerHTML = countValue;
        if(sparcleFlag == false){
          sparcleFlag = true;
          previousBadgeVal = countValue;
        }
        if(previousBadgeVal<countValue && sparcleFlag == true){
            element.classList.add('sparcle');
            footerElement.closest('a').classList.add('sparcle');
            setTimeout(() => {
              element.classList.remove('sparcle');
              footerElement.closest('a').classList.remove('sparcle');
            }, 800);
        }
        previousBadgeVal = countValue;
      } else {
        footerElement.innerHTML = '';
        element.querySelector('label').innerHTML = '';
        element.classList.add('d-none');
        cartLink(_defaultBadgeURL);
      }
    }
}

setCartPath = {
    'FlightBooking': 'javascript:void(0);',
    'Pranaam': _airportURLName+'/pranaam-services/checkout',
    'Dutyfree': document.querySelector("#cart_link") ? document.querySelector("#cart_link").value : '/duty-free/checkout'
};

function cartLink(urlPath) {
    var element = document.querySelector(".header-nav .badge").closest('a');
    var footerElement = document.querySelector(".bottomNav .badge").closest('a');
    element.setAttribute('href', urlPath);
    footerElement.setAttribute('href', urlPath);
}

function updateAddon(addOnServiceID, operation, packageId, price, quantity, serviceDescription, serviceName) {
    var Addon_Headers = new Headers();
    Addon_Headers.append('traceId', traceId);
    Addon_Headers.append('channelId', channelId);
    Addon_Headers.append("Access-Control-Allow-Methods", "GET,POST,PATCH");
    Addon_Headers.append("Access-Control-Allow-Origin", "*");
    Addon_Headers.append('Content-Type', 'application/json');
    if (loginStatus() == 1) {
        Addon_Headers.append('Authorization', 'bearer ' + localStorage.getItem('token'));
    } else if (localStorage.getItem('token') !== '') {
        Addon_Headers.append('agentId', localStorage.getItem('userId') || "");
    }
    fetch(apiBasePath + orderRestructuringAPIVersion["CARTADDON"], {
        method: 'POST',
        headers: Addon_Headers,
        body: JSON.stringify(
            {
                "addOnServiceID": addOnServiceID,
                "operation": addOnServiceID,
                "packageId": packageId,
                "price": price,
                "quantity": quantity,
                "serviceDescription": serviceDescription,
                "serviceName": serviceName,
                "userId": localStorage.getItem('userId') || ""
            }
        )
    })
    .then((response) => response.json())
    .then((data) => {
        getCartId();
    })
    .catch((error) => {
        console.log('Error:', error);
        if(document.querySelector('.cart_dropdown button.remove')){
            document.querySelectorAll('.cart_dropdown button.remove').forEach((o) => {
                loaderShow(false,o);
            });
        }
    });

}

function removeCartPranaam() {
    var Addon_Headers = new Headers();
    Addon_Headers.append('traceId', traceId);
    Addon_Headers.append('channelId', channelId);
    Addon_Headers.append("Access-Control-Allow-Methods", "GET,POST,PATCH");
    Addon_Headers.append("Access-Control-Allow-Origin", "*");
    Addon_Headers.append('Content-Type', 'application/json');
    if (loginStatus() == 1) {
        Addon_Headers.append('Authorization', 'bearer ' + localStorage.getItem('token'));
    } else if (localStorage.getItem('token') !== '') {
        Addon_Headers.append('agentId', localStorage.getItem('userId') || "");
    }
    fetch(apiBasePath + orderRestructuringAPIVersion["ADDTOCARTPRNAAM"], {
        method: 'DELETE',
        headers: Addon_Headers
    })
    .then((response) => response.json())
    .then((data) => {
        getCartId();
    })
    .catch((error) => {
        console.log('Error:', error);
        if(document.querySelector('.cart_dropdown button.remove')){
            document.querySelectorAll('.cart_dropdown button.remove').forEach((o) => {
                loaderShow(false,o);
            });
        }
    });

}

function loaderShow(flag,obj){
    if(flag){
      obj.querySelector('.text').classList.add('d-none');
      obj.querySelector('.loader').classList.remove('d-none');
    } else {
      obj.querySelector('.loader').classList.add('d-none');
      obj.querySelector('.text').classList.remove('d-none');
    }
}

function formatPrice(n) {
    numberValue = Number(n)
    var result = (n - Math.floor(n)) !== 0;   
    if (result) {
        return numberValue.toLocaleString(undefined, {minimumFractionDigits: 2, maximumFractionDigits: 2}) ;
    } else {
        return numberValue.toLocaleString();
    }
}

headerCartFlag = false;
function headerCartList(hclJson,URL){

  miniCartListHtml = '';
  hclJson?.data?.map((widgetItem) => {
    if(!hclJson?.pranaamCartItem) {
        miniCartListHtml +="<li>"+
          "<figure>"+
          "<a href='"+URL+"/duty-free/p/"+((widgetItem.skuName).replaceAll(' ', '-').toLowerCase().trim())+"/"+widgetItem.skuCode+"'><img src='"+widgetItem.productImage+"' alt='' loading='lazy'></a>"+
          "</figure>"+
          "<aside>"+
            "<div class='titleButton'>"+
                "<h5><a href='"+URL+"/duty-free/p/"+((widgetItem.skuName).replaceAll(' ', '-').toLowerCase().trim())+"/"+widgetItem.skuCode+"'>"+widgetItem.skuName+"</a></h5>"+
                "<button class='remove remove_du' data-skucode='"+widgetItem.skuCode+"' data-storetype='"+hclJson.productType+"' data-airportcode='"+hclJson.pickUpLocationCode+"' onclick='loaderShow(true,this);addToCart(event,this,0,true)'><span class='text'>"+deleteSVG+"</span>"+
                    "<span class='loader d-none'><span class='loaderDot' style='background: rgb(0, 0, 0);'></span><span class='loaderDot' style='background: rgb(0, 0, 0);'></span><span class='loaderDot' style='background: rgb(0, 0, 0);'></span></span>"+
                "</button>"+
            "</div>"+
            "<div class='qty'>"+
              "<span>Qty "+widgetItem.quantity+"</span>"+
            "</div>"+
            "<div>"+
            "<span class='price'>₹"+formatPrice(widgetItem.productPrice)+"</span>"+
            (widgetItem.offerPrice != 0 && widgetItem.offerPrice != widgetItem.productPrice ? (
              "<del>₹"+formatPrice(widgetItem.offerPrice)+"</del>"
            ) : (
              "<del></del>"
            ))+
            (widgetItem.discountType != false ? 
            (widgetItem.discountType == 1 ? (
              "<br /><span class='off'>"+formatPrice(widgetItem.offerOff)+"% OFF</span>"
            ) : (
              widgetItem.discountType == 2 && (
                "<br /><span class='off'>₹"+formatPrice(widgetItem.offerOffAmount)+" OFF</span>"
              )
            )):'')+
            "</div>"+
          "</aside>"+
        "</li>"

    } else {
      action = (widgetItem.addOn ? "loaderShow(true,this); updateAddon("+widgetItem.addOnServiceID+","+widgetItem.operation+","+widgetItem.packageId+","+widgetItem.productPrice+",0,\""+widgetItem.serviceDescription+"\",\""+widgetItem.skuName+"\")" : 'loaderShow(true,this); removeCartPranaam()') //);
      miniCartListHtml +="<li>"+
          "<figure class='"+(widgetItem.addOn ? '' : (widgetItem.productImage!=''?'':'prnaam-fallback'))+"' >"+
            "<img class='"+(widgetItem.addOn ? 'coverFit' : '')+"' src='"+(widgetItem.productImage!=''? widgetItem.productImage:( widgetItem.addOn? '/assets/airport/images/header/porter.jpeg' :'/assets/airport/images/header/PranaamLogo.svg' ))+"' loading='lazy' alt=''>"+
          "</figure>"+
          "<aside>"+
            "<div class='titleButton'>"+
                "<h5>"+widgetItem.skuName+"</h5>"+
                "<button class='remove' onclick='"+action+"'><span class='text'>"+deleteSVG+"</span>"+
                    "<span class='loader d-none'><span class='loaderDot' style='background: rgb(0, 0, 0);'></span><span class='loaderDot' style='background: rgb(0, 0, 0);'></span><span class='loaderDot' style='background: rgb(0, 0, 0);'></span></span>"+
                "</button>"+
            "</div>"+
            (widgetItem.addOn? '': (
                (hclJson?.pranaamCartItem == true ? "<p class='"+(hclJson.productType == 'round trip' ? 'pranaamRoundTrip' : '')+"'>"+widgetItem.productLocation+"</p>" : '')+
                "<div class='capitalTxt'>"+
                (hclJson.productType == 'round trip' ? 'Departure' : hclJson.productType)+
                " ("+widgetItem.travelSector+")"+
                "</div>"+
                "<div class='timeNumber'>"+
                    "<span>"+widgetItem.flightTime+"</span>"+
                    "<span>"+widgetItem.flightNumber+"</span>"+
                "</div>"+
                ( widgetItem.arrival.flightTime? 
                    "<div class='capitalTxt'>"+
                    (hclJson.productType == 'round trip' ? 'Arrival' : hclJson.productType)+
                    " ("+widgetItem.arrival.travelSector+")"+
                    "</div>"+
                    "<div class='timeNumber'>"+
                    "<span>"+widgetItem.arrival.flightTime+"</span>"+
                    "<span>"+widgetItem.arrival.flightNumber+"</span>"+
                    "</div>" : ""
                )
              )
            )+
            (widgetItem.addOn? (
                "<div class='qty'>"+
                  "<span>Qty "+widgetItem.quantity+"</span>"+
                "</div>"
            ): '' )+
            "<div class='qty'>"+
            "<span class='price'>₹"+formatPrice(widgetItem.productPrice)+"</span>"+
            "</div>"+
          "</aside>"+
        "</li>"
    }

  }); 

    pickupLocHtml = hclJson.pickupLocation != false ? "<div class='pickupLoc'>"+
                      "<i class='i-map'></i>"+
                      "Pickup : <strong> "+(hclJson.pickupLocation?.split('-')[0])+" - <span>"+(hclJson.pickupLocation?.split('-')[1])+"</span></strong>"+
                    "</div>" : '';

    miniCartHtml = "<div class='headerCartList'>"+
                  "<div class='headerClose'>"+
                    "<aside>"+
                      "<strong>My Cart</strong>"+
                      "<span>("+hclJson.cartItems+")</span>"+
                    "</aside>"+
                  "</div>"+
                  "<div class='CartListItems "+(hclJson.pickupLocation != false? '': 'pranaamItems')+" scroll-visible'>"+
                    pickupLocHtml+
                    "<ul>"+
                    miniCartListHtml+
                    "</ul>"+
                  "</div>"+
                  "<div class='bottomBar'>"+
                  "<div class='totalPriceRow'>Total ₹"+(formatPrice(hclJson.totalPrice))+"</div>"+
                    "<a href='"+hclJson.cartPage+"' type='button' class='adl-button btn btn-primary'>Go to Cart</a>"+
                  "</div>"+
                "</div>";
    document.querySelector("#cartListDP").innerHTML = miniCartHtml;
    if(miniCartListHtml != ''){
        document.querySelector("#cartListDP").classList.remove('d-none');
    } else {
        document.querySelector("#cartListDP").classList.add('d-none');
    }
    if(headerCartFlag){
        document.querySelector(".cart_dropdown").classList.add("cart_dropdown_show")
        setTimeout(() => {
          document.querySelector(".cart_dropdown").classList.remove("cart_dropdown_show")
        }, 3000);
    }
  
}


function emptyCart(){

    miniCartemptyHtml = '';
    
    if (window.innerWidth >= 992 || isTablet()) {
  
      miniCartemptyHtml ="<div class='headerCartList'>"+
          "<div class='headerClose'>"+
              "<aside><strong>My Cart</strong></aside>"+
          "</div>"+
          "<div class='CartListItems scroll-visible'>"+
            "<div class='emptyWrapper  '>"+
                "<figure><img src='/assets/airport/images/header/empty_cart.gif' loading='lazy' alt='Fail to load data'></figure>"+
                "<h6>Your cart looks empty.</h6>"+
            "</div>"+
          "</div>"+
      "</div>";
      document.querySelector("#cartListDP").innerHTML = miniCartemptyHtml;
      document.querySelector("#cartListDP").classList.remove('d-none');
  
    }  
    if (window.innerWidth < 992) {
      miniCartemptyHtml =
          "<div class='headerClose'>"+
              "<button type='button' class='icon_button' onclick='emptyCartToggle(false)'>"+
                "<i class='i-cross'></i>"+
              "</button>"+
              "<aside><strong>Cart</strong></aside>"+
          "</div>"+
          "<div class='CartListItems'>"+
            "<div class='emptyWrapper  '>"+
                "<figure><img src='/assets/airport/images/header/empty_cart.gif' loading='lazy' alt='Fail to load data'></figure>"+
                "<h6>Your cart looks empty.</h6>"+
            "</div>"+
          "</div>";
      
  
      document.querySelector("#emptyCart_overLay").innerHTML = miniCartemptyHtml;
  
    }
  
}
  
function emptyCartToggle(flag=false, htmlBuild = false){
    if(htmlBuild){
        emptyCart();
    }
    if(flag){
        document.querySelector("#emptyCart_overLay").classList.remove('d-none');
    } else {
        document.querySelector("#emptyCart_overLay").classList.add('d-none');
    }  
}

var _cartId = '';
function getCartId() {
    ValidateToken().then((val)=>{
    }).catch((err)=>{
      console.log(err)
    }).finally(()=>{
    if(document.querySelector(".header-nav .badge") || document.querySelector(".bottomNav .badge")){
        if (localStorage.getItem('userId')) {
            var getActiveCart_Headers = new Headers();
            getActiveCart_Headers.append('traceId', traceId);
            getActiveCart_Headers.append('channelId', channelId);
            if (loginStatus() == 0) {
                getActiveCart_Headers.append('annonymousId', localStorage.getItem('userId') || "");
            } else {
                getActiveCart_Headers.append('clientId', clientId);
                getActiveCart_Headers.append('Authorization', 'bearer ' + localStorage.getItem('token'));
            }

            fetch(apiBasePath + orderRestructuringAPIVersion["GETACTIVECART"], {
                method: 'GET',
                headers: getActiveCart_Headers
            })
                .then((response) => response.json())
                .then((data) => {
                    _cartId = ''; 
                    if (data?.data?.businessSubType) {
                        _cartId = data?.data?.cartId ;

                        _headerCartListJSON = {};
                        if(data?.data?.businessSubType  && (data?.data?.businessSubType == "Dutyfree" || data?.data?.businessSubType == "Pranaam" || data?.data?.businessSubType == "FnB" ) ){
                            _headerCartListJSON["cartItems"]= data?.data?.productCount;
                        }

                        redirectionLink = '';
                        if(data?.data?.businessSubType  && data?.data?.businessSubType == "Dutyfree" && data?.data?.productDetails?.Dutyfree?.pickUpLocationCode) {
                            _headerCartListJSON["pickupLocation"] = data?.data?.productDetails?.Dutyfree?.pickUpLocation;
                            _headerCartListJSON["pranaamCartItem"] = false;  
                            _headerCartListJSON["pickUpLocationCode"] =  data?.data?.productDetails?.Dutyfree?.pickUpLocationCode;
                            _headerCartListJSON["productType"] =  data?.data?.productType;
                            dataList = [];

                            data?.data?.productDetails?.Dutyfree?.productLists?.map((widgetItem) => {
                                dataList.push({ 
                                    offerOff: widgetItem?.discountPercentage, 
                                    offerPrice: widgetItem?.totalUnitPrice,
                                    offerOffAmount: widgetItem?.discountPrice,
                                    discountType: widgetItem?.discountType,
                                    productImage: widgetItem?.productImage, 
                                    quantity: widgetItem?.quantity, 
                                    skuName: widgetItem?.productTitle,
                                    productPrice: widgetItem?.totalDiscountPrice,
                                    skuCode: widgetItem?.productId
                                })
                            });
                            _headerCartListJSON["totalPrice"] = data?.data?.productDetails?.Dutyfree?.amount;
                            _headerCartListJSON["data"] = dataList;
                            _headerCartListJSON["cartPage"] = "/"+_adaniairportlistJSON[data?.data?.productDetails?.Dutyfree?.pickUpLocationCode]+'/duty-free/checkout';
                            
                            cartLink("/"+_adaniairportlistJSON[data?.data?.productDetails?.Dutyfree?.pickUpLocationCode]+'/duty-free/checkout');
                            redirectionLink = "/"+_adaniairportlistJSON[data?.data?.productDetails?.Dutyfree?.pickUpLocationCode];
                        } else if(data?.data?.businessSubType  && data?.data?.businessSubType == "Pranaam" && data?.data?.productDetails?.Pranaam?.productDetail?.serviceAirportCode) {

                            _headerCartListJSON["pickupLocation"] = false;
                            _headerCartListJSON["pranaamCartItem"] = true;  
                            _headerCartListJSON["productType"] = data?.data?.productType;
                            dataList = [];
                            if(data?.data?.productDetails?.Pranaam?.productDetail){
                                widgetItem = data?.data?.productDetails?.Pranaam?.productDetail;

                                let flightServiceTime = widgetItem?.flightTime;
                                if (moment(flightServiceTime).isValid()) {
                                    flightServiceTime = moment(flightServiceTime).format('D MMM YYYY, h:mm a');
                                }

                                let flightServiceTime_round = widgetItem?.arrival?.flightTime;
                                if (moment(flightServiceTime_round).isValid()) {
                                    flightServiceTime_round = moment(flightServiceTime_round).format('D MMM YYYY, h:mm a');
                                }

                                let isPranaamStandAloneBooking = widgetItem?.pranaamBookingType ? ["standaloneporterbooking","standalonewheelchairbooking"].includes(widgetItem?.pranaamBookingType?.toLowerCase()) : false;

                                dataList.push({ 
                                    flightNumber: widgetItem?.flightName,
                                    flightTime: flightServiceTime,
                                    productId: widgetItem?.productId,
                                    productImage: widgetItem?.productImage, 
                                    productLocation: widgetItem?.serviceTo + (!isPranaamStandAloneBooking ? " ("+widgetItem?.guest+" Guest"+(Number(widgetItem?.guest)>1?'s':'')+")" : ""),
                                    productPrice: widgetItem?.amount,
                                    skuCode: widgetItem?.productId, 
                                    skuName: widgetItem?.productName,
                                    travelSector: widgetItem?.travelSector,
                                    arrival: {
                                        flightName: widgetItem?.arrival?.flightNumber,
                                        flightNumber: widgetItem?.arrival?.flightName,
                                        flightTime: flightServiceTime_round,
                                        travelSector: widgetItem?.arrival?.travelSector
                                    }
                                })

                                if (data?.data?.productDetails?.Pranaam?.addOnDetail) {
                                    data?.data?.productDetails?.Pranaam?.addOnDetail?.map((addOn) => {
                                        dataList?.push({
                                            addOn: true,
                                            addOnServiceID: addOn?.addOnServiceId,
                                            packageId: addOn?.addOnPackageId,
                                            productImage: addOn?.packageImage,
                                            productPrice: addOn?.amount,
                                            quantity: addOn?.quantity,
                                            serviceDescription: addOn?.addOnServiceDescription,
                                            skuName: addOn?.addOnName,
                                            operation: 3,
                                        });
                                    });
                                }
                            }
                            
                            _headerCartListJSON["totalPrice"] = data?.data?.productDetails?.Pranaam?.amount;
                            _headerCartListJSON["data"] = dataList;
                            _headerCartListJSON["cartPage"] = "/"+_adaniairportlistJSON[data?.data?.productDetails?.Pranaam?.productDetail?.serviceAirportCode]+'/pranaam-services/checkout';
                            cartLink("/"+_adaniairportlistJSON[data?.data?.productDetails?.Pranaam?.productDetail?.serviceAirportCode]+'/pranaam-services/checkout');
                            redirectionLink = "/"+_adaniairportlistJSON[data?.data?.productDetails?.Pranaam?.productDetail?.serviceAirportCode];
                        } else if(data?.data?.businessSubType  && data?.data?.businessSubType == "FnB" && data?.data?.productDetails?.FnBProduct?.pickUpLocationCode){
                            cartLink("/"+_adaniairportlistJSON[data?.data?.productDetails?.FnBProduct?.pickUpLocationCode]+'/food-beverages/checkout');
                        } else {
                            cartLink('/empty-cart');
                        }
                        
                        if(window.innerWidth >=768){
                            headerCartList(_headerCartListJSON,redirectionLink);
                        }
                        
                        cardBadgeToggle(Number(data.data.productCount));

                        var GetCartDetails_Headers = new Headers();
                        GetCartDetails_Headers.append('traceId', traceId);
                        GetCartDetails_Headers.append('channelId', channelId);
                        if (loginStatus() == 0) {
                            GetCartDetails_Headers.append('agentId', localStorage.getItem('userId') || "");
                            GetCartDetails_Headers.append('anonymousUserId', '');
                        } else {
                            GetCartDetails_Headers.append('clientId', clientId);
                            GetCartDetails_Headers.append('Authorization', 'bearer ' + localStorage.getItem('token'));
                        }
                        if (data?.data?.businessSubType && data?.data?.businessSubType == "Dutyfree") {
                            getCartNO(apiBasePath + orderRestructuringAPIVersion["GETCARTDETAIL"], GetCartDetails_Headers, 'Dutyfree');
                        }
                    }
                    else if (data?.code == "ADLMS03") {
                        cardBadgeToggle(0);
                        addedCartQList = {};
                        setAddToCartQuantity();
                        if(_defaultBadgePage){
                            if(window.innerWidth >=768){
                                emptyCart()
                            }
                            if(document.querySelector(".header-nav .badge")){
                                document.querySelector(".header-nav .badge").closest('a').setAttribute('href','javascript:void(0)')
                            }
                            if(document.querySelector(".bottomNav .badge")){
                                document.querySelector(".bottomNav .badge").closest('a').setAttribute('href','javascript:void(0)')
                                document.querySelector(".bottomNav .badge").closest('a').setAttribute('onclick','emptyCartToggle(true, true)');
                            }
                        } else {
                            document.querySelector("#cartListDP").innerHTML = '';
                            document.querySelector("#cartListDP").classList.add('d-none');
                        }
                    }
                    headerCartFlag = true;
                })
                .catch((error) => {
                    console.log('Error:', error);
                    cartNo = 0;
                    cardBadgeToggle(cartNo);
                    cartLink(_defaultBadgeURL);
                });
        }
    }
    })
}

getCartId();

/* For cart Detail */

function logoutUserProfile(){
    if (window.innerWidth >= 992) {
      jQuery("#logoutPopup").modal('show');
    } else {
      jQuery('#logout_offcanvas').offcanvas('show')
    }
  }
  
  function logoutUserProfileAccept(){
    adlstoreValue = '';
    if (window?.localStorage?.getItem('adlstore')) {
        adlstoreValue = window?.localStorage?.getItem('adlstore')
    }
    localStorage.clear();
    if (adlstoreValue != '') {
        localStorage.setItem("adlstore", adlstoreValue);
    }
    deleteCookies('rToken');
    deleteCookies('token');
    deleteCookies('adult');
    
    usernameSet();
    airportCodeSet();
    notificationCheck();

    var element = document.querySelector(".header-nav .badge")
    if(element) {
        var footerElement = document.querySelector(".bottomNav .badge");
        element.classList.add('d-none');
        element.querySelector('label').innerHTML = '' ;
        footerElement.innerHTML = '';
        element.closest('a').setAttribute('href', _defaultBadgeURL);
        footerElement.closest('a').setAttribute('href', _defaultBadgeURL);
    }
    document.querySelectorAll('.userMenuIcon').forEach((o) => {
        o.classList.add('i-user');
        o.querySelector('img')?.remove();
        o.classList.remove('loggedinAvatarImage');
        o.classList.remove('loggedinAvatar');
    });
    
    AirportPopupValue = '';
    if(window?.sessionStorage?.getItem('AirportPopup')){
        AirportPopupValue = window?.sessionStorage?.getItem('AirportPopup')
    }
    sessionStorage.clear();
    if(AirportPopupValue != ''){
        sessionStorage.setItem("AirportPopup",AirportPopupValue);
    }

    loginUser();
    if (window.innerWidth >= 992) {
        jQuery("#logoutPopup").modal('hide');
    } else {
        jQuery('#logout_offcanvas').offcanvas('hide')
    }
}

function loginUser() {
    rewardCoins = document.querySelectorAll(".rewardCoins");
    if (loginStatus() == 1) {
        document.querySelector('.loginProfileView').classList.remove('d-none');
        document.querySelector('.guestUserProfile').classList.add('d-none');
        document.querySelector('.userName').innerHTML =  _userName();
        document.querySelector('.hamburgerLoginDetail label').outerHTML = "<label><strong>"+_userName()+"</strong><span>View Profile</span></label> ";
        document.querySelector('.hamburgerLoginDetail').setAttribute('href', '/my-account/profile');
        document.querySelectorAll('.userMenuIcon').forEach((o) => {
            if(localStorage.getItem("userProfileImage") && localStorage.getItem("userProfileImage") != ''){
                o.classList.add('loggedinAvatarImage');
                o.innerHTML= '<img src="'+localStorage.getItem("userProfileImage")+'" alt="">';
            } else {
                o.classList.add('loggedinAvatar');
            }
            o.classList.remove('i-user');
        });
        rewardCoins.forEach((o) => {
            o.classList.remove('d-none');
        });
        if(localStorage.getItem("loginSuccessMsEnable") && localStorage.getItem("loginSuccessMsEnable") == 'true' ){
            toastMsg('You have logged in successfully','center',false);
            localStorage.removeItem('loginSuccessMsEnable');
        }
    } else {
        document.querySelector('.loginProfileView')?.classList.add('d-none');
        document.querySelector('.guestUserProfile')?.classList.remove('d-none');
        if (document.querySelector('.hamburgerLoginDetail label')?.length) {
            document.querySelector('.hamburgerLoginDetail label').outerHTML = "<label>Login/Signup</label>";
        }
        document.querySelector('.hamburgerLoginDetail')?.setAttribute('href', '/login');
        document.querySelectorAll('.userMenuIcon').forEach((o) => {
            o.classList.add('i-user');
            o.classList.remove('loggedinAvatar');
        });
        rewardCoins.forEach((o) => {
            o.classList.add('d-none');
        });

    }
    document.querySelector('.main-header .user-account')?.classList.remove('d-none');
}

loginUser();

if (document.getElementById('airportServiceListcanvas')) {
    var airportServiceListcanvas = document.getElementById('airportServiceListcanvas')
    airportServiceListcanvas.addEventListener('shown.bs.offcanvas', function () {
        document.getElementById("airportServiceListInput").focus();
    })
}

function confirmAge(flag) {
    localStorage.setItem("adult", flag);
    setCookieVal('adult', flag);
    window.location.href = ageModalHref;
}

/* End age confirmation popup */

function footerToggle(obj){
  if(window.innerWidth <= 600) {
    if(!obj.classList.contains('active')){
      document.querySelectorAll(".footer_link_heading,.footer_link_list").forEach((o) => {
        o.classList.remove('active');
      });
      obj.classList.add('active');
      obj.closest('.footer_nav_grid').querySelector('.footer_link_list').classList.add('active');
    } else {
      obj.classList.remove('active');
      obj.closest('.footer_nav_grid').querySelector('.footer_link_list').classList.remove('active');
    }
  }
}

function removeNotification() {
    window?.localStorage?.setItem('notification', false);
    document.querySelector("#notification").classList.add('d-none');
}

function notificationCheck() {
    if (document.querySelector("#notification")) {
        if (localStorage["notification"] == 'false' || loginStatus() == 1) {
            document.querySelector("#notification").classList.add('d-none');
        } else {
            document.querySelector("#notification").classList.remove('d-none');
        }
    }
}
notificationCheck();

function notificationMore() {
    jQuery('.notification').toggleClass('notification-expand');
    jQuery('.notification p').toggleClass('text-truncate');
}

finalSearchlist_popular = [];
gloabalSearchLen = 1;
function airportServiceListGet() {
    airportService_itemPath = "";
    if (document.querySelector("#airportService_itemPath")) {
        airportService_itemPath = document.querySelector("#airportService_itemPath").value;
    }

    if(airportService_itemPath.trim()!=''){
        fetch('/sitecore/api/layout/placeholder/jss?placeholderName=main&item=' + airportService_itemPath + '&sc_lang=en&sc_apikey={B5AC63B3-9934-4423-8FF9-F619303AC8DF}', {
            method: 'post',
        })
        .then((response) => response.json())
        .then((data) => {
            if (data && data?.elements?.length > 0) {
                const searchWrapper = []
                data?.elements?.map((item) => {
                    const mainTitle = item?.fields?.widget?.title && item?.fields?.widget?.title !== "" ? item?.fields?.widget?.title : item?.fields?.widget?.subTitle;
                    item?.fields?.widget?.widgetItems?.map((widgetItem) => {
                        if (widgetItem?.ctaUrl) {
                            searchWrapper.push({ mainTitle, href: widgetItem?.ctaUrl, title: widgetItem?.title })
                        }
                    })
                })
                let searchWrapper1 = []
                finalSearchlist = [];
                searchWrapper.map((item, index) => {
                    if (searchWrapper1.indexOf(item.title) == -1) {
                        searchWrapper1.push(item.title);
                        if (item.mainTitle == "Popular Services") {
                            finalSearchlist_popular.push({ mainTitle: item.mainTitle, href: item.href, title: item.title })
                        }
                        finalSearchlist.push({ mainTitle: item.mainTitle, href: item.href, title: item.title })
                    }
                });
                p_html = "";
                if (document.querySelector('.main-header .main-nav .search-layer .search-suggester ul.popularSearches')) {
                    finalSearchlist_popular?.forEach(item => {
                        p_html += "<li><a href='" + item.href + "'><i class='i-search'></i>" + item.title + "</a></li>";
                    })
                    p_html += "<li><a href='" + _airportURLName + "/services'><i class='i-search'></i>View All Services</a></li>";
                    document.querySelector('.main-header .main-nav .search-layer .search-suggester ul.popularSearches').innerHTML = p_html;
                    if (document.querySelector('.globalSearch .searchLayer .searchSuggester ul.popularSearches')) {
                        document.querySelector('.globalSearch .searchLayer .searchSuggester ul.popularSearches').innerHTML = p_html;
                    }
                }
            }
            return finalSearchlist;
        })
        .catch((error) => {
            console.log('Error:', error);
        });
    }
}

airportServiceListGet();

var airportServiceListArr;
function airportServiceListFilter(obj) {
    if (finalSearchlist.length == 0) {
        airportServiceListGet();
    }
    html = "";
    if (finalSearchlist) {
        airportServiceListArr = finalSearchlist?.filter((item) => item.title.toString().toLowerCase().includes(obj.value.toLowerCase().trim()))
    }

    if (obj.value && obj.value.toLowerCase().trim().length >= gloabalSearchLen && airportServiceListArr.length == 0) {
        html = "<li class='noData'>Oops! We can't find what you are looking for</li>"
    } else if (obj.value && obj.value.toLowerCase().trim().length >= gloabalSearchLen && airportServiceListArr.length > 0) {
        airportServiceListArr?.forEach(item => {
            html += "<li><a href='" + item.href + "'><i class='i-search'></i>" + item.title + "</a></li>";
        })
    } else {
        html = "";
    }

    if (document.querySelector('.main-header .main-nav .search-layer .search-suggester .allSearchListCntr')) {
        allListCntr = document.querySelector('.main-header .main-nav .search-layer .search-suggester .allSearchListCntr');
        if (obj.value && obj.value.toLowerCase().trim().length >= gloabalSearchLen) {
            if (obj.value.length >= gloabalSearchLen) {
                allListCntr.classList.remove('d-none');
                if (document.querySelector('.globalSearch .searchLayer .searchSuggester .allSearchListCntr')) {
                    document.querySelector('.globalSearch .searchLayer .searchSuggester .allSearchListCntr').classList.remove('d-none');
                }
            } else {
                allListCntr.classList.add('d-none');
                if (document.querySelector('.globalSearch .searchLayer .searchSuggester .allSearchListCntr')) {
                    document.querySelector('.globalSearch .searchLayer .searchSuggester .allSearchListCntr').classList.add('d-none');
                }
            }
        } else {
            allListCntr.classList.add('d-none');
            if (document.querySelector('.globalSearch .searchLayer .searchSuggester .allSearchListCntr')) {
                document.querySelector('.globalSearch .searchLayer .searchSuggester .allSearchListCntr').classList.add('d-none');
            }
        }
    }

    popularSearchesCntrs = document.querySelectorAll(' .searchListCntr .popularSearchesCntr, .searchSuggester .popularSearchesCntr');

    if (obj.value.length >= gloabalSearchLen) {
        obj.parentElement.querySelector('.clearIcon,.clearIcon1').classList.remove('d-none');
        popularSearchesCntrs.forEach((o) => {
            o.classList.add('d-none')
        });
    } else {
        obj.parentElement.querySelector('.clearIcon,.clearIcon1').classList.add('d-none');
        popularSearchesCntrs.forEach((o) => {
            o.classList.remove('d-none')
        });
    }

    if (document.querySelector('.main-header .main-nav .search-layer .search-suggester ul.allSearchList')) {
        document.querySelector('.main-header .main-nav .search-layer .search-suggester ul.allSearchList').innerHTML = html;
    }
    if (document.querySelector('.globalSearch .searchLayer .searchSuggester ul.allSearchList')) {
        document.querySelector('.globalSearch .searchLayer .searchSuggester ul.allSearchList').innerHTML = html;
    }
}

function airportServiceListCross(elemObj) {
    elemPar = elemObj.parentElement
    elemPar.querySelector('input[type="text"]').value = '';
    elemObj.classList.add('d-none');
    airportServiceListFilter(elemPar.querySelector('input[type="text"]'));
}

searchLayerFlag = false;
function dropDownSearchLayer(flag) {
    elem = document.querySelector(".main-header .main-nav .search-layer");
    if (flag) {
        elem.classList.add('d-block');
        elem.classList.remove('d-none');
        document.querySelector('.page-backdrop').classList.add('show');
        document.querySelector(".main-header .main-nav .search-box__field").focus();
    } else {
        if (searchLayerFlag == false) {
            elem.classList.add('d-none');
            elem.classList.remove('d-block');
            document.querySelector('.page-backdrop').classList.remove('show');
            airportServiceListCross(document.querySelector('.main-header .main-nav .search-layer .clearIcon'));
        }
    }
}

document.addEventListener('click', function (event) {
    if (document.querySelector(".main-header .main-nav .search-layer")) {
        var isClickInsideElement = document.querySelector(".main-header .main-nav .search-layer").contains(event.target);
        if (!isClickInsideElement) {
            dropDownSearchLayer(false);
        }
    }
});

var addToCartFlag = '';
function addToCart_change(e, obj, increase_True) {
    e.preventDefault();
    e.stopPropagation();
    buttonElm = obj.closest('button');

    if (!isNaN(buttonElm.querySelector('.count-num').innerHTML)) {
        countValue = Number(buttonElm.querySelector('.count-num').innerHTML);
        if (increase_True) {
            addToCartFlag = "Add";
            addToCart(e, obj.closest('button'), countValue + 1);
        } else {
            if (countValue != 0) {
                addToCartFlag = "Remove"
                addToCart(e, obj.closest('button'), countValue - 1);
                if (countValue == 1) {
                }
            }
        }
    } else {
        addToCartFlag = "Add";
        addToCart(e, obj.closest('button'));
    }
}

var _addItemAfterClear;
function addToCart(e, obj, quantity = 1,addToCartMsg = false) {
    e.preventDefault()
    if (e.target.classList.contains('count-num') && !isNaN(e.target.innerHTML)) {
        e.stopPropagation();
    } else {
        buttonElm = obj.closest('button')

        ValidateToken().then((val)=>{
        }).catch((err)=>{
          console.log(err)
        }).finally(()=>{

        var addToCart_Headers = new Headers();
        addToCart_Headers.append('traceId', traceId);
        addToCart_Headers.append('channelId', channelId);
        addToCart_Headers.append("Access-Control-Allow-Methods", "GET,POST,PATCH");
        addToCart_Headers.append("Access-Control-Allow-Origin", "*");
        addToCart_Headers.append('Content-Type', 'application/json');

        if (loginStatus() == 0) {
            addToCart_Headers.append('agentId', localStorage.getItem('userId') || "");
        } else {
            addToCart_Headers.append('clientId', clientId);
            addToCart_Headers.append('Authorization', 'bearer ' + localStorage.getItem('token'));
        }

        fetch(apiBasePath + orderRestructuringAPIVersion["ADDTOCARTDUTYFREE"], {
            method: 'POST',
            headers: addToCart_Headers,
            body: JSON.stringify(
                {
                    "skuCode": obj.dataset.skucode,
                    "quantity": quantity,
                    "storeType": obj.dataset.storetype,
                    "airportCode": obj.dataset.airportcode,
                    "productImage": obj.dataset.productimage,
                }
            )
        })
        .then((response) => response.json())
        .then((data) => {
            
            if ((data && data?.data && data?.data !== null) || (data?.data == null && data?.status == true)) {
                msgFlagActive = false;
                if(buttonElm.querySelector('.count-num')) {
                    msgFlagActive = buttonElm.classList.contains('btn-outline-primary')
                }
                cartMsg = addToCartMsg ? "Removed from cart" : (addToCartFlag =='Add' ? "Added to cart" : "Removed from cart");
                if(msgFlagActive){
                    cartMsg = 'Added to cart';
                }
                if(cartMsg=='Removed from cart'){
                    toastMsg(cartMsg);
                }
                if(buttonElm.querySelector('.count-num')) {
                    count = buttonElm.querySelector('.count-num');
                    minusElm = buttonElm.querySelector('.minus');
                    addElm = buttonElm.querySelector('.add p');
                    
                    if (quantity == 0) {
                        count.innerHTML = 'Add';
                        buttonElm.classList.add('btn-outline-primary')
                        buttonElm.classList.remove('btn-primary');
                        minusElm.innerHTML = '';
                        addElm.classList.remove('active');
                    } else {
                        count.innerHTML = quantity;
                        buttonElm.classList.remove('btn-outline-primary')
                        buttonElm.classList.add('btn-primary');
                        minusElm.innerHTML = '<span><i class="i-minus"></i></span>';
                        addElm.classList.add('active');
                    }
                }
                getCartId();             

            } else {

                if (data && data?.error?.code === 'DFAC02' && document.querySelector("#changeTerminalPopup")) {
                    addTocartStoreTypeMsgSet(obj.dataset.storetype);
                    if (window.innerWidth >= 992) {
                        jQuery("#changeTerminalPopup").modal('show');
                    } else {
                        jQuery('#changeTerminal_offcanvas').offcanvas('show')
                    }
                    _addItemAfterClear = obj;
                } else if (data && data?.error?.code === 'DFGC05' && document.querySelector("#dfCartClear")) {
                    if (window.innerWidth >= 992) {
                        jQuery("#dfCartClear").modal('show');
                    } else {
                        jQuery('#dfCartClear_offcanvas').offcanvas('show')
                    }
                    _addItemAfterClear = obj;
                } else {
                    toastMsg(data.error.description);
                }
            }
        })
        .catch((error) => {
            console.log('Error:', error);
        });

        })
    }
}

function addTocartStoreTypeMsgSet(msg) {
    if (document.querySelector("#changeTerminalPopup")) {
        if (msg == 'departure') {
            document.querySelector('#newCart').innerHTML = 'Departure';
            document.querySelector('#existCart').innerHTML = 'Arrival';
        } else {
            document.querySelector('#existCart').innerHTML = 'Departure';
            document.querySelector('#newCart').innerHTML = 'Arrival';
        }
    }
    return;
}

function changeTerminalAccept() {
    clearCartFun();
}

function DFNewCartAccept() {
    clearCartFun();
}

function toastMsg(msg,position = 'left',icon = true) {
    toastCntr = document.querySelector('.toast-container');
    if(position == 'center'){
        if(toastCntr.classList.contains('toastTopRight')){
          toastCntr.classList.remove('toastTopRight');
          toastCntr.classList.add('toastTopMiddle');
        }
    } else {
        if(toastCntr.classList.contains('toastTopMiddle')){
            toastCntr.classList.add('toastTopRight');
            toastCntr.classList.remove('toastTopMiddle');
        }
    }
    if(icon){
        toastCntr.querySelector('i').classList.remove('d-none');
    } else {
        toastCntr.querySelector('i').classList.add('d-none');
    }
    toastCntr.querySelector('.toast span').innerHTML = msg;
    toastCntr.querySelector('.toast').classList.add('show');
    setTimeout(() => {
        toastCntr.querySelector('.toast').classList.remove('show');
    }, 2500);
}

function setAddToCartQuantity() {
    document.querySelectorAll('button.add-btn').forEach(function (list) {
        if (document.getElementById(list.id)) {
            buttonElm = document.getElementById(list.id);
            count = buttonElm.querySelector('.count-num');
            minusElm = buttonElm.querySelector('.minus');
            addElm = buttonElm.querySelector('.add p');
            if (addedCartQList[list.id]) {
                count.innerHTML = addedCartQList[list.id];
                buttonElm.classList.remove('btn-outline-primary')
                buttonElm.classList.add('btn-primary');
                minusElm.innerHTML = '<span><i class="i-minus"></i></span>';
                addElm.classList.add('active');
            } else {
                count.innerHTML = 'Add';
                buttonElm.classList.add('btn-outline-primary')
                buttonElm.classList.remove('btn-primary');
                minusElm.innerHTML = '';
                addElm.classList.remove('active');
            }
        }
    });

}

function showLoyaltyPopup(){
    if(document.querySelector("#rewardsPopup")){
      if(window?.localStorage?.getItem("showLoyaltyPopup") == 'true'){
          var rewardsPopupObj = document.getElementById("rewardsPopup");
          var bsOffcanvas_rewardsPopupObj = new bootstrap.Modal(rewardsPopupObj);
          bsOffcanvas_rewardsPopupObj.show();
          //jQuery("#rewardsPopup").modal('show');
          setTimeout(() => {
            window?.localStorage?.setItem('showLoyaltyPopup', 0);
          }, 4000);
      }
    }
}
  
function showLoyaltyPopup_Close(){
    window?.localStorage?.setItem('showLoyaltyPopup', 0);
}



function clearCartConfirmation(){
    if (window.innerWidth >= 992) {
      jQuery("#changeAirportPopup").modal('show');
    } else {
      jQuery('#changeAirport_offcanvas').offcanvas('show')
    }
  }
  
function clearCartFun(Url = ''){
    userID1 = window?.localStorage?.getItem('userId') || '';
    var clearCart_header = new Headers();
    if (loginStatus() == 1) {
        clearCart_header.append('Authorization', 'bearer ' + localStorage?.getItem('token'));
    } else {
        clearCart_header.append('annonymousid', userID1);
    }

    clearCart_header.append('traceId', traceId);
    clearCart_header.append('channelId', channelId);
    clearCart_header.append("Access-Control-Allow-Methods", "GET,POST,PATCH");
    clearCart_header.append("Access-Control-Allow-Origin", "*");

    fetch(apiBasePath+orderRestructuringAPIVersion["DELETECART"], {
        method: 'DELETE',
        headers: clearCart_header
    })
    .then((res) => res.json())
    .then((data) => {
        if (data.status) {
            cardBadgeToggle(0);
            if (Url != '') {
                window.location.href = Url;
            } else {
                jQuery(_addItemAfterClear).trigger('click');
                if (document.querySelector('#changeTerminalPopup')) {
                    if (window.innerWidth >= 992) {
                        jQuery("#changeTerminalPopup").modal('hide');
                    } else {
                        jQuery('#changeTerminal_offcanvas').offcanvas('hide')
                    }
                }
                if (document.querySelector('#dfCartClear')) {
                    if (window.innerWidth >= 992) {
                        jQuery("#dfCartClear").modal('hide');
                    } else {
                        jQuery('#dfCartClear_offcanvas').offcanvas('hide')
                    }
                }
            }
        }
    })
    .catch((error) => {
      console.log('Error:', error);
    });
  }
  
  logochangeURL = "";
  function onLogochangeEvent(){
      jQuery("#airportListBody a").click(function(event){
        if(document.querySelector('#airportListBody a') && document.querySelector(".header-nav .badge label")?.innerHTML.trim() != ''){
          if (_airportURLName == '' || _airportURLName == jQuery(this).attr('href')) {
          } else {
            event.preventDefault();
            logochangeURL = jQuery(this).attr('href');
            clearCartConfirmation();
			jQuery('#airportListOffcanvas').offcanvas('hide');
            //clearCartFun(jQuery(this).attr('href'));
          }
        }
      });
  }
  
  function onLogochange_Desktop(){
      jQuery(".citydropdown .locationLayer a").click(function(event){
        if(document.querySelector('.citydropdown .locationLayer a') && document.querySelector(".header-nav .badge label")?.innerHTML.trim() != ''){
          if (_airportURLName == '' || _airportURLName == jQuery(this).attr('href')) {
          } else {
            event.preventDefault();
            logochangeURL = jQuery(this).attr('href');
            clearCartConfirmation();
            //clearCartFun(jQuery(this).attr('href'));
          }
        }
      });
  }
  
  function changeAirportAccept(){
    clearCartFun(logochangeURL);
  }
  
  var allOffcanvas = document.querySelectorAll('.offcanvas')
  allOffcanvas.forEach((o) => {
    o.addEventListener('shown.bs.offcanvas', function () {
      jQuery("body").addClass('modal-open');
    })
    o.addEventListener('hidden.bs.offcanvas', function () {
      jQuery("body").removeClass('modal-open');
    })
  });


  function horizontalScrollFun (obj) {
  
    const parent = obj.closest(".nav-pills");
    const parentWidth = parent.clientWidth;
    const ele = obj;
    const eleWidth = ele.clientWidth;
    const firstWidth = obj.closest(".nav-pills").querySelector(
      ".nav-item:first-child"
    ).offsetLeft;
    
    const leftWidth = ele.getBoundingClientRect().left;
  
    const req = (parentWidth - eleWidth) / 2;
    const scollWidth = parentWidth - leftWidth;
    if (req > leftWidth) {
      parent.scrollBy({
        left: -scollWidth - firstWidth + (req + eleWidth),
        behavior: "smooth",
      });
    } else if (req < leftWidth) {
      parent.scrollBy({
        left: leftWidth - firstWidth - req,
        behavior: "smooth",
      });
    }
  }
  
  function custom_scrollspy(obj){
    const { innerWidth: width, innerHeight: height } = window;
    gapSize = 20;
    if (width > 992) {
      gapSize = 60;
    }
    get_id_custom = jQuery(obj)?.find(".nav-link_custom")?.attr("href")?.slice(1);
    if(jQuery(this).closest('.offcanvas-body').length > 0) {
      jQuery(this).closest(".offcanvas-body").animate(
        {
          scrollTop: jQuery("#" + get_id_custom).offset().top - 2
        },
        0,
        function() {
          horizontalScrollFun(obj);
        }
      );
    } else {
  
      jQuery("html, body").animate(
        {
          scrollTop: jQuery("#" + get_id_custom).offset().top - gapSize - document.querySelector("#navbar-tabs")?.getBoundingClientRect()?.height || 0,
        },
        0,
        function() {
          horizontalScrollFun(obj);
        }
      );
    }
  }

  if (document.querySelector(".nav-pills.horizontal-tab")) {
    jQuery(".nav-pills.horizontal-tab .nav-item").click(function (event) {
      event.preventDefault();
      get_id = jQuery(this)?.find(".nav-link")?.attr("href")?.slice(1);
  
      //console.log(this.parentElement.getBoundingClientRect().top);//.offset().top);
      if(get_id){
        const timeOut = setTimeout(() => {
          const parent = this.closest(".nav-pills");
          const parentWidth = parent.clientWidth;
          const ele = this;
          const eleWidth = ele.clientWidth;
          const firstWidth = this.closest(".nav-pills").querySelector(
            ".nav-item:first-child"
          ).offsetLeft;
          const leftWidth = ele.getBoundingClientRect().left;
          // const padding = 48;
  
          // if (leftWidth <= padding) {
          //   const scollWidth = parentWidth - (eleWidth + leftWidth);
          //   parent.scrollBy({ left: -scollWidth - firstWidth, behavior:'smooth' });
          //   return;
          // } else if (eleWidth + leftWidth > parentWidth) {
          //   parent.scrollBy({ left: leftWidth - firstWidth, behavior:'smooth' });
          // }
  
          const req = (parentWidth - eleWidth) / 2;
          const scollWidth = parentWidth - leftWidth;
          if (req > leftWidth) {
            parent.scrollBy({
              left: -scollWidth - firstWidth + (req + eleWidth),
              behavior: "smooth",
            });
          } else if (req < leftWidth) {
            parent.scrollBy({
              left: leftWidth - firstWidth - req,
              behavior: "smooth",
            });
          }
  
          return () => {
            clearTimeout(timeOut);
          };
        }, 0);
  
        if(jQuery(this).closest('.offcanvas-body').length > 0) {
          jQuery(this).closest(".offcanvas-body").animate(
            {
              scrollTop: jQuery("#" + get_id).offset().top - 2
            },
            0
          );
        } else {
          jQuery("html, body").animate(
            {
              scrollTop: jQuery("#" + get_id).offset().top,
            },
            0
          );
        }
      } else {
        jQuery(this).closest('.horizontal-tab').find('.nav-link_custom').removeClass('active')
        jQuery(this).find('.nav-link_custom').addClass('active');
        custom_scrollspy(this);      
      }
    });
  }
  

const tabs = document.querySelectorAll(".tab-wrapper .nav-link");

if (tabs.length) {
    tabs.forEach(tabElm => {
        tabElm.onclick = e => {
            const id = e.target.dataset.id;
            if (jQuery("#" + id + " .slider-airports") && window.innerWidth >= 991) {
                jQuery("#" + id + " .slider-airports").slick('slickGoTo', jQuery("#" + id + " .slider-airports .slick-current").attr("data-slick-index"));
                jQuery("#" + id + " .slider-airports .slick-arrow").removeClass('ripple_root');
                jQuery("#" + id + " .slider-airports .slick-arrow .ripple_waves").remove();
            }
            if (id) {
                const tabButton = e.target.closest('.tab-wrapper').querySelectorAll(".nav-link");
                const contents = e.target.closest('.tab-wrapper').querySelectorAll(".tab-wrapper .tab-pane");

                tabButton.forEach(btn => {
                    btn.classList.remove("active");
                });
                e.target.classList.add("active");
                contents.forEach(content => {
                    content.classList.remove("active");
                });
                const element = document.getElementById(id);
                element.classList.add("active");
            }
        }
    });
}

const setCookie = (name) => {
    document.cookie = name + "=accepted; path=/;SameSite=Lax;Secure";
};
const getCookie = (name) => {
    return document.cookie.match("(^|;)\\s*" + name + "\\s*=\\s*([^;]+)");
};
const cookieHandler = () => {
    setCookie("adlCookies");
};
const isCookieExist = getCookie("adlCookies");
const setIsCookie = (flag) => {
    if (window.innerWidth >= 992) {
        if (document.getElementById("cookiesModal")) {
            if (flag) {
                jQuery("#cookiesModal").removeClass('d-none');
            } else {
                jQuery("#cookiesModal").addClass('d-none');
            }
        }
    } else {
        if (document.getElementById("cookies_offcanvas")) {
            var $cookiesOffcanvas = document.getElementById("cookies_offcanvas");
            var cookiesOffcanvas = new bootstrap.Offcanvas($cookiesOffcanvas);
            if (flag) {
                cookiesOffcanvas.show();
            } else {
                cookiesOffcanvas.hide();
            }
        }
    }
}

    if (isCookieExist) {
        setIsCookie(false);
    } else {
        setIsCookie(true);
    }

if (document.getElementById('cookies_offcanvas')) {
    var myOffcanvas = document.getElementById('cookies_offcanvas')
    myOffcanvas.addEventListener('hidden.bs.offcanvas', function () {
        cookieHandler();
    })
}
function closeCookie() {
    cookieHandler();
    setIsCookie(false);
}

function setCookieVal(name, value, options = {}) {
  options = {
    path: '/',
    secure: true, 
    SameSite: 'Lax',
    expires: date,
    ...options
  };

  if (options.expires instanceof Date) {
    options.expires = options.expires.toUTCString();
  }

  let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

  for (let optionKey in options) {
    updatedCookie += "; " + optionKey;
    let optionValue = options[optionKey];
    if (optionValue !== true) {
      updatedCookie += "=" + optionValue;
    }
  }
  document.cookie = updatedCookie;
}

function deleteCookies(name) {
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    document.cookie = name + "=; Path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";
}
function gtmTopNavInteractionEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var type = jQuery(element).attr("data-type");
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            type: type
        });
    }
}
function gtmEventHambergerOpen(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page";
        var subcategory = jQuery(element).attr("data-subcategory");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory
        });
    }
}
function gtmHambergerLinkSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var type = jQuery(element).attr("data-type");
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            type: type
        });
    }
}
function gtmFooterLinkSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var type = jQuery(element).attr("data-type");
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            type: type
        });
    }
}
function gtmFooterBottomTilesClickEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = "help_and_support";
        var label = jQuery(element).attr("data-label");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label
        });
    }
}
function gtmAirportDropdownSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = "airport_selection";
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var businessunit = jQuery(element).attr("data-businessunit");
        var label = jQuery(element).attr("data-label");
        if (label == "" || label == "undefined" || label == undefined) {
            label = businessunit;
        }
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            businessunit: businessunit,
            label: label
        });
    }
}
function gtmSocilaLinkSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var type = jQuery(element).attr("data-type");
        var label = jQuery(element).attr("data-label");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            type: type
        });
    }
}
function gtmAccordionSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var businessunit = document.querySelector('#airportCode') ? document.querySelector('#airportCode').value : '';
        var label = jQuery(element).attr("data-label");
        if (label == "" || label == "undefined" || label == undefined) {
            label = businessunit;
        }
        var faqCategory = businessunit;
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            businessunit: businessunit,
            label: label,
            faq_category: faqCategory
        });
    }
}
function gtmAccordionSeeAllSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page";
        var subcategory = jQuery(element).attr("data-subcategory");
        var businessunit = document.querySelector('#airportCode') ? document.querySelector('#airportCode').value : '';
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            businessunit: businessunit
        });
    }
}
function gtmAddToCartClickEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page";
        var subcategory = jQuery(element).attr("data-storetype");
        var skucode = jQuery(element).attr("data-skucode");
        var skuname = jQuery(element).attr("data-name");
        var productCategory = jQuery(element).attr("data-category");
        var price = jQuery(element).attr("data-price");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            item_id: skucode,
            item_name: skuname,
            product_category: productCategory,
            price: price,
            currency:"INR",
            quantity:"1"
        });
    }
}
function gtmPopularRoutesLinkSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var popular = "";
        var event = jQuery(element).attr("data-event");
        var category = jQuery(element).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        if (window.location.pathname == "/") {
            popular = "homepage";
        } else {
            popular = window.location.pathname.substring(1);
        }
        var pagetype = popular;
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            page_type: pagetype
        });
    }
}
function gtmTopRoutesLinkSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var type = "";
        var event = jQuery(element).attr("data-event");
        var category = jQuery(element).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        if (window.location.pathname == "/") {
            type = "homepage";
        } else {
            type = window.location.pathname.substring(1);
        }
        var pagetype = type;
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            page_type: pagetype
        });
    }
}


function gtmStaticBannerLinkEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var type = "";
        var event = jQuery(element).attr("data-event");
        var category = jQuery(element).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        if (window.location.pathname == "/") {
            type = "homepage";
        } else {
            type = window.location.pathname.substring(1);
        }
        var pagetype = type;
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            page_type: pagetype
        });
    }
}

if (document.querySelectorAll(".gtmFooterLinkClick")) {
    document.querySelectorAll(".gtmFooterLinkClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmFooterLinkSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmFooterBottomTileClick")) {
    document.querySelectorAll(".gtmFooterBottomTileClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmFooterBottomTilesClickEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmSocialLinkClick")) {
    document.querySelectorAll(".gtmSocialLinkClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmSocilaLinkSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmTopNavInteraction")) {
    document.querySelectorAll(".gtmTopNavInteraction").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmTopNavInteractionEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmHambergerOpen")) {
    document.querySelectorAll(".gtmHambergerOpen").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmEventHambergerOpen(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmHambergerLinkSelect")) {
    document.querySelectorAll(".gtmHambergerLinkSelect").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmHambergerLinkSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmAirportDropdownSelect")) {
    document.querySelectorAll(".gtmAirportDropdownSelect").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmAirportDropdownSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmAccordionSelect")) {
    document.querySelectorAll(".gtmAccordionSelect").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                if (jQuery(x).attr("aria-expanded")=="true") {
                    gtmAccordionSelectEvent(e.currentTarget);
                }
            })
        }
    })
}
if (document.querySelectorAll(".gtmAccordionSeeAllSelect")) {
    document.querySelectorAll(".gtmAccordionSeeAllSelect").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmAccordionSeeAllSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmAddToCartEvent")) {
    document.querySelectorAll(".gtmAddToCartEvent").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmAddToCartClickEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmPopularRoutesLinkClick")) {
    document.querySelectorAll(".gtmPopularRoutesLinkClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmPopularRoutesLinkSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmTopRoutesLinkClick")) {
    document.querySelectorAll(".gtmTopRoutesLinkClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmTopRoutesLinkSelectEvent(e.currentTarget);
            })
        }
    })
}

if (document.querySelectorAll(".banner-gtm-triggering")) {
    document.querySelectorAll(".banner-gtm-triggering").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmStaticBannerLinkEvent(e.currentTarget);
            })
        }
    })
}


jQuery( document ).ready(function() {
    if (window.scrollY > 0) {
        document.body.scrollTop = 0;
        document.documentElement.scrollTop = 0;
    }
});

jQuery(".modal.airportListDialog .btn-close").on("click", function (e) {
    e.preventDefault();
    jQuery(".modal.airportListDialog").removeClass("show");
    jQuery(".modal.airportListDialog").css('display', 'none')
    jQuery('.modal-backdrop').remove();
});


// For window
const getOSType = () => {
    if (navigator?.userAgent?.indexOf("Windows") !== -1) {
      document?.documentElement?.classList.add("windows");
    }
  };
  
  getOSType();
