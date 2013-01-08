$(document).ready(function () {
    $('div[data-clickable-url]').click(function () {
        window.location.href = $(this).attr('data-clickable-url');
        return false;
    });
});