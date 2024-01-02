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

  if (window.pageYOffset < horizontaltabHeader.offsetTop) {
        document.getElementsByClassName("tab-nav")[0].classList.add('active');
  }
} catch (error) {       
}
};


/* $(document).ready(function(){
   $(".slide").slice(0, 6).show();
   $("#loadMore").on("click", function(e){
     e.preventDefault();
     $(".slide:hidden").slice(0, 2).slideDown();
     if($(".slide:hidden").length == 0) {
       $("#loadMore").text("No Content").addClass("noContent");
     }
   });
}) */

$(document).ready(function(){
  $(".slideUL").each(function() {
    $(this).find(".slide").slice(0, 6).show();
  });
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(this).closest('.serviceOption').find(".slide:hidden").slice(0, 2).slideDown();
    if($(this).closest('.serviceOption').find(".slide:hidden").length == 0) {
      $(this).text("No Content").addClass("noContent");
    }
  });
});
