export default {
    config:{
        attr:{
            min:'data-input-min',
            max:'data-input-max',
            regex:'data-input-regex',
            error:'data-input-error',
            success:'data-input-success',
            required:"data-input-required",
            validation:'[data-has-validation="enable"]'
        }
    },

    toggle(elm, error){
        if(elm && elm.classList){
            let cls = 'error';
            let clsl = elm.classList;

            if(error) {
                if(clsl.contains(cls)){

                }else{
                    clsl.add(cls);
                }
            }else{
                if(clsl.contains(cls)){
                    clsl.remove(cls);
                }
            }
        }
    },

    errors(validation, arg){
        if(arg && arg.form && arg.form.id){
            let attrs = this.config.attr;
            let fileds = validation.fields;
            let form = document.getElementById(arg.form.id);

            if(form){
                let inputs = form.querySelectorAll(attrs.validation);
                for(let a in inputs){
                    let input = inputs[a];

                    if(input.getAttribute){
                        let name = input.getAttribute('name');

                        if(fileds[name]){
                            this.toggle(input, fileds[name].error);
                        }
                    }
                }
            }
        }
    },

    getAttr(elm, attr, fb){
        let val = elm.getAttribute(attr);

        if(val){
            return val;
        }else{
            if(typeof val != 'undefined'){
                return fb;
            }
        }

        return false;
    },

    validation(elm){
        let attrs = this.config.attr;
        let min = this.getAttr(elm, attrs.min, false);
        let max = this.getAttr(elm, attrs.max, false);
        let regex = this.getAttr(elm, attrs.regex, false);
        let success = this.getAttr(elm, attrs.success, 'Valid');
        let error = this.getAttr(elm, attrs.error, 'Invalid input');
        let required = this.getAttr(elm, attrs.required, 'optional');
        let message = {
            error:error,
            success:success,
        };
        
        return {
            message:message,
            required:required,
            regex:{
                value:regex,
                message:message
            },
            length:{
                min:min,
                max:max,
                message:message
            },
        }
    },

    parse(elm){
        let rval = {
            value:''
        };

        let id = elm.getAttribute('id');
        let name = elm.getAttribute('name');
        let random = Math.floor(100000 + Math.random() * 900000);

        if(!id){
            id = random;
            elm.setAttribute('id', id);
        }

        if(!name){
            name = random;
            elm.setAttribute('name', name);
        }

        rval.name = name;
        rval.value = elm.value;
        rval.validation = this.validation(elm);

        return rval;
    },

    init(arg){
        let rval = {
            data:{},
            validation:{}
        };
        if(arg && arg.form && arg.form.id){
            rval.form = arg.form;
            let attrs = this.config.attr;
            let form = document.getElementById(arg.form.id);

            if(form){
                let inputs = form.querySelectorAll(attrs.validation);

                for(let a in inputs){
                    let input = inputs[a];

                    if(input.getAttribute){
                        let resp = this.parse(input);
                        let name = resp.name; 
                        rval.data[name] = resp.value;
                        rval.validation[name] = resp.validation;
                    }
                };
            }
        }

        return rval;
    }
}