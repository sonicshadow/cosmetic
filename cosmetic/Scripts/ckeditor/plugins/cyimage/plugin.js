
CKEDITOR.plugins.add('cyimage',
{
    lang: 'zh-cn',
    init: function (editor) {
        var pluginName = 'cyimage';
        editor.ui.addButton('cyimage',
           {
               label: editor.lang.cyimage.image,
               command: 'show',
               icon: this.path + 'images/image.png'
           });
        var cmd = editor.addCommand('show', { exec: dalcyimage });
    }
});
var tempUploader = null;
if (uploader != undefined && $("#myModal_ckupload").length > 0) {
    tempUploader = new uploader("#myModal_ckupload", {
        closed: function (data) {
            var html = "";
            $.each(data, function (i, n) {
                html += "<p><img src='" + comm.imgFullUrl(n) + "' style='width:100%'/></p>";
            });
            html += "<p></p>"
            editor.insertHtml(html);
        }
    });
} else {
    console.log("%c#ckupload no found", "color:red");
}
function dalcyimage(e) {
    tempUploader.show();
}