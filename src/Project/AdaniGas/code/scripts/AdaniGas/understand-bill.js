var mywidth = $(".examBillBlk").width();
var myheight = $(".examBillBlk").height();
		
var myBillMapDom  = function(){
		mywidth = $(".examBillBlk").width();
		myheight = $(".examBillBlk").height();
		
			$("#myBill area").remove();
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*38)/908 +','+(myheight*195)/1280+','+(mywidth*312)/908+','+(myheight*212)/1280+'" href="#Head" alt="Your Customer ID registered with Adani Gas. It is unique Identification for all your consumption with us.">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*38)/908 +','+(myheight*240)/1280+','+(mywidth*312)/908+','+(myheight*330)/1280+'" href="#Head" alt="Your Name and Correspondence Address as registered with us">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*195)/1280+','+(mywidth*614)/908+','+(myheight*214)/1280+'" href="#Head" alt="Your current Bill number">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*216)/1280+','+(mywidth*614)/908+','+(myheight*235)/1280+'" href="#Head" alt="Your current Bill Date">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*238)/1280+','+(mywidth*614)/908+','+(myheight*256)/1280+'" href="#Head" alt="Your current Bill Period">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*258)/1280+','+(mywidth*614)/908+','+(myheight*279)/1280+'" href="#Head" alt="Your current Bill Due Date">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*194)/1280+','+(mywidth*856)/908+','+(myheight*214)/1280+'" href="#Head" alt="Your Customer Type">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*216)/1280+','+(mywidth*856)/908+','+(myheight*236)/1280+'" href="#Head" alt="Unique meter number of Meter installed at your premises">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*238)/1280+','+(mywidth*856)/908+','+(myheight*259)/1280+'" href="#Head" alt="VAT classification code for regulatory purpose">');
			
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*32)/908 +','+(myheight*370)/1280+','+(mywidth*443)/908+','+(myheight*584)/1280+'" href="#Head" alt="Calculation of conversion from SCM to MMBTU for your current bill period ">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*470)/908 +','+(myheight*429)/1280+','+(mywidth*877)/908+','+(myheight*370)/1280+'" href="#Head" alt="Adani gas Registration details ">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*470)/908 +','+(myheight*429)/1280+','+(mywidth*877)/908+','+(myheight*488)/1280+'" href="#Head" alt="Your Interest free Consumption Security Deposit with us ">');
			
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*445)/908 +','+(myheight*370)/1280+','+(mywidth*854)/908+','+(myheight*582)/1280+'" href="#Head" alt="Gas consumption pattern">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*29)/908 +','+(myheight*597)/1280+','+(mywidth*206)/908+','+(myheight*643)/1280+'" href="#Head" alt="Current Gas consumption in SCM">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*256)/908 +','+(myheight*597)/1280+','+(mywidth*430)/908+','+(myheight*643)/1280+'" href="#Head" alt="Current Gas consumption in MMBTU">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*479)/908 +','+(myheight*597)/1280+','+(mywidth*654)/908+','+(myheight*643)/1280+'" href="#Head" alt="Amount to be paid before due date for current bill">');		
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*704)/908 +','+(myheight*597)/1280+','+(mywidth*856)/908+','+(myheight*643)/1280+'" href="#Head" alt="Bill amount to be paid before due date for current bill">');
			
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*33)/908 +','+(myheight*650)/1280+','+(mywidth*151)/908+','+(myheight*721)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*175)/908 +','+(myheight*650)/1280+','+(mywidth*292)/908+','+(myheight*721)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*316)/908 +','+(myheight*650)/1280+','+(mywidth*431)/908+','+(myheight*721)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*455)/908 +','+(myheight*650)/1280+','+(mywidth*582)/908+','+(myheight*721)/1280+'" href="#Head" alt="Gas usage charges for current bill">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*607)/908 +','+(myheight*650)/1280+','+(mywidth*729)/908+','+(myheight*721)/1280+'" href="#Head" alt="Amount to be paid before due date for current bill">');
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*750)/908 +','+(myheight*650)/1280+','+(mywidth*877)/908+','+(myheight*721)/1280+'" href="#Head" alt="Bill amount to be paid after due date for current bill">');		
							  
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*764)/1280+','+(mywidth*478)/908+','+(myheight*841)/1280+'" href="#Head" alt="Amount of Gas Usage (Net Quantity * Prevailing Gas Rate">');
			
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*1005)/1280+','+(mywidth*479)/908+','+(myheight*1038)/1280+'" href="#Head" alt="Total Gas usage charges for current bill cycle">');
			
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*534)/908 +','+(myheight*1043)/1280+','+(mywidth*717)/908+','+(myheight*1116)/1280+'" href="#Head" alt="Notice">');
	
			$("#myBill").append('<area shape="rect" coords="'+ (mywidth*32)/908 +','+(myheight*1142)/1280+','+(mywidth*881)/908+','+(myheight*1218)/1280+'" href="#Head" alt="Your Pay-in Slip for this bill">');

		
		var showTooltip = function(event,title){
			$(".mytip").show();
			$(".mytip").html(title);
			
			if(event.clientY+$(".mytip").height() > $(window).height()){
				$(".mytip").css({"top":event.clientY-$(".mytip").height()});
				console.log("asdsad")
			}else{
				$(".mytip").css({"top":event.clientY+25});
				console.log("111111")
			}
			if(event.clientX+$(".mytip").width()+20 > $(window).width()){
				$(".mytip").css({"left":event.clientX-($(".mytip").width()+10)})
			}else{
				$(".mytip").css({"left":event.clientX+0})
			}
			
		};
		$("area").on("mouseover",function(event){
			showTooltip(event,$(this).attr("alt"));
		});
		$("area").on("mousemove",function(event){
			showTooltip(event,$(this).attr("alt"));
		});
		$("area").on("mouseout",function(event){
			$(".mytip").hide();
		});
};
	
var myBillMapCom  = function(){

		mywidth = $(".examBillBlk").width();
		myheight = $(".examBillBlk").height();
	
	
		$("#myBill area").remove();
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*38)/908 +','+(myheight*195)/1280+','+(mywidth*312)/908+','+(myheight*212)/1280+'" href="#Head" alt="Your Customer ID registered with Adani Gas. It is unique Identification for all your consumption with us.">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*38)/908 +','+(myheight*240)/1280+','+(mywidth*312)/908+','+(myheight*330)/1280+'" href="#Head" alt="Your Name and Correspondence Address as registered with us">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*195)/1280+','+(mywidth*614)/908+','+(myheight*214)/1280+'" href="#Head" alt="Your current Bill number">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*216)/1280+','+(mywidth*614)/908+','+(myheight*235)/1280+'" href="#Head" alt="Your current Bill Date">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*238)/1280+','+(mywidth*614)/908+','+(myheight*256)/1280+'" href="#Head" alt="Your current Bill Period">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*258)/1280+','+(mywidth*614)/908+','+(myheight*279)/1280+'" href="#Head" alt="Your current Bill Due Date">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*194)/1280+','+(mywidth*856)/908+','+(myheight*214)/1280+'" href="#Head" alt="Unique meter number of Meter installed at your premises">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*216)/1280+','+(mywidth*856)/908+','+(myheight*236)/1280+'" href="#Head" alt="VAT classification code for regulatory purpose">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*238)/1280+','+(mywidth*856)/908+','+(myheight*259)/1280+'" href="#Head" alt="VAT classification code for regulatory purpose">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*28)/908 +','+(myheight*499)/1280+','+(mywidth*439)/908+','+(myheight*584)/1280+'" href="#Head" alt="Gas Usage reading of Last Four Bills Cycles">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*445)/908 +','+(myheight*370)/1280+','+(mywidth*854)/908+','+(myheight*582)/1280+'" href="#Head" alt="Calculation of conversion from SCM to MMBTU for your  current bill period">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*29)/908 +','+(myheight*597)/1280+','+(mywidth*206)/908+','+(myheight*647)/1280+'" href="#Head" alt="Current Gas consumption in SCM">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*256)/908 +','+(myheight*597)/1280+','+(mywidth*430)/908+','+(myheight*647)/1280+'" href="#Head" alt="Current Gas consumption in MMBTU">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*479)/908 +','+(myheight*597)/1280+','+(mywidth*654)/908+','+(myheight*647)/1280+'" href="#Head" alt="Amount to be paid before due date for current bill">');		
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*704)/908 +','+(myheight*597)/1280+','+(mywidth*856)/908+','+(myheight*647)/1280+'" href="#Head" alt="Bill amount to be paid before due date for current bill">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*30)/908 +','+(myheight*650)/1280+','+(mywidth*178)/908+','+(myheight*721)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*203)/908 +','+(myheight*650)/1280+','+(mywidth*305)/908+','+(myheight*721)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*376)/908 +','+(myheight*650)/1280+','+(mywidth*524)/908+','+(myheight*721)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*550)/908 +','+(myheight*650)/1280+','+(mywidth*683)/908+','+(myheight*721)/1280+'" href="#Head" alt="Gas usage charges for current bill">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*710)/908 +','+(myheight*650)/1280+','+(mywidth*857)/908+','+(myheight*721)/1280+'" href="#Head" alt="Amount to be paid before due date for current bill">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*764)/1280+','+(mywidth*478)/908+','+(myheight*841)/1280+'" href="#Head" alt="Amount of Gas Usage (Net Quantity * Prevailing Gas Rate">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*1015)/1280+','+(mywidth*478)/908+','+(myheight*1038)/1280+'" href="#Head" alt="Total Gas usage charges for current bill cycle">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*489)/908 +','+(myheight*977)/1280+','+(mywidth*734)/908+','+(myheight*1052)/1280+'" href="#Head" alt="Notice">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*35)/908 +','+(myheight*1075)/1280+','+(mywidth*854)/908+','+(myheight*1173)/1280+'" href="#Head" alt="Your Pay-in Slip for this bill">');
		
	
	
	var showTooltip = function(event,title){
		$(".mytip").show();
		$(".mytip").html(title);
		
		if(event.clientY+$(".mytip").height() > $(window).height()){
			$(".mytip").css({"top":event.clientY-$(".mytip").height()});
		}else{
			$(".mytip").css({"top":event.clientY+25});
		}
		if(event.clientX+$(".mytip").width()+20 > $(window).width()){
			$(".mytip").css({"left":event.clientX-($(".mytip").width()+10)})
		}else{
			$(".mytip").css({"left":event.clientX+10})
		}
	};
	$("area").on("mouseover",function(event){
		showTooltip(event,$(this).attr("alt"));
	});
	$("area").on("mousemove",function(event){
		showTooltip(event,$(this).attr("alt"));
	});
	$("area").on("mouseout",function(event){
		$(".mytip").hide();
	});
};
	
	
var myBillMapInd  = function(){
	
		mywidth = $(".examBillBlk").width();
		myheight = $(".examBillBlk").height();
	
	
		$("#myBill area").remove();
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*38)/908 +','+(myheight*195)/1280+','+(mywidth*312)/908+','+(myheight*212)/1280+'" href="#Head" alt="Contact person in behalf of your Adani Gas connection with us">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*38)/908 +','+(myheight*240)/1280+','+(mywidth*312)/908+','+(myheight*330)/1280+'" href="#Head" alt="Your Name and Correspondence Address as registered with us">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*195)/1280+','+(mywidth*614)/908+','+(myheight*214)/1280+'" href="#Head" alt="Your current Bill number">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*216)/1280+','+(mywidth*614)/908+','+(myheight*235)/1280+'" href="#Head" alt="Your current Bill Date">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*238)/1280+','+(mywidth*614)/908+','+(myheight*256)/1280+'" href="#Head" alt="Your current Bill Period">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*318)/908 +','+(myheight*258)/1280+','+(mywidth*614)/908+','+(myheight*279)/1280+'" href="#Head" alt="Your current Bill Due Date">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*194)/1280+','+(mywidth*856)/908+','+(myheight*214)/1280+'" href="#Head" alt="Unique meter number of Meter installed at your premises">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*216)/1280+','+(mywidth*856)/908+','+(myheight*236)/1280+'" href="#Head" alt="VAT classification code for regulatory purpose">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*623)/908 +','+(myheight*238)/1280+','+(mywidth*856)/908+','+(myheight*259)/1280+'" href="#Head" alt="VAT classification code for regulatory purpose">');
		
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*31)/908 +','+(myheight*478)/1280+','+(mywidth*428)/908+','+(myheight*561)/1280+'" href="#Head" alt="Calculation of conversion from SCM to MMBTU for your current bill period ">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*457)/908 +','+(myheight*368)/1280+','+(mywidth*854)/908+','+(myheight*560)/1280+'" href="#Head" alt="Your Contract Type for billing ">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*29)/908 +','+(myheight*576)/1280+','+(mywidth*206)/908+','+(myheight*615)/1280+'" href="#Head" alt="Current Gas consumption in SCM">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*256)/908 +','+(myheight*576)/1280+','+(mywidth*430)/908+','+(myheight*615)/1280+'" href="#Head" alt="Current Gas consumption in MMBTU">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*479)/908 +','+(myheight*576)/1280+','+(mywidth*654)/908+','+(myheight*615)/1280+'" href="#Head" alt="Amount to be paid before due date for current bill">');	
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*704)/908 +','+(myheight*576)/1280+','+(mywidth*856)/908+','+(myheight*615)/1280+'" href="#Head" alt="Bill amount to be paid before due date for current bill">');
		
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*30)/908 +','+(myheight*625)/1280+','+(mywidth*179)/908+','+(myheight*686)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*203)/908 +','+(myheight*625)/1280+','+(mywidth*351)/908+','+(myheight*686)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*375)/908 +','+(myheight*625)/1280+','+(mywidth*523)/908+','+(myheight*686)/1280+'" href="#Head" alt="Previous Bill & Payment Information">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*448)/908 +','+(myheight*625)/1280+','+(mywidth*684)/908+','+(myheight*686)/1280+'" href="#Head" alt="Gas usage charges for current bill">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*709)/908 +','+(myheight*625)/1280+','+(mywidth*857)/908+','+(myheight*686)/1280+'" href="#Head" alt="Amount to be paid before due date for current bill">');
		
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*724)/1280+','+(mywidth*480)/908+','+(myheight*739)/1280+'" href="#Head" alt="Amount of Gas Usage (Net Quantity * Prevailing Gas Rate">');		
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*738)/1280+','+(mywidth*479)/908+','+(myheight*753)/1280+'" href="#Head" alt="VAT & Additional Taxes on current rate of taxation">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*37)/908 +','+(myheight*966)/1280+','+(mywidth*479)/908+','+(myheight*991)/1280+'" href="#Head" alt="Total Gas usage charges for current bill cycle">');
		$("#myBill").append('<area shape="rect" coords="'+ (mywidth*32)/908 +','+(myheight*1031)/1280+','+(mywidth*853)/908+','+(myheight*1118)/1280+'" href="#Head" alt="Your Pay-in Slip for this bill">');                                              
		
	
	
	var showTooltip = function(event,title){
		$(".mytip").show();
		$(".mytip").html(title);
		
		if(event.clientY+$(".mytip").height() > $(window).height()){
			$(".mytip").css({"top":event.clientY-$(".mytip").height()});
		}else{
			$(".mytip").css({"top":event.clientY+25});
		}
		if(event.clientX+$(".mytip").width()+20 > $(window).width()){
			$(".mytip").css({"left":event.clientX-($(".mytip").width()+10)})
		}else{
			$(".mytip").css({"left":event.clientX+10})
		}
		
	};
	$("area").on("mouseover",function(event){
		showTooltip(event,$(this).attr("alt"));
	});
	$("area").on("mousemove",function(event){
		showTooltip(event,$(this).attr("alt"));
	});
	$("area").on("mouseout",function(event){
		$(".mytip").hide();
	});
};
	

$(window).resize(function() {
	setTimeout(function(){
		if(window.location.href.indexOf("domestic") >= 0){
	    	$('#billImage').attr('src','images/bills_domesticConnection.png');
	        myBillMapDom();
	    }
	    else if(window.location.href.indexOf("commercial") >= 0){
	  		$('#billImage').attr('src','images/Bill-Commercial.png');
	  		myBillMapCom();
	 	}
		else if(window.location.href.indexOf("industrial") >= 0){
			$('#billImage').attr('src','images/Bill-Industial.png');
	        myBillMapInd();  
	 	}
	    else
		{
	    	$('#billImage').attr('src','images/bills_domesticConnection.png');
	       myBillMapDom();
		}
	},1000);
});

$(document).ready(function(){


	setTimeout(function(){
		$(window).trigger('resize');
	},1000);
	
		/* if (window.location.href.indexOf("domestic") >= 0) {
			$('#billImage').attr('src','images/bill/bills_domesticConnection.png');
			myBillMapDom();
		} else if (window.location.href.indexOf("commercial") >= 0) {
			$('#billImage').attr('src','images/bill/Bill-Commercial.png');
			myBillMapCom();
		} else if (window.location.href.indexOf("industrial") >= 0) {
			$('#billImage').attr('src','images/bill/Bill-Industial.png');
			myBillMapInd();
		} else {
			$('#billImage').attr('src','images/bill/bills_domesticConnection.png');
			myBillMapDom();
		} */
	
});