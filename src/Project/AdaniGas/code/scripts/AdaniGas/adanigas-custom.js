
$("#divLiveCapture").hide();
$(function () {
    $("#btnOpenCamera").click(function () {
        $("#userPic").hide();
        $("#divLiveCapture").show();
        $(".liveCapture").show();
        $("#btnOpenCamera").hide();
        $("#btnOpenCamera").toggleClass('d-none d-block');
        $("#btn-back").trigger('click');
    });


    //$("#capture").click(function () {

    //    $(".liveCapture").hide();
    //    $("#captureDiv").hide();
    //    $("#showDiv").show();
    //    jQuery.ajax(
    //        {
    //            url: "/api/AdaniGas/CaptureImage",
    //            method: "POST",
    //            contentType: "application/json; charset=utf-8",
    //            data: "{data: '" + $("#userPic")[0].src + "', userId: '" + $("#CustomerID").val() + "'}",
    //            dataType: "json",
    //            success: function (result) {
    //                if (result !== null && result.result === "success") {
    //                    {
    //                        $("#userPic").attr("src", result.filePathPic);
    //                    }
    //                }
    //                else {
    //                    //result("failed");
    //                }
    //                $("#divLiveCapture").hide();
    //                $("#btnOpenCamera").show();
    //            }
    //        });
    //});
    //$("#btnUpload").click(function () {
    //    Webcam.reset();
    //    $(".liveCapture").hide();
    //    $("#captureDiv").hide();
    //    $("#showDiv").show();
    //    jQuery.ajax(
    //        {
    //            url: "/api/AdaniGas/CaptureImage",
    //            method: "POST",
    //            contentType: "application/json; charset=utf-8",
    //            data: "{data: '" + $("#imgCapture")[0].src + "', accountNumber: '" + $("#CANumber").val() + "'}",
    //            dataType: "json",
    //            success: function (result) {
    //                if (result !== null && result.result === "success") {
    //                    {
    //                        $("#userPic").attr("src", result.filePathPic);
    //                    }
    //                }
    //                else {
    //                    //result("failed");
    //                }
    //	 Webcam.reset();
    //                $("#divLiveCapture").hide();
    //                $("#btnOpenCamera").show();
    //            }
    //        });
    //});
});


$(".meter-reading-input").keyup(function () {
    if (this.value.length == this.maxLength) {
        $(this).next('.meter-reading-input').focus();
    }
});

$('.meter-reading-input').focus(function () {
    if ($(this).val() == 0)
        $(this).val("");
});

$(".meter-reading-input").blur(function () {
    if ($(this).val() == "")
        $(this).val(0);

});

$(".meter-reading-input").on("keypress", function (evt) {
    if (this.value.length > 1) {
        $(this).val(this.value.slice(0, 1));
    }
});

$(function () {
    $('.datetimepicker').datetimepicker(
        {
            format: 'DD/MM/YYYY HH:mm',
        });
});

var recaptcha3;
var onloadCallback = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha3 = grecaptcha.render('recaptcha3', {
        'sitekey': '6LclBqwUAAAAAJ2KtS78FPoLPod26RXeKH5iddFy', //Replace this with your Site key
        'theme': 'light'
    });


};

$("#submits").click(function () {
    var response = grecaptcha.getResponse(recaptcha3);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    else {
        $("#gresponse").val(response);
    }   

});

$(function() {
  $("#bars li .bar").each( function( key, bar ) {
    var percentage = $(this).data('percentage');
    
    $(this).css('height', percentage + '%');
    
    //$(this).animate({
    //  'height' : percentage + '%'
    //}, 1000);
  });
}); 

$('#form-gas').submit(function () {
    $('#AjaxLoader').show();
});

$(document).ready(function () {
    $('#AjaxLoader').hide();
	$("#homemodalpopup").modal('show');
	$('[data-toggle="tooltip"]').tooltip() ;
	
    $(function () {
        $('#btn-mrelease').change(function () {
            $('.media-r').hide();
            $('.m-loadMore').hide();
            $('#' + $(this).val()).show();
        });
    });
	
	
// $('#btn-mrelease').on('change', function() {
  // if(this.value == "all") {
    // $('#media-r-parent .media-r').addClass('d-flex');
  // } else {
	  // $('#media-r-parent .media-r').removeClass('d-flex');
  // }
// });

	
	

    $(".loadmore-content p, .loadmore-content ul").slice(0, 1).show();
    $(".loadMore").on("click", function (e) {
        e.preventDefault();
        $(".loadMore").toggleClass("d-block d-none");
        $(".loadmore-content p, .loadmore-content ul").toggleClass("d-none");
        $("p.loadMore").text("").show(1000);
        $(".arrow.loadMore").text("Read more ").show(1000);
        $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');
        if ($(".loadmore-content p:hidden").length == 0) {
            $(".arrow.loadMore").text("Read less ").show(1000);
            $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');
            $(".arrow.loadMore").show(1000);
        }
    });
	
	$('.homepopup-carousel').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 20000,
        autoplayHoverPause: true,
        items: 1,
        nav: true,
        dots: false,
	autoHeight: false,
	autoHeightClass: 'owl-height',
autoplayHoverPause:true,
        navText: ["<i class='fa fa-arrow-left'></i>", "<i class='fa fa-arrow-right'></i>"]
    });
$('.homepopup-carousel').trigger('refresh.owl.carousel');

    $('.single-carousel').owlCarousel({
        loop: false,
        margin: 10,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-arrow-left'></i>", "<i class='fa fa-arrow-right'></i>"],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })




    $('.single-slide').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });

    $('.four-item').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 2,
                nav: false
            },
            600: {
                items: 3
            },
            1000: {
                items: 3
            }
        }
    });
	
$('.homepopup-carousel1').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 20000,
        autoplayHoverPause: true,
        items: 1,
        nav: true,
        dots: false,
	autoHeight: true,
	autoHeightClass: 'owl-height',
autoplayHoverPause:true,
        navText: ["<i class='fa fa-arrow-left'></i>", "<i class='fa fa-arrow-right'></i>"]
    });
$('.homepopup-carousel').trigger('refresh.owl.carousel');

	$('.homepopup-carousel1').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        dots: false,
		autoplayTimeout: 6000,
		autoplay: false,
		lazyload:true,
		autoHeight: true,
		autoHeightClass: 'owl-height',
        navText: ["<i class='fa fa-arrow-left'></i>", "<i class='fa fa-arrow-right'></i>"],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })

    $('.sitemap-link').click(function () {
        $('.ft-mobilemenu').removeClass('d-none');
        $('.sitemap-link').addClass('d-none');
        $('.sitemap-link').removeClass('d-block');
    });

    $("#homeslider").owlCarousel({
        loop: true,
        autoplay: true,
        margin: 0,
        nav: true,
        autoHeight: false,
        dots: true,
        navRewind: false,
        autoplayTimeout: 8000,
        lazyLoad: true,
		navText: ["<i></i>","<i></i>"],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });


    /* Initialize Carousel on home page */
    $(".servicesList").owlCarousel({
        nav: true,
        navText: [],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            },
            1000: {
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            }
        }
    });
    /* Initialize CNG Carousel on home page */
    $("#serviceCarouselCng").owlCarousel({
        nav: true,
        navText: [],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            },
            1000: {
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            }
        }
    });
    $("#gasCategory1").owlCarousel({
        nav: true,
        navText: [],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            },
            1000: {
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            }
        }
    });

    /* Menu on mobile devices */

    $('#dismiss, .overlay, .overlay-top').on('click', function () {
        $('#sideNav').removeClass('active');
        $('.overlay, .overlay-top').removeClass('active');
        $('#topMenu').removeClass('active');
    });

    /*Side menu on mobile devices*/

    $('#sidebarCollapse').on('click', function () {
        $('#sideNav, #mainContent').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('.overlay').addClass('active');
        //$('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    /* Top menu on mobile devices*/

    $('#topNavCollapse').on('click', function () {
        $('#topMenu').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('.overlay-top').addClass('active');

    });

    ///* For smooth page scroll on Top menu link */
    //document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    //    anchor.addEventListener('click', function (e) {
    //        e.preventDefault();

    //        document.querySelector(this.getAttribute('href')).scrollIntoView({
    //            behavior: 'smooth'
    //        });
    //    });
    //});

    if (window.location.href.indexOf("#") > 0) {
        var arr = window.location.href.split('#');
        $('html, body').animate({
            scrollTop: $("#" + arr[1]).offset().top
                - 130
        }, 1000);
        $("#" + arr[1] + " .card-body").addClass('show');
		$("#homemodalpopup").modal('hide');
    }
	
	

});

function openFile(id,imageid){
 
    console.log(id);
    var submitParams = {
	   		'ID':id,
	   		'imageid':imageid
	 };
	 if(confirm("Are you sure want to download file?")){
	  $.ajax({       
   			    type: "get",
                dataType: 'JSON',
                url: '/api/AdaniGas/downloaddodoimage',
                data: {
                    ID: id,
					imagefield:imageid
                },
   			success:function(response)
   			{ 
      			console.log("Response Received");
      			console.log(response);
      			// var file = $.parseJSON(response.Result);
      			 var link = document.createElement("a");
				  link.download = imageid;
				  link.href =  response;
				 // console.log(file.filename1);
				 // console.log(file.filecontent1);
				  document.body.appendChild(link);
				  link.click();
				  document.body.removeChild(link);
				  delete link;
         	}
 	    });  
 	  }   
}


var tableToExcel = (function() {
  var uri = 'data:application/vnd.ms-excel;base64,'
    , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
    , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
    , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
  return function(table, name) {
    if (!table.nodeType) table = document.getElementById(table)
    var ctx = {worksheet: name || 'Worksheet', table: table.innerHTML}
    var link1 = document.createElement("a");
	link1.download = "DODOFormData";
	link1.href = uri + base64(format(template, ctx));
	document.body.appendChild(link1);
	link1.click();
	document.body.removeChild(link1);
	delete link1;
  }
})();






var fixedfocus = {
	init: function(fixedElem) {
		fixedfocus.fixedElem = fixedElem;
		document.body.addEventListener('focusin', fixedfocus.adjust);
	},
	
	adjust: function(e) {
		var fixedBottomPos = fixedfocus.fixedElem.getBoundingClientRect().bottom;
		var rect = e.target.getBoundingClientRect();
		if (rect.top < fixedBottomPos) {
			window.scrollBy(0, rect.top - fixedBottomPos);
		}
	}
};

document.addEventListener('DOMContentLoaded', function() {
	fixedfocus.init(document.querySelector('header'));
});

/* CNG Registration by Ketan -Start*/
$('input[type=radio][name=IsBSVI]').change(function () {
    $('#AjaxLoader').css('display', 'block');
    $("#CNGcustomerRegForm").submit();
});
$('#CNGsendOTP').click(function () {
    $('#CNGsendOTP').attr("disabled", "disabled");
    var mobileNo = $("#CNGCustRegMobile").val();
    $('.sentOTPmsg').remove();
    if (mobileNo === "") {
        $("<p class='sentOTPmsg required'>Please enter your Mobile Number</p>").insertAfter("#CNGCustRegMobile");
        $("#CNGCustRegMobile").focus();
        $('#CNGsendOTP').removeAttr("disabled");
        return false;
    }

    if (mobileNo.length !== 10) {
        $("<p class='sentOTPmsg required'>Mobile Number should be of 10 digit</p>").insertAfter("#CNGCustRegMobile");
        $("#CNGCustRegMobile").focus();
        $('#CNGsendOTP').removeAttr("disabled");
        return false;
    }
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'GET',
        url: '/api/AdaniGas/CNGSendOTP',
        dataType: 'json',
        data: { mobile: mobileNo },
        success: function (response) {
            $("<p class='sentOTPmsg required'>" + response["Message"] + "</p>").insertAfter("#CNGCustRegMobile");
        },
        error: function (ex) {
            alert("Error");
        },
        complete: function () {
            $('#CNGsendOTP').removeAttr("disabled");
            $('#AjaxLoader').css('display', 'none');
        }
    });

});
$('#CNGCustCurrentState').change(function () {
    var State = $("#CNGCustCurrentState").val();
    if (State === "") {
        $('.CurrentStateMsg').remove();
        $("<p class='CurrentStateMsg required'>Please select any State</p>").insertAfter("#CNGCustCurrentState");
        $('#CNGCustCurrentCity').text('');
        $('#CNGCustCurrentCity').append("<option value=''> Select City </option>");
        $("#CNGCustCurrentState").focus();
        return false;
    }
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'GET',
        url: '/api/AdaniGas/GetCNGCity',
        dataType: 'json',
        data: { state: State },
        success: function (obj) {
            $('#CNGCustCurrentCity').text('');
            $('#CNGCustCurrentCity').append("<option value=''> Select City </option>");
            for (var i = 0; i < obj.length; i++) {
                var name = obj[i];
                $('#CNGCustCurrentCity').append("<option value='" + obj[i].Value + "'>" + obj[i].Text + "</option>");
            };
        },
        error: function (ex) {
            alert("Error city list");
        },
        complete: function () {
            $('#AjaxLoader').css('display', 'none');
        }
    });
});
$('#CNGCustRegState').change(function () {
    var State = $("#CNGCustRegState").val();
    if (State === "") {
        $('.RegStateMsg').remove();
        //$("<p class='RegStateMsg required'>Please select any State</p>").insertAfter("#CNGCustRegState");
        $('#CNGCustRegCity').text('');
        $('#CNGCustRegCity').append("<option value=''> Select City </option>");
        $("#CNGCustRegState").focus();
        return false;
    }
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'GET',
        url: '/api/AdaniGas/GetCNGCity',
        dataType: 'json',
        data: { state: State },
        success: function (obj) {
            $('#CNGCustRegCity').text('');
            $('#CNGCustRegCity').append("<option value=''> Select City </option>");
            for (var i = 0; i < obj.length; i++) {
                var name = obj[i];
                $('#CNGCustRegCity').append("<option value='" + obj[i].Value + "'>" + obj[i].Text + "</option>");
            };
        },
        error: function (ex) {
            alert("Error city list");
        },
        complete: function () {
            $('#AjaxLoader').css('display', 'none');
        }
    });
});
$('#CNGVehicleCompany').change(function () {
    var Vehicle = $("#CNGVehicleCompany").val();
    if (Vehicle === "") {
        $('#CNGCustCurrentCity').text('');
        $("<p class='required'>Please select any vehicle company</p>").insertAfter("#CNGVehicleCompany");
        $('#CNGVehicleModel').text('');
        $('#CNGVehicleModel').append("<option value=''> Select City </option>");
        $("#CNGVehicleCompany").focus();
    }
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'POST',
        url: '/api/AdaniGas/GetVehicleModel',
        dataType: 'json',
        data: { VehicleComp: Vehicle },
        success: function (obj) {
            $('#CNGVehicleModel').text('');
            $('#CNGVehicleModel').append("<option value=''> Select vehicle model </option>");
            for (var i = 0; i < obj.length; i++) {
                var name = obj[i];
                $('#CNGVehicleModel').append("<option value='" + obj[i].Value + "'>" + obj[i].Text + "</option>");
            };
        },
        error: function (ex) {
            alert("Error vehicle model list");
        },
        complete: function () {
            $('#AjaxLoader').css('display', 'none');
        }
    });
});
$("#CNGIsSameAddress").change(function () {
    var ischecked = $(this).is(':checked');
    if (ischecked) {
        $("#RegisteredAddressLine1").val($("#CurrentAddressLine1").val()).prop('readonly', true);
        $("#RegisteredAddressLine2").val($("#CurrentAddressLine2").val()).prop('readonly', true);
        $("#CNGCustRegState").val($("#CNGCustCurrentState").val()).prop('readonly', true);
        $("#CNGCustRegState").trigger("change");
        $(document).ajaxComplete(function () {
            var curentcity = $("#CNGCustCurrentCity").val();
            $("#CNGCustRegCity").val(curentcity);
        });
        $("#RegisteredPincode").val($("#CurrentPincode").val()).prop('readonly', true);
        $("#RegisteredArea").val($("#CurrentArea").val()).prop('readonly', true);
    }
    else {
        $("#RegisteredAddressLine1").val("").prop('readonly', false);
        $("#RegisteredAddressLine2").val("").prop('readonly', false);
        $("#CNGCustRegState").val("").prop('readonly', false);
        $("#CNGCustRegState").trigger("change");
        $("#CNGCustRegCity").val("").prop('readonly', false);
        $("#RegisteredPincode").val("").prop('readonly', false);
        $("#RegisteredArea").val("").prop('readonly', false);
    }
});
$('form').on('focus', 'input[type=number]', function (e) {
    $(this).on('wheel.disableScroll', function (e) {
        e.preventDefault();
    });
});
$('form').on('blur', 'input[type=number]', function (e) {
    $(this).off('wheel.disableScroll');
});
/* CNG Registration by Ketan -End*/

$('#DealersendOTP').click(function () {
    $('#DealersendOTP').attr("disabled", "disabled");
    var mobileNo = $("#CNGCustRegMobile").val();
    $('.sentOTPmsg').remove();
    if (mobileNo === "") {
        $("<p class='sentOTPmsg required'>Please enter your Mobile Number</p>").insertAfter("#CNGCustRegMobile");
        $("#CNGCustRegMobile").focus();
        $('#DealersendOTP').removeAttr("disabled");
        return false;
    }

    if (mobileNo.length !== 10) {
        $("<p class='sentOTPmsg required'>Mobile Number should be of 10 digit</p>").insertAfter("#CNGCustRegMobile");
        $("#CNGCustRegMobile").focus();
        $('#DealersendOTP').removeAttr("disabled");
        return false;
    }
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'GET',
        url: '/api/AdaniGas/CNGSendOTP',
        dataType: 'json',
        data: { mobile: mobileNo },
        success: function (response) {
            $("<p class='sentOTPmsg required'>" + response["Message"] + "</p>").insertAfter("#CNGCustRegMobile");
        },
        error: function (ex) {
            alert("Error");
        },
        complete: function () {
            $('#DealersendOTP').removeAttr("disabled");
            $('#AjaxLoader').css('display', 'none');
        }
    });

});
$('#VendorDetails').click(function () {
    $('#VendorDetails').attr("disabled", "disabled");
    var VendorNumber = $("#CNGCustVendorNumber").val();

    if (VendorNumber === "") {
        $("<p class='sentOTPmsg required'>Please enter your Vendor Number</p>").insertAfter("#CNGCustVendorNumber");
        $("#CNGCustVendorNumber").focus();
        $('#VendorDetails').removeAttr("disabled");
        return false;
    }


    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'POST',
        url: '/api/AdaniGas/CNGDealerDetails',
        dataType: 'json',
        data: { VendorNumber: VendorNumber },
        success: function (response) {

        },
        error: function (ex) {
            alert("Error");
        },
        complete: function () {
            $('#VendorDetails').removeAttr("disabled");
            $('#AjaxLoader').css('display', 'none');
        }
    });

});

/*CNG Dealer Login start*/
$("#CNGDealerReSendOTP").on('click', function (event) {
    $("#OTP").removeAttr("required");
    $('#AjaxLoader').css('display', 'block');
});
$("#CNGDealerSaveBtn").on('click', function (event) {
    $(".saveOnly").removeAttr("required");
    if ($('#selectCurrentStatus').val() !== '') {
        $('#AjaxLoader').css('display', 'block');
    }
});
$("#CNGDealerSubmitBtn").on('click', function (event) {
    $("#CNGDealerRegForm input[type=file]").each(function (t) {
        var fileID = this.id;
        if ($('.' + fileID).length) {
            $('#' + fileID).removeAttr("required");
        }
    });
});
function downloadPDF(contentData, customfilename, contentType) {
    const linkSource = `data:${contentType};base64,${contentData}`;
    const downloadLink = document.createElement("a");
    if (contentType === 'application/pdf') {
        const fileEx = `.pdf`;
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
    }
    if (contentType === 'image/jpeg') {
        const fileEx = `.jpg`;
        const fileName = customfilename + fileEx;
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
    }



}
$('.PreviewCustFile').click(function (e) {
    e.preventDefault();
    var datasourc = $(this).attr("data-src");
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'GET',
        url: datasourc,
        dataType: 'json',
        success: function (obj) {
            downloadPDF(obj.DocumentBase64, obj.DocName, obj.DocContentType);
        },
        error: function (ex) {
            alert("Error file preview"); //ValidateCNGdocByAdmin
        },
        complete: function () {
            $('#AjaxLoader').css('display', 'none');
        }
    });
});
/*CNG Dealer Login end*/
/*CNG Admin Login start*/
$('.AdminPreviewCustFile').click(function (e) {
    e.preventDefault();
    var datasourc = $(this).attr("data-src");
    datasourc = datasourc.replace('DownloadCNGdoc', 'AdminDownloadCNGdoc')
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'GET',
        url: datasourc,
        dataType: 'json',
        success: function (obj) {
            downloadPDF(obj.DocumentBase64, obj.DocName, obj.DocContentType);
        },
        error: function (ex) {
            alert("Error file preview"); //ValidateCNGdocByAdmin
        },
        complete: function () {
            $('#AjaxLoader').css('display', 'none');
        }
    });
});
$('.Validate').click(function (e) {
    e.preventDefault();
    var datasourc = $(this).attr("for-data");
    var validFile = this;
    var hostdetails = location.origin;
    $('#AjaxLoader').css('display', 'block');

    $.ajax({
        type: 'POST',
        url: '/api/AdaniGas/ValidateCNGdocByAdmin',
        data: { absoluteURL: datasourc, host: hostdetails, Comment: null },
        dataType: 'json',
        success: function (obj) {
            alert(obj["Message"]);
            $($(validFile).prev("a").find("span")).removeClass("d-none");
            validFile.remove();
        },
        error: function (ex) {
            alert("Error in file validation");
        },
        complete: function () {
            $('#AjaxLoader').css('display', 'none');
        }
    });
});

$("#CNGAdminReSendOTP").on('click', function (event) {
    $("#OTP").removeAttr("required");
    $('#AjaxLoader').css('display', 'block');
});
/*CNG Admin Login end*/

/*cemra new script*/


$(document).ready(function (e) {

    var player = document.getElementById('player');
    var snapshotCanvas = document.getElementById('snapshot');
    var captureButton = document.getElementById('capture');

    //var handleSuccess = function(stream) {
    // Attach the video stream to the video element and autoplay.

    //player.srcObject = stream;
    //};

    captureButton.addEventListener('click', function () {
        var context = snapshot.getContext('2d');
        // Draw the video frame to the canvas.
        context.drawImage(player, 0, 0, snapshotCanvas.width,
            snapshotCanvas.height);
        console.log(context.canvas.toDataURL());
        $("#userPic").attr('src', context.canvas.toDataURL());

        $(".liveCapture").hide();
        $("#captureDiv").hide();
        $("#showDiv").show();
        jQuery.ajax(
            {
                url: "/api/AdaniGas/CaptureImage",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: "{data: '" + $("#userPic")[0].src + "', userId: '" + $("#CustomerID").val() + "'}",
                dataType: "json",
                success: function (result) {
                    if (result !== null && result.result === "success") {
                        {
                            $("#userPic").attr("src", result.filePathPic);
                            $("#userPic").show();
                        }
                    }
                    else {
                        //result("failed");
                    }
                    $("#divLiveCapture").hide();
                    $("#btnOpenCamera").toggleClass('d-none d-block');
                }
            });
    });

    //navigator.mediaDevices.getUserMedia({video: true})
    // .then(handleSuccess);




    (() => {

        const btnFront = document.querySelector('#btn-front');
        const btnBack = document.querySelector('#btn-back');

        const supports = navigator.mediaDevices.getSupportedConstraints();
        if (!supports['facingMode']) {
            alert('Browser Not supported!');
            return;
        }

        let stream;

        const capture = async facingMode => {
            const options = {
                audio: false,
                video: {
                    facingMode,
                },
            };

            try {
                if (stream) {
                    //.getTracks().forEach(videoStream => videoStream.stop());
                    const tracks = stream.getTracks();
                    tracks.forEach(videoStream => videoStream.stop());
                }
                stream = await navigator.mediaDevices.getUserMedia(options);
            } catch (e) {
                alert(e);
                return;
            }
            player.srcObject = null;
            player.srcObject = stream;
            player.play();
        }

        btnBack.addEventListener('click', () => {
            capture('environment');
        });

        btnFront.addEventListener('click', () => {
            capture('user');
        });
    })();
});

$('#btn-front').click(function () {
    $(this).addClass('d-none');
    $('#btn-back').removeClass('d-none');
});
$('#btn-back').click(function () {
    $(this).addClass('d-none');
    $('#btn-front').removeClass('d-none');
});

//added for ENACH by KETAN --Start--
$('#Enachsubmits').on('click', function(e) {
    var CustId = $('#EnachCustomerId').val();
	
	if (CustId!=''){
			$('#AjaxLoader').css('display','block');
	}	
});
$('#Enachsubmit').on('click', function(e) {
    var CustId = $('#EnachCustomerId').val();
	var EnachOTP = $('#EnachOTPNumber').val();
	
	if (CustId!='' && EnachOTP!=''){
			$('#AjaxLoader').css('display','block');
	}	
});
$('#EnachResend').on('click', function(e) {
    var CustId = $('#EnachCustomerId').val();
	
	if (CustId!=''){
			$('#AjaxLoader').css('display','block');
	}	
});

$(document).ready(function () {
    $("#EnachResend").click(function () {
        $("#EnachOTPNumber").removeAttr('required');
    });
});
var timeLeft = 30;
var elem = document.getElementById('OTPtimer');


if(elem != null)
{
	var timerId = setInterval(countdown, 1000);
	function countdown() {
		if (timeLeft == -1) {
			clearTimeout(timerId);
			setDisabled();
		} else {
			elem.innerHTML = timeLeft + ' Seconds to Re-send OTP';
			timeLeft--;
		}
	}
	function setDisabled() {
		$("#OTPtimer").remove();
		$("#EnachResend").removeClass("d-none");
	}
}


//added for ENACH by KETAN --END--


// Hide Header on scroll down
var didScroll;
var lastScrollTop = 0;
var delta = 0;
var navbarHeight = 0;

$(window).scroll(function (event) {
    didScroll = true;
});

setInterval(function () {
    if (didScroll) {
        hasScrolled();
        didScroll = false;
    }
}, 250);
function hasScrolled() {
    var st = $(this).scrollTop();

    // Make sure they scroll more than delta
    if (Math.abs(lastScrollTop - st) <= delta)
        return;

    // If they scrolled down and are past the navbar, add class .nav-up.
    // This is necessary so you never see what is "behind" the navbar.
    $('#back-to-top').fadeIn();
    $('header').addClass('fixed-header');

    if (st > lastScrollTop && st > navbarHeight) {
        // Scroll Down
        //$('.btm-floating').addClass('active');

    } else {
        // Scroll Up
        if (st + $(window).height() < $(document).height()) {

        }
    }
    if (st < 150) {

        $('#back-to-top').hide();
        $('header').removeClass('fixed-header');
        //$('.btm-floating').removeClass('active');
    }
    lastScrollTop = st;
}

$(window).scroll(function(){
    if ($(this).scrollTop() > 750) {
	$('#ymPluginDivContainerInitial').addClass('active');
    } else {
$('#ymPluginDivContainerInitial').removeClass('active');
    }
});