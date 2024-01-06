export default {
    init(){

        let bdy = document.body
        let cls = 'mobile-search-opened';
        
        helpers.plugins.toggle.init({
            class:cls,
            target:bdy,
            action:'mobileSearch'
        });

        helpers.plugins.toggle.init({
            class:cls,
            target:bdy,
            action:'mobileSearchClose'
        })
    }
}