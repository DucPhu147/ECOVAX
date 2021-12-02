
$(document).ready(function () {
    $("#btnTraCuuGDK").click(function () {
        if ($("#formTraCuu").valid()) {
            getGDKItems();
        }
    });
    $("#btnTraCuuGCN").click(function () {
        if ($("#formTraCuu").valid()) {
            getGCNItems();
        }
    });
    $("#btnTraCuuDTC").click(function () {
        getDTCItems();
    });

    $("#formTraCuu").validate({
        rules: {
            txtHoten: {
                required: true,
            },
            txtCMND: {
                required: true,
                digits: true,
                equalRangeLength: [9, 12],
            },
            txtSDT: {
                required: true,
                digits: true,
                equalLength: 10
            },
        },
        messages: {
            txtCMND: {
                equalRangeLength: "CMND/CCCD không đúng định dạng.",
                digits: "CMND/CCCD không đúng định dạng.",
            },
            txtSDT: {
                equalLength: "Số điện thoại không đúng định dạng.",
                digits: "Số điện thoại không đúng định dạng.",
            },
        }
    })
});
const pendingStatus = "Chờ xác nhận";
const successStatus = "Xác nhận";
const rejectStatus = "Từ chối";

function getGDKItems() {
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            cmnd: $("#txtCMND").val(),
            sdt: $("#txtSDT").val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/TraCuu/GetGiayDangKy',
        success: function (jsonResult) {
            $("#tblResult > tbody").empty();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                $("#tblResult").removeClass("d-none");
                $("#lblEmpty").addClass("d-none");
                for (let i = 0; i < result.length; i++) {
                    let trangThaiPD;
                    let actionHtml = "";
                    let thongTinTC = "<td></td><td></td><td></td>";
                    if (result[i].TrangThaiPD == pendingStatus) {
                        trangThaiPD = "<label class='text-warning font-weight-bold'>" + result[i].TrangThaiPD + "</label>";
                        actionHtml = "<a href='#' class='text-danger' onclick='confirmDelete(\"" + result[i].IdGiayDK + "\", \"" + result[i].UpdateTime + "\")'>" +
                            "<i class='fal fa-trash' title='Hủy giấy đăng ký'></i></a > ";
                    } else if (result[i].TrangThaiPD == rejectStatus) {
                        trangThaiPD = "<label class='text-danger font-weight-bold'>" + result[i].TrangThaiPD + "</label>";
                    } else if (result[i].TrangThaiPD == successStatus) {
                        thongTinTC = "<td class='font-weight-bold'>" + result[i].TenDTC + "</td> " +
                            "<td class='font-weight-bold'>" + result[i].NgayTiem + "</td>" +
                            "<td class='font-weight-bold'>" + result[i].BuoiTiem + "</td>";
                        trangThaiPD = "<label class='text-success font-weight-bold'>" + result[i].TrangThaiPD + "</label>";
                        actionHtml = "<a href='/Print?idGiayDK=" + result[i].IdGiayDK+"' class='text-primary' target='_blank'>" + 
                            "<i class='fal fa-print' title='In giấy đăng ký'></i></a > ";
                    }
                    $("#tblResult > tbody:last-child").append(
                        "<tr>" +
                        "<td>" + (i + 1) + "</td>" +
                        "<td>" + result[i].TenNguoiLH + "</td>" +
                        "<td>" + result[i].SDTNguoiLH + "</td>" +
                        "<td>" + result[i].TenNguoiDK + "</td>" +
                        "<td>" + result[i].ThoiGianDK + "</td>" + thongTinTC +
                        "<td>" + trangThaiPD + "</td>" +
                        "<td>" + actionHtml + "</td>" +
                        "</tr > "
                    );
                }
            } else {
                $("#tblResult").addClass("d-none");
                $("#lblEmpty").removeClass("d-none");
            }
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi khi tìm kiếm");
        }
    });
}

function getGCNItems() {
    $.ajax({
        type: "GET",
        dataType: "text json",
        data: {
            cmnd: $("#txtCMND").val(),
            ten: $("#txtHoTen").val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/TraCuu/GetGiayChungNhan',
        success: function (jsonResult) {
            $("#tblResult > tbody").empty();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                $("#tblResult").removeClass("d-none");
                $("#lblEmpty").addClass("d-none");
                for (let i = 0; i < result.length; i++) {
                    $("#tblResult > tbody:last-child").append(
                        "<tr>" +
                        "<td>" + (i + 1) + "</td>" +
                        "<td>" + result[i].Ten + "</td>" +
                        "<td>" + result[i].CMND + "</td>" +
                        "<td>" + result[i].TenVaccine + "</td>" +
                        "<td>" + result[i].LoVaccine + "</td>" +
                        "<td>" + result[i].SoMui + "</td>" +
                        "<td>" + result[i].TenDTC + "</td>" +
                        "<td>" + result[i].ThoiGianTiem + "</td>" +
                        "</tr > "
                    );
                }
            } else {
                $("#tblResult").addClass("d-none");
                $("#lblEmpty").removeClass("d-none");
            }
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi khi tìm kiếm");
        }
    });
}


function getDTCItems() {
    var tinhThanh = $("#ddlTinhThanhPho option:selected").text();
    var quanHuyen = $("#ddlQuanHuyen option:selected").text();
    var phuongXa = $("#ddlPhuongXa option:selected").text();
    $.ajax({
        type: "GET",
        dataType: "text json",
        data: {
            tinhThanh: tinhThanh,
            quanHuyen: quanHuyen,
            phuongXa: phuongXa,
        },
        contentType: "application/json; charset=utf-8",
        url: '/TraCuu/GetDiemTiemChung',
        success: function (jsonResult) {
            $("#tblResult > tbody").empty();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                $("#tblResult").removeClass("d-none");
                $("#lblEmpty").addClass("d-none");
                for (let i = 0; i < result.length; i++) {
                    $("#tblResult > tbody:last-child").append(
                        "<tr>" +
                        "<td>" + (i + 1) + "</td>" +
                        "<td>" + result[i].TenDTC + "</td>" +
                        "<td>" + result[i].DiaChi + "</td>" +
                        "<td>" + result[i].TenCanBo + "</td>" +
                        "<td>" + result[i].SDT + "</td>" +
                        "</tr > "
                    );
                }
            } else {
                $("#tblResult").addClass("d-none");
                $("#lblEmpty").removeClass("d-none");
            }
        },
        error: function (error) {
            showDangerAlert("Đã gặp lỗi khi tìm kiếm");
        }
    });
}

function confirmDelete(idGiayDK, updateTime) {
    if (confirm("Bạn có chắc chắn muốn hủy đơn đăng ký này?.")) {
        $.ajax({
            type: "GET",
            dataType: "text json",
            data: {
                idGDK: idGiayDK,
                updateTime: updateTime
            },
            contentType: "application/json; charset=utf-8",
            url: '/TraCuu/XoaGiayDangKy',
            success: function () {
                showSuccessAlert("Hủy thành công");
                getGDKItems();
            },
            error: function (error) {
                showDangerAlert("Đã gặp lỗi xảy ra khi xóa");
            }
        });
    }
}