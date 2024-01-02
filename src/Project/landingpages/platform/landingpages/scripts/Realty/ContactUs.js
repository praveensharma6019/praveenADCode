
function ValidateEmail(mail) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/; 
  return regex.test(mail);
}

$(document).ready(function () {
    var SelectedRadioVal = "";

    var thankyoupageurl =
    {
        "Atelier Greens, Pune": "/residential-projects/atelier-greens/thank-you",
        "Aster, Ahmedabad": "/residential-projects/shantigram-ahmedabad/aster/thank-you",
        "The North Park, Ahmedabad": "/residential-projects/shantigram-ahmedabad/the-north-park/thank-you",
        "Water Lily, Ahmedabad": "/residential-projects/shantigram-ahmedabad/water-lily/thank-you",
        "La Marina, Ahmedabad": "/residential-projects/shantigram-ahmedabad/la-marina/thank-you",
		"Green View, Ahmedabad": "/residential-projects/green-view-shantigram-ahmedabad/thank-you",
		"The Storeys, Ahmedabad": "/residential-projects/the-storeys-shantigram-ahmedabad/thank-you",
		"Ikaria, Ahmedabad": "/residential-projects/ikaria-shantigram-ahmedabad/thank-you",
		"Archway, Ahmedabad": "/residential-projects/archway-jagatpur-ahmedabad/thank-you",
		"Atrius, Ahmedabad": "/residential-projects/atrius-jagatpur-ahmedabad/thank-you",
        "Oyster Grande, Gurugram": "/residential-projects/oyster-grande/thank-you",
        "Platinum Tower at Oyster Grande, Gurugram": "/residential-projects/Platinum Tower/thank-you",
        "Pratham, Ahmedabad": "/residential-projects/pratham/thank-you",
        "Amogha, Ahmedabad": "/residential-projects/amogha/thank-you",
        "Western Heights, Mumbai": "/residential-projects/western-heights/thank-you",
        "The Views, Mumbai": "/residential-projects/The Views/thank-you",
        "Aangan, Gurugram": "/residential-projects/aangan/thank-you",
        "Samsara, Gurugram": "/residential-projects/samsara/thank-you",
        "Belvedere Golf and Country Club": "/club/thank-you",
        "Belvedere Club Gurgaon": "/club/thank-you",
        "Inspire Ahmedabad": "/commercial-projects/thank-you",
        "Inspire BKC": "/commercial-projects/inspire-bkc/thank-you",
        "Inspire Hub": "/commercial-projects/inspire-hub/thank-you",
        "Miracle Mile": "/commercial-projects/thank-you",
        "Oyster Arcade": "/commercial-projects/thank-you",
		"Aangan Arcade": "/commercial-projects/aangan-arcade/thank-you",
        "Aangan Galleria": "/commercial-projects/aangan-galleria/thank-you",
        "Samsara Vilasa, Gurugram": "/residential-projects/samsara-vilasa/thank-you"
    };

   var data_residential =
 '<option name="name" value="" style="color:#000!important;">Select</option>' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Amogha, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Aster, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > The North Park, Ahmedabad</option > ' +
		'<option style = "color:#000!important;" value = "a4F2v000000IEFN" > La Marina, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Water Lily, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Green View, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > The Storeys, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Ikaria, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Archway, Ahmedabad</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFN" > Atrius, Ahmedabad</option > ' +
		'<option style = "color:#000!important;" value = "a4S9D000000Cbqo" > The Views, Mumbai</option > ' +
		'<option style = "color:#000!important;" value = "a4F2v000000IEFV" > Western Heights, Mumbai</option > ' +
        '<option style = "color:#000!important;" value = "a4S9D000000Cbqo" > Monte South, Mumbai</option > ' +
		'<option style="color:#000!important;" value="a4F2v000000IEFh"> Atelier Greens, Pune</option>' +
		'<option style = "color:#000!important;" value = "a4F2v000000IEFY" > Platinum Tower at Oyster Grande, Gurugram</option > ' + 
        '<option style = "color:#000!important;" value = "a4F2v000000IEFY" > Oyster Grande, Gurugram</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFg" > Samsara Vilasa, Gurugram</option >'+
        '<option style = "color:#000!important;" value = "a4F2v000000IEFc" > Samsara, Gurugram</option > ' +
        '<option style = "color:#000!important;" value = "a4F2v000000IEFe" > Aangan, Gurugram</option > ';

    var data_commercial =
        '<option name="name" value="" style="color:#000!important;">Select</option>' +
        '<option value="a4F2v000000IEFT" style="color:#000!important;">Inspire Ahmedabad</option>' +
        '<option value="a4F2v000000IEFX" style="color:#000!important;">Inspire BKC</option>' +
        '<option value="a4F2v000000IEFW" style="color:#000!important;">Inspire Hub</option>' +
        '<option value="a4F2v000000IEFd" style="color:#000!important;">Miracle Mile</option>'+
        '<option value="a4F2v000000IEFd" style="color:#000!important;">Oyster Arcade</option>'+
		'<option value="a4F2v000000IEFd" style="color:#000!important;">Aangan Arcade</option>'+
        '<option value="a4F2v000000IEFd" style="color:#000!important;">Aangan Galleria</option>';
    var data_club =
        '<option name="name" value="" style="color:#000!important;">Select</option>' +
        '<option name="name" value="a4F2v000000IPBF" style="color:#000!important;">Belvedere Golf and Country Club</option>' +
        '<option name="name" value="a4S9D000000CbrI" style="color:#000!important;">Belvedere Club Gurgaon</option>';

    $("#enquiry_form11 #describe").html(data_residential);
    //$("#enquiry_form11 #describe").html(data_residential);

    $('#submitformabcd').click(function () {
        $('#submitformabcd').attr("disabled", "disabled");
        var firstname = $("#enquiry_form11 #firstname").val();
        if (firstname === "") { alert("Please enter your Firstname"); $("#enquiry_form11 #firstname").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }
        var lastname = $("#enquiry_form11 #lastname").val();
        if (lastname === "") { alert("Please enter your Lastname"); $("#enquiry_form11 #lastname").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }

        var enuiryemail = $("#enquiry_form11 #enuiry-email").val();
        if (enuiryemail === "") { alert("Email is Required"); $("#enquiry_form11 #enuiry-email").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }
        if (!ValidateEmail(enuiryemail)) {
            alert("Email is not Valid"); $("#enquiry_form1 #enuiry-email").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
        }
		var enuirymobile = $("#enquiry_form11 #enuiry-mobile").val();
        if (enuirymobile === "") {
            alert("Please provide your Mobile Number");
            $("#enquiry_form11 #enuiry-mobile").focus();
            $('#submitformabcd').removeAttr("disabled");
            return false;
        }
		else {
            if (document.enquiry_form11.country_code.value == 'IN' && document.enquiry_form11.mobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.enquiry_form11.mobile.focus();
                $('#btnEnquirySubmit').removeAttr("disabled");
                return false;
            }
        }

        var budget = $("#enquiry_form11 #budget").val();
        if (budget === "") { alert("Please Enter your Budget"); $("#enquiry_form11 #budget").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }
        var country = $("#enquiry_form11 #drpcountry").val();
        if (country === "") { alert("Please select your country"); $("#enquiry_form11 #drpcountry").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }
        var state = $("#enquiry_form11 #state").val();
        if (state === "") { alert("Please select your state"); $("#enquiry_form11 #state").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }
        var remarks = $("#enquiry_form11 #remarks").val();
        if (remarks == "") { alert("Please enter your remarks"); $("#enquiry_form11 #remarks").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }

        var prop = $("#enquiry_form11 #describe").val();
        if (prop == "") { alert("Please select a property"); $("#enquiry_form11 #describe").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }

        var SelectedRadioVal = $("#enquiry_form11 input[type=radio][name='recordType']:checked").parent().text();
        if ($("#enquiry_form11 input[type=radio][name='recordType']:checked").val() == '0122800000080i9') {
            var saletype = $("#enquiry_form11 #sellingtype").val();
            if (saletype == "") { alert("Please select a sale type"); $("#enquiry_form11 #sellingtype").focus(); $('#submitformabcd').removeAttr("disabled"); return false; }
        }

        var model = {
            mobile: enuirymobile,
            first_name: firstname,
            email: enuiryemail
        };
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: "api/Realty/Contact",
            contentType: "application/json",
            success: function (data) {
				if (data.status == "2") {
                    alert("OTP limit is exceeded! Please try again after 30 min");
                }
				if (data.status == "1"){
					var countryname = $('#enquiry_form11 #drpcountry :selected').text();
                    var statename = $('#enquiry_form11 #state :selected').text();
					var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
					var formtype = $('#enquiry_form11 #FormType').val();
					var pageinfo = window.location.href;
					var propertytype = ($("#enquiry_form11 input[type=radio][name='recordType']:checked").parent().text()).trim();
					var RecordType = $("#enquiry_form11 input[type=radio][name='recordType']:checked").val();
					var propertilocation = $('#enquiry_form11 #describe option:selected').text().trim();
					var PropertyCode = $('#enquiry_form11 #describe').val();
					var saletype = $('#enquiry_form11 #sellingtype').val();
					var utmSource = $('#enquiry_form11 #utm_source').val();
					var replacedRemark=remarks.replace(/,/g, ';');
                    var otp = prompt("Please enter OTP received on your mobile", "");

                    if (otp !== null) {
						 var savecustomdata = {
                                        first_name: firstname,
                                        last_name: lastname,
                                        mobile: enuirymobile,
										OTP: otp,
                                        email: enuiryemail,
                                        Budget: budget,
                                        country_code: countryname,
                                        state_code: statename,
                                        Projects_Interested__c: propertytype,
                                        PropertyLocation: propertilocation,
                                        sale_type: saletype,
                                        Remarks: replacedRemark,
                                        FormType: formtype,
                                        PageInfo: pageinfo,
                                        FormSubmitOn: currentdate,
                                        UTMSource: utmSource,
                                        RecordType: RecordType,
                                        PropertyCode: PropertyCode,
										LeadSource: "Web to Lead"
                            };                                  

                             $.ajax({
                            type: "POST",
                            data: JSON.stringify(savecustomdata),
                            url: "/api/Realty/Insertcontactdetail",
                            contentType: "application/json",
                            success: function (data) {
                                if (data.status == "1") {
                                    //create json object
                                    if (data.status == "1") {
                                       //var selectedproperty = $('#describe').val();
                                        var selectedproperty = $('#enquiry_form11 #describe option:selected').text().trim();
                                        $('#enquiry_form11 #retURL').val($('#enquiry_form11 #retURL').val() + thankyoupageurl[selectedproperty]);
                                        //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                        //$('#enquiry_form11').submit();
										window.location.href = $('#enquiry_form11 #retURL').val();
                                    }
                                    else if (data.status == "2") {
                                        alert("Invalid OTP");
                                        $('#btnEnquirySubmit').removeAttr("disabled");
                                        return false;
                                    }
                                    else {
                                        alert("Sorry Operation Failed!!! Please try again later");
                                        $('#btnEnquirySubmit').removeAttr("disabled");
                                        return false;
                                    }
                                }
                                else {
                                    alert("Invalid OTP");
                                    $('#btnEnquirySubmit').removeAttr("disabled");
                                    return false;
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.log(textStatus, errorThrown);
                            }
                        });

                    }

                    else {
                        $('#btnEnquirySubmit').removeAttr("disabled");
                    }
                }
                else if (data == "-1") {
                    alert("Invalid Mobile Number");
                    $('#btnEnquirySubmit').removeAttr("disabled");
                }
            }
        });

        return false;
    });


    $("#enquiry_form11 #drpcountry").change(function () {
        loadStateDrp('#enquiry_form11 #state');
    });

    $('#enquiry_form11 input[type=radio][name=recordType]').change(function () {
        var data = "";
        $("#enquiry_form11 #sellingtype").val(null);
        var SelectedRadioVal = $("#enquiry_form11 input[type=radio][name='recordType']:checked").parent().text();
        if (SelectedRadioVal.trim() == 'CLUB' && this.value == '0129D000000AjogQAC') {
            $("#enquiry_form11 #sale_type").hide();

            $("#enquiry_form11 #residentialrb").attr("checked", false);
            $("#enquiry_form11 #commercialrb").attr("checked", false);
            $("#enquiry_form11 #clubrb").attr("checked", "checked");
            data = data_club;
        }
        else if (this.value == '0129D000000AjolQAC') {
            $("#enquiry_form11 #sale_type").hide();
            $("#enquiry_form11 #residentialrb").attr("checked", "checked");
            $("#enquiry_form11 #commercialrb").attr("checked", false);
            $("#enquiry_form11 #clubrb").attr("checked", false);
            data = data_residential;
        }
        else if (this.value == '0129D000000AlARQA0') {
            $("#enquiry_form11 #sale_type").show();
            $("#enquiry_form11 #residentialrb").attr("checked", false);
            $("#enquiry_form11 #commercialrb").attr("checked", "checked");
            $("#enquiry_form11 #clubrb").attr("checked", false);
            data = data_commercial;
        }
        $("#enquiry_form11 #describe").html(data);
    });

    // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }

    var utm_source = getUrlVars()["utm_source"];
	if(utm_source!="" && utm_source!=null)
    $("#utm_source").val(window.location.href);

    function loadStateDrp(statedrp) {
        statedrp = $(statedrp);
        statedrp.html('');
        statedrp.append($('<option value>Select State</option>'));
        if ($("#enquiry_form11 #drpcountry").val() === 'AU') {

            statedrp.append($("<option />").val('ACT').text('Australian Capital Territory'));
            statedrp.append($("<option />").val('NSW').text('New South Wales'));
            statedrp.append($("<option />").val('NT').text('Northern Territory'));
            statedrp.append($("<option />").val('QLD').text('Queensland'));
            statedrp.append($("<option />").val('SA').text('South Australia'));
            statedrp.append($("<option />").val('TAS').text('Tasmania'));
            statedrp.append($("<option />").val('VIC').text('Victoria'));
            statedrp.append($("<option />").val('WA').text('Western Australia'));


        }
        if ($("#enquiry_form11 #drpcountry").val() === 'CA') {
            statedrp.append($("<option />").val('AB').text('Alberta'));
            statedrp.append($("<option />").val('BC').text('British Columbia'));
            statedrp.append($("<option />").val('MB').text('Manitoba'));
            statedrp.append($("<option />").val('NB').text('New Brunswick'));
            statedrp.append($("<option />").val('NL').text('Newfoundland and Labrador'));
            statedrp.append($("<option />").val('NT').text('Northwest Territories'));
            statedrp.append($("<option />").val('NS').text('Nova Scotia'));
            statedrp.append($("<option />").val('NU').text('Nunavut'));
            statedrp.append($("<option />").val('ON').text('Ontario'));
            statedrp.append($("<option />").val('PE').text('Prince Edward Island'));
            statedrp.append($("<option />").val('QC').text('Quebec'));
            statedrp.append($("<option />").val('SK').text('Saskatchewan'));
            statedrp.append($("<option />").val('YT').text('Yukon Territories'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'BR') {
            statedrp.append($("<option />").val('AC').text('Acre'));
            statedrp.append($("<option />").val('AL').text('Alagoas'));
            statedrp.append($("<option />").val('AP').text('Amapá'));
            statedrp.append($("<option />").val('AM').text('Amazonas'));
            statedrp.append($("<option />").val('BA').text('Bahia'));
            statedrp.append($("<option />").val('CE').text('Ceará'));
            statedrp.append($("<option />").val('DF').text('Distrito Federal'));
            statedrp.append($("<option />").val('ES').text('Espírito Santo'));
            statedrp.append($("<option />").val('GO').text('Goiás'));
            statedrp.append($("<option />").val('MA').text('Maranhão'));
            statedrp.append($("<option />").val('MT').text('Mato Grosso'));
            statedrp.append($("<option />").val('MS').text('Mato Grosso do Sul'));
            statedrp.append($("<option />").val('MG').text('Minas Gerais'));
            statedrp.append($("<option />").val('PA').text('Pará'));
            statedrp.append($("<option />").val('PB').text('Paraíba'));
            statedrp.append($("<option />").val('PR').text('Paraná'));
            statedrp.append($("<option />").val('PE').text('Pernambuco'));
            statedrp.append($("<option />").val('PI').text('Piauí'));
            statedrp.append($("<option />").val('RJ').text('Rio de Janeiro'));
            statedrp.append($("<option />").val('RN').text('Rio Grande do Norte'));
            statedrp.append($("<option />").val('RS').text('Rio Grande do Sul'));
            statedrp.append($("<option />").val('RO').text('Rondônia'));
            statedrp.append($("<option />").val('RR').text('Roraima'));
            statedrp.append($("<option />").val('SC').text('Santa Catarina'));
            statedrp.append($("<option />").val('SP').text('São Paulo'));
            statedrp.append($("<option />").val('SE').text('Sergipe'));
            statedrp.append($("<option />").val('TO').text('Tocantins'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'CN') {
            statedrp.append($("<option />").val('34').text('Anhui'));
            statedrp.append($("<option />").val('11').text('Beijing'));
            statedrp.append($("<option />").val('71').text('Chinese Taipei'));
            statedrp.append($("<option />").val('50').text('Chongqing'));
            statedrp.append($("<option />").val('35').text('Fujian'));
            statedrp.append($("<option />").val('62').text('Gansu'));
            statedrp.append($("<option />").val('44').text('Guangdong'));
            statedrp.append($("<option />").val('45').text('Guangxi'));
            statedrp.append($("<option />").val('52').text('Guizhou'));
            statedrp.append($("<option />").val('46').text('Hainan'));
            statedrp.append($("<option />").val('13').text('Hebei'));
            statedrp.append($("<option />").val('23').text('Heilongjiang'));
            statedrp.append($("<option />").val('41').text('Henan'));
            statedrp.append($("<option />").val('91').text('Hong Kong'));
            statedrp.append($("<option />").val('42').text('Hubei'));
            statedrp.append($("<option />").val('43').text('Hunan'));
            statedrp.append($("<option />").val('32').text('Jiangsu'));
            statedrp.append($("<option />").val('36').text('Jiangxi'));
            statedrp.append($("<option />").val('22').text('Jilin'));
            statedrp.append($("<option />").val('21').text('Liaoning'));
            statedrp.append($("<option />").val('92').text('Macao'));
            statedrp.append($("<option />").val('15').text('Nei Mongol'));
            statedrp.append($("<option />").val('64').text('Ningxia'));
            statedrp.append($("<option />").val('63').text('Qinghai'));
            statedrp.append($("<option />").val('61').text('Shaanxi'));
            statedrp.append($("<option />").val('37').text('Shandong'));
            statedrp.append($("<option />").val('31').text('Shanghai'));
            statedrp.append($("<option />").val('14').text('Shanxi'));
            statedrp.append($("<option />").val('51').text('Sichuan'));
            statedrp.append($("<option />").val('12').text('Tianjin'));
            statedrp.append($("<option />").val('65').text('Xinjiang'));
            statedrp.append($("<option />").val('54').text('Xizang'));
            statedrp.append($("<option />").val('53').text('Yunnan'));
            statedrp.append($("<option />").val('33').text('Zhejiang'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'DE') {
            statedrp.append($("<option />").val('BW').text('Baden-Wurttemberg'));
            statedrp.append($("<option />").val('BY').text('Bayern'));
            statedrp.append($("<option />").val('BE').text('Berlin'));
            statedrp.append($("<option />").val('BB').text('Brandenburg'));
            statedrp.append($("<option />").val('HB').text('Bremen'));
            statedrp.append($("<option />").val('HH').text('Hamburg'));
            statedrp.append($("<option />").val('HE').text('Hessen'));
            statedrp.append($("<option />").val('MV').text('Mecklenburg-Vorpommern'));
            statedrp.append($("<option />").val('NI').text('Niedersachsen'));
            statedrp.append($("<option />").val('NW').text('Nordrhein-Westfalen'));
            statedrp.append($("<option />").val('RP').text('Rheinland-Pfalz'));
            statedrp.append($("<option />").val('SL').text('Saarland'));
            statedrp.append($("<option />").val('SN').text('Sachsen'));
            statedrp.append($("<option />").val('ST').text('Sachsen-Anhalt'));
            statedrp.append($("<option />").val('SH').text('Schleswig-Holstein'));
            statedrp.append($("<option />").val('TH').text('Thuringen'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'GB') {
            statedrp.append($("<option />").val('ENG').text('England'));
            statedrp.append($("<option />").val('NIR').text('Northern Ireland'));
            statedrp.append($("<option />").val('SCT').text('Scotland'));
            statedrp.append($("<option />").val('WLS').text('Wales'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'IE') {
            statedrp.append($("<option />").val('CW').text('Carlow'));
            statedrp.append($("<option />").val('CN').text('Cavan'));
            statedrp.append($("<option />").val('CE').text('Clare'));
            statedrp.append($("<option />").val('CO').text('Cork'));
            statedrp.append($("<option />").val('DL').text('Donegal'));
            statedrp.append($("<option />").val('D').text('Dublin'));
            statedrp.append($("<option />").val('G').text('Galway'));
            statedrp.append($("<option />").val('KY').text('Kerry'));
            statedrp.append($("<option />").val('KE').text('Kildare'));
            statedrp.append($("<option />").val('KK').text('Kilkenny'));
            statedrp.append($("<option />").val('LS').text('Laois'));
            statedrp.append($("<option />").val('LM').text('Leitrim'));
            statedrp.append($("<option />").val('LK').text('Limerick'));
            statedrp.append($("<option />").val('LD').text('Longford'));
            statedrp.append($("<option />").val('LH').text('Louth'));
            statedrp.append($("<option />").val('MO').text('Mayo'));
            statedrp.append($("<option />").val('MH').text('Meath'));
            statedrp.append($("<option />").val('MN').text('Monaghan'));
            statedrp.append($("<option />").val('OY').text('Offaly'));
            statedrp.append($("<option />").val('RN').text('Roscommon'));
            statedrp.append($("<option />").val('SO').text('Sligo'));
            statedrp.append($("<option />").val('TA').text('Tipperary'));
            statedrp.append($("<option />").val('WD').text('Waterford'));
            statedrp.append($("<option />").val('WH').text('Westmeath'));
            statedrp.append($("<option />").val('WX').text('Wexford'));
            statedrp.append($("<option />").val('WW').text('Wicklow'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'IN') {
            statedrp.append($("<option />").val('GJ').text('Gujarat'));
			statedrp.append($("<option />").val('RJ').text('Rajasthan'));
			statedrp.append($("<option />").val('MH').text('Maharashtra'));
			statedrp.append($("<option />").val('DL').text('Delhi'));
			statedrp.append($("<option />").val('HR').text('Haryana'));
			statedrp.append($("<option />").val('UP').text('Uttar Pradesh'));
			statedrp.append($("<option />").val('AN').text('Andaman and Nicobar Islands'));
			statedrp.append($("<option />").val('AP').text('Andhra Pradesh'));
			statedrp.append($("<option />").val('AR').text('Arunachal Pradesh'));
			statedrp.append($("<option />").val('AS').text('Assam'));
			statedrp.append($("<option />").val('BR').text('Bihar'));
			statedrp.append($("<option />").val('CH').text('Chandigarh'));
			statedrp.append($("<option />").val('CT').text('Chhattisgarh'));
			statedrp.append($("<option />").val('DN').text('Dadra and Nagar Haveli'));
			statedrp.append($("<option />").val('DD').text('Daman and Diu'));
			statedrp.append($("<option />").val('GA').text('Goa'));
			statedrp.append($("<option />").val('HP').text('Himachal Pradesh'));
			statedrp.append($("<option />").val('JK').text('Jammu and Kashmir'));
			statedrp.append($("<option />").val('JH').text('Jharkhand'));
			statedrp.append($("<option />").val('KA').text('Karnataka'));
			statedrp.append($("<option />").val('KL').text('Kerala'));
			statedrp.append($("<option />").val('LD').text('Lakshadweep'));
			statedrp.append($("<option />").val('MP').text('Madhya Pradesh'));
			statedrp.append($("<option />").val('MN').text('Manipur'));
			statedrp.append($("<option />").val('ML').text('Meghalaya'));
			statedrp.append($("<option />").val('MZ').text('Mizoram'));
			statedrp.append($("<option />").val('NL').text('Nagaland'));
			statedrp.append($("<option />").val('OR').text('Odisha'));
			statedrp.append($("<option />").val('PY').text('Puducherry'));
			statedrp.append($("<option />").val('PB').text('Punjab'));
			statedrp.append($("<option />").val('SK').text('Sikkim'));
			statedrp.append($("<option />").val('TN').text('Tamil Nadu'));
			statedrp.append($("<option />").val('TR').text('Tripura'));
			statedrp.append($("<option />").val('UT').text('Uttarakhand'));
			statedrp.append($("<option />").val('WB').text('West Bengal'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'IT') {
            statedrp.append($("<option />").val('AG').text('Agrigento'));
            statedrp.append($("<option />").val('AL').text('Alessandria'));
            statedrp.append($("<option />").val('AN').text('Ancona'));
            statedrp.append($("<option />").val('AO').text('Aosta'));
            statedrp.append($("<option />").val('AR').text('Arezzo'));
            statedrp.append($("<option />").val('AP').text('Ascoli Piceno'));
            statedrp.append($("<option />").val('AT').text('Asti'));
            statedrp.append($("<option />").val('AV').text('Avellino'));
            statedrp.append($("<option />").val('BA').text('Bari'));
            statedrp.append($("<option />").val('BT').text('Barletta-Andria-Trani'));
            statedrp.append($("<option />").val('BL').text('Belluno'));
            statedrp.append($("<option />").val('BN').text('Benevento'));
            statedrp.append($("<option />").val('BG').text('Bergamo'));
            statedrp.append($("<option />").val('BI').text('Biella'));
            statedrp.append($("<option />").val('BO').text('Bologna'));
            statedrp.append($("<option />").val('BZ').text('Bolzano'));
            statedrp.append($("<option />").val('BS').text('Brescia'));
            statedrp.append($("<option />").val('BR').text('Brindisi'));
            statedrp.append($("<option />").val('CA').text('Cagliari'));
            statedrp.append($("<option />").val('CL').text('Caltanissetta'));
            statedrp.append($("<option />").val('CB').text('Campobasso'));
            statedrp.append($("<option />").val('CI').text('Carbonia-Iglesias'));
            statedrp.append($("<option />").val('CE').text('Caserta'));
            statedrp.append($("<option />").val('CT').text('Catania'));
            statedrp.append($("<option />").val('CZ').text('Catanzaro'));
            statedrp.append($("<option />").val('CH').text('Chieti'));
            statedrp.append($("<option />").val('CO').text('Como'));
            statedrp.append($("<option />").val('CS').text('Cosenza'));
            statedrp.append($("<option />").val('CR').text('Cremona'));
            statedrp.append($("<option />").val('KR').text('Crotone'));
            statedrp.append($("<option />").val('CN').text('Cuneo'));
            statedrp.append($("<option />").val('EN').text('Enna'));
            statedrp.append($("<option />").val('FM').text('Fermo'));
            statedrp.append($("<option />").val('FE').text('Ferrara'));
            statedrp.append($("<option />").val('FI').text('Florence'));
            statedrp.append($("<option />").val('FG').text('Foggia'));
            statedrp.append($("<option />").val('FC').text('Forlì-Cesena'));
            statedrp.append($("<option />").val('FR').text('Frosinone'));
            statedrp.append($("<option />").val('GE').text('Genoa'));
            statedrp.append($("<option />").val('GO').text('Gorizia'));
            statedrp.append($("<option />").val('GR').text('Grosseto'));
            statedrp.append($("<option />").val('IM').text('Imperia'));
            statedrp.append($("<option />").val('IS').text('Isernia'));
            statedrp.append($("<option />").val('AQ').text('Aquila'));
            statedrp.append($("<option />").val('SP').text('La Spezia'));
            statedrp.append($("<option />").val('LT').text('Latina'));
            statedrp.append($("<option />").val('LE').text('Lecce'));
            statedrp.append($("<option />").val('LC').text('Lecco'));
            statedrp.append($("<option />").val('LI').text('Livorno'));
            statedrp.append($("<option />").val('LO').text('Lodi'));
            statedrp.append($("<option />").val('LU').text('Lucca'));
            statedrp.append($("<option />").val('MC').text('Macerata'));
            statedrp.append($("<option />").val('MN').text('Mantua'));
            statedrp.append($("<option />").val('MS').text('Massa and Carrara'));
            statedrp.append($("<option />").val('MT').text('Matera'));
            statedrp.append($("<option />").val('VS').text('Medio Campidano'));
            statedrp.append($("<option />").val('ME').text('Messina'));
            statedrp.append($("<option />").val('MI').text('Milan'));
            statedrp.append($("<option />").val('MO').text('Modena'));
            statedrp.append($("<option />").val('MB').text('Monza and Brianza'));
            statedrp.append($("<option />").val('NA').text('Naples'));
            statedrp.append($("<option />").val('NO').text('Novara'));
            statedrp.append($("<option />").val('NU').text('Nuoro'));
            statedrp.append($("<option />").val('OG').text('Ogliastra'));
            statedrp.append($("<option />").val('OT').text('Olbia-Tempio'));
            statedrp.append($("<option />").val('OR').text('Oristano'));
            statedrp.append($("<option />").val('PD').text('Padua'));
            statedrp.append($("<option />").val('PA').text('Palermo'));
            statedrp.append($("<option />").val('PR').text('Parma'));
            statedrp.append($("<option />").val('PV').text('Pavia'));
            statedrp.append($("<option />").val('PG').text('Perugia'));
            statedrp.append($("<option />").val('PU').text('Pesaro and Urbino'));
            statedrp.append($("<option />").val('PE').text('Pescara'));
            statedrp.append($("<option />").val('PC').text('Piacenza'));
            statedrp.append($("<option />").val('PI').text('Pisa'));
            statedrp.append($("<option />").val('PT').text('Pistoia'));
            statedrp.append($("<option />").val('PN').text('Pordenone'));
            statedrp.append($("<option />").val('PZ').text('Potenza'));
            statedrp.append($("<option />").val('PO').text('Prato'));
            statedrp.append($("<option />").val('RG').text('Ragusa'));
            statedrp.append($("<option />").val('RA').text('Ravenna'));
            statedrp.append($("<option />").val('RC').text('Reggio Calabria'));
            statedrp.append($("<option />").val('RE').text('Reggio Emilia'));
            statedrp.append($("<option />").val('RI').text('Rieti'));
            statedrp.append($("<option />").val('RN').text('Rimini'));
            statedrp.append($("<option />").val('RM').text('Rome'));
            statedrp.append($("<option />").val('RO').text('Rovigo'));
            statedrp.append($("<option />").val('SA').text('Salerno'));
            statedrp.append($("<option />").val('SS').text('Sassari'));
            statedrp.append($("<option />").val('SV').text('Savona'));
            statedrp.append($("<option />").val('SI').text('Siena'));
            statedrp.append($("<option />").val('SO').text('Sondrio'));
            statedrp.append($("<option />").val('SR').text('Syracuse'));
            statedrp.append($("<option />").val('TA').text('Taranto'));
            statedrp.append($("<option />").val('TE').text('Teramo'));
            statedrp.append($("<option />").val('TR').text('Terni'));
            statedrp.append($("<option />").val('TP').text('Trapani'));
            statedrp.append($("<option />").val('TN').text('Trento'));
            statedrp.append($("<option />").val('TV').text('Treviso'));
            statedrp.append($("<option />").val('TS').text('Trieste'));
            statedrp.append($("<option />").val('TO').text('Turin'));
            statedrp.append($("<option />").val('UD').text('Udine'));
            statedrp.append($("<option />").val('VA').text('Varese'));
            statedrp.append($("<option />").val('VE').text('Venice'));
            statedrp.append($("<option />").val('VB').text('Verbano-Cusio-Ossola'));
            statedrp.append($("<option />").val('VC').text('Vercelli'));
            statedrp.append($("<option />").val('VR').text('Verona'));
            statedrp.append($("<option />").val('VV').text('Vibo Valentia'));
            statedrp.append($("<option />").val('VI').text('Vicenza'));
            statedrp.append($("<option />").val('VT').text('Viterbo'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'MX') {
            statedrp.append($("<option />").val('AG').text('Aguascalientes'));
            statedrp.append($("<option />").val('BC').text('Baja California'));
            statedrp.append($("<option />").val('BS').text('Baja California Sur'));
            statedrp.append($("<option />").val('CM').text('Campeche'));
            statedrp.append($("<option />").val('CS').text('Chiapas'));
            statedrp.append($("<option />").val('CH').text('Chihuahua'));
            statedrp.append($("<option />").val('CO').text('Coahuila'));
            statedrp.append($("<option />").val('CL').text('Colima'));
            statedrp.append($("<option />").val('DG').text('Durango'));
            statedrp.append($("<option />").val('DF').text('Federal District'));
            statedrp.append($("<option />").val('GT').text('Guanajuato'));
            statedrp.append($("<option />").val('GR').text('Guerrero'));
            statedrp.append($("<option />").val('HG').text('Hidalgo'));
            statedrp.append($("<option />").val('JA').text('Jalisco'));
            statedrp.append($("<option />").val('ME').text('Mexico State'));
            statedrp.append($("<option />").val('MI').text('Michoacán'));
            statedrp.append($("<option />").val('MO').text('Morelos'));
            statedrp.append($("<option />").val('NA').text('Nayarit'));
            statedrp.append($("<option />").val('NL').text('Nuevo León'));
            statedrp.append($("<option />").val('OA').text('Oaxaca'));
            statedrp.append($("<option />").val('PB').text('Puebla'));
            statedrp.append($("<option />").val('QE').text('Querétaro'));
            statedrp.append($("<option />").val('QR').text('Quintana Roo'));
            statedrp.append($("<option />").val('SL').text('San Luis Potosí'));
            statedrp.append($("<option />").val('SI').text('Sinaloa'));
            statedrp.append($("<option />").val('SO').text('Sonora'));
            statedrp.append($("<option />").val('TB').text('Tabasco'));
            statedrp.append($("<option />").val('TM').text('Tamaulipas'));
            statedrp.append($("<option />").val('TL').text('Tlaxcala'));
            statedrp.append($("<option />").val('VE').text('Veracruz'));
            statedrp.append($("<option />").val('YU').text('Yucatán'));
            statedrp.append($("<option />").val('ZA').text('Zacatecas'));
        }
        if ($("#enquiry_form11 #drpcountry").val() == 'US') {
            statedrp.append($("<option />").val('AL').text('Alabama'));
            statedrp.append($("<option />").val('AK').text('Alaska'));
            statedrp.append($("<option />").val('AZ').text('Arizona'));
            statedrp.append($("<option />").val('AR').text('Arkansas'));
            statedrp.append($("<option />").val('CA').text('California'));
            statedrp.append($("<option />").val('CO').text('Colorado'));
            statedrp.append($("<option />").val('CT').text('Connecticut'));
            statedrp.append($("<option />").val('DE').text('Delaware'));
            statedrp.append($("<option />").val('DC').text('District of Columbia'));
            statedrp.append($("<option />").val('FL').text('Florida'));
            statedrp.append($("<option />").val('GA').text('Georgia'));
            statedrp.append($("<option />").val('HI').text('Hawaii'));
            statedrp.append($("<option />").val('ID').text('Idaho'));
            statedrp.append($("<option />").val('IL').text('Illinois'));
            statedrp.append($("<option />").val('IN').text('Indiana'));
            statedrp.append($("<option />").val('IA').text('Iowa'));
            statedrp.append($("<option />").val('KS').text('Kansas'));
            statedrp.append($("<option />").val('KY').text('Kentucky'));
            statedrp.append($("<option />").val('LA').text('Louisiana'));
            statedrp.append($("<option />").val('ME').text('Maine'));
            statedrp.append($("<option />").val('MD').text('Maryland'));
            statedrp.append($("<option />").val('MA').text('Massachusetts'));
            statedrp.append($("<option />").val('MI').text('Michigan'));
            statedrp.append($("<option />").val('MN').text('Minnesota'));
            statedrp.append($("<option />").val('MS').text('Mississippi'));
            statedrp.append($("<option />").val('MO').text('Missouri'));
            statedrp.append($("<option />").val('MT').text('Montana'));
            statedrp.append($("<option />").val('NE').text('Nebraska'));
            statedrp.append($("<option />").val('NV').text('Nevada'));
            statedrp.append($("<option />").val('NH').text('New Hampshire'));
            statedrp.append($("<option />").val('NJ').text('New Jersey'));
            statedrp.append($("<option />").val('NM').text('New Mexico'));
            statedrp.append($("<option />").val('NY').text('New York'));
            statedrp.append($("<option />").val('NC').text('North Carolina'));
            statedrp.append($("<option />").val('ND').text('North Dakota'));
            statedrp.append($("<option />").val('OH').text('Ohio'));
            statedrp.append($("<option />").val('OK').text('Oklahoma'));
            statedrp.append($("<option />").val('OR').text('Oregon'));
            statedrp.append($("<option />").val('PA').text('Pennsylvania'));
            statedrp.append($("<option />").val('RI').text('Rhode Island'));
            statedrp.append($("<option />").val('SC').text('South Carolina'));
            statedrp.append($("<option />").val('SD').text('South Dakota'));
            statedrp.append($("<option />").val('TN').text('Tennessee'));
            statedrp.append($("<option />").val('TX').text('Texas'));
            statedrp.append($("<option />").val('UT').text('Utah'));
            statedrp.append($("<option />").val('VT').text('Vermont'));
            statedrp.append($("<option />").val('VA').text('Virginia'));
            statedrp.append($("<option />").val('WA').text('Washington'));
            statedrp.append($("<option />").val('WV').text('West Virginia'));
            statedrp.append($("<option />").val('WI').text('Wisconsin'));
            statedrp.append($("<option />").val('WY').text('Wyoming'));
        }
    }
});