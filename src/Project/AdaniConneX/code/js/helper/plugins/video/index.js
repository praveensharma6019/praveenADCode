export default {
    class(arg, played){
        let elm = document.getElementById(arg.holder);

        if(elm){
            let pCls = (arg.playClass?arg.playClass:'played');
            if(played){
                helpers.element.class.add(elm, pCls);
            }else{
                helpers.element.class.remove(elm, pCls);
            }
        }
    },

    init(arg){

        if(arg && arg.btn && arg.player){
            let that = this;
            let elm = document.getElementById(arg.btn);

            let playPause = function(e){
                let video = document.getElementById(arg.player);

                if(video){
                    if(video.paused){
                        video.play();
                        that.class(arg, true); 
                    }else{
                        video.pause();
                        that.class(arg, false); 
                    }
                }
            };

        if(elm){
            elm.addEventListener("click", playPause);
        }
        }
        
    }
}