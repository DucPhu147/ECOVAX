$(document).ready(function () {
    search()
    search2();
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        },
        dom: 'Bfrtip',
        buttons: [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ]
    });
    $("#tblResult2").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        },
        dom: 'Bfrtip',
        buttons: [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ]
    });
    $("#btnSearch").click(function () {
        search();
    });
    $("#btnSearch2").click(function () {
        search2();
    });
});
function search() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/ThongKe/TKMuiDaTiem',
        data: {
            fromDate: $("#dateFrom").val(),
            toDate: $("#dateTo").val()
        },
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    if (result[i].SoMui == "1") {
                        result[i].SoMui = "Mũi thứ nhất";
                    } else {
                        result[i].SoMui = "Mũi tiếp theo";
                    }
                    $("#tblResult").DataTable().row.add([
                        result[i].TenVaccine,
                        result[i].SoMui,
                        result[i].SoLuong,
                    ]).draw();
                }
            } else {
                $("#tblResult").DataTable().columns.adjust().draw();
            }
        },
    });
}

function search2() {
    var tinhThanh = $("#ddlTinhThanhPho option:selected").text();
    var quanHuyen = $("#ddlQuanHuyen option:selected").text();
    var phuongXa = $("#ddlPhuongXa option:selected").text();
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/ThongKe/TKMuiDaTiem2',
        data: {
            fromDate: $("#dateFrom2").val(),
            toDate: $("#dateTo2").val(),
            tinhThanh: tinhThanh,
            quanHuyen: quanHuyen,
            phuongXa: phuongXa
        },
        success: function (jsonResult) {
            $("#tblResult2").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    $("#tblResult2").DataTable().row.add([
                        result[i].TenDTC,
                        result[i].DiaChi,
                        result[i].SoLuong,
                        result[i].SoLuongConLai,
                    ]).draw();
                }
            } else {
                $("#tblResult2").DataTable().columns.adjust().draw();
            }
        },
    });
}