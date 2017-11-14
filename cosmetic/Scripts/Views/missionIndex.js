
new searchUser({ target: "#userID" });

$(".state").click(function (e) {
    var data = {};
    if ($(this).data("val") != "") {
        data.state = $(this).data("val");
    }
    if ($("#userID") != undefined || $("#userID").val() != "") {
        data.userName = $("#userID").val();
    }
    if ($("#dt_type").data("val") != "") {
        data.type = $("#dt_type").data("val");
    }
    location = comm.action("Index", "Mission", data);
});

$(".type").click(function (e) {
    var data = {};
    if ($(this).data("val") != "") {
        data.type = $(this).data("val");
    }
    if ($("#userID") != undefined || $("#userID").val() != "") {
        data.userName = $("#userID").val();
    }
    if ($("#dt_state").data("val") != "") {
        data.state = $("#dt_state").data("val");
    }
    location = comm.action("Index", "Mission", data);
});


$(".finishMission").click(function (e) {
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("MissionFinish", "Mission"),
        data: { id: $this.data("id") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
        }
    });
});

$(".cancelMission").click(function (e) {
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("CancelMissionn", "Mission"),
        data: { id: $this.data("id") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success")
            {
                var $tr = $this.parents("tr");
                $tr.children().eq(2).text("任务失败");
                $tr.children().eq(5).text("管理员中断任务");
                $this.remove();
            }
        }
    });
});
