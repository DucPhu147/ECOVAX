$(document).ready(function () {
    searchVaccine();
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        }
    });
    $("#vaccineForm").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element);  // default placement for everything else
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
    $("#btnAdd").click(function () {
        if ($("#vaccineForm").valid()) {
            addVaccine();
        }
    });
});
function searchVaccine() {
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idDTC: $("#idDTC").val(),
        },
        contentType: "application/json; charset=utf-8",
        url: '/ThongTinVaccine/GetVaccine',
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    var delFlag = "";
                    action = "<a href='#' class='text-danger' onClick='requestDelete(\"" + result[i].IdVaccine + "\",1)' title='Xóa'><i class='fal fa-remove'></i></a>";

                    if (result[i].DeleteFlag == "1") {
                        delFlag = "<i class='fas fa-circle'></i>";
                        action = "<a href='#' class='text-success' onClick='requestDelete(\"" + result[i].IdVaccine + "\",0)' title='Khôi phục'><i class='fal fa-plus'></i></a>" +
                            "<a href='#' class='text-danger ml-3' onClick='requestRemove(\"" + result[i].IdVaccine + "\")' title='Xóa khỏi hệ thống'><i class='fal fa-trash'></i></a>";
                    }
                    //Gỡ hoàn toàn

                    $("#tblResult").DataTable().row.add([
                        result[i].IdVaccine,
                        result[i].TenVaccine,
                        result[i].ThoiHanTiem,
                        delFlag,
                        action
                    ]).draw();
                }
            } else {
                $("#tblResult").DataTable().columns.adjust().draw();
            }
        },
    });
}
function addVaccine() {
    var id = !$("#idVaccine").val() ? -1 : $("#idVaccine").val();
    if (id != -1) {
        var message = "Bạn có chắc muốn cập nhật vắc xin này?";
        if (!confirm(message)) {
            return;
        }
    }
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            id: id,
            ten: $("#tenVaccine").val(),
            thoiHanTiem: $("#thoiHanTiem").val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/ThongTinVaccine/AddVaccine',
        success: function (jsonResult) {
            if (id != -1) {
                showSuccessAlert("Cập nhật vắc xin thành công");
            } else {
                showSuccessAlert("Thêm vắc xin thành công");
            }
            searchVaccine();
        },
        error: function () {
            if (id != -1) {
                showDangerAlert("Đã gặp lỗi khi cập nhật vắc xin");
            } else {
                showDangerAlert("Đã gặp lỗi khi thêm vắc xin");
            }
        }
    });
}

function requestDelete(id, delFlag) {
    var message = "Bạn có chắc muốn xóa vắc xin này?";
    if (delFlag == 0) {
        message = "Bạn có chắc muốn cập nhật vắc xin này?";
    }
    if (confirm(message)) {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: {
                id: id,
                delFlag: delFlag
            },
            contentType: "application/json; charset=utf-8",
            url: '/ThongTinVaccine/DeleteVaccine',
            success: function (jsonResult) {
                if (delFlag == 0) {
                    showSuccessAlert("Cập nhật vắc xin thành công");
                } else {
                    showSuccessAlert("Xóa vắc xin thành công");
                }
                searchVaccine();
            },
            error: function () {
                if (delFlag == 0) {
                    showDangerAlert("Đã gặp lỗi khi cập nhật vắc xin");
                } else {
                    showDangerAlert("Đã gặp lỗi khi xóa vắc xin");
                }
            }
        });
    }
}
function requestRemove(id) {
    if (confirm("Bạn có chắc muốn xóa vắc xin này?")) {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: {
                id: id
            },
            contentType: "application/json; charset=utf-8",
            url: '/ThongTinVaccine/RemoveVaccine',
            success: function (jsonResult) {
                searchVaccine();
                showSuccessAlert("Xóa vắc xin thành công");
            },
            error: function () {
                showDangerAlert("Đã gặp lỗi khi xóa vắc xin");
            }
        });
    }
}