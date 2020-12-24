$(document).ready(function () {
    if (('' + window.location).toLocaleLowerCase().includes("phieuxuatkho/create")) {
        CreatePhieuXuatKho();
    }
        
    if (('' + window.location).toLocaleLowerCase().includes("phieuxuatkho/edit")) {
        EditPhieuXuatKho();
    }

    if (('' + window.location).toLocaleLowerCase().includes("phieuxuatkho/details")) {
        DetailsPhieuXuatKho();
    }

});
function CreatePhieuXuatKho() {
    //basic button handler
    if($('#ngayXuatKho').val() === '' || $('#ngayXuatKho').val() === undefined || $('#ngayXuatKho').length === 0)
        $('#ngayXuatKho').val(new Date($.now()).toLocaleDateString('en-US'));
    console.log($('#ngayXuatKho').val());
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/PhieuXuatKho/LoadThongTinHangHoa', { id: $('#MaHangHoa').val() },
        function (data) {
            if (data != null) {
                $.each(data, function (index, row) {
                    $("#tenHangHoa").val(data.TenHangHoa);
                    $("#size").val(data.Size);
                    $("#soLuongTon").val(data.SoLuongTon);
                    $("#gia").val(data.Gia);
                });
            }
        });

    $('#add').click(function () {
        var isValidItem = true;

        if ($('#tenHangHoa').val() == '') {
            isValidItem = false;
            $('#productItemError').text('Chưa có hàng hóa nào được chọn!');
        }
        else {
            $('#productItemError').hide();
        }

        if (parseInt($('#soLuongXuat').val().trim()) > $('#soLuongTon').val())
        {
            isValidItem = false;
            $('#productItemError').text('Số lượng xuất không được lớn hơn số lượng tồn!');
            $('#productItemError').show();
        }
        else
            $('#productItemError').hide();

        var errorQuantity = 0;
        errorQuantity = CheckEmptyForSoLuongXuat(errorQuantity);

        var errorQuantity1 = 0;
        errorQuantity1 = CheckQuantityForSoLuongXuat(errorQuantity1);


        var error = errorQuantity  + errorQuantity1;

        if (isValidItem == true && error == 0) {

            var i, j;
            var string_value_product = $('#MaHangHoa').val().trim();

            var productID = string_value_product.slice(0, 10);

            if (orderItems.length >= 0) {
                var test = true;
                if (orderItems.length > 0) {
                    var productIdOfTable = "";
                    var row = document.getElementById('productTable').rows.length;
                    if ($('#add').val() == "Lưu" && tmpIndex >= 0) {
                        orderItems.splice(tmpIndex, 1);
                        $('#productItemError').hide();
                    }
                    else {
                        for (var i = 1; i < row; i++) {
                            productIdOfTable = document.getElementById("productTable").rows[i].cells[0].innerHTML;
                            if (productIdOfTable == productID) {
                                test = false;
                                $('#productItemError').text('Hàng hóa đã được thêm trước đó!');
                                $('#productItemError').show();
                                break;
                            }
                            else $('#productItemError').hide();
                        }
                    }
                }
                if (test == true) {
                    $('#MaHangHoa').siblings('span.error').css('visibility', 'hidden');
                    orderItems.push({
                        MaHangHoa: $('#MaHangHoa').val().trim(),
                        TenHangHoa: $("#tenHangHoa").val().trim(),
                        Size: $('#size').val().trim(),
                        SoLuongTon: parseInt($('#soLuongTon').val().trim()),
                        SoLuong: parseInt($('#soLuongXuat').val().trim()),
                        Gia: parseInt($('#gia').val().trim().replace(/,/gi, "")),
                        ThanhTien: parseInt($('#soLuongXuat').val().trim()) * parseInt($('#gia').val().trim().replace(/,/gi, "")),
                    });

                    //Clear fields
                    $('#MaHangHoa').focus().val('');
                    $('#tenHangHoa').val('');
                    $('#size').val('');
                    $('#soLuongTon').val('');
                    $('#soLuongXuat').val('');
                    $('#gia').val('');
                    $('#thanhTien').val('');

                    GeneratedItemsTable();
                    SumTotalAmount();
                }
            }

        }
    });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng Xuất</th><th>Giá</th><th>Thành Tiền</th><th> Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.Size));
                $row.append($('<td/>').html(val.SoLuong));
                $row.append($('<td/>').html(formatNumber(val.Gia)));
                $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
                var $remove = $('<input type="button" value="Xóa" style="padding:1px 5px; margin: 0px 5px" class="btn-danger"/>');
                var $edit = $('<input type="button" value="Sửa" style="padding:1px 5px; margin: 0px 5px" class="btn-primary"/>');
                var $action = $('<div/>');
                $action.append($remove);
                $action.append($edit);
                $remove.click(function (e) {
                    e.preventDefault();
                    orderItems.splice(i, 1);
                    GeneratedItemsTable();
                    if (orderItems.length == 0) {
                        $('#tongTien').val(0);
                    } else {
                        SumTotalAmount();
                    }
                    ClearValue();
                });
                $edit.click(function (e) {
                    e.preventDefault();
                    console.log(orderItems[i]);
                    $('#add').val("Lưu");
                    Replace(orderItems[i]);
                    tmpIndex = i;
                });
                $row.append($('<td width="14%"/>').html($action));
                $tbody.append($row);
            });
            $table.append($tbody);
            $('#orderItems').html($table);
        }
        else {
            $('#orderItems').html('');
        }
    }

    function SumTotalAmount() {
        var amount;

        var total = 0.0;
        var row = document.getElementById('productTable').rows.length;
        for (var i = 1; i < row; i++) {
            amount = document.getElementById("productTable").rows[i].cells[5].innerHTML.replace(/,/gi, "");

            total += parseFloat(amount);
        }
        $('#tongTien').val(formatNumber(parseFloat(total)));
    }

    function Replace(data) {
        $('#MaHangHoa').val(data.MaHangHoa);
        $("#tenHangHoa").val(data.TenHangHoa);
        $('#size').val(data.Size);
        $('#soLuongTon').val(data.SoLuongTon);
        $('#soLuongXuat').val(data.SoLuong);
        $('#gia').val(data.Gia);
        var unitPrice = $('#gia').val().replace(/,/gi, "");
        var quantity = $('#soLuongXuat').val();
        var result = parseInt(unitPrice) * parseInt(quantity);
        if (!isNaN(result)) {
            $('#thanhTien').val(formatNumber(result));
        }
    }

    //Save button click function
    $('#submit').click(function () {
        //validation of inventory ballot detail
        var isAllValid = true;
        if (orderItems.length == 0) {
            $('#orderItems').html('<span class="messageError" style="color:red;">Phải có ít nhất 1 hàng hóa</span>');
            isAllValid = false;
        }

        //Save if valid
        if (isAllValid) {
            var data = {
                NgayXuatKho: $('#ngayXuatKho').val().trim(),
                MaNguoiDung: $('#maNguoiDung').val().trim(),
                LyDoXuat: $('#lyDoXuat').val().trim(),
                TongTien: parseFloat($('#tongTien').val().trim().replace(/,/gi, "")),
                IsDeleted: false,
                ChiTietPhieuXuatKhoes: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuXuatKho/LuuPhieuXuatKho",
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
                        $('#ngayXuatKho').val(new Date($.now()).toLocaleDateString());
                        $('#lyDoXuat').val('');
                        $('#tongTien').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuXuatKho/';
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

    //on change
    $('#MaHangHoa').on("change", function () {
        $.getJSON('/PhieuXuatKho/LoadThongTinHangHoa', { id: $('#MaHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(data.TenHangHoa);
                        $("#size").val(data.Size);
                        $("#soLuongTon").val(data.SoLuongTon);
                        $("#gia").val(data.Gia);
                    });
                }
            });
        tmpIndex = -1;
        $('#add').val("Thêm");
    });

    //this calculates values automatically
    MultiplicaPXK();
    $("#soLuongXuat").on("keydown keyup", function () {
        MultiplicaPXK();
    });

    $("#gia").on("keydown keyup", function () {
        MultiplicaPXK();
    });
    // paste
    $("#soLuongXuat").on('keyup input propertychange paste change', function () {
        CheckEmptyForSoLuongXuat();
        CheckQuantityForSoLuongXuat();
    });

    $("#gia").on('keyup input propertychange paste change', function () {
        CheckEmptyForGiaXuat();
        CheckQuantityForGiaXuat();
    });
}
function EditPhieuXuatKho() {
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/PhieuXuatKho/LoadChiTietPhieuXuatKho', { id: $('#soPhieuXuatKho').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
                $('#tongTien').val(formatNumber(parseFloat($('#tongTien').val())));
                console.log($('#tongTien').val());
            }
        });

    $('#print').click(function () {
        Print();
    });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng Xuất</th><th>Giá</th><th>Thành Tiền</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.Size));
                $row.append($('<td/>').html(val.SoLuong));
                $row.append($('<td/>').html(formatNumber(val.Gia)));
                $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
                $tbody.append($row);
            });
            $table.append($tbody);
            $('#orderItems').html($table);
        }
        else {
            $('#orderItems').html('');
        }
    }

    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%;padding:10px;margin-top: 5px; text-align:center "/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Nhập</th><th>Giá Nhập (VND)</th><th>Thành Tiền (VND)</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.DonViTinh));
            $row.append($('<td/>').html(val.SoLuong));
            $row.append($('<td/>').html(formatNumber(val.GiaNhap)));
            $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=600'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center"><img src="/images/header.png" class="img-responsive watch-right"  /></p>')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Nhập Kho</p>')

        popupWin.document.write('<b>');
        popupWin.document.write('Thông tin phiếu nhập kho');
        popupWin.document.write('</b>');
        popupWin.document.write('<table style="border:solid; width:100%; padding: 10px;margin-top: 5px">')
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu nhập kho: ');
        popupWin.document.write($('#soPhieuNhapKho').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ngày nhập: ');
        popupWin.document.write($('#ngayNhapKho').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Nhà cung cấp: ');
        popupWin.document.write($('#nhaCungCap').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongTien').text().trim() + " VND");
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ghi chú: ');
        popupWin.document.write($('#ghiChu').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('</table>')

        popupWin.document.write('<br/>');
        popupWin.document.write('<b>');
        popupWin.document.write('Danh sách hàng hóa');
        popupWin.document.write('</b>');
        popupWin.document.write(toPrint.innerHTML);

        popupWin.document.write('<p style="text-align:right; padding-right: 50px">')
        popupWin.document.write('Thủ Đức, Ngày .... Tháng .... Năm ....')
        popupWin.document.write('<br>')
        popupWin.document.write('</p>')
        popupWin.document.write('<p style="text-align:center;float: right;margin-right: 125px;margin-top: -10px;">')
        popupWin.document.write('Nhân viên kho')
        popupWin.document.write('<br>')
        popupWin.document.write('(Ký tên)')
        popupWin.document.write('</p>')
        popupWin.document.write('</html>');
        popupWin.document.close();
    }
}
function DetailsPhieuXuatKho() {
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/PhieuXuatKho/LoadChiTietPhieuXuatKho', { id: $('#soPhieuXuatKho').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
                $('#tongTien').text(formatNumber(parseFloat($('#tongTien').text())));
            }
        });

    $('#print').click(function () {
        Print();
    });
    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%;padding:10px;margin-top: 5px; text-align:center "/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng Xuất</th><th>Giá (VND)</th><th>Thành Tiền (VND)</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.Size));
            $row.append($('<td/>').html(val.SoLuong));
            $row.append($('<td/>').html(formatNumber(val.Gia)));
            $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=600'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center"><img src="/images/header.png" class="img-responsive watch-right"  /></p>')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Xuất Kho</p>')

        popupWin.document.write('<b>');
        popupWin.document.write('Thông tin phiếu xuất kho');
        popupWin.document.write('</b>');
        popupWin.document.write('<table style="border:solid; width:100%; padding: 10px;margin-top: 5px">')
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu xuất kho: ');
        popupWin.document.write($('#soPhieuXuatKho').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ngày xuất kho: ');
        popupWin.document.write($('#ngayXuatKho').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Lý do xuất: ');
        popupWin.document.write($('#lyDoXuat').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongTien').text().trim() + " VND");
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('</td></tr>')

        popupWin.document.write('</table>')

        popupWin.document.write('<br/>');
        popupWin.document.write('<b>');
        popupWin.document.write('Danh sách hàng hóa');
        popupWin.document.write('</b>');
        popupWin.document.write(toPrint.innerHTML);

        popupWin.document.write('<p style="text-align:right; padding-right: 50px">')
        popupWin.document.write('Thủ Đức, Ngày .... Tháng .... Năm ....')
        popupWin.document.write('<br>')
        popupWin.document.write('</p>')
        popupWin.document.write('<p style="text-align:center;float: right;margin-right: 125px;margin-top: -10px;">')
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
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng Xuất</th><th>Giá</th><th>Thành Tiền</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.Size));
                $row.append($('<td/>').html(val.SoLuong));
                $row.append($('<td/>').html(formatNumber(val.Gia)));
                $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
                $tbody.append($row);
            });
            $table.append($tbody);
            $('#orderItems').html($table);
        }
        else {
            $('#orderItems').html('');
        }
    }
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

function HideErrorSoLuongXuat() {
    if (document.getElementById('soLuongXuat').value != '' && document.getElementById('soLuongXuat').value <= $('#soLuongTon').val()) {
        $('#soLuongXuat').siblings('span.error').css('visibility', 'hidden');
    }
}

function MultiplicaPXK() {
    if (document.getElementById('soLuongXuat').value == '' || document.getElementById('gia').value == 0) {
        document.getElementById('thanhTien').value = 0;
    }
    else {
        var unitPrice = document.getElementById('gia').value.replace(/,/gi, "");
        var quantity = document.getElementById('soLuongXuat').value;
        var result = parseInt(unitPrice) * parseInt(quantity);
        if (!isNaN(result)) {
            document.getElementById('thanhTien').value = formatNumber(result);
        }
    }
}

// check quantity input
function CheckEmptyForSoLuongXuat(error) {
    if (!($('#soLuongXuat').val().trim() != '' && !isNaN($('#soLuongXuat').val().trim()))) {
        $(".messageErrorinputQuantity").text("Nhập số lượng!");
        $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
        $("#soLuongXuat").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity").addClass("hidden");
        $("#soLuongXuat").removeClass("error");
    }
    $("#soLuongXuat").blur(function () {
        $("#soLuongXuat").val($("#soLuongXuat").val().trim());
    });
    return error;
}

function CheckQuantityForSoLuongXuat(error) {
    if (($('#soLuongXuat').val().trim() == '0') || ($('#soLuongXuat').val().trim() == '00') || ($('#soLuongXuat').val().trim() == '000') || ($('#soLuongXuat').val().trim() == '0000')) {
        $(".messageErrorinputQuantity1").text("Nhập số lượng lớn hơn 0!");
        $(".notifyinputQuantity1").slideDown(250).removeClass("hidden");
        $("#soLuongXuat").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity1").addClass("hidden");
        $("#soLuongXuat").removeClass("error");
    }
    $("#soLuongXuat").blur(function () {
        $("#soLuongXuat").val($("#soLuongXuat").val().trim());
    });
    return error;
}
