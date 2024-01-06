export default {
    init(arg){
        let target = helpers.url.getParams('target');
        let elmH = helpers.element;
       
        if(arg && arg.target){
            target = arg.target;
        }


        if(target){
            let elm = elmH.get.byId(target);

            if(elm && elm.offsetTop){
                window.scrollTo({
                    top:elm.offsetTop,
                    behavior: 'smooth',
                });
            }
        }
    }
}