import scroll from './scroll';
import attr from './attributes';
import create from './create/';
import event from './events/';
import classs from './class';
import get from './get';

export default {
	get:get,
	attr:attr,
	event:event,
	class:classs,
	create:create,
	scroll:scroll,

	insetAfter(prnt, elm){
		prnt.parentNode.insertBefore(elm, prnt.nextSibling)
	},

	remove(id){
		var elm = this.get.byId(id);
		if(elm){
			elm.parentNode.removeChild(elm);
		}
	}
}