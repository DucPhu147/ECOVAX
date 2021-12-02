$(document).ready(function () {
    searchGCN();
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        }
    });
    $("#ddlNguoiLap").select2({
        placeholder: "Người lập giấy...",
        allowClear: true
    });
    $("#ddlSoMui").select2({
        placeholder: "Số mũi tiêm...",
        allowClear: true
    });
    $("#ddlLoVaccine").select2({
        placeholder: "Lô vắc xin...",
        allowClear: true
    });
    $("#ddlDTC").select2({
        placeholder: "Chọn nơi cấp...",
        allowClear: true
    });
    if ($("#mode").text() != "2") {
        $('#ddlDTC').val(null).trigger('change');
    }
    $("#ddlDTC").change(function () {
        getLoVaccine();
    });

    $("#vaccineForm").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element); // default placement for everything else
        },
        rules: {
            IdVaccine: {
                digits: true,
            },
            ThoiHanTiem: {
                required: true,
                digits: true,
            },
            TenVaccine: {
                required: true
            }
        }
    });
    $("#btnSearch").click(function () {
        searchGCN();
    });
    $("#btnAdd").on('click', function () {
        openPopup(null, 1);
    });
    $("#btnSubmit").on('click', function () {
        if ($("#gcnForm").valid()) {
            submitForm();
        }
    });
    getNguoiLap();

    $("#gcnForm").validate({
        errorPlacement: function (error, element) {
            if ($(element).is('select')) {
                element.next().after(error); // special placement for select elements
            } else {
                error.insertAfter(element);  // default placement for everything else
            }
        },
        rules: {
            CMND: {
                required: true,
                digits: true,
                equalRangeLength: [9, 12],
            },
            IdGiayDK: {
                required: true,
            },
            IdDTC: {
                required: true
            },
            SoMui: {
                required: true,
            },
            LoVaccine: {
                required: true
            }
        }
    });
    $("#IdGiayDK").change(function () {
        getGiayDKInfo();
    });
});
function getLoVaccine() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: {
            idDTC: $("#ddlDTC option:selected").val(),
        },
        url: '/QuanLyGCN/GetLoVaccineByIdDTC',
        success: function (jsonResult) {
            const result = JSON.parse(jsonResult);
            $('#ddlLoVaccine').html("");
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    var newOption = new Option(result[i].LoVaccine + " (" + result[i].TenVaccine + ")", result[i].LoVaccine, false, false);
                    $('#ddlLoVaccine').append(newOption);
                }
                $('#ddlLoVaccine').val(null).trigger('change');
            }
        },
    });
}
function getNguoiLap() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyGCN/GetNguoiLap',
        success: function (jsonResult) {
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    var newOption = new Option(result[i].Ten, result[i].IdTaiKhoan, false, false);
                    $('#ddlNguoiLap').append(newOption);
                }
                $('#ddlNguoiLap').val(null).trigger('change');
            }
        },
    });
}

function searchGCN() {
    var soMui = $("#ddlSoMui option:selected").text();
    var nguoiLap = $("#ddlNguoiLap option:selected").text();
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyGCN/GetGCN',
        data: {
            idGCN: $("#idGCN").val(),
            tenDTC: $("#tenDTC").val(),
            soMui: soMui,
            nguoiLap: nguoiLap,
            loVaccine: $("#loVaccine").val(),
        },
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    //UPDATE
                    var action = "<a href='#' onClick='openPopup(\"" + result[i].IdGiayCN + "\",2)' title='Chỉnh sửa'><i class='fal fa-edit'></i></a>";
                    $("#tblResult").DataTable().row.add([
                        result[i].IdGiayCN,
                        result[i].SoMui,
                        result[i].ThoiGianTiem,
                        result[i].TenVaccine,
                        result[i].LoVaccine,
                        result[i].TenNguoiTiem,
                        result[i].CMND,
                        result[i].TenNguoiLap,
                        result[i].TenDTC,
                        result[i].TenNguoiUpdate,
                        result[i].UpdateTime,
                        action
                    ]).draw();
                }
            } else {
                $("#tblResult").DataTable().columns.adjust().draw();
            }
        },
    });
}
function openPopup(idGCN, mode) {
    var popupWindow = window.open('/QuanLyGCN?idGCN=' + idGCN + '&mode=' + mode, 'mypopuptitle', 'width=' + screen.availWidth * 0.7 + ',height=' + screen.availHeight * 0.7);
    var timer = setInterval(function () {
        if (popupWindow.closed) {
            clearInterval(timer);
            searchGCN();
        }
    }, 500);
}
function getGiayDKInfo() {
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idGiayDK: $("#IdGiayDK").val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyGCN/GetGiayDKInfo',
        success: function (jsonResult) {
            var result = jsonResult;
            if (result.status == "error") {
                showDangerAlert(jsonResult.message);
                return;
            }
            $("#TenDTC").val(result.TenDTC);
            $("#NgayTiem").val(result.NgayTiem);
            $("#CMND").val(result.CMND);
            $("#HoTen").val(result.TenNguoiDK);
            $("#SoMui").val(result.SoMui);
            if (result.DdlLoVaccine.length > 0) {
                for (let i = 0; i < result.DdlLoVaccine.length; i++) {
                    var newOption = new Option(result.DdlLoVaccine[i].Text, result.DdlLoVaccine[i].Value, false, false);
                    $('#ddlLoVaccine').append(newOption);
                }
                $('#ddlLoVaccine').val(null).trigger('change');
            }
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi xảy ra khi tìm thông tin giấy đăng ký");
        }
    });
}
function submitForm() {
    if ($("#mode").text() == "2") {
        if (!confirm("Bạn có muốn cập nhật giấy chứng nhận này?")) {
            return;
        }
    }
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            /*cmnd: $("#CMND").val(),
            idDTC: $("#ddlDTC option:selected").val(),
            loVaccine: $("#ddlLoVaccine option:selected").val(),
            soMui: $("#ddlSoMui option:selected").val()*/
            idGiayDK: $("#IdGiayDK").val(),
            loVaccine: $("#ddlLoVaccine option:selected").val(),
        },
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyGCN/SubmitGCNForm',
        success: function (jsonResult) {
            if (jsonResult.status == "error") {
                showDangerAlert(jsonResult.message);
                return;
            }
            if ($("#mode").text() == "2") {
                showSuccessAlert("Cập nhật giấy chứng nhận thành công");
                $("#btnSubmit").prop('disabled', true);
                var timer = setInterval(function () {
                    location.reload();
                    clearTimeout(timer);
                }, 2000);
            } else {
                showSuccessAlert("Thêm giấy chứng nhận thành công");
                $("#btnSubmit").prop('disabled', true);
                var timer = setInterval(function () {
                    location.href = "QuanLyGCN?idGCN=" + jsonResult + "&mode=2";
                    clearTimeout(timer);
                }, 2000);
            }
        },
        error: function () {
            if ($("#mode").text() == "2") {
                showDangerAlert("Đã gặp lỗi xảy ra khi cập nhật giấy chứng nhận");
            } else {
                showDangerAlert("Đã gặp lỗi xảy ra khi thêm giấy chứng nhận");
            }
        }
    });
}