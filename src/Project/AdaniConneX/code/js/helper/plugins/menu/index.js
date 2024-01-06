export default {
    config:{
        closer:'menu-closer',
        holder:'menuHolder',
        opener:'menuOpener',
        openCls:'menu-opened'
    },

    closeMenu(arg){
        let conf = this.config;
        let oc = conf.openCls;

        if(arg && arg.openCls){
            oc = arg.openCls;
        };
        helpers.element.class.remove(document.body, oc);
    },

    menuToggle(arg, id){
        let that = this;
        let conf = this.config;
        let oc = conf.openCls;

        if(arg && arg.openCls){
            oc = arg.openCls;
        };

        let elm = document.getElementById(id);

        let open = function(e){
            helpers.element.class.toggle(document.body, oc);
        };

        if(elm){
            elm.addEventListener("click", open);
        }
    },

    getElement(arg, type, idOnly){
        let conf = this.config;
        let id = conf[type];

        if(arg && arg[type]){
            id = arg[type];
        };

        if(idOnly){
            return id
        }else{
            return document.getElementById(id)
        }
    },

    intToggle(arg, type){
        let id = this.getElement(arg, type, true);

        if(id){
            this.menuToggle(arg, id)
        }
    },

    initCloser(arg){
        let that = this;
        let holder = this.getElement(arg, 'holder')
        let closer = function(e){
            let elm = e.target;

            if(elm === holder){
                that.closeMenu(arg);
            }else{
                let conf = that.config;
                let cls = conf.closer;

                if(arg && arg.closer){
                    cls = arg.closer;
                };

                if(cls){
                    let isCloser = helpers.element.class.has(elm, cls);
                    if(isCloser){
                        that.closeMenu(arg);
                    }
                }
            }
        }

        window.addEventListener('click', closer);
    },

    init(arg){
        this.intToggle(arg, 'opener');
        this.initCloser(arg);
    }
}