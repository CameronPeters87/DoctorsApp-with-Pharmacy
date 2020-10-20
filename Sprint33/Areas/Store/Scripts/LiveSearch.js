$(function () {

    $('input#searchtext').keyup(onSearchClick);
    //$('input#searchtext').blur(function () {
    //    $('input#searchtext').val("");
    //    $("ul#livesearchul").empty();
    //});

    function onSearchClick() {

        var search = $('input#searchtext').val();

        $("ul#livesearchul").empty();

        if (search == "" || search == " ") return false;

        var url = "/store/shop/LiveSearch";

        $.post(url,
            {
                search: search
            }, function (data, textStatus, jqXHR) {

                for (var i = 0; i < data.length; i++) {
                        var obj = data[i];
                    $("ul#livesearchul").append('<li class="livesearchli m-2">' +
                        '<a href="' + obj.ProductLink + '">' +
                        '<img src="' + obj.ImageUrl + '" width="50" height="50" />' 
                        + ' ' + obj.Name + '</a>' +
                        '<span class="price">' + obj.Price + '</span>' +
                        '</li>');
                }
            });
    }
})