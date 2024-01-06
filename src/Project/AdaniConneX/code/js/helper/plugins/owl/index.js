export default {
    init(id, arg){
        if(id && $(id).owlCarousel){
            let conf = arg?arg:{};
                conf.loop = true;
                conf.autoplay = true;
                conf.autoplayTimeout = 5000;
                conf.autoplayHoverPause = true;

            $(id).owlCarousel(conf);
        }
    }
}