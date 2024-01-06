import awards from "./../../components/awards";
import global from "./../../components/global";

(function () {
    let timeout = 0;

    if ((typeof isDev != 'undefined') && isDev === true){
        timeout = 1000;
    }

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
    }, timeout);
})(); 
