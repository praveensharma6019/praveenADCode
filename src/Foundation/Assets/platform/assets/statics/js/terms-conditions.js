function closeOffcanvas() {
    const { innerWidth: width, innerHeight: height } = window;
    if (width < 992) {
        try {
            var $myOffcanvas = document.getElementById('offcanvasBottomTest');
            var $myOffBackdrop = document.getElementsByClassName('offcanvas-backdrop')[0];
            $myOffcanvas.classList.remove('show');
            $myOffcanvas.classList.add('hide');
            $myOffBackdrop.classList.remove('show');
            $myOffBackdrop.classList.add('offcanvas-hide');
            document.body.style.removeProperty('overflow');
            document.body.style.removeProperty('padding-right')
        } catch (error) {

        }
    }
}
window.addEventListener("load", handleResize);
window.addEventListener("resize", handleResize);
function handleResize() {
    let $toggle_offcanvas = document.getElementById("offcanvasBottomTest");
    if (!$toggle_offcanvas) { return; }
    const { innerWidth: width, innerHeight: height } = window;
    if (width < 992) {
        $toggle_offcanvas.classList.remove('offcanvas-desktop');
    }
    else {
        $toggle_offcanvas.classList.add('offcanvas-desktop');
    }
}
let isClicked = false;
const onChange = () => {
    let tabContent = document.getElementById("v-pills-tabContent");
    let tab = document.getElementById("services-main-menu");
    if (isClicked) {
        tabContent.classList.replace(
            "services-container__expand",
            "services-container"
        );
        tab.classList.replace("services-menu__collapse", "services-menu");
        isClicked = !isClicked;
    } else {
        tabContent.classList.replace(
            "services-container",
            "services-container__expand"
        );
        tab.classList.replace("services-menu", "services-menu__collapse");
        isClicked = !isClicked;
    }
    setTimeout(() => {
        sliderNavigation("other-services");
    }, 400);
};

const pageUrl = window.location.search;
const urlParams = new URLSearchParams(pageUrl);
const isappvalue = urlParams.get('isapp')
if (isappvalue == 'true') {
    $('.serviceDetail span').removeClass('i-arrow-d')
    $('.serviceDetail span').removeAttr('data-bs-target')
}


function getAirportList(flag = false) {
    const { innerWidth: width, innerHeight: height } = window;
    if (width > 992) {
        try {
            if (document.getElementById("offcanvasExample") && flag == true) {
                jQuery('#offcanvasExample').offcanvas('hide')
            }
            var $staticBackdrop = document.getElementById("staticBackdrop");
            var bsOffcanvas = new bootstrap.Modal($staticBackdrop);
            if (!window?.sessionStorage?.getItem("AirportPopup") || window?.sessionStorage?.getItem('AirportPopup') == 'true' || flag == true) {
                bsOffcanvas.show();
                window.sessionStorage.setItem("AirportPopup", false);
            }
            var airportList = document.getElementById("airportList");
            airportList.style.display = "grid";
            document.getElementById("modal-body").appendChild(airportList);
            document
                .querySelector(".closeAirportListDialog")
                .setAttribute("data-bs-dismiss", "modal");
        } catch (error) { }
    } else if (width <= 992) {
        try {
            var $myOffcanvas = document.getElementById("offcanvas_airportList");
            var bsOffcanvas = new bootstrap.Offcanvas($myOffcanvas);
            if (!window?.sessionStorage?.getItem("AirportPopup") || window?.sessionStorage?.getItem('AirportPopup') == 'true' || flag == true) {
                bsOffcanvas.show();
                window.sessionStorage.setItem("AirportPopup", false);
            }
            var airportList = document.getElementById("airportList");
            airportList.style.display = "flex";
            document.getElementById("offcanvas-body").appendChild(airportList);
            document
                .querySelector(".closeAirportListDialog")
                .setAttribute("data-bs-dismiss", "offcanvas");
        } catch (error) { }
    }
}

function showAirportList(e) {
    e.preventDefault();
    getAirportList(true);
}

if ($('#serviceList li').length < 11) {
    $('.searchSection').hide();
}