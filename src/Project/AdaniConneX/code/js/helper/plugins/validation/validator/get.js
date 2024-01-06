export default {
    value(arg, map, fallback){
		let rv = 'NA';

		if(arg && (map || map === '')){
                rv = arg;
            let sm = map.split('.');
			
            for(var a in sm){
				if(typeof rv[sm[a]] != 'undefined'){
					rv = rv[sm[a]];
				}else{
					rv = 'NA';
					break;
				}
			}
		};

		if((typeof fallback != 'undefined') && (rv === 'NA')){
			return fallback;
		};

        if(rv != 'NA'){
            return rv;
        }else{
            return ''
        }
	},
    
    message(arg, type, fallback){
        return this.value(arg, (`message.${type}`), fallback);
    }
}