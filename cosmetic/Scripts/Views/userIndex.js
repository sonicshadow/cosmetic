

new searchUser({ target: "#username" });
new searchUser({ target: "#parentname" });
new searchUser({ target: "#reusername" });

$("#enabled").click(function (e) {
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("LockoutEnabled", "User"),
        data: { id: $this.data("id"), username: $("#reusername").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $("[data-whatever='" + $this.data("id") + "']").parent().remove();
                $('#exampleModal').modal('hide')
            }
            else {
                comm.alter(0, data.CyMessage, "", ".modalalert");
            }
        }
    });
});


$("#exampleModal").on("show.bs.modal", function (event) {
    var button = $(event.relatedTarget);
    var recipient = button.data("whatever");
    var modal = $(this);
    modal.find("#enabled").attrdata("id", recipient);
})

$(".lockinAbled").click(function (e) {
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("LockinAbled", "User"),
        data: { id: $this.data("id") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Succcess") {
                $this.siblings(".hidden").removeClass("hidden");
                $this.remove();
            }
        }
    });
});
