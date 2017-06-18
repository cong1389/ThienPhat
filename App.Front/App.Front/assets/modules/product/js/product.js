$(document).ready(function(){
	
	$('#vnt-thumbnail-nav').slick({
		slidesToShow: 1,
		slidesToScroll: 1,
		arrows: false,
		fade: true,
		asNavFor: '#vnt-thumbnail-for',
	});
	$('#vnt-thumbnail-for').slick({
	  	slidesToShow: 4,
	 	slidesToScroll: 1,
	  	asNavFor: '#vnt-thumbnail-nav',
	  	dots: false,
	  	focusOnSelect: true,
	  	arrows:true,
	});
	$("#vnt-product-other").slick({	
		infinite: true,
		slidesToShow: 4,
		slidesToScroll: 4
	});
});