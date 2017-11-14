

$("[data-selector]").click(function (e) {
    if ($(this).data("selector") == "不通过") {
        if ($("#Remark").val() == "") {
            comm.alter(2, "填写审批意见");
            return false;
        }
    }
    var data = {};
    data.id = $("#ID").val();
    data.select = $(this).data("selector");
    data.remark = $("#Remark").val();
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "Mission"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = comm.action("Index", "Mission");
            }
            else {
                comm.alter(2, data.CyMessage);
            }
        }
    });
});

