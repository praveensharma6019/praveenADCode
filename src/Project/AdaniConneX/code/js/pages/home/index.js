import hero from "./hero";
import blog from "./blog";
import awards from "./../../components/awards";
import global from "./../../components/global";
import sustainability from "./../../components/sustainability";
(function(){
    helpers.page.extend('home', {
       init(){
            hero.init();
            blog.init();
            awards.init();
            global.init();
            sustainability.init();
       } 
    });
})();