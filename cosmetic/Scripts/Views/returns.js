
$("#Count,#Price").change(function (e) {
    var total=Number($("#Count").val()) * Number($("#Price").val());
    $("#Total").val(total.toFixed(2));
    var difference = total - (Number($("#Order_Price").val()) * Number($("#Count").val()));
    $("#Difference").val(difference.toFixed(2));
});

$(".result").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("Check", "Returns"),
        data: { id: $("#ID").val(), result: $(this).data("result") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
            else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});

$("#Receivables").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("Receivables", "Returns"),
        data: { id: $("#ID").val(), payeeID: $("#payeeID").val(), fee: $("#Fee").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
            else {
                comm.alter(0, data.CyMessage, null, ".ReceiptAlert");
            }
        }
    });
});

$("#Receipt").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("Receipt", "Returns"),
        data: { id: $("#ID").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
            else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});

$("#Order_Code").keyup(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("OrderByCode", "Order"),
        data: {code:$(this).val()},
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success")
            {
                var item = data.data;
                $("#Order_ID").val(item.ID);
                $("#Order_Products_Name").val(item.Name);
                $("#Order_Count").val(item.Count);
                $("#Order_Price").val(item.Price.toFixed(2));
                $("#Order_Total").val(item.Total.toFixed(2));
                $("#NewProductID option[value='" + item.ProductID + "']").remove();
                change();
            }
        }
    });
});

function change() {
    $.ajax({
        type: "POST",
        url: comm.action("SearchByIDAndCode", "Product"),
        data: { pid: $("#NewProductID").val(), code: $("#Order_Code").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $("#Price").val(data.Price.toFixed(2));
            }
        }
    });
}

$("#NewProductID").change(function (e) {
    change();
});