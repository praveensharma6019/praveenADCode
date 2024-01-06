
function number(e)
{	
	var key;
	var keychar;
	if (window.event){
		key = window.event.keyCode;		
	}else if (e){
		key = e.which;		
	}
	else
		return true;
	
	if((key == 8) || (key == 0))
		return true;
		
	keychar = String.fromCharCode(key);
	keychar = keychar.toLowerCase();
	if((key > 47) && (key < 58)){				
		return true;
	}else
	    return false;	
}
