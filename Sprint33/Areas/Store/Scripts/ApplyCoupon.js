$(function () {

    $('.coupon button').click(
        onApplyCouponClick);

    function onApplyCouponClick () {

        var code = $('#c_code').val();
        var orderId = $('.coupon button').attr("data-orderId")

        var url = ("/store/checkout/ApplyCoupon");

        $.post(url,
            {
                code: code,
                orderId: orderId
            }, function(data) {
                var message = data;

                if(message == "CodeNull") {
                    $(".coupon span.validation").html("Enter a Code");
                }
                else if(message == "Failed") {
                    $(".coupon span.validation").html("That code does not exists");
                }
                else if(message == "CouponInUse") {
                    $(".coupon span.validation").html("You already applied a coupon to your order");
                }
                else {
                    location.reload();
                }

            }).fail(function(xhr, status, error) {
                alert(xhr + " " + status + " " + error);
        });
    }
});