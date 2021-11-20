$(document).ready(function () {
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        },
        scrollX: true,
        fixedColumns: {
            left: 0,
            right: 1
        },
    });
    $("#btnSearch").on('click', function () {
        search();
    });
    const timeOut = setTimeout(function () {
        search();
        clearTimeout(timeOut);
    }, 500);

    $("#ddlNhomUuTien").select2({
        placeholder: "Nhóm ưu tiên...",
        allowClear: true
    });
    getNhomUuTienItems();
});
function getNhomUuTienItems() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/Dashboard/GetNhomUuTien',
        success: function (jsonResult) {
            const resultList = JSON.parse(jsonResult);
            for (let i = 0; i < resultList.length; i++) {
                var newOption = new Option((i + 1) + ". " + resultList[i].Ten, resultList[i].Id, false, false);
                $('#ddlNhomUuTien').append(newOption);
            }
            $('#ddlNhomUuTien').val(null).trigger('change');
        },
        beforeSend: function () {
        },
        complete: function () {
        }
    });
}
function search() {
    var tinhThanh = $("#ddlTinhThanhPho option:selected").text();
    var quanHuyen = $("#ddlQuanHuyen option:selected").text();
    var phuongXa = $("#ddlPhuongXa option:selected").text();
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            dateFrom: $("#dateFrom").val(),
            dateTo: $("#dateTo").val(),
            tinhThanh: tinhThanh,
            quanHuyen: quanHuyen,
            phuongXa: phuongXa,
            nhomUuTien: $('#ddlNhomUuTien').val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/Dashboard/GetGDKPheDuyet',
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    $("#tblResult").DataTable().row.add([
                        result[i].IdGiayDK,
                        result[i].ThoiGianDK == null ? "" : new Date(result[i].ThoiGianDK).toLocaleDateString("en-US"),
                        result[i].TenNguoiLH,
                        result[i].SDTNguoiLH,
                        result[i].QuanHe,
                        result[i].TenNguoiDK,
                        result[i].SDTNguoiDK,
                        result[i].NgaySinh == null ? "" : new Date(result[i].NgaySinh).toLocaleDateString("en-US"),
                        result[i].GioiTinh,
                        result[i].DiaChi,
                        result[i].CMND,
                        result[i].Email,
                        result[i].NgheNghiep,
                        result[i].NhomUuTien,
                        result[i].TenVaccineMuiMot,
                        result[i].NgayTiemMuiMot == null ? "" : new Date(result[i].NgayTiemMuiMot).toLocaleDateString("en-US"),
                        result[i].NgayTiem == null ? "" : new Date(result[i].NgayTiem).toLocaleDateString("en-US"),
                        result[i].BuoiTiem,
                        "<a href='#' onClick='openPopup(\"" + result[i].IdGiayDK +"\")' title='Phê duyệt giấy'><i class='fal fa-edit'></i></a>"
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

function openPopup(idGiayDK) {
    window.open('/PheDuyetGDK?idGiayDK=' + idGiayDK, 'mypopuptitle', 'width=' + screen.availWidth + ',height=' + screen.availHeight);
}