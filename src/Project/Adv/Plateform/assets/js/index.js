import "../../../common.css.proxy.js";

// =========== CHANGE HERE START
import "./adani-disruptive-page.css.proxy.js"
import template from "./AdaniDisruptive.js";
const content = getContent(template);
document.getElementById("root").appendChild(content)
// =========== CHANGE HERE END



// NOTIFICATION IMPORT 
// import "../notification/notification.scss";
// import notification from "../notification/Notification.template.html";
// const notificationContent = getContent(notification);
// document.getElementById("notification").appendChild(notificationContent)

// HEADER IMPORT 
import "../adv-header/adv-header.css.proxy.js";
import header from "../adv-header/AdvHeader.js";
const headerContent = getContent(header);
document.getElementById("header").appendChild(headerContent)

// FOOTER IMPORT
import "../adv-footer/adv-footer.css.proxy.js";
import footer from "../adv-footer/AdvFooter.js";
const footerContent = getContent(footer);
document.getElementById("footer").appendChild(footerContent)

function getContent(template) {
  const contentNode = template.content.cloneNode(true);
  return contentNode;
}