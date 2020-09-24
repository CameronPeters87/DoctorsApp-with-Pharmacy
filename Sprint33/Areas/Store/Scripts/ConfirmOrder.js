$(function () {

    $('input.billing').blur(function()
    {
          if( !this.value ) {
              $(this).css("border-color", "red");
              $('.btn-order').attr("disabled", "disabled");
              $('.btn-order').addClass("cust-disabled");
          }
        else {
              $(this).css("border-color", "#595959");
              $('.btn-order').removeAttr("disabled");
              $('.btn-order').removeClass("cust-disabled");
          }
    });

    $('.btn-order').click(
        onPlaceOrderClick);

    function onPlaceOrderClick () {

        $("span.loader").removeClass("d-none");

        
        var selectedPaymentMethod = $("select.PaymentMethod").children("option:selected").val();
        
        var c_fname = $("#c_fname").val();
        var c_lname = $("#c_lname").val();
        var c_address = $("#c_address").val();
        var c_city = $("#c_city").val();
        var c_state_country = $("#c_state_country").val();
        var c_postal_zip = $("#c_postal_zip").val();
        var c_email_address = $("#c_email_address").val();
        var c_phone = $("#c_phone").val();
        var orderId = $(this).attr("data-orderId");
        var billingId = $(this).attr("data-billingId");

        //var url = "/store/checkout/Confirm";
        var url = ("/store/checkout/UpdateBillingInfo");

        if (selectedPaymentMethod == "") {
            $("span.payment-method-validation").html("Choose a Payment Method");
            $("span.loader").addClass("d-none");
        }
        else {
            $.post(url,
            {
                BillingId: billingId,
                FirstName: c_fname,
                Surname: c_lname,
                Address: c_address,
                City: c_city,
                Country: c_state_country,
                ZipCode: c_postal_zip,
                Email: c_email_address,
                PhoneNumber: c_phone,
                OrderId: orderId,
                PaymentMethod: selectedPaymentMethod
            }, function(data) {
                var message = data;

                if(message == "Failed") {
                    $("input.billing").css("border-color", "red");
                    $("span.loader").addClass("d-none");
                }
                else {
                    
                    if (selectedPaymentMethod == "Cash") 
                    {
                        location.href = "/store/pay/complete?id=1";
                    }
                    else 
                    {
                        $.get("/store/pay/getrequest", { orderId: orderId })
                            .done(data => success(data))
                            .fail(err => error(err));
                    }
                }
            }).fail(function(xhr, status, error) {
                alert(xhr + " " + status + " " + error);
            });

        }

    }

            function success(data) {
            if (data.success) {
                $("#PAY_REQUEST_ID").val(data.results.PAY_REQUEST_ID);
                $("#CHECKSUM").val(data.results.CHECKSUM);

                document.querySelector("#REDIRECT").click();
            }
        }

        function error(data) {
            console.log("Error");
            let node = document.createElement("li");
            node.innerText = data;
            //To display the result message under the Pay button
            document.querySelector("#results").appendChild(node);
        }
});