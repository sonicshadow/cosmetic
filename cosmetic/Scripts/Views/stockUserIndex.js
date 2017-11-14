$("#stockEdit").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("Crear", "Stock"),
        data: { pid: $("#pid").val()},
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location = location;
            }
            else {
                comm.alter(0, data.CyMessage, null, "#alert");
            }
        }
    });
});