import awards from "./../../components/awards";
import global from "./../../components/global";

(function () {
    if ((typeof isDev != 'undefined') && isDev === true){
        window.helpers.page.extend('aboutus', {
            init(){
                global.init();
                awards.init();
            }
        });
    }else{
        global.init();
        awards.init();
    }
    
})();
