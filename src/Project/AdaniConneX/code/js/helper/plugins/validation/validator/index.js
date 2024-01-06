let conf = {
    'name':{
        regex:'name',
        required:'required',
        length:{
            min:6,
            message:{

            }
        },
        message:{
            success:'Passed',
            error:'Name only can cantain alpha bets.'
        }
    },

    'email':{
        regex:{
            map:'',
            value:'^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$'
        },
        required:'optional',
        message:{
            error:'Please a valid email id.'
        }
    }
}

const form = {
    name:'dddd',
    email:''
};

import get from './get';
import set from './set';
import regex from './regex';
import length from './length';

export default {

    schema(val){
        return {
            message:'',
            valid:true,
            error:false,
            value:(val?val:'')
        }
    },

    required(arg){
        let value = '';
        let option = arg.required;
        let error = get.message(arg, 'error');
        let success = get.message(arg, 'success');

        if(arg.value){
            value = (arg.value+'')
        }

        switch(option) {
            case 'optional':
                arg.validation = set.validation(arg.validation, true, success);
            break;
            case 'required':
                if(value.length > 0){
                    arg.validation = set.validation(arg.validation, true, success);
                }else{
                    arg.validation = set.validation(arg.validation, false, error);
                }
            break;
            default:
        };

        return arg.validation;
    },

    start(arg){
        let res = this.required(arg);

        if(res.valid){
            res = length.init(arg);
        }

        if(res.valid){
            res = regex.init(arg);
        }

        return res;
    },

    init(a, d){
        let arg = (a?a:{});
        let data = (d?d:{});
        let rval = {
            fields:{},
            valid:true,
        };

        

        for(let a in arg){
            let item = (arg[a]?arg[a]:{});
                item.value = get.value(data, a);
                item.validation = this.schema(item.value);

            let res = this.start(item);

            if(rval.valid){
                rval.valid = res.valid;
            }

            rval.fields[a] = res;
        }

        return rval;
    }
}