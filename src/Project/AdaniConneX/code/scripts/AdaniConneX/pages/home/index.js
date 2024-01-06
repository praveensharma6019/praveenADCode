import hero from "./hero";
import blog from "./blog";
import awards from "./../../components/awards";
import global from "./../../components/global";
import sustainability from "./../../components/sustainability";
(function(){
    if ((typeof isDev != 'undefined') && isDev === true) {
        window.helpers.page.extend('home', {
            init(){
                hero.init();
                blog.init();
                awards.init();
                global.init();
                sustainability.init();
            } 
        });
    }else{
        hero.init();
        blog.init();
        awards.init();
        global.init();
        sustainability.init();
    }
})();