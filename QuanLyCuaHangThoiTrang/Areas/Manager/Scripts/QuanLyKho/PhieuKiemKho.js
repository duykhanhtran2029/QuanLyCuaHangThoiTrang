$(document).ready(function () {
    if (('' + window.location).toLocaleLowerCase().includes("phieukiemkho/create"))
        CreatePhieuKiemKho();
    if (('' + window.location).toLocaleLowerCase().includes("phieukiemkho/edit"))
        EditPhieuKiemKho();
    if (('' + window.location).toLocaleLowerCase().includes("phieukiemkho/details"))
        DetailsPhieuKiemKho();
    if (('' + window.location).toLocaleLowerCase().includes("phieukiemkho/delete"))
        DetailsPhieuKiemKho();
});
function CreatePhieuKiemKho() {
    //basic button handler
    if($('#ngayKiemKho').val() === '' || $('#ngayKiemKho').val() === undefined || $('#ngayKiemKho').length === 0)
        $('#ngayKiemKho').val(new Date($.now()).toLocaleDateString('en-US'));
    console.log($('#ngayKiemKho').val());
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/PhieuKiemKho/LoadThongTinHangHoa', { id: $('#MaHangHoa').val() },
        function (data) {
            if (data != null) {
                $.each(data, function (index, row) {
                    $("#tenHangHoa").val(data.TenHangHoa);
                    $("#donViTinh").val(data.DonViTinh);
                    $("#soLuongHienTai").val(data.SoLuongHienTai);
                    $("#size").val(data.Size);
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
        if (parseInt($('#soLuongKiemTra').val().trim()) > $('#soLuongHienTai').val()) {
            isValidItem = false;
            $('#productItemError').text('Số lượng kiểm tra không được lớn hơn số lượng tồn!');
            $('#productItemError').show();
        }
        else
            $('#productItemError').hide();

        var errorQuantity = 0;
        errorQuantity = CheckEmptyForSoLuongKiem(errorQuantity);

        var errorQuantity1 = 0;
        errorQuantity1 = CheckQuantityForSoLuongKiem(errorQuantity1);
        var errorEmpty = 0;
        errorEmpty = CheckEmptyForTinhTrang(errorEmpty);
        var error = errorQuantity + errorQuantity1 + errorEmpty;

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
                        DonViTinh: $("#donViTinh").val().trim(),
                        Size: $('#size').val().trim(),
                        SoLuongHienTai: parseInt($('#soLuongHienTai').val().trim()),
                        SoLuongKiemTra: parseInt($('#soLuongKiemTra').val().trim()),
                        TinhTrangHangHoa: $('#tinhTrang').val().trim(),
                    });

                    //Clear fields
                    $('#MaHangHoa').focus().val('');
                    $('#tenHangHoa').val('');
                    $('#donViTinh').val('');
                    $('#size').val('');
                    $('#soLuongHienTai').val('');
                    $('#soLuongKiemTra').val('');
                    $('#tinhTrang').val('');

                    GeneratedItemsTable();
                }
            }

        }
    });

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
                NgayKiemKho: $('#ngayKiemKho').val().trim(),
                MaNguoiDung: $('#maNguoiDung').val().trim(),
                GhiChu: $('#ghiChu').val().trim(),
                IsDeleted: false,
                ChiTietPhieuKiemKhoes: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuKiemKho/LuuPhieuKiemKho",
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
                        $('#ngayKiemKho').val(new Date($.now()).toLocaleDateString());
                        $('#ghiChu').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuKiemKho/';
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
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn</th><th>Số Lượng Kiểm Tra</th><th>Tình Trạng</th><th> Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuongHienTai));
                $row.append($('<td/>').html(val.SoLuongKiemTra));
                $row.append($('<td/>').html(val.TinhTrangHangHoa));
                var $remove = $('<input type="button" value="Xóa" style="padding:1px 5px; margin: 0px 3px" class="btn-danger"/>');
                var $edit = $('<input type="button" value="Sửa" style="padding:1px 5px; margin: 0px 3px" class="btn-primary"/>');
                var $action = $('<div/>');
                $action.append($remove);
                $action.append($edit);
                $remove.click(function (e) {
                    e.preventDefault();
                    orderItems.splice(i, 1);
                    GeneratedItemsTable();
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

    //on change
    $('#MaHangHoa').on("change", function () {
        $.getJSON('/PhieuKiemKho/LoadThongTinHangHoa', { id: $('#MaHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(data.TenHangHoa);
                        $("#donViTinh").val(data.DonViTinh);
                        $("#size").val(data.Size);
                        $("#soLuongHienTai").val(data.SoLuongHienTai);
                    });
                    console.log(data);
                }
            });
        tmpIndex = -1;
        $('#add').val("Thêm");
    });

    // paste
    $("#soLuongKiemTra").on('keyup input propertychange paste change', function () {
        CheckEmptyForSoLuongKiem();
        CheckQuantityForSoLuongKiem();
    });

    // replace data from oderItem
    function Replace(data) {
        $('#MaHangHoa').val(data.MaHangHoa);
        $("#tenHangHoa").val(data.TenHangHoa);
        $('#donViTinh').val(data.DonViTinh);
        $('#size').val(data.Size);
        $('#soLuongHienTai').val(data.SoLuongHienTai);
        $('#soLuongKiemTra').val(data.SoLuongKiemTra);
        $('#tinhTrang').val(data.TinhTrangHangHoa);
    }
}
function EditPhieuKiemKho() {
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;
    $.getJSON('/PhieuKiemKho/LoadChiTietPhieuKiemKho', { id: $('#soPhieuKiemKho').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                console.log(orderItems);
                GeneratedItemsTable();
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
        if (parseInt($('#soLuongKiemTra').val().trim()) > $('#soLuongHienTai').val()) {
            isValidItem = false;
            $('#productItemError').text('Số lượng kiểm tra không được lớn hơn số lượng tồn!');
            $('#productItemError').show();
        }
        else
            $('#productItemError').hide();

        var errorQuantity = 0;
        errorQuantity = CheckEmptyForSoLuongKiem(errorQuantity);

        var errorQuantity1 = 0;
        errorQuantity1 = CheckQuantityForSoLuongKiem(errorQuantity1);
        var errorEmpty = 0;
        errorEmpty = CheckEmptyForTinhTrang(errorEmpty);
        var error = errorQuantity + errorQuantity1 + errorEmpty;

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
                        DonViTinh: $("#donViTinh").val().trim(),
                        Size: $('#size').val().trim(),
                        SoLuongHienTai: parseInt($('#soLuongHienTai').val().trim()),
                        SoLuongKiemTra: parseInt($('#soLuongKiemTra').val().trim()),
                        TinhTrangHangHoa: $('#tinhTrang').val().trim(),
                    });

                    //Clear fields
                    $('#MaHangHoa').focus().val('');
                    $('#tenHangHoa').val('');
                    $('#donViTinh').val('');
                    $('#size').val('');
                    $('#soLuongHienTai').val('');
                    $('#soLuongKiemTra').val('');
                    $('#tinhTrang').val('');

                    GeneratedItemsTable();
                }
            }

        }
    });

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
                SoPhieuKiemKho: $('#soPhieuKiemKho').val().trim(),
                NgayKiemKho: $('#ngayKiemKho').val().trim(),
                MaNguoiDung: $('#maNguoiDung').val().trim(),
                GhiChu: $('#ghiChu').val().trim(),
                IsDeleted: false,
                ChiTietPhieuKiemKhoes: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuKiemKho/SuaPhieuKiemKho",
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
                        $('#ngayKiemKho').val(new Date($.now()).toLocaleDateString());
                        $('#ghiChu').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuKiemKho/';
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
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn</th><th>Số Lượng Kiểm Tra</th><th>Tình Trạng</th><th> Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuongHienTai));
                $row.append($('<td/>').html(val.SoLuongKiemTra));
                $row.append($('<td/>').html(val.TinhTrangHangHoa));
                var $remove = $('<input type="button" value="Xóa" style="padding:1px 5px; margin: 0px 3px" class="btn-danger"/>');
                var $edit = $('<input type="button" value="Sửa" style="padding:1px 5px; margin: 0px 3px" class="btn-primary"/>');
                var $action = $('<div/>');
                $action.append($remove);
                $action.append($edit);
                $remove.click(function (e) {
                    e.preventDefault();
                    orderItems.splice(i, 1);
                    GeneratedItemsTable();
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

    //on change
    $('#MaHangHoa').on("change", function () {
        $.getJSON('/PhieuKiemKho/LoadThongTinHangHoa', { id: $('#MaHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(data.TenHangHoa);
                        $("#donViTinh").val(data.DonViTinh);
                        $("#size").val(data.Size);
                        $("#soLuongHienTai").val(data.SoLuongHienTai);
                    });
                    console.log(data);
                }
            });
        tmpIndex = -1;
        $('#add').val("Thêm");
    });

    // paste
    $("#soLuongKiemTra").on('keyup input propertychange paste change', function () {
        CheckEmptyForSoLuongKiem();
        CheckQuantityForSoLuongKiem();
    });

    // replace data from oderItem
    function Replace(data) {
        $('#MaHangHoa').val(data.MaHangHoa);
        $("#tenHangHoa").val(data.TenHangHoa);
        $('#donViTinh').val(data.DonViTinh);
        $('#size').val(data.Size);
        $('#soLuongHienTai').val(data.SoLuongHienTai);
        $('#soLuongKiemTra').val(data.SoLuongKiemTra);
        $('#tinhTrang').val(data.TinhTrangHangHoa);
    }
}
function DetailsPhieuKiemKho() {
    //basic button handler
    var orderItems = [];
    $.getJSON('/PhieuKiemKho/LoadChiTietPhieuKiemKho', { id: $('#soPhieuKiemKho').val() },
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
        var $table = $('<table id="productTables" style="border: solid; width:100%;padding:10px;margin-top: 5px; text-align:center "/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn</th><th>Số Lượng Kiểm Tra</th><th>Tình Trạng</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.DonViTinh));
            $row.append($('<td/>').html(val.SoLuongHienTai));
            $row.append($('<td/>').html(val.SoLuongKiemTra));
            $row.append($('<td/>').html(val.TinhTrangHangHoa));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=600'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center"><img src="/images/header.png" class="img-responsive watch-right"  /></p>')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Kiểm Kho</p>')

        popupWin.document.write('<b>');
        popupWin.document.write('Thông tin phiếu kiểm kho');
        popupWin.document.write('</b>');
        popupWin.document.write('<table style="border:solid; width:100%; padding: 10px;margin-top: 5px">')
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu kiểm kho: ');
        popupWin.document.write($('#soPhieuKiemKho').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ngày kiểm kho: ');
        popupWin.document.write($('#ngayKiemKho').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung').text().trim());
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

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Tồn</th><th>Số Lượng Kiểm Tra</th><th>Tình Trạng</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuongHienTai));
                $row.append($('<td/>').html(val.SoLuongKiemTra));
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

function HideErrorSoLuongKiem() {
    if (document.getElementById('soLuongKiemTra').value != '') {
        $('#soLuongKiemTra').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorGiaKiem() {
    if (document.getElementById('giaKiem').value != '') {
        $('#giaKiem').siblings('span.error').css('visibility', 'hidden');
    }
}

// check quantity input
function CheckEmptyForSoLuongKiem(error) {
    if (!($('#soLuongKiemTra').val().trim() != '' && !isNaN($('#soLuongKiemTra').val().trim()))) {
        $(".messageErrorinputQuantity").text("Nhập số lượng!");
        $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
        $("#soLuongKiemTra").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity").addClass("hidden");
        $("#soLuongKiemTra").removeClass("error");
    }
    $("#soLuongKiemTra").blur(function () {
        $("#soLuongKiemTra").val($("#soLuongKiemTra").val().trim());
    });
    return error;
}

function CheckQuantityForSoLuongKiem(error) {
    if (($('#soLuongKiemTra').val().trim() == '0') || ($('#soLuongKiemTra').val().trim() == '00') || ($('#soLuongKiemTra').val().trim() == '000') || ($('#soLuongKiemTra').val().trim() == '0000')) {
        $(".messageErrorinputQuantity1").text("Nhập số lượng lớn hơn 0!");
        $(".notifyinputQuantity1").slideDown(250).removeClass("hidden");
        $("#soLuongKiemTra").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity1").addClass("hidden");
        $("#soLuongKiemTra").removeClass("error");
    }
    $("#soLuongKiemTra").blur(function () {
        $("#soLuongKiemTra").val($("#soLuongKiemTra").val().trim());
    });
    return error;
}

function CheckEmptyForTinhTrang(error) {
    if (!$('#tinhTrang').val().trim()) {
        $(".messageErrorinputEmpty").text("Nhập tình trạng!");
        $(".notifyinputEmpty").slideDown(250).removeClass("hidden");
        $("#tinhTrang").addClass("error");
        error++;
    }
    else {
        $(".notifyinputEmpty").addClass("hidden");
        $("#tinhTrang").removeClass("error");
    }
    $("#tinhTrang").blur(function () {
        $("#tinhTrang").val($("#tinhTrang").val().trim());
    });
    return error;
}

