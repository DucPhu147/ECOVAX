﻿$(document).ready(function () {
    getVaccine();
    $("#tblResult").DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.11.3/i18n/vi.json'
        }
    });
    $("#idDTC").change(function () {
        searchLoVaccine();
    })
    $("#loVaccineForm").validate({
        errorPlacement: function (error, element) {
            if ($(element).is('select')) {
                element.next().after(error); // special placement for select elements
            } else {
                error.insertAfter(element);  // default placement for everything else
            }
        },
        rules: {
            IdDTC: {
                required: true,
                digits: true,
            },
            SoLuong: {
                required: true,
                digits: true,
            },
            DdlVaccine: {
                required: true
            },
            LoVaccine: {
                required: true,
            }
        }
    });
    $("#btnAdd").click(function () {
        if ($("#loVaccineForm").valid()) {
            addLoVaccine();
        }
    });
    $("#ddlVaccine").select2({
        placeholder: "Chọn vắc xin...",
    });
});
function searchLoVaccine() {
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idDTC: $("#idDTC").val(),
        },
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyDTC/GetLoVaccine',
        success: function (jsonResult) {
            $("#tblResult").DataTable().clear();
            const result = JSON.parse(jsonResult);
            if (result.length > 0) {
                for (let i = 0; i < result.length; i++) {
                    $("#tblResult").DataTable().row.add([
                        result[i].TenVaccine,
                        result[i].LoVaccine,
                        result[i].SoLuong,
                        "<a href='#' class='text-danger' onClick='requestDelete(\"" + result[i].Id + "\")' title='Xóa'><i class='fal fa-remove'></i></a>"
                    ]).draw();
                }
            } else {
                $("#tblResult").DataTable().columns.adjust().draw();
            }
        },
    });
}
function addLoVaccine() {
    $.ajax({
        type: "GET",
        dataType: "json",
        data: {
            idDTC: $("#idDTC").val(),
            soLuong: $("#soLuong").val(),
            loVaccine: $("#loVaccine").val(),
            vaccine: $("#ddlVaccine option:selected").val()
        },
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyDTC/AddLoVaccine',
        success: function (jsonResult) {
            showSuccessAlert("Thêm lô vắc xin thành công");
            searchLoVaccine();
        },
        error: function () {
            showDangerAlert("Đã gặp lỗi khi thêm lô vắc xin");
        }
    });
}
function getVaccine() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/QuanLyDTC/GetVaccine',
        success: function (jsonResult) {
            const resultList = JSON.parse(jsonResult);
            for (let i = 0; i < resultList.length; i++) {
                var newOption = new Option(resultList[i].TenVaccine, resultList[i].IdVaccine, false, false);
                $('#ddlVaccine').append(newOption);
            }
            $('#ddlVaccine').val(null).trigger('change');
        },
        beforeSend: function () {
        },
        complete: function () {
        }
    });
}

function requestDelete(id) {
    if (confirm("Bạn có chắc muốn xóa lô vắc xin này?")) {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: {
                id: id
            },
            contentType: "application/json; charset=utf-8",
            url: '/QuanLyDTC/DeleteLoVaccine',
            success: function (jsonResult) {
                showSuccessAlert("Xóa lô vắc xin thành công");
                searchLoVaccine();
            },
            error: function () {
                showDangerAlert("Đã gặp lỗi khi xóa lô vắc xin");
            }
        });
    }
}