export default {
    init() {
        helpers.plugins.owl.init('#awardsList', {
            items: 2,
            responsiveClass: true,
            responsive: {
                0: {
                    dots: false,
                    items: 1.1
                },
                767:{
                    dots:true,
                    items:1.5
                },
                1024:{
                    dots:true,
                    items:1.1
                },
                1340: {
                    items: 2,
                    dots: true
                }
            }
        });

        helpers.plugins.owl.init('#awardsList3', {
            items: 3,
            responsiveClass: true,
            responsive: {
                0: {
                    dots: false,
                    items: 1.1
                },
                767:{
                    dots:true,
                    items:1.5
                },
                1024:{
                    dots:true,
                    items:1.1
                },
                1340: {
                    items: 3,
                    dots: true
                }
            }
        });
    }
}