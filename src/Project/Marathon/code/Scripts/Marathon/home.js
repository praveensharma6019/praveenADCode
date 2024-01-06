$(document).ready(function () {
    var $btns = $('.initiatives-title-list a').click(function () {

        if (this.id == 'all') {

            $('#nav-parent > div').fadeIn(450);

        } else {

            var $el = $('.' + this.id).css('display', 'block');

            $('#nav-parent > div').not($el).css('display', 'none');

        }

        $btns.removeClass('active');

        $(this).addClass('active');

    });
    var $btnstab = $('.media-tab a').click(function () {

        if (this.id == 'all') {

            $('.tabcontainerwinner > div').fadeIn(450);

        } else {

            var $el = $('.' + this.id).css('display', 'block');

            $('.tabcontainerwinner > div').not($el).css('display', 'none');

        }

        $btnstab.removeClass('active');

        $(this).addClass('active');
        $(this).parent().parent().find('li').removeClass('active');
        $(this).parent().addClass('active');
    });

    /*Scroll*/
    $(".initiatives-title-list a").click(function (event) {
        var y = $(window).scrollTop();
        y = y + 150;
        $('html, body').animate({ scrollTop: y },800);
    }); 
});

$(function () {
    $(".home-page-banner").owlCarousel({
        loop: !0, responsiveClass: !0, nav: !1, autoplay: false, autoplayTimeout: 4e3, dots: !0, autoplayHoverPause: !0, responsive: {
            0: {
                items: 1, margin: 0
            }
        }
    }
    ), 
	$(".venue-list").owlCarousel({
        loop: !1, responsiveClass: !0, nav: !1, autoplay: !0, autoplayTimeout: 4e3, dots: !1, autoplayHoverPause: !0, margin: 10, nav: !0, navText: ['<i class="fa fa-chevron-right" aria-hidden="true"></i>', '<i class="fa fa-chevron-right"></i>'], responsive: {
            0: {
                items: 1
            }
            , 480: {
                items: 2
            }
            , 768: {
                items: 2
            }
            , 992: {
                items: 2
            }
        }
    }
    ),
	$(".ready-box-grid").owlCarousel({
        loop: !1, responsiveClass: !0, nav: !1, autoplay: !0, autoplayTimeout: 4e3, dots: !1, autoplayHoverPause: !0, margin: 10, nav: !0, navText: ['<i class="fa fa-chevron-right" aria-hidden="true"></i>', '<i class="fa fa-chevron-right"></i>'], responsive: {
            0: {
                items: 1
            }
            , 480: {
                items: 2
            }
            , 768: {
                items: 3
            }
            , 992: {
                items: 4
            }
        }
    }
	
    ),
	
	
    $('.carousel-inner').owlCarousel({
                loop: false,
                margin: 0,
                responsiveClass: true,
                autoplayTimeout: 2000,
                touchDrag:true,
                mouseDrag:true,
                autoplay: true,
                nav:true,
                dots:false,
                navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
                responsive: {
                  0: {
                    items: 1,
                    nav:false
                  },
                  600: {
                    items: 1
                  },
                  1000: {
                    items: 1
                  }
                }
              }),

	
	
	
	
	
	
	
	
        $(".youtubeVideo").show(), $("header nav > ul li").each(function () {
            $(this).removeClass("active")
        }
        ), $(".marathan-grid-head.perc-circle .chart").easyPieChart({
            easing: "easeOutBounce", size: 40, lineWidth: 4, scaleColor: !1, trackColor: !1, barColor: "#19a491", onStep: function (e, a, o) {
                $(this.el).find(".percent").text(Math.round(o))
            }
        }
        ), $(".chart").easyPieChart({
            easing: "easeOutBounce", size: 40, lineWidth: 4, scaleColor: !1, trackColor: !1, barColor: "#ffffff", onStep: function (e, a, o) {
                $(this.el).find(".percent").text(Math.round(o))
            }
        }
        ), $(".partners-list").owlCarousel({
            loop: !0, responsiveClass: !0, nav: !0, autoplay: !0, dots: !1, autoplayTimeout: 4e3, autoplayHoverPause: !0, responsive: {
                0: {
                    items: 2, margin: 10
                }
                , 768: {
                    items: 3, margin: 15
                }
                , 1025: {
                    items: 4, margin: 15
                }
            }
        }
        ), $(".marathan-detail-grid").owlCarousel({
            loop: !1, responsiveClass: !0, nav: !1, dots: !1, responsive: {
                0: {
                    items: 1, margin: 15
                }
                , 480: {
                    items: 2, margin: 15
                }
            }
        }
        )
}
);

// A $( document ).ready() block.
$( document ).ready(function() {
var parent=document.getElementsByClassName("menu-item");
for(var i=0;i<parent.length;i++)
{
if(parent[i].children.length>=2)
{
parent[i].children[0].removeAttribute("href");
}
else
{
parent[i].classList.remove("menu-item-has-children");

}
}

});




$('.sub_submenu').click(function(){
$('.sub_submenu').removeClass('active');
$(this).toggleClass('inactive active');
})


$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})

 $('.scroll-links a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 120
    }, '600');
    e.preventDefault();
});


$(function () {
$('[data-toggle="tooltip"]').tooltip()
})


$(document).ready(function () {
  var navListItems = $('div.setup-panel div a'),
          allWells = $('.setup-content'),
          allNextBtn = $('.nextBtn'),
  		  allPrevBtn = $('.prevBtn');

  allWells.hide();

  navListItems.click(function (e) {
      e.preventDefault();
      var $target = $($(this).attr('href')),
              $item = $(this);

      if (!$item.hasClass('disabled')) {
          navListItems.removeClass('btn-primary').addClass('btn-default');
          $item.addClass('btn-primary');
          allWells.hide();
          $target.show();
          $target.find('input:eq(0)').focus();
      }
  });
  
  allPrevBtn.click(function(){
      var curStep = $(this).closest(".setup-content"),
          curStepBtn = curStep.attr("id"),
          prevStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().prev().children("a");
          prevStepWizard.removeAttr('disabled').trigger('click');
  });

  allNextBtn.click(function(){
      var curStep = $(this).closest(".setup-content"),
          curStepBtn = curStep.attr("id"),
          nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
          curInputs = curStep.find("input[type='text'],input[type='url']"),
          isValid = true;

      $(".form-group").removeClass("has-error");
      for(var i=0; i<curInputs.length; i++){
          if (!curInputs[i].validity.valid){
              isValid = false;
              $(curInputs[i]).closest(".form-group").addClass("has-error");
          }
      }
          nextStepWizard.removeAttr('disabled').trigger('click');
  });

  $('div.setup-panel div a.btn-primary').trigger('click');
});

$(window).scroll(function(){
    if ($(this).scrollTop() > 300) {
       $('.section_raceinfo .parent_run').addClass('block_fixed');
    } else {
       $('.section_raceinfo .parent_run').removeClass('block_fixed');
    }
});

$(window).scroll(function(){
    if ($(this).scrollTop() > 1500) {
       $('.home_race-section .parent_run').addClass('hblock_fixed');
    } else {
       $('.home_race-section .parent_run').removeClass('hblock_fixed');
    }
});
