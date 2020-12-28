$(document).ready(function () {
    if (('' + window.location).toLocaleLowerCase().includes("phieudathang/create"))
        CreatePhieuDatHang();
    if (('' + window.location).toLocaleLowerCase().includes("phieudathang/edit"))
        EditPhieuDatHang();
    if (('' + window.location).toLocaleLowerCase().includes("phieudathang/details"))
        DetailsPhieuDatHang();
    if (('' + window.location).toLocaleLowerCase().includes("phieudathang/delete"))
        DetailsPhieuDatHang();
});
function CreatePhieuDatHang() {
    //basic button handler
    if ($('#ngaydat-pdh').val() === '' || $('#ngaydat-pdh').val() === undefined || $('#ngaydat-pdh').length === 0)
        $('#ngaydat-pdh').val(new Date($.now()).toLocaleDateString('en-US'));
    if ($('#ngaygiao-pdh').val() === '' || $('#ngaygiao-pdh').val() === undefined || $('#ngaygiao-pdh').length === 0)
        $('#ngaygiao-pdh').val(new Date($.now()).toLocaleDateString('en-US'));
    //default: Thanh Toan Khi Nhan
    $('#hinhthucthanhtoan-pdh').val('Thanh Toán Khi Nhận');

    var orderItems = [];
    var tmpIndex = 0;//onchange product => tmpIndex = -1 , edit product => tmpIndex= i in [] 

    $.getJSON('/PhieuDatHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pdh').val() },
        function (data) {
            if (data != null) {
                $.each(data, function (index, row) {
                    $("#tenHangHoa-pdh").val(data.TenHangHoa);
                    $("#giamgia-pdh").val(data.GiamGia);
                    $("#size-pdh").val(data.Size);
                    $('#gia-pdh').val(data.GiaBan * (1 - data.GiamGia));
                    //
                    $('#tonkho-pdh').val(data.SoLuong);
                });
            }
        });

    $('#add-pdh').click(function () {
        var isValidItem = true;
        //check if item is valid(is selected) to show or hide warning
        if ($('#tenHangHoa-pdh').val() == '') {
            isValidItem = false;
            $('#productItemError').text('Chưa có hàng hóa nào được chọn!');
        }
        else {
            $('#productItemError').hide();
        }
        //check if so luong is valid:
        var errorQuantity = 0;
        errorQuantity = CheckEmptyForSoLuongNhap_pdh(errorQuantity);

        var errorQuantity1 = 0;
        errorQuantity1 = CheckQuantityForSoLuongNhap_pdh(errorQuantity1);


        var error = errorQuantity + errorQuantity1; //gia tri nhap vao valid hay ko
        
        if (isValidItem == true && error == 0) {

            var i, j;
            var string_value_product = $('#MaHangHoa-pdh').val().trim();

            var productID = string_value_product.slice(0, 10); 

            if (orderItems.length >= 0) {
                var test = true; //flag to check if product is already added
                if (orderItems.length > 0) { // [] had atleast 1 product
                    var productIdOfTable = "";
                    var row = document.getElementById('productTable').rows.length;
                    if ($('#add-pdh').val() == "Lưu" && tmpIndex >= 0) {
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
                    $('#MaHangHoa-pdh').siblings('span.error').css('visibility', 'hidden');
                    orderItems.push({
                        MaHangHoa: $('#MaHangHoa-pdh').val().trim(),
                        TenHangHoa: $("#tenHangHoa-pdh").val().trim(),
                        Size: $("#size-pdh").val().trim(),
                        SoLuong: parseInt($('#soLuong-pdh').val().trim()),
                        GiaBan: parseInt($('#gia-pdh').val().trim().replace(/,/gi, "")),
                        GiamGia: $("#giamgia-pdh").val().trim(),
                        ThanhTien: parseInt($('#soLuong-pdh').val().trim()) * parseInt($('#gia-pdh').val().trim().replace(/,/gi, "")),
                    });

                    //Clear fields
                    $('#MaHangHoa-pdh').focus().val('');
                    $('#tenHangHoa-pdh').val('');
                    $('#giamgia-pdh').val('');
                    $('#size-pdh').val('');
                    $('#soLuong-pdh').val('');
                    $('#gia-pdh').val('');
                    $('#thanhTien-pdh').val('');

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

        if ($('#tenkhachhang-pdh').val() == '') {
            isAllValid = false;
            $('#warning-tenkhachhang').html('<span class="messageError" style="color:red;">Phải nhập tên khách hàng</span>');
            $('#warning-tenkhachhang').show();
        }
        else
        {
            $('#warning-tenkhachhang').hide();
        }

        if ($('#sodienthoai-pdh').val() == '') {
            isAllValid = false;
            $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Phải nhập số điện thoại</span>');
            $('#warning-sodienthoai').show();
        }
        else
        {
            $('#warning-sodienthoai').hide();
            if ($('#sodienthoai-pdh').val().length < 7 || $('#sodienthoai-pdh').val().length > 11)
            {
                isAllValid = false;
                $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Số điện thoại không hợp lệ</span>');
                $('#warning-sodienthoai').show();
            }
        }

        if ($('#diachi-pdh').val() == '') {
            isAllValid = false;
            $('#warning-diachi').html('<span class="messageError" style="color:red;">Phải nhập địa chỉ giao hàng</span>');
            $('#warning-diachi').show();
        }
        else
        {
            $('#warning-diachi').hide();
        }

        //Save if valid
        if (isAllValid) {
            var data = {
                NgayDat: $('#ngaydat-pdh').val().trim(),
                MaNguoiDung: $('#maNguoiDung-pdh').val().trim(),
                TenKhachHang: $('#tenkhachhang-pdh').val().trim(),
                SoDienThoai: $('#sodienthoai-pdh').val().trim(),
                DiaChi: $('#diachi-pdh').val().trim(),
                Email: $('#email-pdh').val().trim(),
                HinhThucThanhToan: $('#hinhthucthanhtoan-pdh').val().trim(),
                GhiChu: $('#ghiChu-pdh').val().trim(),
                NgayGiao: $('#ngaygiao-pdh').val().trim(),
                DaXacNhan: $('#daxacnhan-pdh').is(":checked"),
                DaThanhToan: $('#dathanhtoan-pdh').is(":checked"),
                TongTien: parseFloat($('#tongtien-pdh').val().trim().replace(/,/gi, "")),
                IsDeleted: false,
                chiTietPhieuDatHangs: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuDatHang/LuuPhieuDatHang",
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
                        $('#ngaydat-pdh').val(new Date($.now()).toLocaleDateString());
                        $('#tenkhachhang-pdh').val('');
                        $('#sodienthoai-pdh').val('');
                        $('#diachi-pdh').val('');
                        $('#email-pdh').val('');
                        $('#hinhthucthanhtoan-pdh').val('');
                        $('#ghiChu-pdh').val('');
                        $('#ngaygiao-pdh').val('');
                        $('#daxacnhan-pdh').val('');
                        $('#dathanhtoan-pdh').val('');
                        $('#tongtien-pdh').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuDatHang/';
                    }
                    else {
                        alert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Đặt Hàng');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Đặt Hàng');
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
                        $('#tongtien-pdh').val(0);
                    } else {
                        SumTotalAmount();
                    }
                    ClearValue();
                });
                $edit.click(function (e) {
                    e.preventDefault();
                    console.log(orderItems[i]);
                    $('#add-pdh').val("Lưu");
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
        $('#tongtien-pdh').val(formatNumber(parseFloat(total)));
    }

    //on change
    $('#MaHangHoa-pdh').on("change", function () {
        $.getJSON('/PhieuDatHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pdh').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa-pdh").val(data.TenHangHoa);
                        $("#giamgia-pdh").val(data.GiamGia);
                        $("#size-pdh").val(data.Size);
                        $('#gia-pdh').val(data.GiaBan * (1 - data.GiamGia));
                        //
                        $('#tonkho-pdh').val(data.SoLuong);
                    });
                }
            });
        tmpIndex = -1;
        $('#add-pdh').val("Thêm");
        //reset input
        $("#soLuong-pdh").val('');
        $("#thanhTien-pdh").val(0);
    });

    //this calculates values automatically
    MultiplicaPDH();
    $("#soLuong-pdh").on("keydown keyup", function () {
        MultiplicaPDH();
    });

    //$("#gia-pdh").on("keydown keyup", function () {
    //    MultiplicaPDH();
    //});
    // paste
    $("#soLuong-pdh").on('keyup input propertychange paste change', function () {
        CheckEmptyForSoLuongNhap_pdh();
        CheckQuantityForSoLuongNhap_pdh();
    });

    $("#gia-pdh").on('keyup input propertychange paste change', function () {
        CheckEmptyForGiaBan();
        CheckQuantityForGiaBan();
    });

    // replace data from oderItem
    function Replace(data) {
        $('#MaHangHoa-pdh').val(data.MaHangHoa);
        $("#tenHangHoa-pdh").val(data.TenHangHoa);
        $('#size-pdh').val(data.Size);
        $('#soLuong-pdh').val(data.SoLuong);
        $('#gia-pdh').val(data.GiaBan); //gia da ap dung giam gia
        $('#giamgia-pdh').val(data.GiamGia);
        var unitPrice = $('#gia-pdh').val().replace(/,/gi, "");
        var quantity = $('#soLuong-pdh').val();
        var result = parseFloat(unitPrice) * parseFloat(quantity) ;
        if (!isNaN(result)) {
            $('#thanhTien-pdh').val(formatNumber(result));
        }
    }
}
function EditPhieuDatHang() {
    $.getJSON('/PhieuDatHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pdh').val() },
        function (data) {
            if (data != null) {
                $.each(data, function (index, row) {
                    $("#tenHangHoa-pdh").val(data.TenHangHoa);
                    $("#giamgia-pdh").val(data.GiamGia);
                    $("#size-pdh").val(data.Size);
                    $('#gia-pdh').val(data.GiaBan * (1 - data.GiamGia));
                    //
                    $('#tonkho-pdh').val(data.SoLuong);
                });
            }
        });
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;
    $.getJSON('/PhieuDatHang/LoadChiTietPhieuDatHang', { id: $('#soPhieuDatHang').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
                $('#tongtien-pdh').val(formatNumber(parseFloat($('#tongtien-pdh').val())));
            }
        });

    $('#add-pdh').click(function () {
        var isValidItem = true;

        if ($('#tenHangHoa-pdh').val() == '') {
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
            var string_value_product = $('#MaHangHoa-pdh').val().trim();

            var productID = string_value_product.slice(0, 10);

            if (orderItems.length >= 0) {
                var test = true;
                if (orderItems.length > 0) {
                    var productIdOfTable = "";
                    var row = document.getElementById('productTable').rows.length;
                    if ($('#add-pdh').val() == "Lưu" && tmpIndex >= 0) {
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
                    $('#MaHangHoa-pdh').siblings('span.error').css('visibility', 'hidden');
                    orderItems.push({
                        MaHangHoa: $('#MaHangHoa-pdh').val().trim(),
                        TenHangHoa: $("#tenHangHoa-pdh").val().trim(),
                        Size: $("#size-pdh").val().trim(),
                        SoLuong: parseInt($('#soLuong-pdh').val().trim()),
                        GiaBan: parseInt($('#gia-pdh').val().trim().replace(/,/gi, "")),
                        GiamGia: $("#giamgia-pdh").val().trim(),
                        ThanhTien: parseInt($('#soLuong-pdh').val().trim()) * parseInt($('#gia-pdh').val().trim().replace(/,/gi, "")),
                    });

                    //Clear fields
                    $('#MaHangHoa-pdh').focus().val('');
                    $('#tenHangHoa-pdh').val('');
                    $('#giamgia-pdh').val('');
                    $('#size-pdh').val('');
                    $('#soLuong-pdh').val('');
                    $('#gia-pdh').val('');
                    $('#thanhTien-pdh').val('');

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
        if (orderItems.length == 0) {
            $('#orderItems').html('<span class="messageError" style="color:red;">Phải có ít nhất 1 hàng hóa</span>');
            isAllValid = false;
        }

        if ($('#tenkhachhang-pdh').val() == '') {
            isAllValid = false;
            $('#warning-tenkhachhang').html('<span class="messageError" style="color:red;">Phải nhập tên khách hàng</span>');
            $('#warning-tenkhachhang').show();
        }
        else {
            $('#warning-tenkhachhang').hide();
        }

        if ($('#sodienthoai-pdh').val() == '') {
            isAllValid = false;
            $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Phải nhập số điện thoại</span>');
            $('#warning-sodienthoai').show();
        }
        else {
            $('#warning-sodienthoai').hide();
            if ($('#sodienthoai-pdh').val().length < 7 || $('#sodienthoai-pdh').val().length > 11) {
                isAllValid = false;
                $('#warning-sodienthoai').html('<span class="messageError" style="color:red;">Số điện thoại không hợp lệ</span>');
                $('#warning-sodienthoai').show();
            }
        }

        if ($('#diachi-pdh').val() == '') {
            isAllValid = false;
            $('#warning-diachi').html('<span class="messageError" style="color:red;">Phải nhập địa chỉ giao hàng</span>');
            $('#warning-diachi').show();
        }
        else {
            $('#warning-diachi').hide();
        }


        //Save if valid
        if (isAllValid) {
            var data = {
                SoPhieuDatHang: $('#soPhieuDatHang').val().trim(),
                NgayDat: $('#ngaydat-pdh').val().trim(),
                MaNguoiDung: $('#maNguoiDung-pdh').val().trim(),
                TenKhachHang: $('#tenkhachhang-pdh').val().trim(),
                SoDienThoai: $('#sodienthoai-pdh').val().trim(),
                DiaChi: $('#diachi-pdh').val().trim(),
                Email: $('#email-pdh').val().trim(),
                HinhThucThanhToan: $('#hinhthucthanhtoan-pdh').val().trim(),
                GhiChu: $('#ghiChu-pdh').val().trim(),
                NgayGiao: $('#ngaygiao-pdh').val().trim(),
                DaXacNhan: $('#daxacnhan-pdh').is(":checked"),
                DaThanhToan: $('#dathanhtoan-pdh').is(":checked"),
                TongTien: parseFloat($('#tongtien-pdh').val().trim().replace(/,/gi, "")),
                IsDeleted: false,
                chiTietPhieuDatHangs: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuDatHang/SuaPhieuDatHang",
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
                        $('#ngaydat-pdh').val(new Date($.now()).toLocaleDateString());
                        $('#tenkhachhang-pdh').val('');
                        $('#sodienthoai-pdh').val('');
                        $('#diachi-pdh').val('');
                        $('#email-pdh').val('');
                        $('#hinhthucthanhtoan-pdh').val('');
                        $('#ghiChu-pdh').val('');
                        $('#ngaygiao-pdh').val('');
                        $('#daxacnhan-pdh').val('');
                        $('#dathanhtoan-pdh').prop('checked', false);;
                        $('#tongtien-pdh').prop('checked', false);;
                        $('#orderItems').empty();
                        window.location.href = '/Manager/PhieuDatHang/';
                    }
                    else {
                        alert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Đặt Hàng');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Đặt Hàng');
                }
            });
        }
    });

    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable"  class="table table-bordered"/>');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Size</th><th>Số Lượng</th><th>Giá (Đã áp dụng giảm giá)</th><th>Giảm Giá</th><th>Thành Tiền</th><th>Trạng Thái Kinh Doanh</th><th> Hành Động</th></tr></thead>');
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
                if (val.NgungKinhDoanh)
                    $row.append($('<td/>').html("Ngừng Kinh Doanh"));
                else
                    $row.append($('<td/>').html("Đang Kinh Doanh"));
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
                        $('#tongtien-pdh').val(0);
                    } else {
                        SumTotalAmount();
                    }
                    ClearValue();
                });
                $edit.click(function (e) {
                    if (!orderItems[i].NgungKinhDoanh)
                    {
                        e.preventDefault();
                        console.log(orderItems[i]);
                        $('#add-pdh').val("Lưu");
                        Replace(orderItems[i]);
                        tmpIndex = i;
                    }
                    else
                        alert("Sản phẩm đã ngừng kinh doanh")
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
        $('#tongtien-pdh').val(formatNumber(parseFloat(total)));
    }


    //on change
    $('#MaHangHoa-pdh').on("change", function () {
        $.getJSON('/PhieuDatHang/LoadThongTinHangHoa', { id: $('#MaHangHoa-pdh').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa-pdh").val(data.TenHangHoa);
                        $("#giamgia-pdh").val(data.GiamGia);
                        $("#size-pdh").val(data.Size);
                        $('#gia-pdh').val(data.GiaBan * (1 - data.GiamGia));
                        //
                        $('#tonkho-pdh').val(data.SoLuong);
                    });
                }
            });
        tmpIndex = -1;
        $('#add-pdh').val("Thêm");
        //reset input
        $("#soLuong-pdh").val('');
        $("#thanhTien-pdh").val(0);
    });

    //this calculates values automatically
    MultiplicaPDH();
    $("#soLuong-pdh").on("keydown keyup", function () {
        MultiplicaPDH();
    });

    $("#gia-pdh").on("keydown keyup", function () {
        MultiplicaPDH();
    });
    // paste
    $("#soLuong-pdh").on('keyup input propertychange paste change', function () {
        CheckEmptyForSoLuongNhap_pdh();
        CheckQuantityForSoLuongNhap_pdh();
    });

    $("#gia-pdh").on('keyup input propertychange paste change', function () {
        CheckEmptyForGiaBan();
        CheckQuantityForGiaBan();
    });

    // replace data from oderItem
    function Replace(data) {
        $('#MaHangHoa-pdh').val(data.MaHangHoa);
        $("#tenHangHoa-pdh").val(data.TenHangHoa);
        $('#size-pdh').val(data.Size);
        $('#soLuong-pdh').val(data.SoLuong);
        $('#gia-pdh').val(data.GiaBan ); //
        $('#giamgia-pdh').val(data.GiamGia);
        var unitPrice = $('#gia-pdh').val().replace(/,/gi, "");
        var quantity = $('#soLuong-pdh').val();
        var result = parseFloat(unitPrice) * parseFloat(quantity);
        if (!isNaN(result)) {
            $('#thanhTien-pdh').val(formatNumber(result));
        }
    }
}
function DetailsPhieuDatHang() {
    //basic button handler
    var orderItems = [];
    var tmpIndex = 0;

    $.getJSON('/PhieuDatHang/LoadChiTietPhieuDatHang', { id: $('#soPhieuDatHang').val() },
        function (data) {
            if (data != null) {
                orderItems = JSON.parse(data);
                GeneratedItemsTable();
                $('#tongtien-pdh').text(formatNumber(parseFloat($('#tongtien-pdh').text())));
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
        popupWin.document.write($('#ngaydat-pdh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#maNguoiDung-pdh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Tên khách hàng: ');
        popupWin.document.write($('#tenkhachhang-pdh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số điện thoại: ');
        popupWin.document.write($('#sodienthoai-pdh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Địa chỉ: ');
        popupWin.document.write($('#diachi-pdh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Email: ');
        popupWin.document.write($('#email-pdh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongtien-pdh').text().trim() + " VND");
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Hình thức thanh toán: ');
        popupWin.document.write($('#hinhthucthanhtoan-pdh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Ghi chú: ');
        popupWin.document.write($('#ghiChu-pdh').text().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Ngày giao: ');
        popupWin.document.write($('#ngaygiao-pdh').text().trim());
        popupWin.document.write('</td>')
        popupWin.document.write('<td>')
        popupWin.document.write('Trạng thái xác nhận: ');
        if ($('#daxacnhan-pdh').find('input').is(":checked")) { popupWin.document.write('Đã xác nhận'); }
        else { popupWin.document.write('Chưa xác nhận');}
        popupWin.document.write('</td></tr>')
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Trạng thái thanh toán: ');
        if ($('#dathanhtoan-pdh').find('input').is(":checked")) { popupWin.document.write('Đã thanh toán'); }
        else { popupWin.document.write('Chưa thanh toán');}
        
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

    $('#taophieubanhang').click(function () {
            var data = {
                NgayBan: new Date($.now()).toLocaleDateString('en-US'),
                TenKhachHang: $('#tenkhachhang-pdh').text().trim(),
                SoDienThoai: $('#sodienthoai-pdh').text().trim(),
                GhiChu: $('#ghiChu-pdh').text().trim(),
                TongTien: parseFloat($('#tongtien-pdh').text().trim().replace(/,/gi, "")),
                IsDeleted: false,
                chiTietPhieuBanHangs: orderItems
            }
            console.log(data);
            $(this).val('Xin Chờ.....');

            $.ajax({
                url: "/PhieuBanHang/LuuPhieuBanHangTuPhieuDatHang",
                type: "POST",
                data: JSON.stringify(data),
                dataType: "JSON",
                contentType: "application/json",
                success: function (d) {
                    //check is successfully save to database
                    if (d.status == true) {
                        //will send status from server side
                        //clear form
                        //orderItems = [];
                        //$('#ngayban-pbh').val(new Date($.now()).toLocaleDateString());
                        //$('#tenkhachhang-pbh').val('');
                        //$('#sodienthoai-pbh').val('');
                        //$('#ghiChu-pbh').val('');
                        //$('#tongtien-pbh').val('');
                        //$('#orderItems').empty();
                        window.location.href = '/Manager/PhieuBanHang/';
                    }
                    else {
                        alert("Số lượng hàng hóa không đủ để tạo phiếu bán hàng!", "error");
                    }
                },
                error: function () {
                    alert('Error. Please try again.');
                }
            });
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
function HideErrorProductName_pdh() {checkNumber_pdh
    if (document.getElementById('tenHangHoa-pdh').value != '') {
        $('#tenHangHoa-pdh').siblings('span.error').css('visibility', 'hidden');
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

function HideErrorMaHangHoa_pdh() {
    if (document.getElementById('MaHangHoa-pdh').value != '') {
        $('#MaHangHoa-pdh').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorTenHangHoa_pdh() {
    if (document.getElementById('tenHangHoa-pdh').value != '') {
        $('#tenHangHoa-pdh').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorSoLuongNhap_pdh() {
    if (document.getElementById('soLuongNhap-pdh').value != '') {
        $('#soLuong-pdh').siblings('span.error').css('visibility', 'hidden');
    }
}

function HideErrorGiaBan_pdh() {
    if (document.getElementById('gia-pdh').value != '') {
        $('#gia-pdh').siblings('span.error').css('visibility', 'hidden');
    }
}

function MultiplicaPDH() {
    if (document.getElementById('soLuong-pdh').value == '' || document.getElementById('gia-pdh').value == 0) {
        document.getElementById('thanhTien-pdh').value = 0;
    }
    else {
        var unitPrice = document.getElementById('gia-pdh').value.replace(/,/gi, "");
        var quantity = document.getElementById('soLuong-pdh').value;
        var result = parseFloat(unitPrice) * parseFloat(quantity) ;
        if (!isNaN(result)) {
            document.getElementById('thanhTien-pdh').value = formatNumber(result);
        }
    }
}

// check quantity input
function CheckEmptyForSoLuongNhap_pdh(error) {
    if (!($('#soLuong-pdh').val().trim() != '' && !isNaN($('#soLuong-pdh').val().trim()))) {
        $(".messageErrorinputQuantity").text("Nhập số lượng!");
        $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
        $("#soLuong-pdh").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity").addClass("hidden");
        $("#soLuong-pdh").removeClass("error");
    }
    $("#soLuong-pdh").blur(function () {
        $("#soLuong-pdh").val($("#soLuong-pdh").val().trim());
    });
    return error;
}


function CheckQuantityForSoLuongNhap_pdh(error) {
    var soluongtonkho = $('#tonkho-pdh').val().trim();
    
    if (parseInt($('#soLuong-pdh').val()) > soluongtonkho) {
        $(".messageErrorinputQuantity_tonkho").text("Vượt quá số lượng hàng trong kho!");
        $(".notifyinputQuantity_tonkho").slideDown(250).removeClass("hidden");
        $("#soLuong-pdh").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity_tonkho").addClass("hidden");
        $("#soLuong-pdh").removeClass("error");
    }

    if (($('#soLuong-pdh').val().trim() == '0') || ($('#soLuong-pdh').val().trim() == '00') || ($('#soLuong-pdh').val().trim() == '000') || ($('#soLuong-pdh').val().trim() == '0000')) {
        $(".messageErrorinputQuantity1").text("Nhập số lượng lớn hơn 0!");
        $(".notifyinputQuantity1").slideDown(250).removeClass("hidden");
        $("#soLuong-pdh").addClass("error");
        error++;
    }
    else {
        $(".notifyinputQuantity1").addClass("hidden");
        $("#soLuong-pdh").removeClass("error");
    }
    $("#soLuong-pdh").blur(function () {
        $("#soLuong-pdh").val($("#soLuong-pdh").val().trim());
    });
    return error;
}

