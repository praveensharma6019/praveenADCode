export default {
    init(){
        let cls = 'show';
        let target = 'takVirtaulTourPopup'
        let actions = document.getElementsByClassName('takVirtaulTourAction');

        for(let a in actions){
            let item = actions[a];
            if(item.getAttribute){
                helpers.plugins.toggle.init({
                    class:cls,
                    target:target,
                    action:item,
                    bodyClss:'no-scroll'
                });
            }
        }
        

        helpers.plugins.toggle.init({
            class:cls,
            target:target,
            bodyClss:'no-scroll',
            action:'takVirtaulTourPopupClose'
        });
    }
}