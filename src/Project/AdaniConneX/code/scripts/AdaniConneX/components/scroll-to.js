export default {
    init() {
        let elmH = helpers.element;
        let actions = document.getElementsByClassName('scrollTo');

        const onClick = function(elm){
            let target = elm.currentTarget.getAttribute('data-scroll-to');
            if(target){
                let elm = elmH.get.byId(target);
                if(elm && elm.offsetTop){
                    window.scrollTo({
                        top:elm.offsetTop - 40,
                        behavior: 'smooth',
                    });
                }
            }
        }

        for(let a in actions){
            let item = actions[a];
            if(item.getAttribute){
                 item.addEventListener('click', onClick);
            }
        }
    }
}