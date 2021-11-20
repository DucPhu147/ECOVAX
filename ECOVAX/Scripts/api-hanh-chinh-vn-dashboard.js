getTinhThanhPhoData();
$(document).ready(function () {
    $("#ddlTinhThanhPho").select2({
        placeholder: "Tỉnh/Thành phố",
        allowClear: true
    });
    $("#ddlQuanHuyen").select2({
        placeholder: "Quận/Huyện",
        allowClear: true
    });
    $("#ddlPhuongXa").select2({
        placeholder: "Phường/Xã",
        allowClear: true
    });

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
                var newOption = new Option(resultList[i][1].name_with_type, resultList[i][1].code, false, false);
                $('#ddlTinhThanhPho').append(newOption);
            }
            $('#ddlTinhThanhPho').val(null).trigger('change');
        },
        beforeSend: function () {
        },
        complete: function () {
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
                    var newOption = new Option(resultList[i][1].name_with_type, resultList[i][1].code, false, false);
                    $('#ddlQuanHuyen').append(newOption);
                }
                $('#ddlQuanHuyen').val(null).trigger('change');
            },
            beforeSend: function () {
            },
            complete: function () {
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
                    var newOption = new Option(resultList[i][1].name_with_type, resultList[i][1].path_with_type, false, false);
                    $('#ddlPhuongXa').append(newOption);
                }
                $('#ddlPhuongXa').val(null).trigger('change');
            },
            beforeSend: function () {
            },
            complete: function () {
            }
        });
    } else {
        $('#ddlPhuongXa').prop('disabled', true);
    }
}