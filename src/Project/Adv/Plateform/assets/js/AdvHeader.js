const loader = (fileContents) => {
    const container = document.createElement("div");
    container.innerHTML = fileContents;

    const templateMerge = (dataObject, sourceNode, bindingAttribute) => {
        const targetNode = sourceNode.cloneNode(true);
        const props = Object.getOwnPropertyNames(dataObject);

        props.forEach(prop => {
            targetNode.querySelectorAll(`[${bindingAttribute}="${prop}"]`).forEach(ele => {
                ele.innerHTML = dataObject[prop];
            });
        });

        return targetNode;
    }

    const asModules = [...container.childNodes].map(template => {
        const markup = (template.innerHTML || "").trim();
        const content = template.content;
        const merge = (dataObject, bindingAttribute = "data-bind") => {
            return templateMerge(dataObject, content, bindingAttribute);
        };

        return { markup, template, content, merge };
    }).filter(item => item.markup != "");

    asModules.getById = (templateId) => {
        return asModules.filter(mod => { return mod.template.id === templateId })[0];
    }

    return asModules.length === 1 ? asModules[0] : asModules;
};
const markup = "<template>\r\n  <div class=\"custom-header\">\r\n    <div class=\"row\">\r\n      <div class=\"col-md-3 header-logo\">\r\n        <a href=\"/adani-disruptive-ventures\">\r\n          <img src=\"/assets/images/adv/logo.png\" alt=\"Adani Disruptive Ventures\">\r\n        </a>\r\n        <div class=\"mobile-trigger\">\r\n          <img src=\"/assets/images/adv/hamburger.png\" alt=\"Adani Disruptive Ventures\" class=\"mob-icon\">\r\n          <img src=\"/assets/images/adv/menu-close.png\" alt=\"Adani Disruptive Ventures\" class=\"mob-close\">\r\n        </div>\r\n      </div>\r\n      <div class=\"col-md-9 menu-section\">\r\n        <ul>\r\n          <li><a href=\"/adv-mission\">Mission</a></li>\r\n          <li><a href=\"/adv-portfolio\">Portfolio</a></li>\r\n          <li><a href=\"/adv-team\">Team</a></li>\r\n          <li><a href=\"/adv-contact\">Contact</a></li>\r\n        </ul>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</template>";
const templates = loader(markup);
export default templates;