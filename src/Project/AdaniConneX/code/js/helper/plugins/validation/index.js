import ui from './ui/';
import validator from "./validator/";

export default {
    ui:ui,
    start(arg, data){
        return validator.init(arg, data);
    },

    init(arg){
        let resp = this.ui.init(arg);
        let form = this.start(resp.validation, resp.data);
        let error = this.ui.errors(form, arg);
        return form
    }
    
}