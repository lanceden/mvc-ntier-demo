function UrlClass() {
    this.fielUrl = "/Wegamesoa/file/";
}
$('input[type="submit"]').mousedown(function () {
    $(this).css('background', '#2ecc71');
});
$('input[type="submit"]').mouseup(function () {
    $(this).css('background', '#1abc9c');
});
$('#loginform').click(function () {
    $('.login').fadeToggle('slow');
    $(this).toggleClass('green');
});
$(document).mouseup(function (e) {
    var container = $(".login");
    if (!container.is(e.target) // if the target of the click isn't the container...
        && container.has(e.target).length === 0) { // ... nor a descendant of the container
        container.hide();
        $('#loginform').removeClass('green');
    }
});
$("#btnLogin").bind('click', null, function () {
    var txtLoginName = $("#txtLoginName").val();
    var txtLoginPwd = $("#txtLoginPwd").val();
    $.post("/wegamesoa/admin/login", { txtLoginName: txtLoginName, txtLoginPwd: txtLoginPwd }, function (jsonObj) {
        switch (jsonObj.statu) {
            case "ok":
                window.location.href = jsonObj.data;
                break;
        case "err":
            alert(jsonObj.msg);
            break;
        }
    }, "json");
});
function checkSubmit(e) {
    if (e && e.keyCode == 13) {
        //document.forms[0].submit();
        $("#btnLogin").click();
    }
}