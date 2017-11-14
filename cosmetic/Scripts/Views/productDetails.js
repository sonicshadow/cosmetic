$("#order").click(function (e) {

    if (Number($("#Count").val()) < Number($(".min").text())) {
        comm.alter(0, "下单数量不小于" + $(".min").text());
        return false;
    }

    var data = {
        ProductID: $("#ID").val(),
        Count: $("#Count").val(),
    }
    $.ajax({
        type: "POST",
        url: comm.action("Create", "Order"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = comm.action("UserOrder", "Order");
            } else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});