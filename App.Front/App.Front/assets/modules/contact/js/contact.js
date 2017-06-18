$(document).ready(function(){
	var map_off=$(".map-contact").offset().top;
	$(".view-map").click(function(){
		$("html,body").animate({scrollTop: map_off},500);
	});
});