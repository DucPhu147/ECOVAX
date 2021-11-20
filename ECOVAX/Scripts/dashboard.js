$(document).ready(function () {
    var menuId = $('#menuId').text();
    $('#' + menuId).addClass("active");

    var menuName = $('#menuName').text();
    $('#breadcumbMenuItem').text(menuName);
    $('#breadcumbMenuItem').attr("href", menuId);
});

function windowLock() {
    $("#windowLoadingScreen").css("display", "flex");
}
function windowUnlock() {
    $("#windowLoadingScreen").css("display", "none");
}
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