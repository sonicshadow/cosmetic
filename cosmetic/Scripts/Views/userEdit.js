
$(".edit").click(function (e) {
    var $this = $(this);
    var $thistr = $this.parents("tr");
    var data = {
        ID: $this.data("id"),
        price: $thistr.find(".price").val(),
        min: $thistr.find(".min").val(),
        twiceMin: $thistr.find(".twiceMin").val(),
    };
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "UserProduct"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                comm.alter(1, data.CyMessage);
            }
        }
    });
});

function load() {
    if ($("#msg").val() != "")
    {
        comm.alter(1, $("#msg").val());
    }
}
load();

new searchUser({ target: "#Recommend" });