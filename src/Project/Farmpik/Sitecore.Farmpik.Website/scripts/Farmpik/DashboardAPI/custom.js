const Farmpik = {
    downloadTemplate: function () {
        let target = event.target;
        fetch(target.getAttribute('data-href'))
            .then(response => {
                if (response.redirected) {
                    window.location.reload();
                    return;
                }
                return response.blob();
            })
            .then(blob => {
                Farmpik.download(blob, target.getAttribute('data-download-name'));
            }).catch((error) => {
                if (error.message == 'Failed to fetch' || error.message == 'Load failed' || error.message == 'NetworkError when attempting to fetch resource.') {
                    window.location.reload();
                    return;
                }
            });
    },

    download: (function () {
        var a = document.createElement("a");
        document.body.appendChild(a);
        a.style = "display: none";
        return function (blob, fileName) {  
            url = window.URL.createObjectURL(blob);
            a.href = url;
            a.download = fileName;
            a.click();
            window.URL.revokeObjectURL(url);
            setTimeout(() => {
                Farmpik.successToaster('Entries Downloaded Successfully!');
            }, 333);
        };
    }()),

    updateImportDetail: ($control, result) => {
        $($control).find('.NoOfUploads').html(result.NoOfUploads);
        $($control).find('.DisplayLastImportedDate').html(result.DisplayLastImportedDate);
        $($control).find('.LastImportedUser').html(result.LastImportedUser);
        $($control).find('.ErrorRecords').html(result.ErrorRecords);
        $($control).find('.TotalRecords').html(`${result.ErrorRecords && result.TemplateName == 'Price Master' ? 0 : result.TotalRecords - result.ErrorRecords}/${result.TotalRecords}`);
        $($control).find('.tool-tip').html(`Error in ${result.ErrorRecords} entries. Download it, rectify and re-upload entries`);
    },

    protectFromReSubmition: () => {
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
    },

    enableToaster: () => {
        $('.toast').toast({ animation: true, autohide: true, delay: 3500 });
        if ($('#InValidCredential').val() || $('#IsSignOut').val()) { $('#InValidCredential').val() ? Farmpik.errorToaster($('#ErrorMessage').val()) : Farmpik.successToaster('Sign Out Successfully!'); }
    },

    successToaster: (message) => {
        $('.toast').removeClass('bg-danger');
        $('.toast').addClass('bg-farmpik-success');
        $('.toast .toast-body').html(`<img src="/-/media/Project/Farmpik/DashboardAPI/image/success.svg" class="rounded mr-2" alt="..."> &nbsp; ${message}`);
        $('.toast').toast('show');
    },

    errorToaster: (errorMessage) => {
        $('.toast').removeClass('bg-farmpik-success');
        $('.toast').addClass('bg-danger');
        $('.toast .toast-body').html(`<img src="/-/media/Project/Farmpik/DashboardAPI/image/error.svg" class="rounded mr-2" alt="..."> &nbsp; ${errorMessage}`);
        $('.toast').toast('show');
    },

    dragAndDropEvents: () => {
        $('.drag-drop .before-upload').click(function () {
            $(this).siblings('.after-upload').find('input').click();
        });

        $('.drag-drop .upload-icon').click(function () {
            let $parentDiv = $(this).parents('.template-form');
            let $errordiv = $($parentDiv).find('.template-records.template-error');
            const formData = new FormData();
            let $input = $(this).siblings('input')[0];
            formData.append('TemplateName', this.getAttribute('data-template-name'));
            formData.append('ExcelFile', $input.files[0]);
            let $progressBar = $($parentDiv).find('.progress');
            $progressBar.css("display", "block");
            $progressBar.css("width", "50%");
            $progressBar.addClass('progress-bar-animated');

            fetch('/api/FarmpikDashboard/ImportTemplate', {
                method: 'POST',
                body: formData
            }).then((response) => {
                if (response.redirected) {
                    window.location.reload();
                    return;
                }
                return response.json();
            }).then((result) => {
                    if (result) {
                        if (result.IsValidTemplate && result.TotalRecords > 0) {
                            Farmpik.updateImportDetail($parentDiv, result);
                            Farmpik.successToaster(result.ErrorRecords ? `${result.TemplateName == 'Price Master' ? 'Old data retained' : 'File processed successfully'}. Please check status of records in Records Uploaded and Error Records column` : 'Updated Successfully!');
                            if (result.ErrorRecords > 0) {
                                $($errordiv).css("display", "block");
                            }
                        } else {
                            Farmpik.errorToaster(!result.IsValidTemplate ? `Invalid ${result.TemplateName} template!` : `Empty ${result.TemplateName} template!`);
                            $($errordiv).css("display", "none")
                        }
                    } else {

                    }
                    $progressBar.removeClass('progress-bar-animated');
                    $progressBar.css("width", "100%");
                })
                .catch((error) => {
                    if (error.message == 'Failed to fetch' || error.message == 'Load failed' || error.message == 'NetworkError when attempting to fetch resource.') {
                        window.location.reload();
                        return;
                    }
                    $progressBar.removeClass('progress-bar-animated');
                    $progressBar.css("width", "100%");
                    Farmpik.errorToaster(`Something Went Wrong!`);
                });
        });

        $('.drag-drop .delete-icon').click(function () {
            let $dragElement = $(this).parents('.drag-drop');
            let $input = $(this).siblings('input')[0];
            $input.value = ""
            $dragElement.removeClass('uploaded');
            $dragElement.find('.file-name').html('');
            let $progressBar = $($dragElement).find('.progress');
            $progressBar.css("display", "none");
        });

        $('.drag-drop').on('dragover', function (event) {
            event.preventDefault();
        });

        $('.drag-drop').on('dragleave', function (event) {
        });

        $('.drag-drop').on('drop', function (event) {
            event.preventDefault();

            let $input = $(this).find('input')[0];
            if (event.originalEvent.dataTransfer.files[0].type == 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
                $input.files = event.originalEvent.dataTransfer.files;
                $($input).change();

            } else {
                Farmpik.errorToaster('Invalid template!');
            }
        });
    },

    loginButtonEnable: () => {
        let email = $('#EmailId').val();
        let password = $('#Password').val();
        if (($("#EmailId").is(":-webkit-autofill") && $("#Password").is(":-webkit-autofill")) || $('#EmailId').val().length > 2 && $('#Password').val().length > 2) {
            $('form.login-form .login-button').removeAttr('disabled');
        } else {
            $('form.login-form .login-button').attr('disabled', 'disabled');
        }
    },

    registerEvents: () => {
        $('form').submit(function () {
            Farmpik.loader();
        });
        $('form.login-form input').keyup(Farmpik.loginButtonEnable);

        $('.template-form input').change(function () {
            if (this.files[0].type == 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' && (this.files[0].size / (1024 * 15)) <= (1024 * 15)) {
                $('form .login-button').removeAttr('disabled');
                let $dragElement = $(this).parents('.drag-drop');
                $dragElement.addClass('uploaded');
                $dragElement.find('.file-name').html(this.files[0].name);
            } else {
                Farmpik.errorToaster(`${(this.files[0].size / (1024 * 15)) <= (1024 * 15) ? 'Please correct the format of upload file' : 'File size must under 15MB!'}`);
                this.files = undefined;
            }
        });

        $('.download-template').click(Farmpik.downloadTemplate);
    },

    loader: () => { $('.loader').css("display", "block"); },

    onPageLoad: () => {
        Farmpik.protectFromReSubmition();
        Farmpik.enableToaster();
        Farmpik.registerEvents();
        Farmpik.dragAndDropEvents();

        if ($("#EmailId")[0]) {
            setTimeout(Farmpik.loginButtonEnable, 1000);
        }        
    }
}
$(() => {
    Farmpik.onPageLoad();
    $("input:text,form").attr("autocomplete", "off");

})