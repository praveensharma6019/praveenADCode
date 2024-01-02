window.onload = function (){
   document.body.scrollTop = 0;
   document.documentElement.scrollTop = 0;
   setTimeout(() => {
      var navlinks=document.getElementsByClassName("tab-nav");
      for (var i=0; i<navlinks.length; i++) {
         navlinks[i].classList.remove('active');
         if (i===0)
         {
            navlinks[i].classList.add('active');
         }
      }
      document.body.scrollTop = 0 ; // For Safari
      document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera  
   }, 200);
}

window.onscroll = function () {
   try {
   let horizontaltabHeader = document.getElementById("navbar-tabs");

   if (window.pageYOffset < horizontaltabHeader.offsetTop && document.querySelector('#navbar-tabs .tab-nav.active') == null) {
         document.getElementsByClassName("tab-nav")[0].classList.add('active');
   }
} catch (error) {       
}
};


function closeOffcanvas(){
   const { innerWidth: width, innerHeight: height } = window;
   if (width<992){
      try {
         var $myOffcanvas = document.getElementById('offcanvasBottomTest');
         var $myOffBackdrop = document.getElementsByClassName('offcanvas-backdrop')[0];
       $myOffcanvas.classList.remove('show');
       $myOffcanvas.classList.add('hide');
       $myOffBackdrop.classList.remove('show');
       $myOffBackdrop.classList.add('offcanvas-hide');
       document.body.style.removeProperty('overflow');
       document.body.style.removeProperty('padding-right')
      } catch (error) {
         
      }
  }
}
window.addEventListener("load", handleResize);
window.addEventListener("resize", handleResize);
function handleResize(){
   let $toggle_offcanvas= document.getElementById("offcanvasBottomTest");
   if(!$toggle_offcanvas){return;}
   const { innerWidth: width, innerHeight: height } = window;
      if (width < 992) { 
         $toggle_offcanvas.classList.remove('offcanvas-desktop');
      }
      else {
         $toggle_offcanvas.classList.add('offcanvas-desktop');
      }
}
let isClicked = false;
const onChange = () => {
   let tabContent = document.getElementById("v-pills-tabContent");
   let tab = document.getElementById("services-main-menu");
   if (isClicked) {
      tabContent.classList.replace(
         "services-container__expand",
         "services-container"
      );
      tab.classList.replace("services-menu__collapse", "services-menu");
      isClicked = !isClicked;
   } else {
      tabContent.classList.replace(
         "services-container",
         "services-container__expand"
      );
      tab.classList.replace("services-menu", "services-menu__collapse");
      isClicked = !isClicked;
   }
   setTimeout(() => {
      sliderNavigation("other-services");
   }, 400);
};

sliderNavigation("other-services");

function readMore(obj,ID){
   targetObj = jQuery('#'+ID);
   if(!targetObj.hasClass('show')){
     targetObj.addClass('show');
     targetObj.fadeIn(400);
     jQuery(obj).text('Read Less');
   } else {
     targetObj.removeClass('show');
     targetObj.fadeOut(400);
     jQuery(obj).text('Read More');
   }
 }