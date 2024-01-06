import get from "./get";
import set from "./set";

export default {

    message(arg, message, success){
        if(message){
            return message;
        }else{
            if(success){
                return message;
            }else{
                let min = arg.min;
                let max = arg.max;
    
                if(min > 0 && max > 0){
                    return (`Length range is ${min} to ${max}`)
                }else{
                    if(min > 0){
                        return (`Minimum required length is ${min}`)
                    }else{
                        if(max > 0){
                            return (`Length can't be more than ${max}`)
                        }
                    }
                }
            }
        }
    },

    init(arg){
        let value = '';

        if(arg.value){
            value = (arg.value+'')
        }

        if(arg.length){
            let min = arg.length.min;
            let max = arg.length.max;
            let error = get.message(arg.length, 'error');
            let success = get.message(arg.length, 'success');
                error = this.message(arg.length, error);
                success = this.message(arg.length, success, true);

            if((min && min > 0) || (max && max > 0)){
                if(min && min > 0){
                    if(value.length >= min){
                        arg.validation = set.validation(arg.validation, true, success);
                        if(max && max > 0){
                            if(value.length <= max){
                                arg.validation = set.validation(arg.validation, true, success);
                            }else{
                                arg.validation = set.validation(arg.validation, false, error);
                            }
                        }
                    }else{
                        arg.validation = set.validation(arg.validation, false, error);
                    }
                }else{
                    if(max && max > 0){
                        if(value.length <= max){
                            arg.validation = set.validation(arg.validation, true, success);
                        }else{
                            arg.validation = set.validation(arg.validation, false, error);
                        }
                    }
                }
            }
        }

        return arg.validation;
    }
}