$(document).ready(function () {
    $('#datatable').DataTable();
    $('.dataTables_length').remove();
    $('.dataTables_info').remove();
    $('#datatable_paginate').remove();
    $('.dataTables_filter').remove();

    $(".btnDel").attr('data-toggle', 'modal');
    $(".btnDel").attr('data-target', '#Modal');
});

$(document).ajaxComplete(function () {
    $(".btnDel").attr('data-toggle', 'modal');
    $(".btnDel").attr('data-target', '#Modal');
});

function LoadDatatable() {
    $('#datatable').DataTable({
        "order": []
    });
    $('.dataTables_length').remove();
    $('.dataTables_info').remove();
    $('#datatable_paginate').remove();
    $('.dataTables_filter').remove();
}