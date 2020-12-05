$(document).ready(function () {

    var active1 = false;
    var active2 = false;
    $('.parent2').on('mousedown touchstart', function () {
        if (!active1) $(this).find('.test1').css({ 'background-color': 'gray', 'transform': 'translate(-60px,75px)' });
        else $(this).find('.test1').css({ 'background-color': 'dimGray', 'transform': 'none' });
        if (!active2) $(this).find('.test2').css({ 'background-color': 'gray', 'transform': 'translate(0px,105px)' });
        else $(this).find('.test2').css({ 'background-color': 'darkGray', 'transform': 'none' });
        active1 = !active1;
        active2 = !active2;
    });
});