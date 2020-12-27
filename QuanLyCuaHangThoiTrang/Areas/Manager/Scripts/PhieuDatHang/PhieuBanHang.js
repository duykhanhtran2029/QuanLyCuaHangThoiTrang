$(document).ready(function () {
    if (('' + window.location).toLocaleLowerCase().includes("phieubanhang/create"))
        CreatePhieuBanHang();
    if (('' + window.location).toLocaleLowerCase().includes("phieubanhang/edit"))
        EditPhieuBanHang();
    if (('' + window.location).toLocaleLowerCase().includes("phieubanhang/details"))
        DetailsPhieuBanHang();
    if (('' + window.location).toLocaleLowerCase().includes("phieubanhang/delete"))
        DetailsPhieuBanHang();
});
function CreatePhieuBanHang() {
    //basic button handler
    if ($('#ngayban-pbh').val() === '' || $('#ngayban-pbh').val() === undefined || $('#ngayban-pbh').length === 0)
        $('#ngayban-pbh').val(new Date($.now()).toLocaleDateString('en-US'));

    var orderItems = [];
    var tmpIndex = 0;//onchange product => tmpIndex = -1 , edit product => tmpIndex= i in [] 

    $.getJSON('/PhieuBanHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pbh').val() },
        function (data) {
            if (data != null) {
                $.each(data, function (index, row) {
                    $("#tenHangHoa-pbh").val(data.TenHangHoa);
                    $("#giamgia-pbh").val(data.GiamGia);
                    $("#size-pbh").val(data.Size);
                    $('#gia-pbh').val(data.GiaBan * (1 - data.GiamGia));
                    //
                    $('#tonkho-pbh').val(data.SoLuong);
                });
            }
        });

    $('#add-pbh').click(function () {
        var isValidItem = true;
        //check if item is valid(is selected) to show or hide warning
        if ($('#tenHangHoa-pbh').val() == '') {
            isValidItem = false;
            $('#productItemError').text('Chưa có hàng hóa nào được chọn!');
        }
        else {
            $('#productItemError').hide();
        }
        //check if so luong is valid:
        var errorQuantity = 0;
        errorQuantity = CheckEmptyForSoLuongNhap_pbh(errorQuantity);

        var errorQuantity1 = 0;
        errorQuantity1 = CheckQuantityForSoLuongNhap_pbh(errorQuantity1);


        var error = errorQuantity + errorQuantity1; //gia tri nhap vao valid hay ko

        if (isValidItem == true && error == 0) {

            var i, j;
            var string_value_product = $('#MaHangHoa-pbh').val().trim();

            var productID = string_value_product.slice(0, 10);

            if (orderItems.length >= 0) {
                var test = true; //flag to check if product is already added
                if (orderItems.length > 0) { // [] had atleast 1 product
                    var productIdOfTable = "";
                    var row = document.getElementById('productTable').rows.length;
                    if ($('#add-pbh').val() == "Lưu" && tmpIndex >= 0) {
                        orderItems.splice(tmpIndex, 1);//delete orderItems[tmpIndex] if tmpIndex>0 (when edit to readd the item again),  
                        $('#productItemError').hide();
                    }
                    else {//temp= -1 => just onchange mahanghoa
                        for (var i = 1; i < row; i++) {//check the [] if product is added or not to show or hide warning and set 'test' value
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

                if (test == true) { //if product is not already added then add to []
                    $('#MaHangHoa-pbh').siblings('span.error').css('visibility', 'hidden');
                    orderItems.push({
                        MaHangHoa: $('#MaHangHoa-pbh').val().trim(),
                        TenHangHoa: $("#tenHangHoa-pbh").val().trim(),
                        Size: $("#size-pbh").val().trim(),
                        SoLuong: parseInt($('#soLuong-pbh').val().trim()),
                        GiaBan: parseInt($('#gia-pbh').val().trim().replace(/,/gi, "")),
                        GiamGia: $("#giamgia-pbh").val().trim(),
                        ThanhTien: parseInt($('#soLuong-pbh').val().trim()) * parseInt($('#gia-pbh').val().trim().replace(/,/gi, "")),
                    });

                    //Clear fields
                    $('#MaHangHoa-pbh').focus().val('');
                    $('#tenHangHoa-pbh').val('');
                    $('#giamgia-pbh').val('');
                    $('#size-pbh').val('');
                    $('#soLuong-pbh').val('');
                    $('#gia-pbh').val('');
                    $('#thanhTien-pbh').val('');

                    GeneratedItemsTable();
                    SumTotalAmount();
                }
            }

        }
    });

    //Save button click function
    $('#submit').click(function () {
        //validation of inventory ballot detail. check if there is any HangHoa
        var isAllValid = true;
        if (orderItems.length == 0) {
            $('#orderItems').html('<span class="messageError" style="color:red;">Phải có ít nhất 1 hàng hóa</span>');
            isAllValid = false;
        }

        if ($('#tenkhachhang-pbh').val() == '') {
            isAllValid = false;
            $('#warning-tenkhachhang').html('<span class="messageError" style="color:red;">Phải nhập tên khách hàng</span>');
            $('#warning-tenkhachhang').show();
        }
        else {
            $('#warning-tenkhachhang').hide();
        }

        if ($('#sodienthoai-pbh').val() == '') {
            isAllValid = false;
            $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Phải nhập số điện thoại</span>');
            $('#warning-sodienthoai').show();
        }
        else {
            $('#warning-sodienthoai').hide();
            if ($('#sodienthoai-pbh').val().length < 7 || $('#sodienthoai-pbh').val().length > 11) {
                isAllValid = false;
                $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Số điện thoại không hợp lệ</span>');
                $('#warning-sodienthoai').show();
            }
        }


        //Save if valid
        if (isAllValid) {
            var data = {
                NgayBan: $('#ngayban-pbh').val().trim(),
                MaNguoiDung: $('#maNguoiDung-pbh').val().trim(),
                TenKhachHang: $('#tenkhachhang-pbh').val().trim(),
                SoDienThoai: $('#sodienthoai-pbh').val().trim(),
                GhiChu: $('#ghiChu-pbh').val().trim(),
                TongTien: parseFloat($('#tongtien-pbh').val().trim().replace(/,/gi, "")),
                IsDeleted: false,
                chiTietPhieuBanHangs: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuBanHang/LuuPhieuBanHang",
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
                        $('#ngayban-pbh').val(new Date($.now()).toLocaleDateString());
                        $('#tenkhachhang-pbh').val('');
                        $('#sodienthoai-pbh').val('');
                        $('#ghiChu-pbh').val('');
                        $('#tongtien-pbh').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuBanHang/';
                    }
                    else {
                        alert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Bán Hàng');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Bán Hàng');
                }
            });
        }
    });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng</th><th>Giá (Đã áp dụng giảm giá)</th><th>Giảm Giá</th><th>Thành Tiền</th><th> Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.Size));
                $row.append($('<td/>').html(val.SoLuong));
                $row.append($('<td/>').html(formatNumber(val.GiaBan)));
                $row.append($('<td/>').html(val.GiamGia));
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
                        $('#tongtien-pbh').val(0);
                    } else {
                        SumTotalAmount();
                    }
                    ClearValue();
                });
                $edit.click(function (e) {
                    e.preventDefault();
                    console.log(orderItems[i]);
                    $('#add-pbh').val("Lưu");
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
            amount = document.getElementById("productTable").rows[i].cells[6].innerHTML.replace(/,/gi, "");

            total += parseFloat(amount);
        }
        $('#tongtien-pbh').val(formatNumber(parseFloat(total)));
    }

    //on change
    $('#MaHangHoa-pbh').on("change", function () {
        $.getJSON('/PhieuBanHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pbh').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa-pbh").val(data.TenHangHoa);
                        $("#giamgia-pbh").val(data.GiamGia);
                        $("#size-pbh").val(data.Size);
                        $('#gia-pbh').val(data.GiaBan * (1 - data.GiamGia));
                        //
                        $('#tonkho-pbh').val(data.SoLuong);
                    });
                }
            });
        tmpIndex = -1;
        $('#add-pbh').val("Thêm");
        //reset input
        $("#soLuong-pbh").val('');
        $("#thanhTien-pbh").val(0);
    });

    //this calculates values automatically
    MultiplicaPBH();
    $("#soLuong-pbh").on("keydown keyup", function () {
        MultiplicaPBH();
    });

    //$("#gia-pbh").on("keydown keyup", function () {
    //    MultiplicaPDH();
    //});
    // paste
    $("#soLuong-pbh").on('keyup input propertychange paste change', function () {
        CheckEmptyForSoLuongNhap_pbh();
        CheckQuantityForSoLuongNhap_pbh();
    });

    //$("#gia-pbh").on('keyup input propertychange paste change', function () {
    //    CheckEmptyForGiaBan();
    //    CheckQuantityForGiaBan();
    //});

    // replace data from oderItem
    function Replace(data) {
        $('#MaHangHoa-pbh').val(data.MaHangHoa);
        $("#tenHangHoa-pbh").val(data.TenHangHoa);
        $('#size-pbh').val(data.Size);
        $('#soLuong-pbh').val(data.SoLuong);
        $('#gia-pbh').val(data.GiaBan); //gia da ap dung giam gia
        $('#giamgia-pbh').val(data.GiamGia);
        var unitPrice = $('#gia-pbh').val().replace(/,/gi, "");
        var quantity = $('#soLuong-pbh').val();
        var result = parseFloat(unitPrice) * parseFloat(quantity);
        if (!isNaN(result)) {
            $('#thanhTien-pbh').val(formatNumber(result));
        }
    }
}
function EditPhieuBanHang() {
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;
    $.getJSON('/PhieuBanHang/LoadChiTietPhieuBanHang', { id: $('#soPhieuBanHang').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
                $('#tongtien-pbh').val(formatNumber(parseFloat($('#tongtien-pbh').val())));
            }
        });

    $('#add-pbh').click(function () {
        var isValidItem = true;

        if ($('#tenHangHoa-pbh').val() == '') {
            isValidItem = false;
            $('#productItemError').text('Chưa có hàng hóa nào được chọn!');
        }
        else {
            $('#productItemError').hide();
        }

        var errorQuantity = 0;
        errorQuantity = CheckEmptyForSoLuongNhap_pdh(errorQuantity);

        var errorQuantity1 = 0;
        errorQuantity1 = CheckQuantityForSoLuongNhap_pdh(errorQuantity1);


        var error = errorQuantity + errorQuantity1; //gia tri nhap vao valid hay ko

        if (isValidItem == true && error == 0) {

            var i, j;
            var string_value_product = $('#MaHangHoa-pbh').val().trim();

            var productID = string_value_product.slice(0, 10);

            if (orderItems.length >= 0) {
                var test = true;
                if (orderItems.length > 0) {
                    var productIdOfTable = "";
                    var row = document.getElementById('productTable').rows.length;
                    if ($('#add-pbh').val() == "Lưu" && tmpIndex >= 0) {
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
                if (test == true) { //if product is not already added then add to [] or when edit
                    $('#MaHangHoa-pbh').siblings('span.error').css('visibility', 'hidden');
                    orderItems.push({
                        MaHangHoa: $('#MaHangHoa-pbh').val().trim(),
                        TenHangHoa: $("#tenHangHoa-pbh").val().trim(),
                        Size: $("#size-pbh").val().trim(),
                        SoLuong: parseInt($('#soLuong-pbh').val().trim()),
                        GiaBan: parseInt($('#gia-pbh').val().trim().replace(/,/gi, "")),
                        GiamGia: $("#giamgia-pbh").val().trim(),
                        ThanhTien: parseInt($('#soLuong-pbh').val().trim()) * parseInt($('#gia-pbh').val().trim().replace(/,/gi, "")),
                    });

                    //Clear fields
                    $('#MaHangHoa-pbh').focus().val('');
                    $('#tenHangHoa-pbh').val('');
                    $('#giamgia-pbh').val('');
                    $('#size-pbh').val('');
                    $('#soLuong-pbh').val('');
                    $('#gia-pbh').val('');
                    $('#thanhTien-pbh').val('');

                    GeneratedItemsTable();
                    SumTotalAmount();
                }
            }

        }
    });

    //Save button click function
    $('#submit').click(function () {
        //validation of inventory ballot detail
        var isAllValid = true;

        if ($('#tenkhachhang-pbh').val() == '') {
            isAllValid = false;
            $('#warning-tenkhachhang').html('<span class="messageError" style="color:red;">Phải nhập tên khách hàng</span>');
            $('#warning-tenkhachhang').show();
        }
        else {
            $('#warning-tenkhachhang').hide();
        }

        if ($('#sodienthoai-pbh').val() == '') {
            isAllValid = false;
            $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Phải nhập số điện thoại</span>');
            $('#warning-sodienthoai').show();
        }
        else {
            $('#warning-sodienthoai').hide();
            if ($('#sodienthoai-pbh').val().length < 7 || $('#sodienthoai-pbh').val().length > 11) {
                isAllValid = false;
                $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Số điện thoại không hợp lệ</span>');
                $('#warning-sodienthoai').show();
            }
        }


        //Save if valid
        if (isAllValid) {
            var data = {
                SoPhieuBanHang: $('#soPhieuBanHang').val().trim(),
                Ngayban: $('#ngayban-pbh').val().trim(),
                MaNguoiDung: $('#maNguoiDung-pbh').val().trim(),
                TenKhachHang: $('#tenkhachhang-pbh').val().trim(),
                SoDienThoai: $('#sodienthoai-pbh').val().trim(),
                GhiChu: $('#ghiChu-pbh').val().trim(),
                TongTien: parseFloat($('#tongtien-pbh').val().trim().replace(/,/gi, "")),
                IsDeleted: false,
                chiTietPhieuBanHangs: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuBanHang/SuaPhieuBanHang",
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
                        $('#ngayban-pbh').val(new Date($.now()).toLocaleDateString());
                        $('#tenkhachhang-pbh').val('');
                        $('#sodienthoai-pbh').val('');
                        $('#ghiChu-pbh').val('');
                        $('#tongtien-pbh').prop('checked', false);;
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuBanHang/';
                    }
                    else {
                        alert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Bán Hàng');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Bán Hàng');
                }
            });
        }
    });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng</th><th>Giá (Đã áp dụng giảm giá)</th><th>Giảm Giá</th><th>Thành Tiền</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.Size));
                $row.append($('<td/>').html(val.SoLuong));
                $row.append($('<td/>').html(formatNumber(val.GiaBan)));
                $row.append($('<td/>').html(val.GiamGia));
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

    function SumTotalAmount() {
        var amount;

        var total = 0.0;
        var row = document.getElementById('productTable').rows.length;
        for (var i = 1; i < row; i++) {
            amount = document.getElementById("productTable").rows[i].cells[6].innerHTML.replace(/,/gi, "");

            total += parseFloat(amount);
        }
        $('#tongtien-pbh').val(formatNumber(parseFloat(total)));
    }

    //on change
    //$('#MaHangHoa-pbh').on("change", function () {
    //    $.getJSON('/PhieuBanHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pbh').val() },
    //        function (data) {
    //            if (data != null) {
    //                $.each(data, function (index, row) {
    //                    $("#tenHangHoa-pbh").val(data.TenHangHoa);
    //                    $("#giamgia-pbh").val(data.GiamGia);
    //                    $("#size-pbh").val(data.Size);
    //                    $('#gia-pbh').val(data.GiaBan * (1 - data.GiamGia));
    //                    //
    //                    $('#tonkho-pbh').val(data.SoLuong);
    //                });
    //            }
    //        });
    //    tmpIndex = -1;
    //    $('#add-pbh').val("Thêm");
    //    //reset input
    //    $("#soLuong-pbh").val('');
    //    $("#thanhTien-pbh").val(0);
    //});

    //this calculates values automatically
    MultiplicaPBH();
    //$("#soLuong-pbh").on("keydown keyup", function () {
    //    MultiplicaPBH();
    //});

    //$("#gia-pbh").on("keydown keyup", function () {
    //    MultiplicaPBH();
    //});
    // paste
    //$("#soLuong-pbh").on('keyup input propertychange paste change', function () {
    //    CheckEmptyForSoLuongNhap_pbh();
    //    CheckQuantityForSoLuongNhap_pbh();
    //});

    //$("#gia-pbh").on('keyup input propertychange paste change', function () {
    //    CheckEmptyForGiaBan();
    //    CheckQuantityForGiaBan();
    //});

    // replace data from oderItem
    //function Replace(data) {
    //    $('#MaHangHoa-pbh').val(data.MaHangHoa);
    //    $("#tenHangHoa-pbh").val(data.TenHangHoa);
    //    $('#size-pbh').val(data.Size);
    //    $('#soLuong-pbh').val(data.SoLuong);
    //    $('#gia-pbh').val(data.GiaBan); //
    //    $('#giamgia-pbh').val(data.GiamGia);
    //    var unitPrice = $('#gia-pbh').val().replace(/,/gi, "");
    //    var quantity = $('#soLuong-pbh').val();
    //    var result = parseFloat(unitPrice) * parseFloat(quantity);
    //    if (!isNaN(result)) {
    //        $('#thanhTien-pbh').val(formatNumber(result));
    //    }
    //}
}
function DetailsPhieuBanHang() {
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/PhieuBanHang/LoadChiTietPhieuBanHang', { id: $('#soPhieuBanHang').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
                $('#tongtien-pbh').text(formatNumber(parseFloat($('#tongtien-pbh').text())));
            }
        });
    $('#print').click(function () {
        Print();
    });

    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%;padding:10px;margin-top: 5px; text-align:center "/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng</th><th>Giá (Đã áp dụng giảm giá)</th><th>Giảm Giá</th><th>Thành Tiền</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.Size));
            $row.append($('<td/>').html(val.SoLuong));
            $row.append($('<td/>').html(formatNumber(val.GiaBan)));
            $row.append($('<td/>').html(formatNumber(val.GiamGia)));
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

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Đặt Hàng</p>')

        popupWin.document.write('<b>');
        popupWin.document.write('Thông tin phiếu đặt hàng');
        popupWin.document.write('</b>');
        popupWin.document.write('<table style="border:solid; width:100%; padding: 10px;margin-top: 5px">')
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu đặt hàng: ');
        popupWin.document.write($('#soPhieuDatHang').val().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ngày đặt: ');
        popupWin.document.write($('#ngaydat-pbh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Tên khách hàng: ');
        popupWin.document.write($('#tenkhachhang-pbh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số điện thoại: ');
        popupWin.document.write($('#sodienthoai-pbh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Địa chỉ: ');
        popupWin.document.write($('#diachi-pbh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Email: ');
        popupWin.document.write($('#email-pbh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongtien-pbh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Hình thức thanh toán: ');
        popupWin.document.write($('#hinhthucthanhtoan-pbh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ghi chú: ');
        popupWin.document.write($('#ghiChu-pbh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Ngày giao: ');
        popupWin.document.write($('#ngaygiao-pbh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Trạng thái xác nhận: ');
        popupWin.document.write($('#daxacnhan-pbh').is(":checked").trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Trạng thái thanh toán: ');
        popupWin.document.write($('#dathanhtoan-pbh').is(":checked").trim());
        popupWin.document.write('</td>')
        popupWin.document.write('</tr>')

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
        popupWin.document.write('Nhân viên đặt hàng')
        popupWin.document.write('<br>')
        popupWin.document.write('(Ký tên)')
        popupWin.document.write('</p>')
        popupWin.document.write('</html>');
        popupWin.document.close();
    }

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng</th><th>Giá (Đã áp dụng giảm giá)</th><th>Giảm Giá</th><th>Thành Tiền</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.Size));
                $row.append($('<td/>').html(val.SoLuong));
                $row.append($('<td/>').html(formatNumber(val.GiaBan)));
                $row.append($('<td/>').html(formatNumber(val.GiamGia)));
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

//hidden error when user enter into textbox productID
function HideErrorProductName_pbh() {
    checkNumber_pdh
    if (document.getElementById('tenHangHoa-pbh').value != '') {
        $('#tenHangHoa-pbh').siblings('span.error').css('visibility', 'hidden');
    }
}


function HideErrorMaHangHoa_pbh() {
    if (document.getElementById('MaHangHoa-pbh').value != '') {
        $('#MaHangHoa-pbh').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorTenHangHoa_pbh() {
    if (document.getElementById('tenHangHoa-pbh').value != '') {
        $('#tenHangHoa-pbh').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorSoLuongNhap_pbh() {
    if (document.getElementById('soLuongNhap-pbh').value != '') {
        $('#soLuong-pbh').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorGiaBan_pbh() {
    if (document.getElementById('gia-pbh').value != '') {
        $('#gia-pbh').siblings('span.error').css('visibility', 'hidden');
    }
}

function MultiplicaPBH() {
    if (document.getElementById('soLuong-pbh').value == '' || document.getElementById('gia-pbh').value == 0) {
        document.getElementById('thanhTien-pbh').value = 0;
    }
    else {
        var unitPrice = document.getElementById('gia-pbh').value.replace(/,/gi, "");
        var quantity = document.getElementById('soLuong-pbh').value;
        var result = parseFloat(unitPrice) * parseFloat(quantity);
        if (!isNaN(result)) {
            document.getElementById('thanhTien-pbh').value = formatNumber(result);
        }
    }
}

// check quantity input
function CheckEmptyForSoLuongNhap_pbh(error) {
    if (!($('#soLuong-pbh').val().trim() != '' && !isNaN($('#soLuong-pbh').val().trim()))) {
        $(".messageErrorinputQuantity").text("Nhập số lượng!");
        $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
        $("#soLuong-pbh").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity").addClass("hidden");
        $("#soLuong-pbh").removeClass("error");
    }
    $("#soLuong-pbh").blur(function () {
        $("#soLuong-pbh").val($("#soLuong-pbh").val().trim());
    });
    return error;
}


function CheckQuantityForSoLuongNhap_pbh(error) {
    var soluongtonkho = $('#tonkho-pbh').val().trim();

    if (parseInt($('#soLuong-pbh').val()) > soluongtonkho) {
        $(".messageErrorinputQuantity_tonkho").text("Vượt quá số lượng hàng trong kho!");
        $(".notifyinputQuantity_tonkho").slideDown(250).removeClass("hidden");
        $("#soLuong-pbh").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity_tonkho").addClass("hidden");
        $("#soLuong-pbh").removeClass("error");
    }

    if (($('#soLuong-pbh').val().trim() == '0') || ($('#soLuong-pbh').val().trim() == '00') || ($('#soLuong-pbh').val().trim() == '000') || ($('#soLuong-pbh').val().trim() == '0000')) {
        $(".messageErrorinputQuantity1").text("Nhập số lượng lớn hơn 0!");
        $(".notifyinputQuantity1").slideDown(250).removeClass("hidden");
        $("#soLuong-pbh").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity1").addClass("hidden");
        $("#soLuong-pbh").removeClass("error");
    }
    $("#soLuong-pbh").blur(function () {
        $("#soLuong-pbh").val($("#soLuong-pbh").val().trim());
    });
    return error;
}

