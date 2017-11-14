var editor = CKEDITOR.replace('Content', { height: 250 });
editor.on("instanceReady", function () {
    $(".cke_button__maximize").click(function (e) {
        $(".alertList").toggleClass("fixed-ckeditor-max");
    });
})

$("#CreateTime").date();
