
import awards from "./../../components/awards";
import global from "./../../components/global";
import testimonial from "./../../components/testimonial";
import sustainability from "./../../components/sustainability";
import explocation from "./../../components/explore-location";

(function(){

    helpers.page.extend('dc-details', {
        init(){
            global.init();
            awards.init();
            testimonial.init();
            sustainability.init();
            explocation.init();

            // Hero banner arrow click
            console.log('Hero banner arrow click');
            $('.hero-section .banner-arrow').click(() => {
                const bannerHeight = $('.hero-section').outerHeight();
                const bannerOffset = $('.hero-section').offset().top;
                const scrollPos = bannerHeight + bannerOffset;
                $('body, html').animate({scrollTop: scrollPos})
            });
        }
     });

     // Initialize and add the map
        function initMap() {
            // The location of Uluru
            const uluru = { lat: -25.344, lng: 131.036 };
            // The map, centered at Uluru
            const map = new google.maps.Map(document.getElementById("map"), {
                zoom: 4,
                center: uluru,
            });
            // The marker, positioned at Uluru
            const marker = new google.maps.Marker({
                position: uluru,
                map: map,
            });
        }

        setTimeout(() => {
            initMap();
        }, 200);
})();