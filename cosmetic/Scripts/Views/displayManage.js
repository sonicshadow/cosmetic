
$("#btnAdd").click(function (e) {
    var id = $("#DiaplayID").val();
    $.ajax({
        type: "POST",
        url: comm.action("Create", "DisplayManage"),
        data: { id: $("#DiaplayID").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            } else {
                comm.alter(0, $("#DiaplayID").text() + "已存在");
            }
        }
    });
});

$("[name=btnDel]").click(function (e) {
    var id = $(this).data("id");
    var name = $(this).data("name");
    $.ajax({
        type: "POST",
        url: comm.action("Delete", "DisplayManage"),
        data: { id: id },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            } else {
                comm.alter(0, name + "不存在");
            }
        }
    });
});