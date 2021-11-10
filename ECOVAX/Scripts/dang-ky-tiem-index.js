$(document).ready(function () {
    $("#registInfoForm").validate({
        errorPlacement: function (error, element) {
            if ($(element).is('select')) {
                element.next().after(error); // special placement for select elements
            } else {
                error.insertAfter(element);  // default placement for everything else
            }
        },
        rules: {
            TenNguoiLH: {
                required: true,
            },
            SdtNguoiLH: {
                required: true,
                digits: true,
                equalLength: 10
            },
            SdtNguoiDK: {
                digits: true,
                equalLength: 10
            },
            TenNguoiDK: {
                required: true,
            },
            NgaySinh: {
                required: true,
            },
            DiaChi: {
                required: true,
            },
            CMND: {
                required: true,
                digits: true,
                equalRangeLength: [9, 12],
            },
            Email: {
                email: true,
            },
            NhomUuTien: {
                required: true,
            },
            QuanHe: {
                required: true,
            },
            SoMui: {
                required: true
            },
            GioiTinh: {
                required: true,
            },
            NgayTiem: {
                greaterThanDate: new Date()
            },
            NgaySinh: {
                smallerThanDate: new Date()
            },
            TenVaccineMuiMot: {
                required: true,
            },
            NgayTiemMuiMot: {
                required: true,
                smallerThanDate: new Date()
            },
            DdlTinhThanhPho: {
                required: true
            },
            DdlQuanHuyen: {
                required: true
            },
            DdlPhuongXa: {
                required: true
            }
        },
        messages: {
            CMND: {
                equalRangeLength: "CMND/CCCD không đúng định dạng.",
                digits: "CMND/CCCD không đúng định dạng.",
            },
            SdtNguoiDK: {
                equalLength: "Số điện thoại không đúng định dạng.",
                digits: "Số điện thoại không đúng định dạng.",
            },
            SdtNguoiLH: {
                equalLength: "Số điện thoại không đúng định dạng.",
                digits: "Số điện thoại không đúng định dạng.",
            },
            NgayTiem: {
                greaterThanDate: "Ngày tiêm phải lớn hơn ngày hiện tại."
            },
            NgaySinh: {
                smallerThanDate: "Ngày sinh phải nhỏ hơn ngày hiện tại."
            },
            NgayTiemMuiMot: {
                smallerThanDate: "Ngày tiêm mũi một phải nhỏ hơn ngày hiện tại."
            },
            NhomUuTien: {
                required: "Trường này không được bỏ trống.",
            },
            QuanHe: {
                required: "Trường này không được bỏ trống.",
            },
            SoMui: {
                required: "Trường này không được bỏ trống."
            },
            GioiTinh: {
                required: "Trường này không được bỏ trống.",
            },
            TenVaccineMuiMot: {
                required: "Trường này không được bỏ trống.",
            },
            DdlTinhThanhPho: {
                required: "Trường này không được bỏ trống.",
            },
            DdlQuanHuyen: {
                required: "Trường này không được bỏ trống.",
            },
            DdlPhuongXa: {
                required: "Trường này không được bỏ trống.",
            }
        }
    });
    $("#btnBack").click(function () {
        window.history.back();
    });

    $("#btnReset").click(function () {
        $("#ddlNhomUuTien").val('default').selectpicker("refresh");
        $("#ddlQuanHe").val('default').selectpicker("refresh");
        $("#ddlGioiTinh").val('default').selectpicker("refresh");
        $("#ddlBuoiTiem").val('default').selectpicker("refresh");
        $("#ddlSoMui").val('default').selectpicker("refresh");
    });

    $('.selectpicker').on('hide.bs.select', function () {
        $(this).trigger("focusout");
    });

    //Trang sàng lọc
    $('input[type=radio]').each(function () {
        $(this).rules('add', {
            required: true,  // example rule
            // another rule, etc.
        });
    });
    $("#cbConfirm").change(function () {
        if (this.checked) {
            $("#btnSubmit").prop("disabled", false);
        } else {
            $("#btnSubmit").prop("disabled", true);
        }
    });

    $('#ddlSoMui').on('changed.bs.select', function (e) {
        var selected = $(e.currentTarget).val();
        if (selected === "2") {
            $("#firstVacInfoContainer").removeClass("d-none");
        } else {
            $("#firstVacInfoContainer").addClass("d-none");
        }
    });
});
