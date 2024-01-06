export default {
    init(){
        helpers.plugins.owl.init('#data-center-list', {
            items:3,
            responsiveClass: true,
            responsive: {
                0:{
                    dots:false,
                    items:1.1
                },
                1340:{
                    items:3,
                    dots:true
                }
            }
        });
    }
}