export default {
    bind(elm){
        const onKeyup = (e) => {
            let cls = 'filled';
            let elm = e.currentTarget;

            if((elm.value+'').length > 0){
                helpers.element.class.add(elm, cls);
            }else{
                helpers.element.class.remove(elm, cls)
            }
        };
    
        elm.addEventListener('keyup', onKeyup);
    },

    init(){
        let inputs = document.getElementsByClassName('input');

        for(let a in inputs){
            let elm = inputs[a];
            let type = elm.tagName;

            console.log(type);
            switch(type) {
                case 'INPUT':
                  this.bind(elm);
                break;
                case 'TEXTAREA':
                    this.bind(elm);
                break;
                default:
                  
              }
        };
    }
}