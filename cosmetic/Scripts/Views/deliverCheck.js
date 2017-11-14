$(".send").click(function (e) {
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("CheckDeliver", "DeliverHistory"),
        data: { id: $this.data("id"), result: $this.data("result") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
            else {
                comm.alter(2, data.CyMessage);
            }
        }
    });
});