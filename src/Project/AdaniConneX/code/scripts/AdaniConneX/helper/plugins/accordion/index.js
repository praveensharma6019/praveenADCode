    export default {
        config:{
            activeClass:'active',
            link:'adl-accordion-action'
        },
        toggle(event, arg, h, c, t){
            let cElm = event.currentTarget;
            let holder =document.getElementById(h);
            let target =document.getElementById(t);
            let screen = cElm.getAttribute('data-accordion-skip-lh');
            let linked = cElm.getAttribute('data-accordion-linked');
    
            if(screen){
                screen = parseInt(screen);
            }
    
            if((!screen) || (screen && (screen > 0) && (screen >= window.innerWidth))){
                if(holder && c){
                    let aCls = this.getElement(arg, 'activeClass', true);
                    let containers = holder.getElementsByClassName(c);
    
                    for(let a in containers){
                        let elm = containers[a];
                        if(elm.getAttribute){
                            helpers.element.class.remove(elm, aCls);
                        }
                    }
    
                    let links = holder.getElementsByClassName(arg.link);
                    for(let a in links){
                        let elm = links[a];
                        if(elm.getAttribute){
    
                            if(elm != cElm){
                                helpers.element.class.remove(elm, aCls);
                            }else{
                                helpers.element.class.add(elm, aCls);
                            }
                        }
                    }
    
                    if(linked){
                        let links = holder.getElementsByClassName(linked);
                        for(let a in links){
                            let elm = links[a];
                            if(elm.getAttribute){
                                helpers.element.class.add(elm, aCls);
                            }
                        }
                    }
    
                    if(target){
                        helpers.element.class.toggle(target, aCls);
                    }
                }
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
                return document.getElementsByClassName(id)
            }
        },
    
        bind(arg){
            let that = this;
            let elms = this.getElement(arg, 'link');
    
            if(elms){
                for(let a in elms){
                    let elm = elms[a];
                    if(elm.getAttribute){
                        let target = elm.getAttribute('data-accordion-target');
                        let holder = elm.getAttribute('data-accordion-holder');
                        let contents = elm.getAttribute('data-accordion-content');
    
                        let open = function(e){
                            that.toggle(e, arg, holder, contents, target);
                        };
    
                        if(holder){
                            elm.addEventListener("click", open);
                        }
                    }
                }
            }
        },
    
        init(arg){
            let conf = {...this.config, ...(arg?arg:{})}
            this.bind(conf);
        }
    }

