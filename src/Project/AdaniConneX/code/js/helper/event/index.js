export default {

    dispatch(name, elm, cb, arg){
        if(name && elm){
            const event = new Event(name, {
                bubbles:true,
                composed:false,
                cancelable:true,
                arg:(arg?arg:false),
                cb:(cb?cb:()=>{}),
            });
            elm.dispatchEvent(event);
        }
    },

    bind(name, elm, cb){
        if(name && elm){
            elm.addEventListener(name, (e) => {
                if(cb){
                    cb(e);
                }
            });
        }
    }
}