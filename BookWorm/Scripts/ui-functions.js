var menuOpen = false;

function toggleDiscoveryMenu() {
    if (menuOpen) {
        $("#discovery_menu").fadeOut(200);
        $("#discovery_menu_toggle").html("Find a book <i class='icon-chevron-down'></i>");
        menuOpen = false;
    }
    else {
        $("#discovery_menu").fadeIn(200);
        $("#discovery_menu_toggle").html("Close <i class='icon-chevron-up'></i>");
        $("#searchQuery").focus();
        menuOpen = true;
    }

}