
$("[name=time]").datetime();

$(".sure").click(function (e) {
    $.ajax({
        beforeSend: function () {
            var result = true;
            if ($("[name=time]").val() == "") {
                result = false;
                $("[data-valmsg-for=time]").text("日期 字段是必需的。");
            }
            if ($("[name=price]").val() == "") {
                result = false;
                $("[data-valmsg-for=price]").text("价格 字段是必需的。");
            }
            if ($("[name=fee]").val() == "") {
                result = false;
                $("[data-valmsg-for=fee]").text("手续费 字段是必需的。");
            }
            if (!result) {
                $("form").submit(function () {
                    return false;
                });
            }
        }
    });
});