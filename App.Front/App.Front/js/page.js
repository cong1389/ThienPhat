$(function () {

    //$.doTimeout(2500, function () {
    //    $('.repeat.go').removeClass('go');

    //    return true;
    //});
    //$.doTimeout(2520, function () {
    //    $('.repeat').addClass('go');
    //    return true;
    //});
    //setInterval(showDiv, 3000);
    //var counter = 0;

    //function showDiv() {
    //    if (counter % 2 == 0) {
    //        $(".slogan-second").animate({
    //            opacity: 1
    //        }, 'fast').fadeIn('slow');
    //        $(".slogan-first").animate({
    //            opacity: 0
    //        }, 'fast').fadeOut(0);
    //    } else {
    //        $(".slogan-first").animate({
    //            opacity: 1
    //        }, 'fast').fadeIn('slow');
    //        $(".slogan-second").animate({
    //            opacity: 0
    //        }, 'fast').fadeOut(0);
    //    }
    //    counter++;
    //}
    ////$(document).on('click', '.browse', function () {
    ////    var file = $(this).parent().parent().parent().find('.file');
    ////    file.trigger('click');
    ////});
    //$(document).on('change', '.file', function () {
    //    $(this).parent().find('.form-control').val($(this).val().replace(/C:\\fakepath\\/i, ''));
    //});
    //$(".step").click(function () {
    //    $(".flow-form").show('fast');
    //    // $('html, body').animate({
    //    //     scrollTop: $("#form-flow").offset().top
    //    // }, 5000);
    //});

    //showTool();
   // if ($("#homepage-slider").length) {
        LoadCategoryHome($(".megacategory .pull-right a").attr("data-virtualid"), $(".megacategory .pull-right a").attr("data-parentid"));
   // }

    //$(".nav-fixed .pull-left li > a").click(function (e) {
    //    e.preventDefault();
    //    if (!$(this).hasClass("active")) {
    //        $(".nav-fixed .pull-left a").removeClass("active");
    //        $.post('/left-fixitem.html',
    //            { Id: $(this).attr("data-id") },
    //            function (response) {
    //                if (response.success) {
    //                    $(".nav-fixed .pull-right").empty().html(response.data);
    //                    LoadCategoryHome($(".nav-fixed .pull-right li >a.active").attr("data-id"));
    //                }
    //            });
    //        $(this).addClass("active");
    //        goToByScroll('fixed-items');
    //    }
    //    setTimeout(function () {
    //        $(".leftProductContent").find('.animated').removeClass('go').addClass('go');
    //    }, 300);
    //    return false;
    //});

    $(document).on("click", ".megacategory .pull-right li > a",
        function (e) {
          
            e.preventDefault();
            if (!$(this).hasClass("active")) {              
                $(".megacategory .pull-right a").removeClass("active");
                LoadCategoryHome($(this).attr("data-virtualid"), $(this).attr("data-parentid"));
                $(this).addClass("active");
              
               // goToByScroll('fixed-items');
            }
            setTimeout(function () {
                $(".leftProductContent_" + $(this).attr("data-parentid") + "").find('.animated').removeClass('go').addClass('go');
            }, 300);
            return false;
        });

    $(".primary_block .conten-attr .item input")
      .click(function () {
          $(".conten-attr .item input").parents(".item").removeClass("active");
          $(".conten-attr .item input").removeAttr('checked');
          $(this).parents(".item").toggleClass("active");
          $(this).attr('checked', 'checked');
          var value = this.value;

          $.post("/gallery-images.html",
              { postId: $(this).attr("data-post"), typeId: value },
              function (response) {
                  if (response.success) {
                      $("#gallery").data('royalSlider').destroy();;
                      $("#gallery").html(response.data);
                      initGallery();
                  }
              });
          $.post("/getprice.html",
              { proId: $(this).attr("data-post"), at: value },
              function (response) {
                  $(".primary_block .product-price span").html(response);
              });
      });
});
jQuery(document).ready(function () {
    //if ($("#homeslide").length) {
    //    if ($(".product-items").length) {
    //        var page = 1;
    //        setInterval(function () {
    //            $(".product-items").each(function (index) {
    //                var id = $(this).attr("attr-id");
    //                if (id != undefined) {
    //                    $.post('/Post/GetProductNewHome', { page: page, id: $("input[name=" + id + "]").val() }, function (response) {
    //                        $("#" + id + "").empty().html(response.data);
    //                    });
    //                    page++;
    //                    if (page > 2) {
    //                        page = 1;
    //                    }
    //                }
    //            })
    //            //for (var i = 0; i < $(".product-items").length - 1; i++) {  
    //            //    var id = $($(".product-items")[i]).attr("attr-id"); 
    //            //    $.post('/Post/GetProductNewHome', { page: page, id: $("input[name="+id+"]").val() }, function (response) { 
    //            //            $("#" + id + "").empty().html(response.data);
    //            //        });
    //            //    page++;
    //            //    if (page > 2) {
    //            //        page = 1;
    //            //    }
    //            //}
    //        }, 5000);
    //        setTimeout(function () {
    //            $(".product-block").find('.animated').removeClass('go').addClass('go');
    //        }, 300);

    //    }


    //    //var pageOld = 1;
    //    //setInterval(function () {
    //    //    $.post('/Post/GetProductNewHome',
    //    //        { page: pageOld, id: $("#NewProduct").val(), isNew: false },
    //    //        function (response) {
    //    //            $(".old-block").empty().html(response.data);
    //    //        });
    //    //    pageOld++;
    //    //    if (pageOld > 2) {
    //    //        pageOld = 1;
    //    //    }
    //    //}, 5000);
    //    //setTimeout(function () {
    //    //    $(".old-block").find('.animated').removeClass('go').addClass('go');
    //    //}, 300);
    //    var pageAccess = 1;
    //    setInterval(function () {
    //        $.post('/Post/GetAccesssoriesHome',
    //            { page: pageAccess, id: $("#Accessories").val() },
    //            function (response) {
    //                $(".accessories-block").empty().html(response.data);
    //            });
    //        pageAccess++;
    //        if (pageAccess > 2) {
    //            pageAccess = 1;
    //        }
    //    }, 5000);
    //    setTimeout(function () {
    //        $(".accessories-block").find('.animated').removeClass('go').addClass('go');
    //    }, 300);
    //}
});

function LoadCategoryHome(virtualId, parentId) {
    $.post('/fixitem-content.html',
        { virtualId: virtualId },
        function (response) {
            if (response.success) {
                $(".leftProductContent_" + parentId + "").empty().html(response.data);
            }
        });
}

function showTool() {
    var heightToShow = $(".top-head").height() +
        $(".header").height() +
        $(".nav-menu").height() +
        $("#homeslide").height();

    $(window).scroll(function () {
        if ($(window).scrollTop() >= heightToShow) {
            $(".nav-tools").stop().animate({
                left: '0',
                top: $(window).height() / 3
            })
        } else {
            $(".nav-tools").stop().animate({
                left: '-80px',
                top: heightToShow
            });
        }
    });
}
function goToByScroll(id) {
    // Remove "link" from the ID
    id = id.replace("link", "");
    // Scroll
    $('html,body').animate({
        scrollTop: $("." + id).offset().top
    },
        'slow');
}

function handleError(msg) {
    $("#msg-error").html(msg).show();
}
function handleSuccess(msg) {
    $("#msg-success").html(msg).show();
}
$(function () {
    $("#CheckProduct").click(function (e) {
        var fromId = "#checking";
        formAjax(fromId);
        return false;
    });
    $("#BuyProduct").click(function (e) {
        var fromId = "#formbuy";
        formAjax(fromId);
        return false;
    });
    $("#checkorder").click(function (e) {
        var code = $("#oderCode").val();
        var name = $("#NameOrPhome").val();
        $.post('/home/CheckOrder', { phone: name, ordercode: code }, function (response) {
            $(".result_check").empty().html(response.data);
        })
        return false;
    });
});
function formAjax(element) {
    var $form = $(element);
    var options = {
        beforeSend: function () {
            $(".ajax-loading").show();
        },
        dataType: 'json',
        complete: function (responseText, statusText, xhr) {
            var resonse = responseText.responseJSON;
            if (resonse.success) {
                $form[0].reset();
                alert("Gửi thông tin thành công.");
            } else {
                $(".ajax-loading").hide();
                //show message
                alert(resonse.errors);
                $('html, body').animate({
                    scrollTop: $("#form-flow").offset().top
                },
                    2000);
            }
        }
    };

    if ($form.valid()) {
        $form.ajaxSubmit(options);
    }
    return false;
}
