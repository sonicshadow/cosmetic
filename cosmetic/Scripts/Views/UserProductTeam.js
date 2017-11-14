$("#btnSearch").click(function (e) {
    var data = {};
    if ($("#txtFilter").val() != "") {
        data.filter = $("#txtFilter").val().trim();
    }
    if ($("#isRecommend").val() != "") {
        data.isRecommend = $("#isRecommend").val();
    }
    if ($("#Rank").val() != "") {
        data.Rank = $("#Rank").val();
    }
    location = comm.action("Team", "UserProduct", data);
});

$(".isrecommend").click(function (e) {
    console.log("fdsgfd");
    var data = {};
    if ($("#txtFilter").val() != "") {
        data.filter = $("#txtFilter").val().trim();
    }
    data.isRecommend = $(this).data("type");
    if ($("#Rank").val() != "") {
        data.Rank = $("#Rank").val();
    }
    location = comm.action("Team", "UserProduct", data);
});

$(".rank").click(function (e) {
    var data = {};
    if ($("#txtFilter").val() != "") {
        data.filter = $("#txtFilter").val().trim();
    }
    if ($("#isRecommend").val() != "") {
        data.isRecommend = $("#isRecommend").val();
    }
    data.Rank = $(this).data("type");
    location = comm.action("Team", "UserProduct", data);
});