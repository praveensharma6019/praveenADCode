$(function(){
    "use strict";
    

   
    var iconMinimize=$("#minimize");
    var iconHelp=$("#help");
    var windowStatus=1;
    var chatWindow=$("frame");
    //used to naviage to the home page
    iconHelp.click(function(){

        window.open('help.html', '_new');

    });

    iconMinimize.click(function(){

        if(windowStatus==1){

            $("#frametohide").hide(400,function(){
                iconMinimize.attr("src","images/maximize.svg");
            });
            $( "#frame" ).animate({
               
               
                height: "50px", 
                 }, 400 );
          
            windowStatus=0;
         
        }
        else{
            $("#frametohide").show(400,function(){
                iconMinimize.attr("src","images/minimize.svg");
            });
            $( "#frame" ).animate({
               
                height: "540px", 
                 }, 400 );
            windowStatus=1;
           
        }

    });
    $(document).on('click', '.ac-action-openUrl', function () {
        // your function here
        var url = $(this).attr('data-ac-url');
        window.open(url, '_blank');
    })

 

});

