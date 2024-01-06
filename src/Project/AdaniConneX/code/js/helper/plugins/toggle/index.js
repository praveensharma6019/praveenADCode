export default {
    init(arg){
        if(arg && arg.action && arg.target && arg.class){
            let target = arg.target;
            let elmH = helpers.element;
            let action = elmH.get.byId(arg.action);

            if(typeof target === 'string'){
                target = elmH.get.byId(arg.target);
            }

            let toggle = () => {
                elmH.class.toggle(target, arg.class);
            }

            if(action && target){
                action.addEventListener("click", toggle);
            }
        }
    }
}