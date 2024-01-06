export default {
	add(el, cls) {
		if(el){
			var hasCls = this.has(el, cls);
			if(!hasCls){
				if(el.classList.value){
					var clses = el.classList.value;
						el.classList.value = clses+' '+cls;
				}else{
					let	clsList =	el.getAttribute('class');
					if(clsList){
						clsList = clsList.split(' ');
					}else{
						clsList = []
					}
					clsList.push(cls);
					el.setAttribute('class', clsList.join(' '));
				}
			}
		}
	},
	
	remove(elm, cls) {
		if(elm){
			let clss = elm.classList.value;
			if(!clss){
				clss = elm.getAttribute('class');
			};

			if(clss){
				clss = clss.split(' ');
				let clsI = clss.indexOf(cls);
				if(clsI >= 0){
					clss.splice(clsI, 1);
					clss = clss.join(' ');	
					elm.setAttribute('class', clss);
					this.remove(elm, cls);
				};
			}	  
		}
	},
			
	has(elm, cls){
		var clsList = []
		
		if(elm && elm.classList){
			clsList =	elm.getAttribute('class');
			//clsList = elm.classList.value;
		    if(clsList){
		    	clsList = clsList.split(' ');
		    }
		};
		if(clsList && clsList.indexOf(cls) > -1){
			return true;
		};
		return false;
	},
	
	toggle(elm, cls){
		var hasClass = this.has(elm, cls);
	    if(hasClass){
	      this.remove(elm, cls);
	    }else{
	      this.add(elm, cls)
	    }
	}
}