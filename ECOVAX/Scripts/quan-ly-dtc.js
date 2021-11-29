$(document).ready(function () {
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        }
    });
    $("#btnSearch").on('click', function () {
        searchDTC();
    });
    $("#btnAdd").on('click', function () {
        openPopup(null, 1);
    });
    const timeOut = setTimeout(function () {
        searchDTC();
        clearTimeout(timeOut);
    }, 500);

    $("#ddlNguoiPT").select2({
        placeholder: "Chọn người phụ trách"
    });

    $("#cbDiaChiMoi").change(function () {
        if (this.checked) {
            $("#ddlTinhThanhPho").prop('disabled', false);
            $("#diaChiHanhChinh").prop('disabled', false);
        } else {
            $("#ddlTinhThanhPho").prop('disabled', true);
            $("#diaChiHanhChinh").prop('disabled', true);
        }
    });
    $("#btnSubmit").click(function () {
        if ($("#dtcForm").valid()) {
            submitForm();
        }
    })
    $("#dtcForm").validate({
        errorPlacement: function (error, element) {
            if ($(element).is('select')) {
                element.next().after(error); // special placement for select elements
            } else {
                error.insertAfter(element);  // default placement for everything else
            }
        },
        rules: {
            TenDTC: {
                required: true
            },
            ThoiGianLamViec: {
                required: true,
            },
            TenNguoiPT: {
                required: true
            },
            DiaChiHanhChinh: {
                required: true,
            },
            TinhThanh: {
                required: true,
            },
        }
    });
});
function submitForm() {
    var diaChi;
    var diaChiHanhChinh;
    if ($("#cbDiaChiMoi").is(":checked") || !$("#cbDiaChiMoi").length) {
        diaChi = $("#ddlPhuongXa option:selected").val();
        diaChiHanhChinh = $("#diaChiHanhChinh").val();

    } else {
        diaChi = "";
        diaChiHanhChinh = $("#diaChiHienTai").text();
    }
    if ($("#cbDiaChiMoi").length) {
        if (!confirm("Bạn có chắc muốn cập nhật điểm tiêm chủng này?")) {
            return;
        }
    }

    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idDTC: $("#idDTC").val(),
            tenDTC: $("#tenDTC").val(),
            thoiGianLamViec: $("#thoiGianLamViec").val(),
            idNguoiPT: $("#ddlNguoiPT option:selected").val(),
            diaChi: diaChi,
            diaChiHanhChinh: diaChiHanhChinh
        },
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyDTC/SubmitDTCForm',
        success: function (jsonResult) {
            if ($("#diaChiHienTai").length) {
                showSuccessAlert("Cập nhật điểm tiêm chủng thành công");
                $("#btnSubmit").prop('disabled', true);
                var timer = setInterval(function () {
                    location.reload();
                    clearTimeout(timer);
                }, 2000);
            } else {
                showSuccessAlert("Thêm điểm tiêm chủng thành công");
                $("#btnSubmit").prop('disabled', true);
                var timer = setInterval(function () {
                    location.href = "QuanLyDTC?idDTC=" + jsonResult + "&mode=2";
                    clearTimeout(timer);
                }, 2000);
            }
        },
        error: function () {
            if ($("#diaChiHienTai").length) {
                showDangerAlert("Đã gặp lỗi xảy ra khi cập nhật điểm tiêm chủng");
            } else {
                showDangerAlert("Đã gặp lỗi xảy ra khi thêm điểm tiêm chủng");
            }
        }
    });
}
function searchDTC() {
    var tinhThanh = $("#ddlTinhThanhPho option:selected").text();
    var quanHuyen = $("#ddlQuanHuyen option:selected").text();
    var phuongXa = $("#ddlPhuongXa option:selected").text();
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idDTC: $("#idDTC").val(),
            tenDTC: $("#tenDTC").val(),
            tinhThanh: tinhThanh,
            quanHuyen: quanHuyen,
            phuongXa: phuongXa,
            delFlag: $("#cbDelFlag").is(':checked')
        },
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyDTC/GetDTC',
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    //UPDATE
                    var actionOne = "<a href='#' onClick='openPopup(\"" + result[i].IdDTC + "\",2)' title='Chỉnh sửa'><i class='fal fa-edit'></i></a>";
                    //DELETE
                    var actionTwo = "<a href='#' class='text-danger ml-3' onClick='requestDelete(\"" + result[i].IdDTC + "\",1)' title='Xóa'><i class='fal fa-remove'></i></a>";
                    if ($("#cbDelFlag").is(":checked")) {
                        // Khôi phục xóa
                        actionOne = "<a href='#' class='text-success' onClick='requestDelete(\"" + result[i].IdDTC + "\",0)' title='Khôi phục'><i class='fal fa-plus'></i></a>";
                        //Gỡ hoàn toàn
                        actionTwo = "<a href='#' class='text-danger ml-3' onClick='requestRemove(\"" + result[i].IdDTC + "\")' title='Xóa khỏi hệ thống'><i class='fal fa-trash'></i></a>";
                    }
                    $("#tblResult").DataTable().row.add([
                        result[i].IdDTC,
                        result[i].TenDTC,
                        result[i].DiaChi,
                        result[i].ThoiGianLamViec,
                        result[i].Ten,
                        result[i].SDT,
                        actionOne +
                        actionTwo
                    ]).draw();
                }
            } else {
                $("#tblResult").DataTable().columns.adjust().draw();
            }
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi khi tìm kiếm");
        }
    });
}

function openPopup(idDTC, mode) {
    var popupWindow = window.open('/QuanLyDTC?idDTC=' + idDTC + '&mode=' + mode, 'mypopuptitle', 'width=' + screen.availWidth * 0.7 + ',height=' + screen.availHeight * 0.7);
    var timer = setInterval(function () {
        if (popupWindow.closed) {
            clearInterval(timer);
            searchDTC();
        }
    }, 500);
}

function requestDelete(idDTC, delFlag) {
    var message = "Bạn có chắc muốn xóa điểm tiêm chủng này?";
    if (delFlag == "0") {
        message = "Bạn có chắc muốn cập nhật điểm tiêm chủng này?";
    }
    if (confirm(message)) {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: {
                idDTC: idDTC,
                delFlag: delFlag,
            },
            contentType: "application/json; charset=utf-8",
            url: '/QuanLyDTC/DeleteDTCByDelFlag',
            success: function (jsonResult) {
                showSuccessAlert("Xóa điểm tiêm chủng thành công");
                searchDTC();
            },
            error: function () {
                showDangerAlert("Đã gặp lỗi khi xóa điểm tiêm chủng");
            }
        });
    }
}

function requestRemove(idDTC) {
    if (confirm("Bạn có chắc muốn xóa điểm tiêm chủng này?")) {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: {
                idDTC: idDTC
            },
            contentType: "application/json; charset=utf-8",
            url: '/QuanLyDTC/DeleteDTC',
            success: function (jsonResult) {
                showSuccessAlert("Xóa điểm tiêm chủng thành công");
                searchDTC();
            },
            error: function () {
                showDangerAlert("Đã gặp lỗi khi xóa điểm tiêm chủng");
            }
        });
    }
}