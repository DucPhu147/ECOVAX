$(document).ready(function () {
    searchPhieuSangLoc();
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        },
    });
    $("#pslForm").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element);  // default placement for everything else
        },
        rules: {
            IdPSL: {
                digits: true,
            },
            TenPSL: {
                required: true
            }
        }
    });
    $("#btnAdd").click(function () {
        if ($("#pslForm").valid()) {
            addPhieuSangLoc();
        }
    });
});
function searchPhieuSangLoc() {
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idPSL: $("#idPSL").val(),
        },
        contentType: "application/json; charset=utf-8",
        url: '/DanhMucPhieuSangLoc/GetPhieuSangLoc',
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    var delFlag = "";
                    action = "<a href='#' class='text-danger' onClick='requestDelete(\"" + result[i].IdPhieuSangLoc + "\",1)' title='Xóa'><i class='fal fa-remove'></i></a>";

                    if (result[i].DeleteFlag == "1") {
                        delFlag = "<i class='fas fa-circle'></i>";
                        action = "<div class='d-inline-flex'><a href='#' class='text-success' onClick='requestDelete(\"" + result[i].IdPhieuSangLoc + "\",0)' title='Khôi phục'><i class='fal fa-plus'></i></a>" +
                            "<a href='#' class='text-danger ml-3' onClick='requestRemove(\"" + result[i].IdPhieuSangLoc + "\")' title='Xóa khỏi hệ thống'><i class='fal fa-trash'></i></a></div>";
                    }
                    //Gỡ hoàn toàn

                    $("#tblResult").DataTable().row.add([
                        result[i].IdPhieuSangLoc,
                        result[i].TenDanhMuc,
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
function addPhieuSangLoc() {
    var id = !$("#idPSL").val() ? -1 : $("#idPSL").val();
    if (id != -1) {
        var message = "Bạn có chắc muốn cập nhật danh mục này?";
        if (!confirm(message)) {
            return;
        }
    }
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            id: id,
            ten: $("#tenPSL").val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/DanhMucPhieuSangLoc/AddPhieuSangLoc',
        success: function (jsonResult) {
            if (id != -1) {
                showSuccessAlert("Cập nhật danh mục thành công");
            } else {
                showSuccessAlert("Thêm danh mục thành công");
            }
            searchPhieuSangLoc();
        },
        error: function () {
            if (id != -1) {
                showDangerAlert("Đã gặp lỗi khi cập nhật danh mục");
            } else {
                showDangerAlert("Đã gặp lỗi khi thêm danh mục");
            }
        }
    });
}

function requestDelete(id, delFlag) {
    var message = "Bạn có chắc muốn xóa danh mục này?";
    if (delFlag == 0) {
        message = "Bạn có chắc muốn cập nhật danh mục này?";
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
            url: '/DanhMucPhieuSangLoc/DeletePhieuSangLoc',
            success: function (jsonResult) {
                if (delFlag == 0) {
                    showSuccessAlert("Cập nhật danh mục thành công");
                } else {
                    showSuccessAlert("Xóa danh mục thành công");
                }
                searchPhieuSangLoc();
            },
            error: function () {
                if (delFlag == 0) {
                    showDangerAlert("Đã gặp lỗi khi cập nhật danh mục");
                } else {
                    showDangerAlert("Đã gặp lỗi khi xóa danh mục");
                }
            }
        });
    }
}
function requestRemove(id) {
    if (confirm("Bạn có chắc muốn xóa danh mục này?")) {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: {
                id: id
            },
            contentType: "application/json; charset=utf-8",
            url: '/DanhMucPhieuSangLoc/RemovePhieuSangLoc',
            success: function (jsonResult) {
                searchPhieuSangLoc();
                showSuccessAlert("Xóa danh mục thành công");
            },
            error: function () {
                showDangerAlert("Đã gặp lỗi khi xóa danh mục");
            }
        });
    }
}