$(document).ready(function () {
    if (('' + window.location).toLocaleLowerCase().includes("baocaotonkho/create"))
        CreateBaoCaoTonKho();
    if (('' + window.location).toLocaleLowerCase().includes("baocaotonkho/details"))
        DetailsBaoCaoTonKho();
    if (('' + window.location).toLocaleLowerCase().includes("baocaotonkho/delete"))
        DetailsBaoCaoTonKho();
});
function CreateBaoCaoTonKho() {
    //basic button handler
    if($('#ngayLap').val() === '' || $('#ngayLap').val() === undefined || $('#ngayLap').length === 0)
        $('#ngayLap').val(new Date($.now()).toLocaleDateString('en-US'));
    var thang = (new Date($.now()).getMonth() + 1);
    var nam = new Date($.now()).getFullYear();
    $('#thangNam').val(nam + '-' + thang);
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/BaoCaoTonKho/LoadThongTinHangHoa', { thang: thang, nam: nam },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                $('#tongSoHangHoa').val(orderItems.length);
                GeneratedItemsTable();
            }
        });

    //Save button click function
    $('#submit').click(function () {
        //validation of inventory ballot detail
        var isAllValid = true;
        var value = $('#thangNam').val().split("-");
        var thang = value[1];
        var nam = value[0];
        //Save if valid
        if (isAllValid) {
            var data = {
                Thang: thang,
                Nam: nam,
                MaNguoiDung: $('#maNguoiDung').val().trim(),
                NgayLap: $('#ngayLap').val().trim(),
                TongSoHangHoa: $('#tongSoHangHoa').val().trim(),
                IsDeleted: false,
                ChiTietBaoCaoTonKhoes: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/BaoCaoTonKho/LuuBaoCaoTonKho",
                type: "POST",
                data: JSON.stringify(data),
                dataType: "JSON",
                contentType: "application/json",
                success: function (d) {
                    //check is successfully save to database
                    if (d.status == true) {
                        //will send status from server side
                        //clear form
                        orderItems = [];
                        $('#ngayLap').val(new Date($.now()).toLocaleDateString());
                        $('#tongSoHangHoa').val('');
                        $('#orderItems').empty();
                        thang = (new Date($.now()).getMonth() + 1);
                        nam = new Date($.now()).getFullYear();
                        $('#thangNam').val(nam + '-' + thang);
                        window.location.href = '/Manager/BaoCaoTonKho/';
                    }
                    else {
                        alert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu');
                }
            });
        }
    });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn Đầu</th><th>Số Lượng Nhập</th><th>Số Lượng Xuất</th><th>Số Lượng Tồn Cuối</th><th>Tình Trạng Hàng Hóa</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuongTonDau));
                $row.append($('<td/>').html(val.SoLuongNhap));
                $row.append($('<td/>').html(val.SoLuongXuat));
                $row.append($('<td/>').html(val.SoLuongTonCuoi));
                $row.append($('<td/>').html(val.TinhTrangHangHoa));
                $tbody.append($row);
            });
            $table.append($tbody);
            $('#orderItems').html($table);
        }
        else {
            $('#orderItems').html('');
        }
    }

    //on change
    $('#thangNam').on("change", function () {
        var value = $('#thangNam').val().split("-");
        var thang = value[1];
        var nam = value[0];
        $.getJSON('/BaoCaoTonKho/LoadThongTinHangHoa', { thang: thang, nam: nam },
            function (data) {
                if (data != null) {
                    orderItems = JSON.parse(data);
                    $('#tongSoHangHoa').val(orderItems.length);
                    GeneratedItemsTable();
                }
            });
    });
}
function DetailsBaoCaoTonKho() {
    //basic button handler
    var orderItems = [];

    $.getJSON('/BaoCaoTonKho/LoadChiTietBaoCaoTonKho', { id: $('#maBaoCaoTonKho').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
            }
        });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered table2excel"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn Đầu</th><th>Số Lượng Nhập</th><th>Số Lượng Xuất</th><th>Số Lượng Tồn Cuối</th><th>Tình Trạng Hàng Hóa</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuongTonDau));
                $row.append($('<td/>').html(val.SoLuongNhap));
                $row.append($('<td/>').html(val.SoLuongXuat));
                $row.append($('<td/>').html(val.SoLuongTonCuoi));
                $row.append($('<td/>').html(val.TinhTrangHangHoa));
                $tbody.append($row);
            });
            $table.append($tbody);
            $('#orderItems').html($table);
        }
        else {
            $('#orderItems').html('');
        }
    }

    $(function () {
        $(".exportToExcel").click(function (e) {
            var tb = document.getElementById('orderItems');
            var table = $(tb).find('#productTable');
            console.log(table);
            if (table && table.length) {
                var preserveColors = (table.hasClass('table2excel_with_colors') ? true : false);
                $(table).table2excel({
                    exclude: ".noExl",
                    name: "Excel Document Name",
                    filename: "BaoCaoTonKhoThang" + $('#thangNam').text().trim() + ".xls",
                    fileext: ".xls",
                    exclude_img: true,
                    exclude_links: true,
                    exclude_inputs: true,
                    preserveColors: preserveColors
                });
            }
        });

    });
}
// function only enter number
function checkNumber(e, element) {
    var charcode = (e.which) ? e.which : e.keyCode;
    //Check number
    if (charcode > 31 && (charcode < 48 || charcode > 57)) {
        return false;
    }
    return true;
}

//hidden error when user enter into textbox productID
function HideErrorProductName() {
    if (document.getElementById('tenHangHoa').value != '') {
        $('#tenHangHoa').siblings('span.error').css('visibility', 'hidden');
    }
}

function ClearValue() {
    $("#productName").val('');
    $("#unitName").val('');
    $("#currentQuantity").val('');

    $('#productID').siblings('span.error').css('visibility', 'hidden');
    $('#productName').siblings('span.error').css('visibility', 'hidden');
    $('#checkQuantity').siblings('span.error').css('visibility', 'hidden');
}

function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}

function HideErrorMaHangHoa() {
    if (document.getElementById('MaHangHoa').value != '') {
        $('#MaHangHoa').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorTenHangHoa() {
    if (document.getElementById('tenHangHoa').value != '') {
        $('#tenHangHoa').siblings('span.error').css('visibility', 'hidden');
    }
}
