(function () {
    var link = document.querySelector("link[rel*='icon']") || document.createElement('link');;
    document.head.removeChild(link);
    link = document.querySelector("link[rel*='icon']") || document.createElement('link');
    document.head.removeChild(link);
    link = document.createElement('link');
    link.type = 'image/x-icon';
    link.rel = 'shortcut icon';
    link.href = '/favicon.png';
    document.getElementsByTagName('head')[0].appendChild(link);
})();

//(function calllooppostman() {

//    setTimeout(() => {
//        let notyet = bringpostman();
//        if (notyet)
//            calllooppostman();
//    }, 400)
//})();

//$(document).ready(() => {


//    $(window).on("load", function () {

//        $('head').append('<link type="image/x-icon" rel="icon" href="/favicon-16x16.png" sizes="16x16">')
//            .append('<link type="image/x-icon" rel="icon" href="/favicon-96x96.png" sizes="96x96">');

//    });


//});


