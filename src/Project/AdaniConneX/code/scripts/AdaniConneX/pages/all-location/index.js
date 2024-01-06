import global from "./../../components/global";

(function () {
    let timeout = 0;

    if ((typeof isDev != 'undefined') && isDev === true) {
        timeout = 1000;
    }

    setTimeout(() => {
        global.init();
        //console.log(timeout);
    }, timeout); 
})();
