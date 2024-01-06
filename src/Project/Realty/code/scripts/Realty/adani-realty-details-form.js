$("#formRealtyBookNow").on('submit', function () {

    var MakePayment = this.name;
    var message = '';
    var error = false;
    //$('#btnContactUsSubmit').attr("disabled", "disabled");


    var PanNo = $("#PanNumber").val().toUpperCase();
    if (PanNo != "") {
        var IsValidPAN = validatePAN(PanNo);
        if (!IsValidPAN) {
            message = message + "Please enter valid PAN Card number eg: AAAAA1234A </br>";
            error = true;
        }
    }

    //if (PanNo == "") { alert("Please specify your PanNo"); $("#PanNumber").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var AadharNo = $("#AadharNumber").val();
    if (AadharNo != "") {
        var IsValidAadhar = validateAadhar(AadharNo);
        if (!IsValidAadhar) {
            message = message + "Please enter valid 12 digit Aadhar number eg: 987456214520 </br>";
            error = true;
        }
    }

    //if (AadharNo == "") { alert("Please specify your AadharNo"); $("#AadharNumber").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#EmailAddress").val();
    if (mailid != "") {
        var IsValidMail = validateEmail(mailid);
        if (!IsValidMail) {
            message = message + "Invalid Email Address </br>";
            error = true;
        }
    }
    //if (mailid == "") { alert("Email is btnContactUsSubmit"); $("#EmailAddress").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#Mobile").val();
    //if (ccontactno == "") { alert("Please enter your mobile number"); $("#Mobile").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    //if (ccontactno.length != 10) {
    //    alert("Contact Number should be of 10 digit"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false;

    //}
    var country = $("#Country").val();
    if (country == "") {
        message = message + "Please select your country </br>";
        error = true;
    }




    var formtype = $("#FormType").val();
    var pageinfo = window.location.href;

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    if ($('#termsCb1').is(":not(:checked)") || $('#termsCb2').is(":not(:checked)") || $('#termsCb3').is(":not(:checked)")) {
        message = message + "Please select accept all declarations </br>";
        error = true;
    }

    if (error) {
        $("#docErrorMessage").html(message);
        return false;
    }
    else $("#docErrorMessage").html("");


});

function validateAadhar(number) {
    var regex = /^\d{12}$/;
    return regex.test(number);
};
function validatePAN(number) {
    var regex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}/;
    return regex.test(number);
};

$("#Country").change(function () {
    loadStateDrp('#State');
});

function loadStateDrp(statedrp) {
    var parTag = '';
    if ($("#Country").val() == 'Others') {
        parTag = $("#State").parent();
        $(parTag).text('');
        $(parTag).append('<label for="">State / Province</label>');
        $(parTag).append('<input class="form-control" id="State" name="State" required="required" type="text" value="">');

    }
    else {
        parTag = $("#State").parent();
        $(parTag).text('');
        $(parTag).append('<label for="">State / Province</label>');
        $(parTag).append('<select class="form-control" id="State" name="State"><option value="">Select State</option></select>');
        statedrp = $(statedrp);
        statedrp.html('');
        statedrp.append($('<option value="">Select State</option>'));
        if ($("#Country").val() == 'AU') {

            statedrp.append($("<option />").val('ACT').text('Australian Capital Territory'));
            statedrp.append($("<option />").val('NSW').text('New South Wales'));
            statedrp.append($("<option />").val('NT').text('Northern Territory'));
            statedrp.append($("<option />").val('QLD').text('Queensland'));
            statedrp.append($("<option />").val('SA').text('South Australia'));
            statedrp.append($("<option />").val('TAS').text('Tasmania'));
            statedrp.append($("<option />").val('VIC').text('Victoria'));
            statedrp.append($("<option />").val('WA').text('Western Australia'));


        }
        if ($("#Country").val() == 'CA') {
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
        if ($("#Country").val() == 'BR') {
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
        if ($("#Country").val() == 'CN') {
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
        if ($("#Country").val() == 'DE') {
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
        if ($("#Country").val() == 'GB') {
            statedrp.append($("<option />").val('ENG').text('England'));
            statedrp.append($("<option />").val('NIR').text('Northern Ireland'));
            statedrp.append($("<option />").val('SCT').text('Scotland'));
            statedrp.append($("<option />").val('WLS').text('Wales'));
        }
        if ($("#Country").val() == 'IE') {
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
        if ($("#Country").val() == 'IN') {
            statedrp.append($("<option />").val('AN').text('Andaman and Nicobar Islands'));
            statedrp.append($("<option />").val('AP').text('Andhra Pradesh'));
            statedrp.append($("<option />").val('AR').text('Arunachal Pradesh'));
            statedrp.append($("<option />").val('AS').text('Assam'));
            statedrp.append($("<option />").val('BR').text('Bihar'));
            statedrp.append($("<option />").val('CH').text('Chandigarh'));
            statedrp.append($("<option />").val('CT').text('Chhattisgarh'));
            statedrp.append($("<option />").val('DN').text('Dadra and Nagar Haveli'));
            statedrp.append($("<option />").val('DD').text('Daman and Diu'));
            statedrp.append($("<option />").val('DL').text('Delhi'));
            statedrp.append($("<option />").val('GA').text('Goa'));
            statedrp.append($("<option />").val('GJ').text('Gujarat'));
            statedrp.append($("<option />").val('HR').text('Haryana'));
            statedrp.append($("<option />").val('HP').text('Himachal Pradesh'));
            statedrp.append($("<option />").val('JK').text('Jammu and Kashmir'));
            statedrp.append($("<option />").val('JH').text('Jharkhand'));
            statedrp.append($("<option />").val('KA').text('Karnataka'));
            statedrp.append($("<option />").val('KL').text('Kerala'));
            statedrp.append($("<option />").val('LD').text('Lakshadweep'));
            statedrp.append($("<option />").val('MP').text('Madhya Pradesh'));
            statedrp.append($("<option />").val('MH').text('Maharashtra'));
            statedrp.append($("<option />").val('MN').text('Manipur'));
            statedrp.append($("<option />").val('ML').text('Meghalaya'));
            statedrp.append($("<option />").val('MZ').text('Mizoram'));
            statedrp.append($("<option />").val('NL').text('Nagaland'));
            statedrp.append($("<option />").val('OR').text('Odisha'));
            statedrp.append($("<option />").val('PY').text('Puducherry'));
            statedrp.append($("<option />").val('PB').text('Punjab'));
            statedrp.append($("<option />").val('RJ').text('Rajasthan'));
            statedrp.append($("<option />").val('SK').text('Sikkim'));
            statedrp.append($("<option />").val('TN').text('Tamil Nadu'));
            statedrp.append($("<option />").val('TR').text('Tripura'));
            statedrp.append($("<option />").val('UT').text('Uttarakhand'));
            statedrp.append($("<option />").val('UP').text('Uttar Pradesh'));
            statedrp.append($("<option />").val('WB').text('West Bengal'));
        }
        if ($("#Country").val() == 'IT') {
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
        if ($("#Country").val() == 'MX') {
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
        if ($("#Country").val() == 'US') {
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
}