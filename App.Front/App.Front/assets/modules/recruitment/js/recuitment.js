$(document).ready(function(){
    $(".view_map").fancybox({
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