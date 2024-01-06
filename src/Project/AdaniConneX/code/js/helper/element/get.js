export default {
	
	byClass(a){
		
	},

	byName(name){
		if(name){
			let elm = document.getElementsByTagName(name)[0];
			if(elm){
				return elm;
			}
		};
		return false;
	},
	
	byId(a){
		if(a){
			var elm = document.getElementById(a);
			if(elm){
				return elm;
			}
		}
		return false;
	},
	
	byAttr(a){
		
	},

	height(elm){
		if(elm){
			return elm.offsetHeight;
		}
		return 0;
		let topOffset = elm.offsetTop;
        let height = elm.offsetHeight;
	},

	offset(elm, from){
		if(typeof elm === 'string'){
			elm = this.byId(elm);
		};

		if(elm){
			if(from === 'left'){
				
			}else{
				return elm.offsetTop
			}
		}
		return 0;
	},

	scrollTo(elm, from, px){
		let val = (px?px:0)
		if(elm){
			switch(from) {
				case 'left':
					elm.scrollTop = val;
				break;
				default:
					elm.scrollTop = val;
			}
		}
	},

	scroll(elm, from){
		if(elm){
			if(from === 'left'){
				
			}else{
				return elm.offsetTop
			}
		}
		window.pageYOffset
	}
	
}