getTinhThanhPhoData();
$(document).ready(function () {
    $('#ddlTinhThanhPho').on('change', function () {
        getQuanHuyenData(this.value);
    });
    $('#ddlQuanHuyen').on('change', function () {
        getPhuongXaData(this.value);
    });
});
function getTinhThanhPhoData() {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '../Content/hanh-chinh-vn/tinh-thanhpho/tinh_tp.json',
        success: function (jsonResult) {
            var resultList = Object.entries(jsonResult);
            resultList.sort((a, b) => (a[1].name > b[1].name) ? 1 : -1);
            for (let i = 0; i < resultList.length; i++) {
                $('#ddlTinhThanhPho').append($('<option>', {
                    value: resultList[i][1].code,
                    text: resultList[i][1].name_with_type
                }));
            }
        }
    });
}

function getQuanHuyenData(codeTinhThanh) {
    $('#ddlQuanHuyen').empty();
    if (codeTinhThanh != '') {
        $('#ddlQuanHuyen').prop('disabled', false);
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: '../Content/hanh-chinh-vn/quan-huyen/' + codeTinhThanh + '.json',
            success: function (jsonResult) {
                var resultList = Object.entries(jsonResult);
                resultList.sort((a, b) => (a[1].name_with_type < b[1].name_with_type) ? 1 : -1);

                for (let i = 0; i < resultList.length; i++) {
                    $('#ddlQuanHuyen').append($('<option>', {
                        value: resultList[i][1].code,
                        text: resultList[i][1].name_with_type
                    }));
                }
                $('#ddlQuanHuyen').selectpicker("refresh");
            }
        });
    } else {
        $('#ddlQuanHuyen').prop('disabled', true);
    }
}

function getPhuongXaData(codeQuanHuyen) {
    $('#ddlPhuongXa').empty();
    if (codeQuanHuyen != '') {
        $('#ddlPhuongXa').prop('disabled', false);
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: '../Content/hanh-chinh-vn/phuong-xa/' + codeQuanHuyen + '.json',
            success: function (jsonResult) {
                var resultList = Object.entries(jsonResult);
                resultList.sort((a, b) => (a[1].name > b[1].name) ? 1 : -1);

                for (let i = 0; i < resultList.length; i++) {
                    $('#ddlPhuongXa').append($('<option>', {
                        value: resultList[i][1].path_with_type,
                        text: resultList[i][1].name_with_type
                    }));
                }
                $('#ddlPhuongXa').selectpicker("refresh");
            }
        });
    } else {
        $('#ddlPhuongXa').prop('disabled', true);
    }
}