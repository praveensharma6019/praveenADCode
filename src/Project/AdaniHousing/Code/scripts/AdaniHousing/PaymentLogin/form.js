
var text1 = document.getElementById("inputtag1");
var text2 = document.getElementById("inputtag2");
var butt1 = document.getElementById("btn1form1");
butt1.disabled = true;
function inputtagfuction() {
    if (text1.value == null || text1.value == "") {
        if (text2.value == null || text2.value == "") {
            text1.disabled = false;
            text2.disabled = false;
            butt1.style.backgroundColor = "gray";
            butt1.disabled = true;
        } else {
            text1.disabled = true;
            text2.disabled = false;
            butt1.disabled= false;
            butt1.style.backgroundColor = "teal";
            var mobileno = text2.value;

            var name = "Mobile Number : ";
            var number = mobileno;
            // console.log(name + number)
        }
    }
    else {
        text2.disabled = true;
        text1.disabled = false;
        butt1.disabled= false;
        butt1.style.backgroundColor = "teal";
        var loanno = text1.value;

        var name = "Loan Account Number : ";
        var number = loanno;
        // console.log(name + number)
    }
    console.log(name + number)
document.getElementById("demo").innerHTML = name + number

}
var radiobutton1 = document.getElementById("radio1")
var radiobutton2 = document.getElementById("radio2")
var butt2 = document.getElementById("btn1")
var alert1= document.getElementById("alertmodaldiv")
alert1.style.display = "none";
butt2.disabled = true;


function buttonfunction() {
if(radiobutton1.checked!=true && radiobutton2.checked!=true)
{
    alert("Choose your bank Account");
  
}
}




function myfun() {
    var rowLength = document.getElementById("DetailTable").rows.length;
    for (var i = 2; i <= rowLength; i++) {
        //var isChecked = document.getElementById("DetailTable").rows[i].cells[1].innerHTML;
        var count = i - 1;
        var radioBtn = "radio" + count;
        var LoanAccountNumber = "td_loanAccountNumber" + count;
        var PaymentDue = "td_PaymentDue" + count;
        var MobileNumber = "td_MobileNumber" + count;

        var radioButton = document.getElementById(radioBtn);
        if (radioButton.checked == true) {

            //setting pay button enable and disable. Also, update value in it.
            var payBtn = document.getElementById("btn1");
            payBtn.disabled = false;
            var radiobutton1_Amount = document.getElementById(PaymentDue).textContent;
            payBtn.value = "Pay  " + radiobutton1_Amount;
            document.getElementById("btn1").style.backgroundColor = "teal";

            //check login is using LAN or MN, to set specific fields
            var loginInfo = document.getElementById("demo").textContent.includes("Mobile Number");
            
            if (loginInfo) {
                var alert1 = document.getElementById("alertmodaldiv");
                alert1.style.display = 'block';
                LoanAccountNumberValue = document.getElementById(LoanAccountNumber).textContent;
                document.getElementById('alertmodal').innerHTML = " You Have Selected Loan Account Number " + LoanAccountNumberValue;
            }

            //set values in hidden fields 
            LoanAccountNumber = document.getElementById(LoanAccountNumber).textContent;
            PaymentDue = document.getElementById(PaymentDue).textContent;
            MobileNumber = document.getElementById(MobileNumber).textContent;               

            document.getElementById("LAC_Hidden").value = LoanAccountNumber;
            document.getElementById("MN_Hidden").value = MobileNumber;
            document.getElementById("IsAccountValid").value = true;
            document.getElementById("StageInfo_Hidden").value = "Payment";
        }
    }
}