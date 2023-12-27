
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