


new searchUser({ target: "[name=userID]" });
new searchUser({ target: "[name=parentUser]" });
$("[name=start],[name=end]").date();

$(".delOrder").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("CancelOrder", "Order"),
        data: { id: $(this).data("id") },
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
    return false;
});

$("#surePay").click(function (e) {
    var data = {
        id: $("#ID").val(),
        fee: $("#Fee").val()
    };
    if ($("#User_Parent").val() == "") {
        if ($("#payeeID").val() == null || $("#payeeID").val() == 0) {
            comm.alter(0, "请先维护收款人", "", $(".ReceiptAlert"));
            return false;
        }
        data.payeeID = $("#payeeID").val();
    }
    pay(data, ".ReceiptAlert");
});

$("#edit_pay").click(function (e) {
    var data = {
        id: $("#ID").val(),
        fee: 0
    };
    pay(data, null)
});

function pay(data,tag) {
    $.ajax({
        type: "POST",
        url: comm.action("EditByPay", "Order"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            } else {
                comm.alter(0, data.CyMessage, null, tag);
            }
        }
    });
}

$("#cancelOrder").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("CancelOrder", "Order"),
        data: { id: $("#ID").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $("#cancelOrder,#surePay,#payForRe").remove();
            }
            else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});

$(".sendOrder").click(function (e) {
    var deliverHistory = {
        Address: $("#User_Address").val(),
        Code: $("#Code").val(),
        Count: $("#Count").val(),
        Express: $("#Express").val(),
        OrderID: $("#ID").val(),
        UserID: $("#UserID").val(),
    };
    var data = {
        deliver: deliverHistory,
    };
    if ($("#User_Parent").val() == "") {
        var supplierProduct = {
            SupplierID: $("#sp_number").val(),
            Count: $("#sp_count").val(),
        };
        data.sp = supplierProduct;
    }
    $.ajax({
        type: "POST",
        url: comm.action("Create", "DeliverHistory"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                if (data.CyMessage == "发货成功") {
                    location = location;
                }
                else {
                    comm.alter(1, data.CyMessage, null, "#modelAlter");
                }
            } else {
                comm.alter(0, data.CyMessage, null, "#modelAlter");
            }
        }
    });
});

$(".sp_number").change(function (e) {
    getStock();
});

if ($("#User_Parent").val() == "") {
    getStock();
}

function getStock() {
    $.ajax({
        type: "POST",
        url: comm.action("GetBySid", "SupplierProduct"),
        data: { sid: $(".sp_number").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $(".sp_number").parents("tr").find("#sp_sendCount").val(data.Data);
            }
        }
    });
}
