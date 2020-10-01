$(function() {

    $('input.cat-rad').click(function () {

        var selected = $(this).val();

        var url = "/store/shop/index"

        //alert(selected);

    //    $.get(url,
    //        {
    //            page: 1,
    //            selected: selected
    //        }, function(data) {
    //            //$("html").html(data);
    //            alert(selected);
    //        });
    });
})