import awards from "./../../components/awards";
import global from "./../../components/global";

(function () {
    setTimeout(() => {
        global.init();
        awards.init();

        // Tab
        $(".tab-title-wrp ul li a").click((e) => {
            const $this = $(e.currentTarget);

            const tabAttr = $this.attr("title");
            $(".tab-acc-content").hide();
            $(`.tab-acc-content[tab-content="${tabAttr}"]`).show();

            $this.parent().siblings().find("a").removeClass("active");
            $this.toggleClass("active");
        });

        // Accordion
        $(".acc-title").click((e) => {
            const $this = $(e.currentTarget);

            const tabAttr = $this.attr("title");
            $this
                .closest(".acc-wrp")
                .siblings()
                .find(".tab-acc-content")
                .slideUp();
            $(`.tab-acc-content[tab-content="${tabAttr}"]`).slideToggle();

            $this
                .closest(".acc-wrp")
                .siblings()
                .find(".acc-title")
                .removeClass("active");
            $this
                .closest(".acc-wrp")
                .siblings()
                .removeClass("active");
            $this.toggleClass("active");
            $this.parent().toggleClass("active");
        });

        // Hero banner arrow click
        console.log("Hero banner arrow click");
        $(".hero-section .banner-arrow").click(() => {
            const bannerHeight = $(".hero-section").outerHeight();
            const bannerOffset = $(".hero-section").offset().top;
            const scrollPos = bannerHeight + bannerOffset;

            $("body, html").animate({ scrollTop: scrollPos });
        });
    }, 1000);
})();
