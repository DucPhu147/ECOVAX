$(document).ready(function () {
    searchGCN();
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
function searchGCN() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyGCN/GetGCN',
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    //UPDATE
                    var action = "<a href='#' onClick='openPopup(\"" + result[i].IdDTC + "\",2)' title='Chỉnh sửa'><i class='fal fa-edit'></i></a>";
                    $("#tblResult").DataTable().row.add([
                        result[i].IdGiayCN,
                        result[i].SoMui,
                        result[i].ThoiGianTiem,
                        result[i].TenVaccine,
                        result[i].LoVaccine,
                        result[i].Ten,
                        result[i].TenDTC,
                        action
                    ]).draw();
                }
            } else {
                $("#tblResult").DataTable().columns.adjust().draw();
            }
        },
    });
}