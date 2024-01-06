$("btnsubmit").click(function(){
 var CustomerCode = $("#CustomerCode").val();
    if (CustomerCode == "")
	{ alert("Please enter Customer Code"); 
	$("#CustomerCode").focus(); 
return false;	
	}
   
   var CustomerName = $("#CustomerName").val();
    if (CustomerName == "")
	{ alert("Please enter Customer Name"); 
	$("#CustomerName").focus(); 
	 }
	
	  var GSTIN = $("#GSTIN").val();
    if (GSTIN == "")
	{ alert("Please enter GSTIN"); 
	$("#GSTIN").focus(); 
	}
	
	  var UTR = $("#UTR").val();
    if (UTR == "")
	{ alert("Please enter UTR"); 
	$("#UTR").focus(); 
	 }
	
	 var ContentPlaceHolder1_date = $("#ContentPlaceHolder1_date").val();
    if (ContentPlaceHolder1_date == "")
	{ alert("Please enter Remittance Date"); 
	$("#ContentPlaceHolder1_date").focus(); 
	 }
	
	 var InvoiceNumber = $("#InvoiceNumber").val();
    if (InvoiceNumber == "")
	{ alert("Please enter Invoice Number"); 
	$("#InvoiceNumber").focus(); 
	}
	
	 var ContentPlaceHolder2_date = $("#ContentPlaceHolder2_date").val();
    if (ContentPlaceHolder2_date == "")
	{ alert("Please enter Invoice Date"); 
	$("#InvoiceNumber").focus(); 
	}
	
	 var InvoiceAmount = $("#InvoiceAmount").val();
    if (InvoiceAmount == "")
	{ alert("Please enter Invoice Amount"); 
	$("#InvoiceAmount").focus(); 
	}
	
	 var TDSAmount = $("#TDSAmount").val();
    if (TDSAmount == "")
	{ alert("Please enter TDS Amount"); 
	$("#TDSAmount").focus(); 
	}
	
	 var NetPayment = $("#NetPayment").val();
    if (NetPayment == "")
	{ alert("Please enter Net Payment"); 
	$("#NetPayment").focus(); 
	}
	
	 var Remarks = $("#Remarks").val();
    if (Remarks == "")
	{ alert("Please enter Remarks"); 
	$("#Remarks").focus(); 
	}

var  response  =  grecaptcha.getResponse(recaptcha1);
if (response.length  ==  0) {
    alert("Captcha required.");
   $('#btnsubmit').removeAttr("disabled");
   
}
});


