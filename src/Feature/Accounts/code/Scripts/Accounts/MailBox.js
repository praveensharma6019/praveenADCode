var table;
//** Compose Form Submition **//
$(document).on('click', '#btnSubmit', function (event) {
    event.preventDefault();
    composeFormSubmit(event);
});

function composeFormSubmit(event) {
    event.preventDefault();
    //var $captcha = $('#recaptcha'),
    //    response = grecaptcha.getResponse();

    //if (response.length === 0) {
    //    $('.msg-error').text("CAPTCHA is mandatory");
    //    if (!$captcha.hasClass("error")) {
    //        $captcha.addClass("error");
    //        $("#validationRecaptcha").show();
    //        return;
    //    }
    //} else {
    //    $('.msg-error').text('');
    //    $captcha.removeClass("error");
    //    $("#validationRecaptcha").hide();
    //}

    var composeformcontrol = jQuery("#ComposeForm");
    if (composeformcontrol.valid()) {

        var fd = new FormData();
        var file_data = $('input[type="file"]')[0].files;
        for (var i = 0; i < file_data.length; i++) {
            fd.append("file_" + i, file_data[i]);
        }
        var other_data = $("#ComposeForm").serializeArray();
        $.each(other_data, function (key, input) {
            fd.append(input.name, input.value);
        });
        $("#dvMessage").hide();
        jQuery.ajax(
            {
                url: "/api/Accounts/Compose",
                method: "POST",
                data: fd,
                contentType: false,
                processData: false,
                success: function (data) {
                    //grecaptcha.reset();
                    if (data != undefined && data != null) {
                        if (data.status == true) {
                            $("#dvMessage").show();
                            $("#dvMessage>div>div").removeClass("alert-danger").addClass("alert-success");
                            $("#dvMessage>div>div").html(data.message); // Set Message from Dictionary.
                            ResetElement("ComposeForm");
                        }
                        else {
                            $("#dvMessage").show();
                            $("#dvMessage>div>div").removeClass("alert-info").addClass("alert-danger");
                            $("#dvMessage>div>div").html(data.message); // Set Message from Dictionary.
                            ResetElement("ComposeForm");
                        }
                    }
                },
                error: function (data) {
                    ResetElement("ComposeForm");
                    //grecaptcha.reset();
                }
            });
    }
    return false;
}


//** Mail Box Tables **//
// InBox //
$(document).ready(function () {
    BindInboxDataTable("tblInbox", "/api/Accounts/LoadData_Inbox");
});

// InBox //
$(document).on('click', '#li_inbox', function () {
    BindInboxDataTable("tblInbox", "/api/Accounts/LoadData_Inbox");
});

// OutBox //
$(document).on('click', '#li_outbox', function () {
    BindOutboxDataTable("tblOutbox", "/api/Accounts/LoadData_Outbox");
});

// Trash //
$(document).on('click', '#li_trash', function () {
    BindTrashDataTable("tblTrash", "/api/Accounts/LoadData_Trash");
});

function BindInboxDataTable(tableId, actionURL) {
    var TableIdWithPrefix = $("#" + tableId);
    table = TableIdWithPrefix.DataTable({

        //"processing": true, // for show progress bar  
        //"serverSide": true, // for process server side  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once  
        "pageLength": 10,

        "ajax": {
            "url": actionURL,
            "type": "GET",
            "datatype": "json"
        },

        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    },
        //    {
        //        "targets": [7],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [8],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [9],
        //        "searchable": false,
        //        "orderable": false
        //    }],

        "columns": [
            { "data": "FromEmail", "name": "FromEmail", "autoWidth": false },
            {
                "render": function (data, type, full, meta) { return '<a class="txt-orange" href="' + full.PageURL + '?emailitemid=' + full.ItemId+'">' + full.Subject + '</a>'; }
            },
            { "data": "OnDate", "name": "OnDate", "autoWidth": false },

            //{ "data": "ContactName", "title": "ContactName", "name": "ContactName", "autoWidth": true },
            //{ "data": "ContactTitle", "name": "ContactTitle", "autoWidth": true },
            //{ "data": "City", "name": "City", "autoWidth": true },
            //{ "data": "PostalCode", "name": "PostalCode", "autoWidth": true },
            //{ "data": "Country", "name": "Country", "autoWidth": true },
            //{ "data": "Phone", "name": "Phone", "title": "Status", "autoWidth": true },
            //{
            //    data: null, render: function (data, type, row) {
            //        return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";
            //    }
            //},

        ],
        "bDestroy": true

    });
}

function BindOutboxDataTable(tableId, actionURL) {
    $("#dvMessageoutbox").hide();
    var TableIdWithPrefix = $("#" + tableId);
    table = TableIdWithPrefix.DataTable({

        //"processing": true, // for show progress bar  
        //"serverSide": true, // for process server side  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once  
        "pageLength": 10,

        "ajax": {
            "url": actionURL,
            "type": "GET",
            "datatype": "json"
        },

        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    },
        //    {
        //        "targets": [7],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [8],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [9],
        //        "searchable": false,
        //        "orderable": false
        //    }],

        "columns": [
            {
                "render": function (data, type, full, meta) {

                    return '<input type="checkbox"  class="checkbox" value=' + full.ItemId + '>';
                }
            },
            { "data": "ToEmail", "name": "ToEmail", "autoWidth": false },
            {
                "render": function (data, type, full, meta) { return '<a class="txt-orange" href="' + full.PageURL + '?emailitemid=' + full.ItemId + '">' + full.Subject + '</a>'; }
            },
            { "data": "OnDate", "name": "OnDate", "autoWidth": false },
            //{ "data": "ContactName", "title": "ContactName", "name": "ContactName", "autoWidth": true },
            //{ "data": "ContactTitle", "name": "ContactTitle", "autoWidth": true },
            //{ "data": "City", "name": "City", "autoWidth": true },
            //{ "data": "PostalCode", "name": "PostalCode", "autoWidth": true },
            //{ "data": "Country", "name": "Country", "autoWidth": true },
            //{ "data": "Phone", "name": "Phone", "title": "Status", "autoWidth": true },
            //{
            //    "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Demo/Edit/' + full.CustomerID + '">Edit</a>'; }
            //},
            //{
            //    data: null, render: function (data, type, row) {
            //        return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";
            //    }
            //},

        ],
        "bDestroy": true

    });
}

function BindTrashDataTable(tableId, actionURL) {
    $("#dvMessagetrash").hide();
    var TableIdWithPrefix = $("#" + tableId);
    table = TableIdWithPrefix.DataTable({

        //"processing": true, // for show progress bar  
        //"serverSide": true, // for process server side  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once  
        "pageLength": 10,

        "ajax": {
            "url": actionURL,
            "type": "GET",
            "datatype": "json"
        },

        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    },
        //    {
        //        "targets": [7],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [8],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [9],
        //        "searchable": false,
        //        "orderable": false
        //    }],

        "columns": [
            {
                "render": function (data, type, full, meta) {

                    return '<input type="checkbox"  class="checkbox" value=' + full.ItemId + '>';
                }
            },
            { "data": "ToEmail", "name": "ToEmail", "autoWidth": false },
            {
                "render": function (data, type, full, meta) { return '<a class="txt-orange" href="' + full.PageURL + '?emailitemid=' + full.ItemId + '">' + full.Subject + '</a>'; }
            },
            { "data": "OnDate", "name": "OnDate", "autoWidth": false },
            //{ "data": "ContactName", "title": "ContactName", "name": "ContactName", "autoWidth": true },
            //{ "data": "ContactTitle", "name": "ContactTitle", "autoWidth": true },
            //{ "data": "City", "name": "City", "autoWidth": true },
            //{ "data": "PostalCode", "name": "PostalCode", "autoWidth": true },
            //{ "data": "Country", "name": "Country", "autoWidth": true },
            //{ "data": "Phone", "name": "Phone", "title": "Status", "autoWidth": true },
            //{
            //    "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Demo/Edit/' + full.CustomerID + '">Edit</a>'; }
            //},
            //{
            //    data: null, render: function (data, type, row) {
            //        return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";
            //    }
            //},

        ],
        "bDestroy": true

    });
}

//Delete Rows from Datatable
function fnDeleteOutboxItem(tableid, deleteurl, loadurl) {
    var checkedItems = new Array();
    $('input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
            checkedItems.push($(this).attr('value'));
        }
    });
    var postData = { values: checkedItems };
    $.ajax({
        type: "POST",
        url: deleteurl,
        dataType: "json",
        data: postData,
        success: function (data) {
            BindOutboxDataTable(tableid, loadurl);
            $("#dvMessageoutbox").show();
            $("#dvMessageoutbox>div>div").html(data.Result);
        }
    });
}

function fnDeleteTrashItem(tableid, deleteurl, loadurl) {
    var checkedItems = new Array();
    $('input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
            checkedItems.push($(this).attr('value'));
        }
    });
    var postData = { values: checkedItems };
    $.ajax({
        type: "POST",
        url: deleteurl,
        dataType: "json",
        data: postData,
        success: function (data) {
            BindTrashDataTable(tableid, loadurl);
            $("#dvMessagetrash").show();
            $("#dvMessagetrash>div>div").html(data.Result);
        }
    });
}

//** Reset Input Field **//
function ResetElement(formId) {
    var FormIdWithPrefix = jQuery("#" + formId);
    var resetInputField = FormIdWithPrefix.find(".reset-control");
    resetInputField.val("");
}