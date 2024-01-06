export default {

    conmmon(){
        helpers.plugins.input.init();

        window.contactUs = function(){
            helpers.plugins.validation.init({form:{id:'contact-us'}});
        }

        window.takeATour = function(){
            helpers.plugins.validation.init({form:{id:'takeATour'}});
        }

        window.whitePaper = function(){
            helpers.plugins.validation.init({form:{id:'whitePaperForm'}});
        }

        window.getInTouch = function(){
            helpers.plugins.validation.init({form:{id:'getInTouch'}});
        }

        window.closeModal = function(){
            let elm = document.getElementsByClassName('popup-box show');
            helpers.element.class.toggle(elm[0], 'show', 'no-scroll');
        }
    },

    onInclude(){
        let page = helpers.element.attr.get(document.body, 'data-page');
        if ((typeof isDev != 'undefined') && isDev === true) {
            if(window.pages && window.pages[page] && window.pages[page].init){
                window.pages[page].init();
                this.conmmon();
            };
        }else{
            this.conmmon();
        }
        
    },

    extend(name, arg){
        window.pages = window.pages || {};
        if(name && arg){
            window.pages[name] = arg;
        }
    }
}