$(document).ready(function () {
    $("#btnSubmit").click(function () {
        if ($("#pheDuyetForm").valid()) {
            pheDuyetGDK();
        }
    });
    $("#pheDuyetForm").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element);  // default placement for everything else
        },
        rules: {
            TrangThaiPD: {
                required: true
            },
            NgayTiemThucTe: {
                required: true,
                customGreaterThanDate: $("#txtNgayTiemMongMuon").text(),
                customGreaterThanDate2: $("#txtNgayTiemMongMuon").text()
            },
            BuoiTiemThucTe: {
                required: true
            },
            IdDTC: {
                required: true,
                digits: true,
            },
        }
    });
    $("#tblDTC").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        }
    });
    $("#btnSearch").click(function () {
        getDTC();
    });
    $('#ddlTrangThai').on('change', function (e) {
        var selected = $(e.currentTarget).val();
        if (selected !== "Xác nhận") {
            $("#ddlBuoiTiem").prop("disabled", true);
            $("#ngayTiem").prop("disabled", true);
            $("#idDTC").prop("disabled", true);
            $("#ddlVaccine").prop("disabled", true);
        } else {
            $("#ddlBuoiTiem").prop("disabled", false);
            $("#ngayTiem").prop("disabled", false);
            $("#idDTC").prop("disabled", false);
            $("#ddlVaccine").prop("disabled", false);
        }
    });
});
jQuery.validator.addMethod("customGreaterThanDate",
    function (value, element, param) {
        if (value !== "" && param == "") {
            var inputDate = new Date(value);
            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() + 3);

            return inputDate.getTime() > currentDate.getTime();
        }
        return true;
    }, 'Ngày tiêm phải lớn hơn ngày hiện tại ít nhất 3 ngày.');

jQuery.validator.addMethod("customGreaterThanDate2",
    function (value, element, param) {
        if (value !== "" && param !== "") {
            var paramDate = new Date(param).getTime();
            return new Date(value).getTime() >= paramDate;
        }
        return true;
    }, 'Ngày tiêm phải lớn hơn hoặc bằng ngày tiêm mong muốn.');

function getDTC() {

    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idVaccine: $("#ddlVaccine").val(),
        },
        contentType: "application/json; charset=utf-8",
        url: '/PheDuyetGDK/GetDTC',
        success: function (jsonResult) {
            $("#tblDTC").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    var hanSuDung = "";
                    if (result[i].HanSuDung != null) {
                        hanSuDung = result[i].HanSuDung.replace("T00:00:00", "");
                    }
                    $("#tblDTC").DataTable().row.add([
                        result[i].IdDTC,
                        result[i].TenDTC,
                        result[i].DiaChi,
                        result[i].ThoiGianLamViec,
                        result[i].Ten,
                        result[i].SDT,
                        result[i].LoVaccine,
                        result[i].SoLuong,
                        hanSuDung,
                    ]).draw();
                }
            } else {
                $("#tblDTC").DataTable().columns.adjust().draw();
            }
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi khi tìm kiếm");
        }
    });
}

function pheDuyetGDK() {
    if (!confirm("Bạn có chắc muốn phê duyệt đơn này")) {
        return;
    }
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            buoiTiem: $("#ddlBuoiTiem").val(),
            idDTC: $("#idDTC").val(),
            trangThaiPD: $("#ddlTrangThai").val(),
            ngayTiem: $("#ngayTiem").val(),
            vaccine: $("#ddlVaccine option:selected").text()
        },
        contentType: "application/json; charset=utf-8",
        url: '/PheDuyetGDK/PheDuyetGDK',
        success: function (jsonResult) {
            showSuccessAlert("Phê duyệt thành công");
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi khi phê duyệt giấy đăng ký");
        }
    });
}