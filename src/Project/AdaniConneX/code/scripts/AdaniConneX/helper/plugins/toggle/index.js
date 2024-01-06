export default {
    init(arg){
        if(arg && arg.action && arg.target && arg.class){
            let target = arg.target;
            let elmH = helpers.element;
            let action = arg.action;

            if(typeof action === 'string'){
                action = elmH.get.byId(arg.action);
            }

            if(typeof target === 'string'){
                target = elmH.get.byId(arg.target);
            }

            let toggle = (e) => {
                e.preventDefault();
                elmH.class.toggle(target, arg.class, arg.bodyClss);
            }

            if(action && target){
                action.addEventListener("click", toggle);
            }
        }
    }
}