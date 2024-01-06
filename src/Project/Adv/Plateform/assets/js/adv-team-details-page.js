let elem = document.querySelector('.team-wrap .right-area .img-section'); 
let clone = elem.outerHTML;
document.querySelector('.team-head').insertAdjacentHTML('afterend',clone);