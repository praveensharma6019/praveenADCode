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
const markup = "<template>\r\n  <footer class=\"custom-footer\" style=\"background-image: url(/assets/images/adv/footer-bg.png);\">\r\n    <div class=\"footer-inner\">\r\n      <div class=\"footer-description\">\r\n        <div class=\"container\">\r\n          <h3 class=\"bg-new\">Does your idea add<br> that 'extra' to the ordinary?</h3>\r\n          <h3 class=\"bg-new\">We're all ears.</h3>\r\n        </div>\r\n      </div>\r\n      <div class=\"footer-top\">\r\n        <div class=\"container\">\r\n          <div class=\"row\">\r\n            <div class=\"col-md-6 left-section\" data-aos=\"fade-up\" data-aos-easing=\"ease\">\r\n              <h3>Where to Find Us</h3>\r\n              <p>Adani House, 83,<br> Institutional Area, Sector 32,<br> Gurugram, Haryana 122001</p>\r\n              <ul>\r\n                <li><a href=\"/adv-mission\">Mission</a></li>\r\n                <li><a href=\"/adv-portfolio\">Portfolio</a></li>\r\n                <li><a href=\"/adv-team\">Team</a></li>\r\n                <li><a href=\"/adv-contact\">Contact</a></li>\r\n              </ul>\r\n            </div>\r\n            <div class=\"col-md-6 right-section\" data-aos=\"fade-up\" data-aos-easing=\"ease\">\r\n              <div class=\"inner\">\r\n                <h3>Join our social community</h3>\r\n                <form>\r\n                  <div class=\"form-group\">\r\n                    <input type=\"email\" placeholder=\"Email ID\" class=\"form-control\">\r\n                    <button type=\"submit\" class=\"btn btn-primary\">Subscribe</button>\r\n                  </div>\r\n                </form>\r\n                <div class=\"social-icons\">\r\n                  <ul>\r\n                    <li><a href=\"#\"><img src=\"/assets/images/adv/twitter.png\" alt=\"Twitter\"></a></li>\r\n                    <li><a href=\"#\"><img src=\"/assets/images/adv/linkedin.png\" alt=\"linkedin\"></a></li>\r\n                  </ul>\r\n                </div>\r\n              </div>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"footer-bottom\">\r\n        <div class=\"container\">\r\n          <div class=\"row\">\r\n            <div class=\"col-md-6\">\r\n              <p>© 2022 Adani Disruptive Ventures</p>\r\n            </div>\r\n            <div class=\"col-md-6\">\r\n              <ul>\r\n                <li><a href=\"/adv-privacy\">Privacy</a></li>\r\n                <li><a href=\"/adv-privacy\">Legal</a></li>\r\n                <li><a href=\"/adv-privacy\">Terms</a></li>\r\n              </ul>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </footer>\r\n</template>";
const templates = loader(markup);
export default templates;