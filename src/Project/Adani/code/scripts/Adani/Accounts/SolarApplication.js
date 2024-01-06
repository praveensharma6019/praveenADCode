$(document).ready(function () {

    $("#frmRegister").submit(function (event) {
        var buttonName = $(document.activeElement).attr('name');

        if (buttonName === "Submit") {
            try {
                var FileUpload1 = $("[id*='file_']");
                var docname = $("[id*='docnumber_']");
                var madfile = $("[id*='manddocname_']");
                var error = false;

                var message = "* All files are required.</br>";
                for (var i = 0; i < FileUpload1.length; i++) {
                    if ($("." + FileUpload1[i].id).val() === undefined) {
                        if ($("#" + FileUpload1[i].id).val() === '') {
                            error = true;
                            //message = message + " " + $("#" + madfile[i].id).val() + "</br>";
                        }
                    }
                }
                if (error) {
                    $("#docErrorMessage").html(message);
                    return false;
                }
            }
            catch (tes) {
                $("#docErrorMessage").html("Please upload all files");
                return false;
            }
        }
        return true;
    });

});

