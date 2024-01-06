"use strict";

const helper = {
  "baseTag" : "include",
  "attrTag" : "component",
  "baseDir" : "components",
  "defaultComponentFile" : "index.html",
  "componentFilePath"  : function(componentName){
    return `/${helper.baseDir}/${componentName}/${helper.defaultComponentFile}`;
  },
  "includeHTML" : function (cb) {
  
    let tags, file, component;
    tags = document.getElementsByTagName(helper.baseTag);
    // console.time('pf');
  
    Object.values(tags).forEach((element) => {
      component = element.getAttribute(helper.attrTag);
  
      if (component) {
  
        file = helper.componentFilePath(component);
        
        fetch(file)
          .then((response) =>
            response.status === 200 ? response.text() : "Component loading error."
          )
          .then((html) => { 
            
            element.innerHTML = html;
  
          });
      }
      if (cb) cb();
    });
  
    // console.timeEnd('pf');
  }

};

export { helper };
