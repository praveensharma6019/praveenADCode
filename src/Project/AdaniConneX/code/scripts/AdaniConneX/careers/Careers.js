         $(document).ready(function () {

            window.uploadImg = "/-/media/Project/AdaniConneX/Career/Form/upload.png";
            window.removeImg = "/-/media/Project/AdaniConneX/Career/Form/remove_cv.png";
            $(".uploadCvPopupAction").on('click',function(){
               $('#uploadCvPopup').toggleClass('show');
            });
             $(".getInTouchActionnew1").on('click',function(){
               $('#ConnectWiththeHR').toggleClass('show');
            }); 

            $("#uploadCvPopupClose").on('click',function(){
               $('#uploadCvPopup').toggleClass('show');
            });

            $("#cvfileInput").on('click',function(){
               $("#cvfile").trigger("click");
            });

            $("#cvfile").on("change", function(){ 
               $(".uploadCvFileSize").css("color","rgba(0,0,0,.5)");
               if($(this).val()){
                     if(this.files[0].size < 5000000){
                        $("#cvfileInput").addClass("filled")
                        $("#cvfileInput").val($(this).val().replace(/.*(\/|\\)/, '') );
                        $(".removeCvFile").attr("src", window.removeImg);
                     }
                     else{
                        $(this).val("");
                        $(".uploadCvFileSize").css("color","red");
                     }
               }
            });

            $('.dr_upload_proxy').on('click', function(){
               if($('.removeCvFile').attr('src') == window.uploadImg){
                     $('#cvfileInput').trigger('click');
               }
            });
            


            $(".removeCvFile").on("click",function(){
               $("#cvfileInput").removeClass("filled")
               $("#cvfileInput").val("");
               $(".removeCvFile").attr("src", window.uploadImg);
            });

            $('#dr_esg_slider').owlCarousel({
               items: 1,
               nav: false,
               dots: true,
               // mouseDrag: true,
               loop: true,
               autoplay: true,
               autoplayHoverPause: true,
               responsiveClass: true,
               responsive: {
                     0: {
                        items: 1,
                     },
                     767: {
                        items: 1,
                     }
               }
            });

            $('#dr_life_slider').slick({
               dots: true,
               infinite: true,
               speed: 300,
               arrows: false,
               slidesToShow: 2,
               autoplay: true,
               loop: true,
               slidesToScroll: 2,
               responsive: [
                     {
                     breakpoint: 767,
                     settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                     }
                     }
               ]
               });

            $('#dr_career_slider').slick({
               slidesToShow: 1,
               autoplay: true,
               slidesToScroll: 1,
               arrows: true,
               fade: false,
               asNavFor: '#dr_career_text'
            });
            $('#dr_career_text').slick({
               slidesToShow: 1,
               slidesToScroll: 1,
               autoplay: true,
               asNavFor: '#dr_career_slider',
               dots: false,
               arrows: false,
               fade: true,
            });
            $('#dr_values_slider').owlCarousel({
               items: 3,
               nav: false,
               dots: true,
               // mouseDrag: true,
               loop: true,
               autoplay: true,
               autoplayHoverPause: true,
               responsiveClass: true,
               responsive: {
                     0: {
                        items: 1.15,
                     },
                     767: {
                        items: 1.6,
                     },
                     992: {
                        items: 3,
                     }
               }
            });

            $('#dr_unique_slider').owlCarousel({
               items: 3,
               nav: false,
               dots: true,
               // mouseDrag: true,
               loop: true,
               autoplay: true,
               autoplayHoverPause: true,
               responsiveClass: true,
               responsive: {
                     0: {
                        items: 1.15,
                     },
                     767: {
                        items: 1.6,
                     },
                     992: {
                        items: 3,
                     }
               }
            });

            $('.dr_hide_div .close-modal').on('click', function(){
               $('.dr_hide_div').removeClass('dr_open');
            });

            $('.dr_btn_click').on('click', function(){
               $(this).parents().find('.dr_hide_div').addClass('dr_open');
               $(this).parents().siblings('.owl-item').find('.dr_hide_div').removeClass('dr_open');
            });
            
            $(".dr_locations_icon li").on('click', function(){
               $(".dr_locations_image, .dr_locations_icon li").removeClass("active");
               $(this).addClass("active");
               $(".dr_our_locations .full ." + $(this).attr('data-href')).addClass("active");
            });

            
         });