$("#Release").date();

function productDetail() {

    $(".detail_create").click(function (e) {
        var $this = $(this);
        var $thistr = $this.parents("tr");
        if (Number($thistr.find(".min").val()) <= 0) {
            comm.alter(2, "首批最低进货数量不能小于0");
            return false;
        }
        if (Number($thistr.find(".twiceMin").val()) <= 0) {
            comm.alter(2, "二次最低进货数量不能小于0");
            return false;
        }
        if ($thistr.find(".price").val() == "") {
            comm.alter(2, "填写单价");
            return false;
        }
        if (isNaN($thistr.find(".price").val())) {
            comm.alter(2, "填写正确单价");
            return false;
        }
        var demo = $thistr.clone();
        demo.find(".detail_del").removeClass("hidden").siblings(".detail_create").remove();
        $thistr.find(".form-control").val("");
        $thistr.before(demo);
        $(".detail_del,.detail_create").unbind();
        productDetail();
    });

    $(".detail_del").click(function (e) {
        var $this = $(this);
        $this.parents("tr").remove();
    });

    $(".useType").change(function (e) {
        var selected = $(this).find("option:selected").attr("selected", "selected");
        $(this).children().not(selected).removeAttr("selected");
    });

}
productDetail();

//修改
$("#submit_edit").click(function (e) {
    edit(comm.action("Edit", "Product"));
});
//添加
$("#submit").click(function (e) {
    edit(comm.action("Create", "Product"));
});

function edit(url) {
    var detail = new Array();
    $.each($("table").find(".detail_del:not(.hidden)"), function (i, item) {
        var $tr = $(this).parents("tr");
        var detailItem = {
            min: $tr.find(".min").val(),
            twiceMin: $tr.find(".twiceMin").val(),
            price : $tr.find(".price").val(),
            useType: $tr.find(".useType").val(),
        };
        if ($tr.data("id") != undefined) {
            detailItem.id = $tr.data("id");
        }
        detail.push(detailItem);
    });
    var data = {};
    data.productDetail = detail;
    if ($("#ID").length > 0) {
        data.ID = $("#ID").val();
    }
    data.Name = $("#Name").val();
    data.Price = $("#Price").val();
    data.Info = $("#Info").val();
    data.Spec = $("#Spec").val();
    data.Number = $("#Number").val();
    data.Unit = $("#Unit").val();
    data.No = $("#No").val();
    data.Release = $("#Release").val();
    if ($("#slist").val() != 0) {
        if (Number($("#PurchasePrice").val()) == 0 || $("#PurchasePrice").val() == "") {
            comm.alter(0, "填写采购价");
            return false;
        }
        var SupplierProducts = new Array();
        var item = {
            supplierId: $("#slist").val(),
            price: $("#PurchasePrice").val(),
        };
        if ($("#SupplierProductsID").length > 0) {
            if ($("#SupplierProductsID").val() != 0) {
                item.ID = $("#SupplierProductsID").val();
            }
        }
        SupplierProducts.push(item);
        data.SupplierProducts = SupplierProducts;
    }
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                location=comm.action("Index","Product")
            }
            else {
                comm.alter(2, data.CyMessage);
            }
        }
    });
}
