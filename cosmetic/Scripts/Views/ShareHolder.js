
$("#sure").click(function (e) {
    if (isNaN(Number($("#editAmount").val()))) {
        comm.alter(0, "填写正确资金", null, ".zijinalert");
        return false;
    }
    if (Number($("#editAmount").val()) >= 0) {
        comm.alter(0, "预留公司流动资金是负数", null, ".zijinalert");
        return false;
    }
    $('#myModal').modal('toggle');
    $("#amount").text($("#editAmount").val());
    var total = Number($("#profit").text()) + Number($("#editAmount").val());
    $("#total").text(total.toFixed(2));
});
$("#gudongfenhong").click(function (e) {
    if ($("#amount").text() == "")
    {
        comm.alter(0, "先设置预留公司流动资金");
        return false;
    }
    location = comm.action("ShareHolder", "Report", { total: $("#total").text() });
});


$("#time").datetime();

$("#transfer").click(function (e) {
    var data = {
        total: $("#total").val(),
        payee: $("#PayeeId").val(),
        time: $("#time").val()
    }
    if (data.time == "")
    {
        comm.alter(0, "填写日期", null, ".zijinalert");
        return false;
    }
    $.ajax({
        type: "POST",
        url: comm.action("Transfer", "Report"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $('#myModal').modal('toggle');
                comm.alter(1, data.CyMessage);
            }
            else {
                comm.alter(0, data.CyMessage, null, ".zijinalert");
            }
        }
    });
});

