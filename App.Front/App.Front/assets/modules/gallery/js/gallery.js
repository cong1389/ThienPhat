$(document).ready(function(){
    $(".fancybox-gallery").fancybox({
        prevEffect : 'none',
        nextEffect : 'none',
        padding:0,
        closeBtn  : true,
        arrows    : true,
        nextClick : true,

        helpers : {
            thumbs : {
                width  : 110,
                height : 80
            }
        }
    });
});