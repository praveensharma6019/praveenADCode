export default {
	
	get(el, atr){
		if(el && (el.getAttribute(atr) || el.getAttribute(atr) === '')){
			return el.getAttribute(atr);
		}
		return false;
	},
			
	set(elm, attr, val){
		if(elm && elm.setAttribute){
			elm.setAttribute(attr, val);
		};
	},
	
	has(elm, attr){
		
	},
	
	remove(elm, attr){
		if(elm && elm.removeAttribute){
			elm.removeAttribute(attr);
		};
	}
	
}