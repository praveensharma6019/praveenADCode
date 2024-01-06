import page from './page';
import event from './event';
import element from './element/';
import plugins from './plugins/';
import device from './user-agent/';

(function(){
    window.helpers = {
        page:page,
        event:event,
        device:device,
        element:element,
        plugins:plugins 
    };

    window.helpers.plugins.include.init();
})();