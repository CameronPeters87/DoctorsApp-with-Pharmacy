$(function () {

    $('.buy-now').click(
        onAddToCartClick);

    function onAddToCartClick () {

        // Patient or Null
        var session = $(".buy-now").attr("data-session");

        if (session == "") {
            // Popup Login Modal
            $('#login').modal('show');

            $('#login button').click(
                onLogin);
        }
        else {
            onAddToCart();
        }
    }

    function onAddToCart () {
        var productId = $('.buy-now').attr("data-id");
        var quantity = $('.quantity').val();

        var url = ("/store/cart/AddItemToCart");

        $.post(url,
            {
                id: productId,
                qty: quantity
            }, function(data) {
                var message = data;

                if(message == "Item in Cart") {
                    alert("Item is already in Cart");
                }
                else {
                    location.reload();
                }

            }).fail(function(xhr, status, error) {
                alert(xhr + " " + status + " " + error);
        });
    }

    function onLogin() {

        $('.loader').removeClass("d-none");

        var email = $('#Email').val();
        var password = $('#Password').val();

        var url = "/store/login/index";

        $.post(url,
            {
                Email: email,
                Password: password
            }, function (data) {

                var message = data;

                if(message == "Failed") {

                    $('.validation').removeClass("d-none");
                    $('.loader').addClass("d-none");
                }
                else {
                    location.reload();
                }

            }).fail(function(xhr, status, error) {
                alert(xhr + " " + status + " " + error);
        });
    }
});