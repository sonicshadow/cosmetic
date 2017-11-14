
$("#setting").click(function (e) {
    var order = {
        UserID: $("#Order_UserID").val(),
        ProductID: $("#Order_ProductID").val(),
        Count: $("#Order_Count").val(),
        Price: $("#Order_Price").val(),
        Total: $("#Order_Total").val(),
    };
    var array = new Array();
    $.each($("tbody").children(), function (i, item) {
        var up = {
            ID: $(this).find("#item_ID").val(),
            Min: $(this).find("#item_Min").val(),
            TwiceMin: $(this).find("#item_TwiceMin").val(),
            Price: $(this).find("#item_Price").val()
        };
        array.push(up);
    });
    var data = {
        Order: order,
        UserProduct: array,
        saleOrder: $('#saleOrder').is(':checked'),
        salePrice: $("#salePrice").val(),
    };
    if ($("#PayeeID").val() != undefined) {
        data.PayeeID = $("#PayeeID").val();
        data.fee = $("#fee").val();
    }
    if ($("#salePayeeID").val() != undefined) {
        data.salePayeeID = $("#salePayeeID").val();
        data.saleFee = $("#salefee").val();
    }
    $.ajax({
        type: "POST",
        url: comm.action("UserSetting", "Account"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = comm.action("Index", "User");
            }
            else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});

$("#Order_Count").change(function (e) {
    if (!isNaN($('#Order_Count').val())) {
        Total();
    }
});

$("#Order_ProductID").change(function (e) {
    sum();
});
sum();
function sum() {
    $.ajax({
        type: "GET",
        url: comm.action("GetPriceByPidAndUser", "UserProduct"),
        data: { pid: $("#Order_ProductID").val(), user: $("#Order_UserID").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $("#Order_Price").val(Number(data.price).toFixed(2));
                $("#Order_Count").val(data.count);
                $("#salePrice").val(Number(data.salePrice).toFixed(2));
                Total();
            }
        }
    });
}

function Total() {
    var totla = (Number($("#Order_Count").val()) * Number($("#Order_Price").val())).toFixed(2);
    $("#Order_Total").val(totla);
}
