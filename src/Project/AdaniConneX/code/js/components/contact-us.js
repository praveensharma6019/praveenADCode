export default {
    init(){
        let cls = 'contact-active';
        let target = 'contactUsHolder'
        helpers.plugins.toggle.init({
            class:cls,
            target:target,
            action:'contactUsBtn'
        });

        helpers.plugins.toggle.init({
            class:cls,
            target:target,
            action:'contactUsBackdrop'
        });
    }
}