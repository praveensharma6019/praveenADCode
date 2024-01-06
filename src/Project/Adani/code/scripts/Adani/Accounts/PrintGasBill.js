/* Sample function that returns boolean in case the browser is Internet Explorer*/
function isIE() {
    ua = navigator.userAgent;
    /* MSIE used to detect old browsers and Trident used to newer ones*/
    var is_ie = ua.indexOf("MSIE ") > -1 || ua.indexOf("Trident/") > -1;

    return is_ie;
}


function b64toBlob(b64Data, contentType) {
    contentType = contentType || '';
    var sliceSize = 512;
    b64Data = b64Data.replace(/^[^,]+,/, '');
    b64Data = b64Data.replace(/\s/g, '');
    var byteCharacters = window.atob(b64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);

        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);

        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, {
        type: contentType
    });
    return blob;
}

function isMobile() {
    var match = window.matchMedia || window.msMatchMedia;
    if (match) {
        var mq = match("(pointer:coarse)");
        return mq.matches;
    }
    return false;
}


$(document).ready(function () {
    $(".lnk_Printbill").on('click', function (event) {
        event.preventDefault();
        var invoiceval = $(this).attr('data-document');
        $.ajax({
            type: 'GET',
            url: '/api/Adanigas/PrintBill',
            dataType: 'json',
            data: { invoiceId: $(this).attr('data-document') },
            success: function (response) {
                if (isIE()) {
                    alert('It is InternetExplorer or mobile');
                    var filename = "bill.pdf";
                    var blob = b64toBlob(response, "application/pdf;base64;");
                    navigator.msSaveBlob(blob, filename);
                } else {
                    $("#main-pdf").show();
                    $("#main-pdf").html("<iframe id='iframe1' src='data:application/pdf;base64, " + response + "' width='100%' style='height:100%'></iframe>");
                    $("#printPDFModal").modal('show');
                    $("#pdftodownload").attr('href', "data:application/pdf;base64, " + response);
                }
                if (isMobile()) {
                    $("#printPDFModal").modal('hide');

                }
            },
            error: function (ex) {
                alert(Error);
            }
        });
    });
});


