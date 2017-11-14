var addressSelector = function (option) {
    var $selector = $(".addressSelector");
    var $selected = $(".addressSelector-selected");
    var $content = $(".addressSelector-content");
    var $head_a = $(".addressSelector-head").find("a");
    var $head_p = $head_a.eq(0);
    var $head_c = $head_a.eq(1);
    var $head_d = $head_a.eq(2);
    var $address = $(".addressSelector-address");
    var $province = $(".addressSelector-province");
    var $city = $(".addressSelector-city");
    var $district = $(".addressSelector-district");
   
    var _province = $selector.data("province");
    var _city = $selector.data("city");
    var _district = $selector.data("district");
    if (option == undefined) {
        option = {
            done: function () { }
        };
    } else {
        if (option.done == undefined) {
            option.done = function () { };
        }
    }

    $selected.click(function (e) {
        var tempWidth = $selector.width() + "px";
        $selected.toggleClass("active");
        if ($selected.hasClass("active")) {
            $content.removeClass("hidden");
            $selector.css("width", tempWidth);
        } else {
            $content.addClass("hidden");
            $selector.css("width", "");
        }
        $selected.find("span:last-child")
            .toggleClass("glyphicon-triangle-bottom")
            .toggleClass("glyphicon-triangle-top");
    });

    $head_a.click(function (e) {
        $head_a.removeClass("active");
        $(this).addClass("active");
        $address.find("ul").addClass("hidden");
        var a = $(this).data("address");
        $(".addressSelector-" + a).removeClass("hidden");
    });

    function setVal() {
        $selector.attrdata("province", _province);
        $selector.attrdata("city", _city);
        $selector.attrdata("district", _district);

        if (_province == _city) {
            $selected.find("span:first-child").text(_city + " " + _district);
        } else {
            $selected.find("span:first-child").text(_province + " " + _city + " " + _district);
        }

    }

    $province.find("li").click(function () {
        var $this = $(this);
        $province.find("li").removeClass("active");
        var thisProvince = $this.addClass("active").text();
        $province.addClass("hidden");
        $city.removeClass("hidden");
        $head_c.removeClass("hidden").addClass("active");
        $head_p.removeClass("active");
        if (thisProvince != _province) {
            $head_d.addClass("hidden");
            $head_p.text(thisProvince);
            $head_c.text("请选择");
            $.ajax({
                type: "GET",
                url: comm.action("GetCity", "AddressSelector"),
                data: { province: $this.text() },
                dataType: "json",
                success: function (data) {
                    $city.children().remove();
                    $.each(data, function (i, n) {
                        $city.append("<li>" + n + "</li>");
                    });
                    initCity();
                    if (data.length == 1) {
                        $head_c.addClass("hidden");
                        $city.find("li").click();
                    }
                }
            });
        }
        _province = thisProvince;
    })

    function initCity() {
        $city.find("li").click(function (e) {
            var $this = $(this);
            $city.find("li").removeClass("active");
            var thisCity = $this.addClass("active").text();
            $city.addClass("hidden");
            $district.removeClass("hidden");
            $head_d.removeClass("hidden").addClass("active");
            $head_c.removeClass("active");
            if (_city != thisCity) {
                $head_c.text(thisCity);
                $head_d.text("请选择");
                $.ajax({
                    type: "GET",
                    url: comm.action("GetDistrict", "AddressSelector"),
                    data: { city: $this.text() },
                    dataType: "json",
                    success: function (data) {
                        $district.children().remove();
                        $.each(data, function (i, n) {
                            $district.append("<li>" + n + "</li>");
                        });
                        initDistrict();
                    }
                });
            }
            _city = thisCity;
        });
    }

    function initDistrict() {
        $district.find("li").click(function (e) {
            var $this = $(this);
            $district.find("li").removeClass("active");
            _district = $this.addClass("active").text();
            $head_d.text(_district);
            setVal();
            $selected.click();
            option.done(_province, _city, _district);
        });
    }

    initCity();
    initDistrict();

}

