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
//Thêm method so sánh date
jQuery.validator.addMethod("greaterThanDate",
    function (value, element, param) {
        if (value !== "" && param !== "") {
            return new Date(value).getTime() >= new Date(param).getTime();
        }
        return true;
    }, 'Ngày phải lớn hơn {0}.');

jQuery.validator.addMethod("smallerThanDate",
    function (value, element, param) {
        if (value !== "" && param !== "") {
            var curDate = new Date();
            var valueDate = new Date(value);
            curDate.setHours(0,0,0,0);
            valueDate.setHours(0,0,0,0);
            return valueDate < curDate;
        }
        return true;
    }, 'Ngày phải nhỏ hơn {0}.');

jQuery.validator.addMethod("equalRangeLength", function (value, element, param) {
    return this.optional(element) || value.length == param[0] || value.length == param[1];
}, "Số ký tự phải là {0} hoặc {1} ký tự");

jQuery.validator.addMethod("equalLength", function (value, element, param) {
    return this.optional(element) || value.length == param;
}, "Số ký tự phải là {0} ký tự");

//Chỉnh sửa message mặc định
jQuery.extend(jQuery.validator.messages, {
    required: "Trường này không được bỏ trống.",
    remote: "Hãy sửa lại trường này.",
    email: "Định dạng email không đúng.",
    url: "Định dạng URL không đúng.",
    date: "Định dang ngày tháng không đúng.",
    dateISO: "Định dang ngày tháng(ISO) không đúng.",
    number: "Định dạng số không đúng.",
    digits: "Hãy nhập số.",
    creditcard: "Định dạng thẻ không đúng.",
    equalTo: "Hãy nhập đúng giá trị.",
    accept: "Hãy nhập giá trị có phần mở rộng hợp lệ.",
    maxlength: jQuery.validator.format("Không được nhập quá {0} ký tự."),
    minlength: jQuery.validator.format("Phải nhập ít nhất {0} ký tự."),
    rangelength: jQuery.validator.format("Hãy nhập chuỗi trong khoảng từ {0} đến {1} ký tự."),
    range: jQuery.validator.format("Hãy nhập giá trị trong khoảng {0} và {1}."),
    max: jQuery.validator.format("Hãy nhập giá trị chỉ nhỏ hơn hoặc bằng {0}."),
    min: jQuery.validator.format("Hãy nhập giá trị chỉ lớn hơn hoặc bằng {0}.")
});

function windowLock() {
    $("#windowLoadingScreen").css("display", "flex");
}
function windowUnlock() {
    $("#windowLoadingScreen").css("display", "none");
}
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
}
