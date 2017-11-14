
$("#txtStart,#txtEnd").date();

var $ddlKind = $("#ddlKind");
$ddlKind.find(">ul>li").click(function (e) {
    $ddlKind.find(">ul>li").removeClass("active");
    var $this = $(this);
    $this.addClass("active");
    $ddlKind.find(">a>x").text($this.data("name"));
    var selectedID = $this.data("id");
    var kind = selectedID == 0 ? null : selectedID;
    $ddlKind.attrdata("kind", kind)
    return false;
});
$("#btnSearch").click(function (e) {
    var p = {};
    var k = $ddlKind.data("kind");
    if (k != null) {
        p.Kind = k;
    }
    var start = $("#txtStart").val().trim();
    if (start != "") {
        p.Start = start;
    }
    var end = $("#txtEnd").val().trim();
    if (end != "") {
        p.End = end;
    }
    var bandCard = $("#txtBandCard").val().trim();
    if (bandCard != "") {
        p.BandCard = bandCard;
    }
    location = comm.action("Index", "AccountManage", p);
});

$("#cancel").click(function (e) {
    if ($("#remark").val() == "")
    {
        comm.alter(0, "填写取消原因", null, "#cancelAlert");
        return false;
    }
    $.ajax({
        type: "POST",
        url: comm.action("Cancel", "AccountManage"),
        data: { id: $(this).data("id"), remark: $("#remark").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                comm.alter(1, data.CyMessage, null, "#cancelAlert");
            } else {
                comm.alter(0, data.CyMessage, null, "#cancelAlert");
            }
        }
    });
});

$('#exampleModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget)
    var id = button.data('id')
    var modal = $(this)
    modal.find('.modal-footer  #cancel').attrdata("id", id);
})

$(".check").click(function (e) {
    console.log("brer");
    var $this = $(this);
    $.ajax({
        type: "POST",
        url: comm.action("Check", "AccountManage"),
        data: { id: $this.data("id"), result: $this.data("result") },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
            else {
                comm.alter(2, data.CyMessage);
            }
        }
    });
});