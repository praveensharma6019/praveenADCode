export default {
    init(){
        helpers.plugins.owl.init('#blogDesktop', {items:1, dots:true});
        helpers.plugins.owl.init('#blogModule_mob', {items:1.1, dots:false});
    }
}