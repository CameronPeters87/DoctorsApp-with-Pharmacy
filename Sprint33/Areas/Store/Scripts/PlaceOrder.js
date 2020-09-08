$(function () {

    $('.btn-order').click(
        onPlaceOrderClick);

    function onPlaceOrderClick () {

        $("span.loader").removeClass("d-none");

        var selectedPaymentMethod = $("select.PaymentMethod").children("option:selected").val();
        
        var c_fname = $("#c_fname").val();
        var c_lname = $("#c_lname").val();
        var c_address = $("#c_address").val();
        var c_state_country = $("#c_state_country").val();
        var c_postal_zip = $("#c_postal_zip").val();
        var c_email_address = $("#c_email_address").val();
        var c_phone = $("#c_phone").val();
        var orderId = $(this).attr("data-orderId");
        var billingId = $(this).attr("data-billingId");

        //var url = "/store/checkout/Confirm";
        var url = ("/store/checkout/Confirm");

        if (selectedPaymentMethod == "") {
            $("span.payment-method-validation").html("Choose a Payment Method");
            $("span.loader").addClass("d-none");
        }
        else {
            $.post(url,
            {
                BillingId: billingId,
                FirstName: c_fname,
                Surname: c_fname,
                Address: c_address,
                Country: c_state_country,
                ZipCode: c_postal_zip,
                Email: c_email_address,
                PhoneNumber: c_phone,
                OrderId: orderId

            }, function(data) {
                var message = data;
                alert(message);
                $("span.loader").addClass("d-none");

            }).fail(function(xhr, status, error) {
                alert(xhr + " " + status + " " + error);
            });

        }

    }
});