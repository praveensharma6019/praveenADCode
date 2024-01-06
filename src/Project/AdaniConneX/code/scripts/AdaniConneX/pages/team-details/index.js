import global from "./../../components/global";

(function () {
    if ((typeof isDev != 'undefined') && isDev === true) {
        window.helpers.page.extend('team-details', {
            init(){
                global.init();
                window.helpers.plugins.owl.init('#teamPersons', {
                    items: 2,
                    responsiveClass: true,
                    responsive: {
                        0: {
                            dots: false,
                            items: 1.2,
                            margin:30
                        },
                        1340: {
                            items:4,
                            margin:30,
                            dots: true
                        }
                    }
                });
            } 
        });
    }else{
        global.init();
        window.helpers.plugins.owl.init('#teamPersons', {
            items: 2,
            responsiveClass: true,
            responsive: {
                0: {
                    dots: false,
                    items: 1.2,
                    margin:30
                },
                1340: {
                    items:4,
                    margin:30,
                    dots: true
                }
            }
        });
    }
})();
