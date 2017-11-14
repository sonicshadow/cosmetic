$("[vdo-fileid]").click(function (e) {
    var $this = $(this);
    var fileid = $this.attr("vdo-fileid");
    var autoplay = $this.attr("vdo-autoplay")
    var data = { id: fileid };
    if (autoplay != undefined) {
        data.autoplay = autoplay == "1" ? true : false;
    }
    $.ajax({
        type: "POST",
        url: comm.action("Index", "Player"),
        data: data,
        dataType: "html",
        success: function (data) {
            comm.mask();
            
            $("body").append(data);
            var $play = $("#playerModal");
            $play.find("span").click(function () {
                comm.mask();
                $play.remove();
            })
        }
    });
});