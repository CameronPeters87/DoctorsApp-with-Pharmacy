$(function () {

    $('input#searchtext').keyup(onSearchClick);

    function onSearchClick() {

        var search = $('input#searchtext').val();

        var url = "/store/shop/LiveSearch";

        $.post(url,
            {
                search: search
            }, function (data, textStatus, jqXHR) {
                alert(data.length);
                for (var i = 0; i < data.length; i++) {
                        var obj = data[i];
                    $("ul#livesearchul").append('<li class="livesearchli m-2">' +
                        '<a href="' + obj.ProductLink + '">' +
                        '<img src="' + obj.ImageUrl + '" width="50" height="50" />' 
                        + ' ' + obj.Name + '</a>' +
                        '<span class="price">' + obj.Price+ '</span>' +
                        '</li>');
                    
                }
            });
    }
})