$("#btnAdd").click(function (e) {
    var name = $("#txtName").val().trim();
    if (name == "") {
        comm.alter(2, "银行名称不能空");
    }
    $.ajax({
        type: "POST",
        url: comm.action("Create", "BankManage"),
        data: { name: name },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            } else {
                comm.alter(0, data.Message);
            }
        }
    });
});

$("[name=btnDel]").click(function (e) {
    var name = $(this).data("name");
    $.ajax({
        type: "POST",
        url: comm.action("Delete", "BankManage"),
        data: { name: name },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            } else {
                comm.alter(0, data.Message);
            }
        }
    });
});