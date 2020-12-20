$('#menu-action').click(function () {
    $('.sidebar').toggleClass('active');
    $('.main').toggleClass('active');
    $(this).toggleClass('active');

    if ($('.sidebar').hasClass('active')) {
        $(this).find('i').addClass('fa-close');
        $(this).find('i').removeClass('fa-bars');
    }
    else
    {
        $(this).find('i').addClass('fa-bars');
        $(this).find('i').removeClass('fa-close');
    }
});

//// Add hover feedback on menu
$('#menu-action').hover(function () {
    $('.sidebar').toggleClass('hovered');
    $('#menu-action').css('background:#fff');
});
function Open(url, replaceheader = true) {
    url = '/Manager/' + url;
    $('#content').hide();
    $('#loading').show();
    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {
            $('#content').html($(response).find('#content'));
            if (replaceheader)
                $('#header').html($(response).find('#header'));
            if (document.getElementById("pageTitle")) {
                var pageTitle = document.getElementById("pageTitle").innerText;
                document.title = pageTitle;
            }
            $('#content').show();
            $('#loading').hide();
            LoadDatatable();
            window.history.pushState({ "html": response, "pageTitle": document.title, "rHeader": replaceheader }, "", url);        },
    });
}
window.onpopstate = function (e) {
    console.log(e);
    if (e.state) {
        $('#content').html($(e.state.html).find('#content'));
        if (e.state.rHeader)
            $('#header').html($(e.state.html).find('#header'));
        console.log(e.state.html);
        document.title = e.state.pageTitle;
    }
    else
        Open('Home/Index');
};