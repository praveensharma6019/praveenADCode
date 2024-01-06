export default {

    getDomain(origin){
        let d = 'monsterindia.com';
        let host = window.location.hostname;

        if(host === 'localhost' || host === 'local.monsterindia.com'){
            return host;
        }else{

            let org = window.location.host
                org = org.split('monster');
    
            if(org && org.length > 1){
                d = ('monster'+org[1]);   
            }

            if(!origin && typeof _siteProps_ != 'undefined' && _siteProps_.domain){
                return _siteProps_.domain;
            }
    
            if(origin){
                return location.origin;
            };
        }

        return d;
    },

    getParams(name, url) {
        url = url?url:window.location.href;
        let qS = url ? url.split('?')[1] : window.location.search.slice(1);
        let rVal = {};

        if(qS){
            qS = qS.split('#')[0];
            let arr = qS.split('&');
            for(let i = 0; i < arr.length; i++) {
                let a = arr[i].split('=');
                let pN = a[0];
                let pV = typeof (a[1]) === 'undefined' ? true : a[1];

                if(pN.match(/\[(\d+)?\]$/)){
                    let key = pN.replace(/\[(\d+)?\]/, '');
                    
                    if(!rVal[key]) rVal[key] = [];
                    if (pN.match(/\[\d+\]$/)) {
                        let index = /\[(\d+)\]/.exec(pN)[1];
                        rVal[key][index] = pV;
                    }else{
                        rVal[key].push(pV);
                    }
                } else {
                    if (!rVal[pN]) {
                        rVal[pN] = pV;
                    }else if (rVal[pN] && typeof rVal[pN] === 'string'){
                        rVal[pN] = [rVal[pN]];
                        rVal[pN].push(pV);
                    }else{
                        rVal[pN].push(pV);
                    }
                }
            }
        }
      
        if(name){
            return rVal[name]?rVal[name]:false;
        }
        return rVal;
    },

    getHash(url){
        let path = (url?url:location.href);
            path = path.split('#');
        if(path.length > 1){
            let last = (path.length-1);
            return path[last]
        }else{
            return '';
        }
    },

    getOriginPath(url){
        let path = (url?url:location.href);
        return (path.split('?')[0])
    },

    serailize(arg, encode){
		var r = "";
		for(var a in arg) {
            if(a){
                let val = arg[a];
                if(val || val === 0){
                    if(encode){
                        r += a+"="+encodeURIComponent(val)+"&";
                    }else{
                        r += a+"="+val+"&";
                    }
                    
                }   
            }
		}
		return r.substr(0, r.length-1);
	},

    removeParams(list, url, get){
        let params = this.getParams(false, url);
        let href = url?url:location.href;
        for(let a = 0; a <list.length; a++){
            let name =  list[a];
                href = mHelper.string.replace.word(href, ('&'+name+'='+params[name]), '');
                href = mHelper.string.replace.word(href, (name+'='+params[name]), '');
        };

        if(get){
            return href;
        }else{
            window.history.pushState({}, document.title, href);
        }
    },

    pushParams(arg, url, reload, get){
        let href = (url?url:location.href);
        let hash = this.getHash(href);
        let params = this.getParams(false, href);
            href = this.getOriginPath(href);
            params = params?params:{};
        
        if(arg){
            for(let a in arg){
                params[a] = arg[a];
            };
        };

        href = (href+"?"+this.serailize(params));
        href = (hash?(href+'#'+hash):href);

        if(get){
            return href;
        }else{
            if(reload){
                window.location.href = href;
            }else{
                window.history.pushState({}, document.title, href);
            }
        }
    },

    login(params){
        let hash = this.getHash();
        let query = this.getParams();
        let rurl = '/seeker/dashboard';
        let path = window.location.pathname;
        let excludes = ['/rio/login', '/rio/sign-out'];
        let loginPath = mHelper.json.getValue(window, '_ssoPath_.riologinPath', '/rio/login/seeker');

        if(path && path.length > 0 && excludes.indexOf(path) > -1){
            
        }else{
            query = mHelper.json.merge((query?query:{}), (params?params:{}));
            rurl = (path+"?"+this.serailize(query));
            rurl = (hash?(rurl+'#'+hash):rurl);
        };

        return (`${loginPath}?return_url=${(encodeURIComponent(rurl))}`);
    },

    redirect(url, params, get){
        let rurl = false;
        switch(url) {
            case 'login':
                rurl = this.login(params);
            break;
            default:
              // code block
        }

        if(rurl){
            if(get){
                return rurl;
            }else{
                window.location.href = rurl;
            }
        }else{
            if(get){
                return '#';
            }
        }
    },

    appendSpl(u){
        let url = (u?u:window.location.href);
        if(url){
            let hash = this.getHash(url);
            let spls = mHelper.spl.params();
            let path = this.getOriginPath(url);
            let query = this.getParams(false, url);
            let params = mHelper.json.merge((spls?spls:{}), (query?query:{}));


            //delete params['application_source'];
            delete params['x-source-platform'];

            let rurl = (path+"?"+this.serailize(params));
            return (hash?(rurl+'#'+hash):rurl);
        }else{
            return url;
        }
    }
}