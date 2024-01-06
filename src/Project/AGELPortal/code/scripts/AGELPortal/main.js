//import { helper } from "./helper.js";
import { sticky } from "./sticky-nav.js";
import { Carousel } from "/scripts/AGELPortal/components/Carousel/index.js";
import { StickySideNav } from "/scripts/AGELPortal/components/Header/index.js";
import { accordion } from "/scripts/AGELPortal/components/Nav/index.js";
import { modal } from "/scripts/AGELPortal/components/Modal/index.js";
import { SideNav } from "/scripts/AGELPortal/components/Header/sidenav.js";
import { fileUpload } from "/scripts/AGELPortal/fileUpload.js";

window.labelActiveOnLoad = () => {
$(".input-field input").each(function () {
var getVal = $(this).val();
console.log("labelActiveOnLoad");
if (getVal !== "") {
$(this).next("label").addClass("active");
}
});
};


window.sidenav_tab_customClick = () => {
  $('.side-nav .tabs .tab a').each(function() {
    if ($(this).attr('href') !== undefined) {
      var getAttr = $(this).attr('href').replace('#', '');
      $('.carousel_wrapper .card[id="'+getAttr+'"]').siblings('.card').removeClass('show-tile');
      $('.carousel_wrapper .card[id="'+getAttr+'"]').addClass('show-tile');
    }    
  });

  $('.side-nav .tabs .tab a').click(function() {
    if ($(this).attr('href') !== undefined) {
      var getAttr = $(this).attr('href').replace('#', '');
      $('.carousel_wrapper .card[id="'+getAttr+'"]').siblings('.card').removeClass('show-tile');
      $('.carousel_wrapper .card[id="'+getAttr+'"]').addClass('show-tile');
    }
  });
};

window.initDropDown = () => {

var elems_trigger = document.querySelectorAll(".dropdown-toggle-trigger");

var instances = M.Dropdown.init(elems_trigger, {});

};

function video_documentlist() 
{       
try 
{        
	if (window.location.href.toLowerCase().indexOf("video_list") > 0 || window.location.href.toLowerCase().indexOf("document_list") > 0) 
	{            
		if ($(".tabs").find("li:first").find("a") != null && $(".tabs").find("li:first").find("a") != undefined) 
		{                
            var getAttr = $(".tabs").find("li:first").find("a")[0].href.replace(window.location.href + "#", "");
			$('.carousel_wrapper .card[id="' + getAttr + '"]').siblings('.card').removeClass('show-tile');
			$('.carousel_wrapper .card[id="' + getAttr + '"]').addClass('show-tile');            
		}

    }    
 } 
 catch (e) 
    {

    }
}


//window.onload = helper.includeHTML();
setTimeout(() => {
  sticky.init('main')
  Carousel.init();
  StickySideNav();
  accordion();
  modal();
  SideNav();
  M.Tabs.init(document.querySelectorAll(".tabs"));
  var elems = document.querySelectorAll('select');
    var instances = M.FormSelect.init(elems);
    fileUpload();
    labelActiveOnLoad();
sidenav_tab_customClick();
initDropDown();
video_documentlist();
}, 1500);




