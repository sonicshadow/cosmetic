new searchUser({ target: "[name=filter]" });

$('#exampleModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var type = button.data('type');
    var modal = $(this)
    modal.find('.modal-title').text(recipient)
    modal.find('#sp_edit')
        .attrdata("type", type);
});

$("#sp_edit").click(function (e) {
    if (isNaN($("#sp_count").val())) {
        comm.alter(2, "填写正确数量", null, ".exampleModalAlert");
        return false;
    }
    if (isNaN($("#sp_count").val()) <= 0) {
        comm.alter(2, "填写数量", null, ".exampleModalAlert");
        return false;
    }
    var $this = $(this);
    var data = {
        id: $("#ID").val(),
        type: $this.data("type"),
        count: $("#sp_count").val(),
        remark: $("#remark").val()
    };
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "SupplierProduct"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                $('#exampleModal').modal('hide')
                var count = Number($("#sp_count").val());
                if ($this.data("type") == "Loss") {
                    count = -count;
                }
                $(".remaining").text(Number($(".remaining").text()) + count);
            }
            else {
                comm.alter(2, data.CyMessage, null, ".exampleModalAlert");
            }
        }
    });
});