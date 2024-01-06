export default {
    init(){
        let cls = 'show';
        let target = 'whitePaperPopup'
        let actions = document.getElementsByClassName('whitePaperAction');

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
            action:'whitePaperPopupClose',
            bodyClss:'no-scroll'
        });
    }
}