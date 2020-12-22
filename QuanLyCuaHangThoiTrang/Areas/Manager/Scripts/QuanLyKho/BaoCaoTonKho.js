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

    $('#print').click(function () {
        Print();
    });

    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%; text-align:center"/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn Đầu</th><th>Số Lượng Nhập</th><th>Số Lượng Xuất</th><th>Số Lượng Tồn Cuối</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.DonViTinh));
            $row.append($('<td/>').html(val.SoLuongTonDau));
            $row.append($('<td/>').html(val.SoLuongNhap));
            $row.append($('<td/>').html(val.SoLuongXuat));
            $row.append($('<td/>').html(val.SoLuongCuoi));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=600'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center"><img src="/Content/image/header.png" class="img-responsive watch-right"  /></p>')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Nhập Kho</p>')

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">')
        popupWin.document.write('Thông tin phiếu nhập kho');
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu nhập kho: ');
        popupWin.document.write($('#soBaoCaoTonKho').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ngày nhập: ');
        popupWin.document.write($('#ngayLap').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Nhà cung cấp: ');
        popupWin.document.write($('#nhacungcap').find("option:selected").text());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongTien').val().trim() + " VND");
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ghi chú: ');
        popupWin.document.write($('#ghiChu').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('</table>')

        popupWin.document.write('<br>');
        popupWin.document.write('Danh sách hàng hóa');
        popupWin.document.write(toPrint.innerHTML);

        popupWin.document.write('<p style="text-align:center">')
        popupWin.document.write('Nhân viên kho')
        popupWin.document.write('<br>')
        popupWin.document.write('(Ký tên)')
        popupWin.document.write('</p>')
        popupWin.document.write('</html>');
        popupWin.document.close();
    }

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
                chiTietBaoCaoTonKhoes: orderItems
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
                    $('#submit').val('Lưu Phiếu Nhập Kho');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Nhập Kho');
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

    $('#print').click(function () {
        Print();
    });

    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%; text-align:center"/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Nhập</th><th>Giá Nhập (VND)</th><th>Thành Tiền (VND)</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.DonViTinh));
            $row.append($('<td/>').html(val.SoLuong));
            $row.append($('<td/>').html(formatNumber(val.GiaKiem)));
            $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=600'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center"><img src="/Content/image/header.png" class="img-responsive watch-right"  /></p>')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Nhập Kho</p>')

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">')
        popupWin.document.write('Thông tin phiếu nhập kho');
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu nhập kho: ');
        popupWin.document.write($('#soBaoCaoTonKho').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ngày nhập: ');
        popupWin.document.write($('#ngayLap').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Nhà cung cấp: ');
        popupWin.document.write($('#nhacungcap').find("option:selected").text());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongTien').val().trim() + " VND");
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ghi chú: ');
        popupWin.document.write($('#ghiChu').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('</table>')

        popupWin.document.write('<br>');
        popupWin.document.write('Danh sách hàng hóa');
        popupWin.document.write(toPrint.innerHTML);

        popupWin.document.write('<p style="text-align:center">')
        popupWin.document.write('Nhân viên kho')
        popupWin.document.write('<br>')
        popupWin.document.write('(Ký tên)')
        popupWin.document.write('</p>')
        popupWin.document.write('</html>');
        popupWin.document.close();
    }

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
    console.log($('#thangNam').value);
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
