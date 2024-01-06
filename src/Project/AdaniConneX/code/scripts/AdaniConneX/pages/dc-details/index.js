
import awards from "./../../components/awards";
import global from "./../../components/global";
import testimonial from "./../../components/testimonial";
import sustainability from "./../../components/sustainability";
import explocation from "./../../components/explore-location";

(function(){
    // function initMap() {
    //     // The location of Uluru
    //     const uluru = { lat: -25.344, lng: 131.036 };
    //     // The map, centered at Uluru
    //     const map = new google.maps.Map(document.getElementById("map"), {
    //         zoom: 4,
    //         center: uluru,
    //     });
    //     // The marker, positioned at Uluru
    //     const marker = new google.maps.Marker({
    //         position: uluru,
    //         map: map,
    //     });
    // }

    if ((typeof isDev != 'undefined') && isDev === true) {
        window.helpers.page.extend('dc-details', {
            init(){
                global.init();
                awards.init();
                testimonial.init();
                sustainability.init();
                explocation.init();
                // initMap();
            }
        });
    }else{
        global.init();
        awards.init();
        testimonial.init();
        sustainability.init();
        explocation.init();
        // initMap();
    }   
})();