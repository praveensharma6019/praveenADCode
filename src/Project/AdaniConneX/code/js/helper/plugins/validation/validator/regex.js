import get from "./get";
import set from "./set";

export default { 

    regex(arg){
        let regx = arg.value;

        if(regx){
            if(regx instanceof RegExp){
                return regx;
            }else{
                return new RegExp(regx)   
            }
        }

        return false;
    },

    init(arg){
        let value = '';

        if(arg.value){
            value = (arg.value+'')
        }

        if(arg.regex){
            let regex = this.regex(arg.regex);

            if(regex){
                let error = get.message(arg.regex, 'error');
                let success = get.message(arg.regex, 'success');

                if(value.match(regex)){
                    arg.validation = set.validation(arg.validation, true, success);
                }else{
                    arg.validation = set.validation(arg.validation, false, error);
                }
            }

        }

        return arg.validation;
    }
}