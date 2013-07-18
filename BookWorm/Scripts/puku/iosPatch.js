$(document).ready(function() {
    if ((navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPod/i)) || (navigator.userAgent.match(/iPad/i))) {
        $(".book-image").hover(function() {
            var closest = $(this).closest("a");
            if (closest && closest.attr("href")) {
                window.location.href = closest.attr("href");
            }
        });
    }
});