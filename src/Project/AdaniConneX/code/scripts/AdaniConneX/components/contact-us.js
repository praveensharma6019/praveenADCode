export default {
    init(){
        let cls = 'contact-active';
        let target = 'contactUsHolder';

        helpers.plugins.toggle.init({
            class:cls,
            target:target,
            bodyClss:'no-scroll',
            action:'contactUsBtn'
        });

        helpers.plugins.toggle.init({
            class:cls,
            target:target,
            bodyClss:'no-scroll',
            action:'contactUsBackdrop'
        });
    }
}