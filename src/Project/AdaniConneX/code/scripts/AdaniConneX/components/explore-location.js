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
                767:{
                    dots:true,
                    items:1.5
                },
                1024:{
                    dots:true,
                    items:2.2
                },
                1340:{
                    items:3,
                    dots:true
                }
            }
        });
    }
}