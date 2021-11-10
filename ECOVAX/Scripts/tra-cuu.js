
$(document).ready(function () {
    $("#btnTraCuuGDK").click(function () {
        getGDKItems();
    });
    $("#btnTraCuuGCN").click(function () {
        getGCNItems();
    });
    $("#btnTraCuuDTC").click(function () {
        getDTCItems();
    });
});
const pendingStatus = "Chờ xác nhận";
const successStatus = "Đã xác nhận";
const rejectStatus = "Không được xác nhận";

function getGDKItems() {
    windowLock();
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
                    let deleteHtml = "";
                    if (result[i].TrangThaiPD == pendingStatus) {
                        trangThaiPD = "<label class='text-warning'>" + result[i].TrangThaiPD + "</label>";
                        deleteHtml = "<a href='#' class='text-secondary font-weight-bold' onclick='confirmDelete(\"" + result[i].IdGiayDK + "\")'>Xóa</a>";
                    } else if (result[i].TrangThaiPD == rejectStatus) {
                        trangThaiPD = "<label class='text-danger'>" + result[i].TrangThaiPD + "</label>";
                    } else if (result[i].TrangThaiPD == successStatus) {
                        trangThaiPD = "<label class='text-success'>" + result[i].TrangThaiPD + "</label>";
                    }
                    $("#tblResult > tbody:last-child").append(
                        "<tr>" +
                        "<td>" + (i + 1) + "</td>" +
                        "<td>" + result[i].TenNguoiLH + "</td>" +
                        "<td>" + result[i].SDTNguoiLH + "</td>" +
                        "<td>" + result[i].TenNguoiDK + "</td>" +
                        "<td>" + result[i].SDTNguoiDK + "</td>" +
                        "<td>" + result[i].CMND + "</td>" +
                        "<td>" + result[i].ThoiGianDK + "</td>" +
                        "<td>" + trangThaiPD + "</td>" +
                        "<td>" + deleteHtml + "</td>" +
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
        },
        complete: function () {
            const timeOut = setTimeout(function () {
                windowUnlock();
                clearTimeout(timeOut);
            }, 200);
        }
    });
}

function getGCNItems() {
    windowLock();
    $.ajax({
        type: "GET",
        dataType: "json",
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
                        "<td>" + result[i].TenCongDan + "</td>" +
                        "<td>" + result[i].CMND + "</td>" +
                        "<td>" + result[i].TenVaccine + "</td>" +
                        "<td>" + result[i].NoiDung + "</td>" +
                        "<td>" + result[i].Ten + "</td>" +
                        "<td>" + result[i].ThoiGianTiem + "</td>" +
                        "<td>" + result[i].ThoiGianCap + "</td>" +
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
        },
        complete: function () {
            const timeOut = setTimeout(function () {
                windowUnlock();
                clearTimeout(timeOut);
            }, 200);
        }
    });
}


function getDTCItems() {
    windowLock();
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            tinhThanh: $("#ddlTinhThanhPho option:selected").text(),
            quanHuyen: $("#ddlQuanHuyen option:selected").text(),
            phuongXa: $("#ddlPhuongXa option:selected").text(),
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
        error: function () {
            showDangerAlert("Đã gặp lỗi khi tìm kiếm");
        },
        complete: function () {
            const timeOut = setTimeout(function () {
                windowUnlock();
                clearTimeout(timeOut);
            }, 200);
        }
    });
}
