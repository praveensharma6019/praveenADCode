window.onscroll = function () {
      try {
      let horizontaltabHeader = document.getElementById("navbar-tabs");
      if (window.pageYOffset < horizontaltabHeader.offsetTop) {
            document.getElementsByClassName("tab-nav")[0].classList.add('active');
      }
   } catch (error) {       
   }
   };
  
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