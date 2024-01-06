export default {

    scrollTop(elm, to){
        if(elm.scrollTop){
            elm.scrollTop = (to?to:0)
        }
    },

    byId(id, type, to){
        if(id){
			let elm = document.getElementById(id);
			if(elm){
				switch(type) {
                    case 'left':
                      // code block
                    break;
                    case 'top':
                        this.scrollTop(elm, to);
                    break;
                    default:
                      // code block
                  }
			}
		}
    }
}