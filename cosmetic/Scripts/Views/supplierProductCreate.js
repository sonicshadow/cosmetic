function total() {
    if (isNaN($("#Price").val()))
    {
        return false;
    }
    if (isNaN($("#Count").val())) {
        return false;
    }
    $("#Total").val((Number($("#Price").val()) * Number($("#Count").val())).toFixed(2));
}
total();
$("#Price").change(function (e) {
    total();
});

$("#Count").change(function (e) {
    total();
});
