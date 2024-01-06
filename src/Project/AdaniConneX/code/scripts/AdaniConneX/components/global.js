import search from "./search";
import scroll from "./scroll-to";
import contact from "./contact-us";
import takeATour from "./take-a-tour";
import whitePaper from "./white-paper";
import getInTouch from "./get-in-touch";
import notification from "./notification";
import takeVirtualTour from "./take-virtual-tour";
import goToTop from "./go-to-top";

export default {
    init(){
        scroll.init();
        search.init();
        contact.init();
        takeATour.init();
        whitePaper.init();
        getInTouch.init();
        notification.init();
        takeVirtualTour.init();
        goToTop.init();
        helpers.plugins.menu.init();
        helpers.plugins.scroll.init();
        helpers.plugins.accordion.init();

        $(".hero-section .banner-arrow").click(() => {
            const bannerHeight = $(".hero-section").outerHeight();
            const bannerOffset = $(".hero-section").offset().top;
            const scrollPos = bannerHeight + bannerOffset;
            $("html").animate({ scrollTop: scrollPos });
        });
    }
}