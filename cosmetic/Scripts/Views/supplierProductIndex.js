
$(".product").click(function (e) {
    var data = {};
    if ($(this).data("val") != "") {
        data.pid = $(this).data("val");
    }
    if ($(".supplier_a").data("val") != "") {
        data.sid = $(".supplier_a").data("val");
    }
    location = comm.action("Index", "SupplierProduct", data);
});

$(".supplier").click(function (e) {
    var data = {};
    if ($(".product_a").data("val") != "") {
        data.pid = $(".product_a").data("val");
    }
    if ($(this).data("val") != "") {
        data.sid = $(this).data("val");
    }
    location = comm.action("Index", "SupplierProduct", data);
});

$('#exampleModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var type = button.data('type');
    var modal = $(this)
    modal.find('.modal-title').text(recipient)
    modal.find('#sp_edit')
        .attrdata("type", type);
});

$("#sp_edit").click(function (e) {
    if (isNaN($("#sp_count").val())) {
        comm.alter(2, "填写正确数量", null, ".exampleModalAlert");
        return false;
    }
    if (Number($("#sp_count").val())<=0)
    {
        comm.alter(2, "填写数量", null, ".exampleModalAlert");
        return false;
    }
    var $this = $(this);
    var data = {
        id: $("#ID").val(),
        type: $this.data("type"),
        count: $("#sp_count").val(),
        remark: $("#remark").val()
    };
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "SupplierProduct"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $('#exampleModal').modal('hide')
                var count = Number($("#sp_count").val());
                if ($this.data("type") == "Loss") {
                    count = -count;
                }
                $(".remaining").text(Number($(".remaining").text()) + count);
            }
            else {
                comm.alter(2, data.CyMessage, null, ".exampleModalAlert");
            }
        }
    });
});

$("#sp_editpay").click(function (e) {
    if (isNaN($("#sp_total").val())) {
        comm.alter(2, "填写正确金额", null, ".PayeeAlert");
        return false;
    }
    if (Number($("#sp_total").val())<=0) {
        comm.alter(2, "填写金额", null, ".PayeeAlert");
        return false;
    }
    var data = {
        id: $("#ID").val(),
        price: $("#sp_total").val(),
        payeeID: $("#PayeeID").val()
    };
    $.ajax({
        type: "POST",
        url: comm.action("EditByPayee", "SupplierProduct"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $('#payment').modal('hide')
                $(".sendTotal").text((Number($(".sendTotal").text()) + Number($("#sp_total").val())).toFixed(2));
                $(".isSendTotal").text((Number($(".isSendTotal").text()) - Number($("#sp_total").val())).toFixed(2));
                if (Number($(".isSendTotal").text()) <= 0) {
                    $(".payment").remove();
                }
            }
            else {
                comm.alter(2, data.CyMessage, null, ".PayeeAlert");
            }
        }
    });
});

$("#sp_editcount").click(function (e) {
    if (isNaN($("#sp_sendCount").val())) {
        comm.alter(2, "填写正确数量", null, ".ReceiptAlert");
        return false;
    }
    if (Number($("#sp_sendCount").val()) <= 0) {
        comm.alter(2, "填写数量", null, ".ReceiptAlert");
        return false;
    }
    var data = {
        id: $("#ID").val(),
        count: $("#sp_sendCount").val()
    };
    $.ajax({
        type: "POST",
        url: comm.action("EditByCount", "SupplierProduct"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $('#send').modal('hide')
                $(".send").text(Number($(".send").text()) + Number($("#sp_sendCount").val()));
                $(".isSendCount").text(Number($(".isSendCount").text()) - Number($("#sp_sendCount").val()));
                if (Number($(".isSendCount").text()) <= 0) {
                    $(".sendbtn").remove();
                }
            }
            else {
                comm.alter(2, data.CyMessage, null, ".ReceiptAlert");
            }
        }
    });
});

