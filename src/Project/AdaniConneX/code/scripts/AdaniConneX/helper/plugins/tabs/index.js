export default {
    config:{
        holder:false,
        current:1,
        doneCls:'completed',
        activeCls:'active-step',
        action:'stepper-action',
        indexAttr:'data-stepper-action-index',
    },
    
    toggleContent(elm, show){
        if(elm && elm.getAttribute){
          elm.classList.add((show?'active':'hide'));
          elm.classList.remove((show?'hide':'active'));
        }
    },
    
    resetContent(elm){
        let cHoler = elm.getAttribute('data-stepper-content-holder');
        let cWrapper = elm.getAttribute('data-stepper-content-wrapper');
        if(cWrapper && cHoler){
          let wrapper = document.getElementById(cWrapper);
          if(wrapper){
            let cl = wrapper.getElementsByClassName(cHoler);
            for(let a in cl){
              this.toggleContent(cl[a]);
            }
          }
        }
    },
    
    resetSteps(event, holder, arg){
        let elm = event.currentTarget;
        let al = holder.getElementsByClassName(arg.action);
        let index = elm.getAttribute(this.config.indexAttr);
    
        for(let a in al){
          let action = al[a];
          if(action.getAttribute){
            if(parseInt(a) > parseInt(index)){
              action.classList.remove(arg.doneCls);
              action.classList.remove(arg.activeCls);
            }
    
            if(parseInt(a) < parseInt(index)){
              action.classList.add(arg.doneCls);
              action.classList.remove(arg.activeCls);
            }
    
            if(elm === action){
              action.classList.remove(arg.doneCls);
              action.classList.add(arg.activeCls);
            }
          }
        }
    
        let target = elm.getAttribute('data-content-target');
        if(target){
          this.toggleContent(document.getElementById(target), true);
        }
      },
    
      action(e, arg){
        let elm = document.getElementById(arg.holder);
        if(elm){
            this.resetContent(elm);
            this.resetSteps(e, elm, arg);
        }
      },
    
      goTo(name, index, cb){
        let elm = document.getElementById((`${name}-action-${index}`));
        if(elm){
          elm.click();
        }
    
        if(cb){
          cb()
        }
      },
    
      bind(a){
        let that = this;
        let arg = (a?a:{});
        let conf = {...this.config, ...arg};
        
        const action = (e) => {
          that.action(e, conf);
        };
    
    
    
        if(conf.action && conf.holder){
          let holder = document.getElementById(arg.holder);
          if(holder){
            let list = holder.getElementsByClassName(conf.action);
            for(let a in list){
              let elm = list[a];
              if(elm.getAttribute){
                elm.addEventListener('click', action);
                elm.setAttribute(conf.indexAttr, a);
                elm.setAttribute('id', (`${conf.holder}-action-${parseInt(a)+1}`));
              }
            }
          }else{
            console.log('Stepper holder is not defined....')
          }
    
          that.goTo(arg.holder, conf.current);
        }
      },
    
      init(a){
        this.bind(a);
        window.goToStepNo = this.goTo;
      }
}