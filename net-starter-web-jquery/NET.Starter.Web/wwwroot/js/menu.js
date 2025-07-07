$(function () {
    const url = window.location;
    $('ul.nav-sidebar a').each(function () {
        const href = $(this).attr('href');
        if (url.pathname == href) {
            $(this).addClass('active');
            return false;
        }
    });

    $('ul.nav-treeview a').filter(function () {
        return this.href == url;
    }).parentsUntil(".nav-sidebar > .nav-treeview").addClass('menu-open').prev('a').addClass('active');
});