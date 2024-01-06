export default {
    "baseTag":"include",
    "attrTag":"component",
    "baseDir":"components",

    componentFilePath(componentName){
        return `./${this.baseDir}/${componentName}/index.html`;
    },

    start(){
        let tags = document.getElementsByTagName(this.baseTag);
            tags = Object.values(tags);
        let len = tags.length;
    
        if(tags && tags.length > 0){
            tags.forEach((element, index) => {
                let component = element.getAttribute(this.attrTag);

                if (component){
                    let file = this.componentFilePath(component);
    
                    fetch(file).then((response) =>
                        response.status === 200 ? response.text() : ""
                    ).then((html) => {
                        element.innerHTML = html;
                        if(len === (index+1)){
                            setTimeout(()=>{
                                helpers.page.onInclude();
                            }, 10);
                        }
                    });
                }
            });
        }else{
            helpers.page.onInclude(true);
        }
    },

    init(){
        this.start();
    }
};