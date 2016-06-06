$(function () {
    Example.init(".bb-alert");
    $('input[type=file]').bootstrapFileInput();
    $("#contactForm input,#contactForm textarea").jqBootstrapValidation({
        preventSubmit: true,
        submitError: function ($form, event, errors) {
            // additional error messages or events
        },
        submitSuccess: function ($form, event) {
            // Prevent spam click and default submit behaviour
            $("#btnSubmit").attr("disabled", true);
            event.preventDefault();

            // get values from FORM
            var isCheckAgreebbrule = $("#agreebbrule").prop("checked");
            var isCompleteFileUpload = $("#up_progress").html();
            if (isCompleteFileUpload.indexOf("上傳完成") === -1) {
                Example.show("請上傳影片,並耐心等候影片上傳完成!");
                return;
            }
            if (!isCheckAgreebbrule) {
                Example.show("請同意參賽規則!");
                return;
            }
            var data = $("#contactForm").serialize();
            $.ajax({
                url: pliClass.formPostUrl,
                type: "POST",
                data: data,
                cache: false,
                success: function(jsonObj) {
                    // Enable button & show success message
                    //$("#btnSubmit").attr("disabled", false);
                    $('#success').html("<div class='alert alert-success'>");
                    $('#success > .alert-success').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                        .append("</button>");
                    $('#success > .alert-success')
                        .append("<strong>"+jsonObj.msg+"</strong>");
                    $('#success > .alert-success')
                        .append('</div>');

                    //clear all fields
                    $("#pli_videoFileOriName").val("");
                    $('#contactForm').trigger("reset");
                },
                error: function() {
                    // Fail message
                    $('#success').html("<div class='alert alert-danger'>");
                    $('#success > .alert-danger').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                        .append("</button>");
                    $('#success > .alert-danger').append("<strong>Sorry " + firstName + ", it seems that my mail server is not responding. Please try again later!");
                    $('#success > .alert-danger').append('</div>');
                    //clear all fields
                    $('#contactForm').trigger("reset");
                },
            });
        },
        filter: function () {
            return $(this).is(":visible");
        }
    });
    pliClass.uploadFileFunc(pliClass.uploadFileUrl);
    $("a[data-toggle=\"tab\"]").click(function(e) {
        e.preventDefault();
        $(this).tab("show");
    });
    $('input[type="checkbox"]').on('change', function (e) {
        if (e.target.checked) {
            $('#divNoticeModal').modal();
        }
    });
    $("#btnNotAgree").click(function () {
        $('input[type="checkbox"]').attr("checked", false);
    });
});

var pliClass = {
    uploadFileUrl: "/PliEvent/Competition/VideoUpload",
    formPostUrl: "/PliEvent/Competition/FormSubmit",
    uploadFileFunc: function(url) {
        $("#btnFile").click(function (evt) {
            evt.preventDefault();
            var files = $('#pli_videoFileOriName').get(0).files;
            var fileData = new FormData();
            fileData.append(files[0].name, files[0]);
            fileData.append("size",files[0].size);
            var xhr = new XMLHttpRequest();
            var upProgress = document.getElementById('up_progress');
            xhr.open('POST', url);
            xhr.onload = function (e) {
                upProgress.innerHTML = '100 %, 上傳完成';
            };
            xhr.upload.onprogress = function (evt) {
                //上傳進度
                if (evt.lengthComputable) {
                    var complete = (evt.loaded / evt.total * 100 | 0);
                    if (100 == complete) complete = 99.9;
                    upProgress.innerHTML = complete + ' %';
                }
            }
            xhr.send(fileData);
        });
    },
}


// When clicking on Full hide fail/success boxes
$('#name').focus(function() {
    $('#success').html('');
});

