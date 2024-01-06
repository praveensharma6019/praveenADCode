export default {
    init(){
        let cls = 'show';
        let target = 'takeATourPopup'
        let actions = document.getElementsByClassName('takeTourAction');

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
            action:'takeATourPopupClose',
            bodyClss:'no-scroll'
        });
    }
}