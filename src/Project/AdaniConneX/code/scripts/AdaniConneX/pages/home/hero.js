export default {
    init(){
        window.helpers.plugins.owl.init('#takeTour', {items:1, dots:true});

        // Hero banner arrow click
        console.log('Hero banner arrow click');
        $('.hero-section .banner-arrow').click(() => {
            const bannerHeight = $('.hero-section').outerHeight();
            const bannerOffset = $('.hero-section').offset().top;
            const scrollPos = bannerHeight + bannerOffset;
            
            $('body, html').animate({scrollTop: scrollPos})
        });
        
    }
}