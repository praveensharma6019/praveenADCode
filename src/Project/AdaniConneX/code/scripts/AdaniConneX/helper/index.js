import url from './url';
import page from './page';
import event from './event';
import element from './element/';
import plugins from './plugins/';
import device from './user-agent/';

(function(){
    window.helpers = {
        url:url,
        page:page,
        event:event,
        device:device,
        element:element,
        plugins:plugins 
    };

    window.isDev = false;
    let host = window.location.host;
    let hostmap = {
        '127.0.0.1:8003':true,
        'localhost:8003':true,
        '127.0.0.1:8080':true,
        'localhost:8080':true
    }

    if(hostmap[host]){
        window.isDev = true;
    }

    window.helpers.plugins.include.init();
})();