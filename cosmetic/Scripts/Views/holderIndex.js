$("#create").click(function (e) {
    var data = {
        UserID: $("#create_userID").val(),
        IDCard: $("#create_IDCard").val(),
        Phone: $("#create_phone").val(),
        BankName: $("#create_bankName").val(),
        BankCard: $("#create_bankCard").val(),
        Stock: $("#create_stock").val()
    };
    if (!isnull(data))
    {
        comm.alter(0, "填写完整信息");
        return false;
    }
    if (Number(data.Stock) <= 0)
    {
        comm.alter(0, "填写正确的股份");
        return false;
    }
    if (check().isIDCard(data.BankCard))
    {
        comm.alter(0, "填写正确的身份证号码");
        return false;
    }
    if (!check().isTelephone(data.Phone))
    {
        comm.alter(0, "填写正确的电话号码");
        return false;
    }
    $.ajax({
        type: "POST",
        url: comm.action("Create", "Holder"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success")
            {
                location = location;
            }
        }
    });
});
function isnull(data) {
    var result = true;
    if (data.UserID == "") {
        result = false;
    }
    if (data.IDCard == "") {
        result = false;
    }
    if (data.BankName == "") {
        result = false;
    }
    if (data.BankCard == "") {
        result = false;
    }
    if (data.Stock == "") {
        result = false;
    }
    return result;
}

$(".del").click(function (e) {
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("Delete", "Holder"),
        data: { id: $this.data("id") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $this.parents("tr").remove();
            }
        }
    });
});

$(".edit").click(function (e) {
    var $this = $(this);
    var $tr = $this.parents("tr");
    var data={ 
        id: $this.data("id"),
        IDCard: $("#item_IDCard").val(),
        Phone: $("#item_Phone").val(),
        BankName: $("#item_BankName").val(),
        BankCard: $("#item_BankCard").val(),
        stock: $tr.find("#item_Stock").val(), 
        UserID:$tr.find("#item_UserID").val()
    };
    if (!isnull(data)) {
        comm.alter(0, "填写完整信息");
        return false;
    }
    if (Number(data.Stock) <= 0) {
        comm.alter(0, "填写正确的股份");
        return false;
    }
    if (check().isIDCard(data.BankCard)) {
        comm.alter(0, "填写正确的身份证号码");
        return false;
    }
    if (!check().isTelephone(data.Phone)) {
        comm.alter(0, "填写正确的电话号码");
        return false;
    }
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "Holder"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                comm.alter(1, data.CyMessage);
            }
        }
    });
});