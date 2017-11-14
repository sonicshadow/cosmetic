var searchContent = function (target, option) {
    var $target = $(target);
    if (option.controls == undefined)
    {
        option.controls = $target.nextAll(".keyWordSearch").first();
    }
    var $keyWordSearch = option.controls;
    var $close = $keyWordSearch.find(".close");
    var $type = $keyWordSearch.find("#type");
    var $keyWord = $keyWordSearch.find(".keyWord");
    var $submit = $keyWordSearch.find(".submit");
    var $order = $keyWordSearch.find(".order");
    var $searchResult = $keyWordSearch.find(".searchResult");
    var $fixedList = $keyWordSearch.find(".fixedList");
    if (option.selected == undefined) {
        option.selected = function () { };
    }
    $target.focus(function (e) {
        $keyWordSearch.removeClass("hidden");
    });
    $close.click(function (e) {
        $keyWordSearch.addClass("hidden");
    });
    $type.change(function (e) {
        ajax();
    });
    $submit.click(function (e) {
        ajax();
    });
    $order.click(function (e) {
        $searchResult.empty();
        $fixedList.removeClass("hidden");
    });
    $fixedList.find(".item").click(function (e) {
        //var data = {};
        //data.Title = $(this).find("td").eq(0).text();
        //data.URL = $(this).find("td").eq(1).text();
        //option.click(data);
        $keyWordSearch.addClass("hidden");
        $searchResult.empty();
    });
    function ajax() {
        var data = {};
        data.keyWord = $keyWord.val();
        data.type = $type.val();
        $.ajax({
            type: "POST",
            url: comm.action("Index", "KeyWordSearch"),
            data: data,
            dataType: "html",
            success: function (data) {
                $fixedList.addClass("hidden");
                $searchResult.empty().append(data);
                $searchResult.find(".item").click(function (e) {
                    option.selected(JSON.parse($(this).find(".info").val()));
                    $keyWordSearch.addClass("hidden");
                });
            }
        });
    }

    this.show = function () {
        $keyWordSearch.removeClass("hidden");
    };
    this.hide = function () {
        $keyWordSearch.addClass("hidden");
    }
}

//var select = new searchContent("#text", {
//    selected: function (data) {
//        $("#text").val(data.URL);
//    }
//})
