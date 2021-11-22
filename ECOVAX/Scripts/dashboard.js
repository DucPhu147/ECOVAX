
$(document).ready(function () {
    var menuId = $('#menuId').text();
    $('.' + menuId).addClass("active");

    var menuName = $('#menuName').text();
    var subMenuName = $("#subMenuName").text();
    if (subMenuName !== "") {
        $("#breadcumbSubMenuItem").text(subMenuName);
    } else {
        $("#breadcumbSubMenuItem").parent().remove();
    }
    $('#breadcumbMenuItem').text(menuName);
    $('#breadcumbMenuItem').attr("href", menuId);
});
$.ajaxSetup({
    beforeSend: function () {
        windowLock();
    },
    complete: function () {
        const timeOut = setTimeout(function () {
            windowUnlock();
            clearTimeout(timeOut);
        }, 200);
    }
});
function showSuccessAlert(text) {
    $("#alertDialog").removeClass("alert-danger");
    $("#alertDialog").addClass("alert-success");
    showAlert(text);
}
function showDangerAlert(text) {
    $("#alertDialog").removeClass("alert-success");
    $("#alertDialog").addClass("alert-danger");
    showAlert(text);
}

function showAlert(text) {
    $("#alertDialog").css("right", "0");
    $("#alertDialog").text(text);
    const timeOut = setTimeout(function () {
        $("#alertDialog").css("right", "-100vw");
        clearTimeout(timeOut);
    }, 3000);
} function showSuccessAlert(text) {
    $("#alertBar").removeClass("alert-danger");
    $("#alertBar").addClass("alert-success");
    showAlert(text);
}
function showDangerAlert(text) {
    $("#alertBar").removeClass("alert-success");
    $("#alertBar").addClass("alert-danger");
    showAlert(text);
}

function showAlert(text) {
    $("#alertBar").text(text);
    $("#alertBar").addClass("d-block");
}
function windowLock() {
    $("#windowLoadingScreen").css("display", "flex");
}
function windowUnlock() {
    $("#windowLoadingScreen").css("display", "none");
}