export default {

    onInclude(){
        let page = helpers.element.attr.get(document.body, 'data-page');
        if(window.pages && window.pages[page] && window.pages[page].init){
            window.pages[page].init();
            helpers.plugins.input.init();
            window.contactUs = function(){
                helpers.plugins.validation.init({form:{id:'contact-us'}});
            }
        };
    },

    extend(name, arg){
        window.pages = window.pages || {};
        if(name && arg){
            window.pages[name] = arg;
        }
    }
}