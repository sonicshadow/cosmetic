


var checkbok = comm.checkboxList(".parent", ".chlid:not(:disabled)");

$(".type").click(function (e) {
    var data = {};
    if ($(this).data("val") != "") {
        data.type = $(this).data("val");
    }
    if ($("#start").val() != "") {
        data.start = $("#start").val();
    }
    if ($("#end").val() != "") {
        data.end = $("#end").val();
    }
    if (($("#isPay").data("val")).toString() != "") {
        data.isPay = $("#isPay").data("val");
    }
    location = comm.action("Index", "UserIncome", data);
});

$(".isplay").click(function (e) {
    var data = {};
    if ($("#type").data("val") != "") {
        data.type = $("#type").data("val");
    }
    if ($("#start").val() != "") {
        data.start = $("#start").val();
    }
    if ($("#end").val() != "") {
        data.end = $("#end").val();
    }
    if (($(this).data("val")).toString() != "") {
        data.isPay = $(this).data("val");
    }
    location = comm.action("Index", "UserIncome", data);
});

$("#start,#end").date();

$("#surepay").click(function (e) {
    var data = {
        ids: getCookie(),
        payeeId: $("#PayeeId").val()
    }
    location = comm.action("Payment", "UserIncome", data);
});

function load() {
    var username = getCookie();
    if (username != null && username != "") {
        var array = username.split(",");
        for (var i = 0; i < checkbok.selectedValues().length ; i++) {
            var item = checkbok.selectedValues()[i];
            if (array.indexOf(item) < 0) {
                array.push(item);
            }
        }
        setCookie(array.join(","), 1);
    }
    else {
        setCookie(checkbok.selectedValuesString(), 1);
    }
}
load();

function change(tag) {
    var $tag = $(tag);
    var check = $tag.is(":checked");
    var username = getCookie();
    if (check) {
        if (username != null && username != "") {
            var array = username.split(",");
            if (array.indexOf($tag.val()) < 0) {
                array.push($tag.val());
            }
            setCookie(array.join(","), 1);
        }
        else {
            setCookie($tag.val(), 1);
        }
    }
    else {
        if (username != null && username != "") {
            var array = username.split(",");
            if (array.indexOf($tag.val()) >= 0) {
                array.splice(array.indexOf($tag.val()), 1);
            }
            setCookie(array.join(","), 1);
        }
    }
}

$(".chlid").click(function (e) {
    change($(this));
});

$(".parent").click(function (e) {
    $.each($(".chlid"), function (i, item) {
        change($(this));
    });
});

//cookie
function getCookie() {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf("ids" + "=")
        if (c_start != -1) {
            c_start = c_start + "ids".length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}

function setCookie(value, expiredays) {
    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)
    document.cookie = "ids" + "=" + escape(value) +
    ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())
}

$("#payForRe").click(function (e) {
    $.ajax({
        type: "POST",
        url: comm.action("EditIsPay", "UserIncome"),
        data: { ids: $("#ids").val(), payee: $("#payeeId").val() },
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                comm.alter(1, data.CyMessage);
                setCookie("", 1);
            } else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});