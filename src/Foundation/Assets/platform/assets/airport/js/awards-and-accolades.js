function mobileSlider(objID, cardClass = 'mobSlideItem') {
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}
function sliderAdd() {

  if (window.innerWidth >= 991) {

    jQuery(".v-slider").slick({
      slidesToShow: 3,
      slidesToScroll: 3,
      dots: false,
      infinite: false,
      nav: true,
      prevArrow: '<i class="i-arrow-l slick-prev"></i>',
      nextArrow: '<i class="i-arrow-r slick-next"></i>',
      responsive: [
        {
          breakpoint: 1200,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
          },
        },
        {
          breakpoint: 480,
          nav: false,
          settings: {
            slidesToShow: 1.5,
            slidesToScroll: 1,
          },
        },
      ],
    });

    jQuery(".gradient-slider").slick({
      slidesToShow: 4,
      slidesToScroll: 4,
      dots: false,
      infinite: false,
      nav: true,
      prevArrow: '<i class="i-arrow-l slick-prev"></i>',
      nextArrow: '<i class="i-arrow-r slick-next"></i>',
      responsive: [
        {
          breakpoint: 1200,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
          },
        },
        {
          breakpoint: 480,
          nav: false,
          settings: {
            slidesToShow: 1.5,
            slidesToScroll: 1,
          },
        },
      ],
    });


  } else {
    mobileSlider(".gradient-slider, .v-slider", 'mobSlideItem');
  }
}

sliderAdd();
window.onload = function () {
  logoChange();
}


setTimeout(function () {
  jQuery('.airport-features, .mobile-slider, .slider-airports').removeClass('blank-holder');
  jQuery('.blank').removeClass('blank');
}, 2000);




// Custom select dropdown
/* let x = ''
let y = ''
$(".selectBox").on("click", function (e) {
  // let item1=''
  e.stopPropagation();
  $(this).toggleClass("show");
  var dropdownItem = e.target;
  var container = $(this).find(".selectBox__value span");
  container.text(dropdownItem.text);
  // if(dropdownItem.text){
  // item1=dropdownItem.getAttribute('data-id')
  // x.push(item1)
  // }
  $(dropdownItem)
    .addClass("active")
    .siblings()
    .removeClass("active");
  $(this).siblings('.selectBox').removeClass('show');
  x = dropdownItem.getAttribute('data-id');
  if (dropdownItem.text) {
    // console.log(document.querySelectorAll('.fnb_list_item'),"aaaaaaaa")
    Array.from(document.querySelectorAll('.fnb_list_item')).forEach((item) => {
      console.log(item)
      if (x && y) {
        if (item.getAttribute('data-class')?.includes(x) && item.getAttribute('data-class')?.includes(y)) {
          item.style.display = 'flex'
        }
        else {
          item.style.display = 'none'
        }
      }
      if (x) {
        if (item.getAttribute('data-class')?.includes(x)) {
          item.style.display = 'flex'
        }
        else {
          item.style.display = 'none'
        }
      }
      if (y) {
        if (item.getAttribute('data-class')?.includes(y)) {
          item.style.display = 'flex'
        }
        else {
          item.style.display = 'none'
        }
      }




    })
  }
});

$(".selectBox-item").on("click", function (e) {
  e.stopPropagation();
  $(this).toggleClass("show");
  var dropdownItem = e.target;
  // arr.push(dropdownItem.text)
  var container = $(this).find(".selectBox-item__value span");
  console.log(dropdownItem.text, "dropdown")
  container.text(dropdownItem.text);
  // console.log(arr,"text")
  $(dropdownItem)
    .addClass("active")
    .siblings()
    .removeClass("active");
  $(this).siblings('.selectBox-item').removeClass('show');
  y = dropdownItem.getAttribute('data-id');
  if (dropdownItem.text) {
    console.log(x, y, "xy")
    // console.log(document.querySelectorAll('.fnb_list_item'),"aaaaaaaa")
    Array.from(document.querySelectorAll('.fnb_list_item')).forEach((item) => {
      console.log(item)
      if (x && y) {
        if (item.getAttribute('data-class')?.includes(x) && item.getAttribute('data-class')?.includes(y)) {
          item.style.display = 'flex'
        }
        else {
          item.style.display = 'none'
        }
      }
      if (x) {
        if (item.getAttribute('data-class')?.includes(x)) {
          item.style.display = 'flex'
        }
        else {
          item.style.display = 'none'
        }
      }
      if (y) {
        if (item.getAttribute('data-class')?.includes(y)) {
          item.style.display = 'flex'
        }
        else {
          item.style.display = 'none'
        }
      }




    })
  }

});
$("body").on("click", function () {
  $(".selectBox-item").removeClass('show');
}); */

$(document).ready(function() {
  // Custom dropdown click event handler
  $('.custom-dropdown').on('click', function() {
    $(this).find('.custom-dd-menu').toggle();
    $(this).toggleClass('show');
  });

  // Custom dropdown option click event handler
  $('.custom-dd-menu a').on('click', function(e) {
    e.stopPropagation();
    var selectedValue = $(this).data('id');
    var selectedText = $(this).text();
    $('.selectBox__value').find('span').text(selectedText);
    $('.custom-dd-menu').hide();
    $(this).addClass('active').siblings().removeClass('active');
    $('.custom-dropdown').removeClass('show');

    // Filter product items based on selected option
    if (selectedValue === 'all') {
      $('.filter-content .accordion-item .accordion-collapse').removeClass('show');
      $('.filter-content .accordion-item').show().removeClass('single-item');
      $('.filter-content .accordion-item:first-child .accordion-collapse').addClass('show');
      $('.filter-content .accordion-item:not(:first-child) .accordion-button').addClass('collapsed');
    } else {
      $('.filter-content .accordion-item').hide();
      $('.filter-content .accordion-item[data-category="' + selectedValue + '"]').show().addClass('single-item').siblings().removeClass('single-item');
      $('.filter-content .accordion-item[data-category="' + selectedValue + '"] .accordion-collapse').addClass('show');
      $('.filter-content .accordion-item[data-category="' + selectedValue + '"] .accordion-button').removeClass('collapsed');
    }
  });

  // Close custom dropdown when clicked outside
  $(document).on('click', function(event) {
    if (!$(event.target).closest('.custom-dropdown').length) {
      $('.custom-dd-menu').hide();
      $('.custom-dropdown').removeClass('show');
    }
  });
});
