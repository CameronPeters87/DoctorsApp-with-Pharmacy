$(function () {

    /***************** Remove Category ********/

    // Document.ready -> link up remove event handler

    //$(".MarkRead").click(function () {
    $(document).on('click', '.MarkRead', function () {
        // Get the CartId from the link
        var notifToRead = $(this).attr("data-id");
        if (notifToRead != '') {
            // Perform the ajax post
            $.post("/Notifications/MarkRead", { "notificationId": notifToRead },
                function (data) {
                    //Successful requests get here
                    //Update the page elements
                    location.reload();
                });
        }
    });

    //////////////////////////////////////////////////////////////////////////////
});
