export default {

	keyup(elm, arg, clb){
		if(elm){
			elm.onkeyup = function(e) {
			    if(clb){
			    	clb(e, arg);
			    }
			};
		};
	},
			
	keydown(elm, arg, clb){
		if(elm){
			elm.onkeydown = function(e) {
			    if(clb){
			    	clb(e, arg);
			    }
			};
		};
	},

	onfocus(elm, arg, clb){
		if(elm){
			elm.onkeydown = function(e) {
			    if(clb){
			    	clb(e, arg);
			    }
			};
		};
	},

	bindCallback(e, type){
        let arg = e.currentTarget.arg;
        if(arg.callback && arg.callback[type]){
            arg.callback[type](e, e.currentTarget);
        };
	},

	bindEvents(elm, events){
        for(var a in events){
            elm.addEventListener(a, this.bindCallback);
        }
    },

	init(id, arg){
        let that = this;
        setTimeout(function(){
            var elm = getCore().helper.element.get.byId(id);
            if(elm && arg){
                elm.arg = arg;
                that.bindEvents(elm, arg.events);
            }
        }, 100);
    }
}