var $txtFilter = $("#txtFilter");
$("#btnSearch").click(function (e) {
    var filter = $txtFilter.val().trim();
    if (filter != "") {
        location = comm.action("Index", "Staffs", { filter: filter });
    } else {
        location = comm.action("Index", "Staffs");
    }
});

$("#EntryTime").date();