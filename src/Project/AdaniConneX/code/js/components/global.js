import search from "./search";
import contact from "./contact-us";

export default {
    init(){
        search.init();
        contact.init();
        helpers.plugins.menu.init();
        helpers.plugins.accordion.init();
    }
}