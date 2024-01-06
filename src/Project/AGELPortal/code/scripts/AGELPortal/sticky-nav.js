const sticky = {

    stickyNav(elm_){
        let body = document.body;
        let elm = document.getElementById('main');

        if(elm){
            let nTop = elm.offsetTop;
            let height = elm.offsetHeight;

            if (window.scrollY >= height) {
                elm.classList.add('not-floating');
                body.classList.add('sticky-header');
                body.style.paddingTop = (height+'px');
            } else {
                body.style.paddingTop = 0;
                elm.classList.remove('not-floating');
                body.classList.remove('sticky-header');
            }
        }
    },

    init(id){
        setTimeout(() => {
            if(id){
                let elm = document.getElementById(id);
                if(elm){
                    window.addEventListener('scroll', this.stickyNav);
                }
            };
        }, 100);
    }
}

export { sticky };


// const sticky = {

//     stickyNav(elm_){
//         let body = document.body;
//         let elm = document.getElementById('main');

//         if(elm){
//             let nTop = elm.offsetTop;
//             let height = elm.offsetHeight;

//             if (window.scrollY >= height) {
//                 elm.classList.add('not-floating');
//                 body.classList.add('sticky-header');
//                 body.style.paddingTop = (height+'px');
//             } else {
//                 body.style.paddingTop = 0;
//                 elm.classList.remove('not-floating');
//                 body.classList.remove('sticky-header');
//             }
//         }
//     },

//     init(id){
//         setTimeout(() => {
//             if(id){
//                 let elm = document.getElementById(id);
//                 if(elm){
//                     window.addEventListener('scroll', this.stickyNav);
//                 }
//             };
//         }, 100);
//     }
// }

// export { sticky };