export default {
	init(arg){
		/*--var temp = {
				conf:{
					submit:false,
					skipAppend:true,
					attrs:{
						method:'post',
						action:''
					}
				},
				inputs:[
				   {
					   name:'test',
					   id:'testid'
				   },
				   {
					   name:'test1',
					   id:'testid1',
					   type:'text'
				   }
				]
			}
		arg = temp;--*/
		var conf = arg.conf;
		if(conf && arg.inputs){
			var inputs = arg.inputs,
				f = this.createForm(conf.attrs);
			for(var i in inputs){
				f.appendChild(this.createInput(inputs[i]));
			};
			return this.submitForm(f, conf);
		}
		return false;
	},
		
	submitForm(f, arg){
		if(arg.submit){
			document.body.appendChild(f);
			f.submit();
		}else{
			return f;
		}
	},
		
	createForm(arg){
		return this.setMultipleAttr(document.createElement('form'), arg);
	},
		
	createInput(arg){
		if(!arg.type){
			arg.type = 'hidden';
		};
		return this.setMultipleAttr(document.createElement('input'), arg);
	},

	setMultipleAttr(el, arg){
		for(var a in arg){
			if(arg[a]){
				el.setAttribute(a, arg[a]);
			}
		};
		return el;
	}
}