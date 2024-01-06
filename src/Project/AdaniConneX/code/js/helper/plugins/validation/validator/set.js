export default {
    validation(a, v, m){

		let rval = (a?a:{
            valid:v,
			value:'',
            error:(!v),
			message:''
		});

		rval.valid = v;
		rval.error = (!v);
		rval.message = (m?m:'');

		return rval;
	}
}