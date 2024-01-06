export default {
	init(arg){
		/*arg = {
			id:'',
			url:'',
			params:{

			},
			callback:{
				onerror:function(arg){
					
				},
				onload:function(arg){
					
				}
			}
		}--*/

		if(arg.url){
			var app = this.getApp(),
				iframe = document.createElement('iframe');

				app.helper.element.remove(arg.id);

				iframe.frameBorder=0;
				iframe.width = 0;
				iframe.height = 0;
				iframe.id = arg.id?arg.id:'';
				iframe.setAttribute("src", app.helper.url.get(arg.url, arg.params));

			if(arg.callback){
				if(arg.callback.onerror){
					iframe.onerror = function(){
						arg.callback.onerror(arg)
					}
				}
				if(arg.callback.onload){
					iframe.onload = function(){
						arg.callback.onload(arg)
					}
				}
			}

			document.body.appendChild(iframe);
		}
	}
}