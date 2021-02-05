$(function () {
    Initializepage();
    $(".sidebar").hide();
    $("body").addClass("sidebar-mini");
    $(".container-fluid").hide();
    $("#btnCheckout").on("click", getReceipt);
});


var imageName;
var purchasedItems = [];
var itemtable;
var t;
function Initializepage() {
    t = $('#SalesTable').DataTable({
        ordering: false,
        searching: false,
        lengthChange: false,
        lengthMenu: [[100, 50, 100], [100, 50, 100]],
        paging: false,
        bInfo:false,
    });

   
    itemtable =$('#ItemTable').DataTable({
        ajax: {
            url: '../POS/GetItemList',
            type: "POST",
            datatype: "json"
        },
        lengthMenu: [[10, 50, 100], [10, 50, 100]],
        lengthChange: true,
        scrollCollapse: true,
        serverSide: "true",
        order: [0, "asc"],
        processing: "true",
        ordering:false,
        language: {
            "processing": "processing... please wait"
        },
        //dom: 'Bfrtip',
        destroy: true,
        columns: [
            { title: "ID", data: "ID", visible: false },
            { title: "Rownum", data: "Rownum", visible: false },
            {
                title: "Photo", data: function (x) {
                    var logO = "";
                    if (x.Photo == '' || x.Photo == null || x.Photo == "undefined") {
                        logO = '/PictureResources/no_item.gif';
                    }
                    else {
                        logO = "/PictureResources/ItemPhoto/" + x.Photo.replace(/\s/g, '');
                        if (logO == null) {
                            logO = "/PictureResources/ServicePhoto/" + x.Photo.replace(/\s/g, '');
                        }
                    }
                    return "<img class='card-img-top img-responsive' style='cursor: pointer;width:50px;height:30px;' src='" + logO + "' alt='Card image cap'>";
                }
            },
            { title: "Item Name", data: "ItemName", name: "ItemName" },
            { title: "Stocks", data: "CurrentStock", name: "CurrentStock" },
            { title: "Price", data: "RetailPrice", name: "RetailPrice" },
            {
                title: "Amount", data: function (x) {
                    if (x.Type == "Item") {
                        return " <input class='form-control' type='number' name='ItemName" + x.Rownum + "' id='ItemName" + x.Rownum + "' value=1 />"

                    }
                    else {
                        return " <input class='form-control' type='number' name='ItemName" + x.Rownum + "' id='ItemName" + x.Rownum + "' value=1 readonly/>"

                    }
                }, name: "Amount", width:"5%"
            },
            {
                title: "", data: function (x) {
                    return "<button type='button' class='btn btn-sm btn-info btnadd' id=data" + x.Rownum + ">" +
                        "      <i class='material-icons'>add_shopping_cart</i> Add " +
                        "</button> "

                }, width:"5%"
            },
           
        ],
        initComplete: function () {


        }
    });
    $('#ItemTable tbody').off('click');
    $('#ItemTable tbody').on('click', '.btnadd', function () {
        var tabledata = $('#ItemTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        if (data.CurrentStock >= Number($("#ItemName" + data.Rownum).val())) {
            
            if (!containsObject(data, purchasedItems)) {
                purchasedItems.push(data);
            }
            else {
                var currentamount = Number($("#ItemName" + data.Rownum).val()) + 1;
                if (data.CurrentStock >= currentamount) {
                    purchasedItems = purchasedItems.filter(function (obj) {
                        return obj.Rownum !== data.Rownum;
                    });

                    $("#ItemName" + data.Rownum).val(currentamount);
                    purchasedItems.push(data);
                }
            }


            t.clear().draw();
            var total = 0;
            purchasedItems.forEach(function (i) {
                t.row.add([
                    i.ID,
                    i.Rownum,
                    i.Type,
                    i.Price,
                    i.RetailPrice,
                    i.ItemName,
                    $("#ItemName" + i.Rownum).val(),
                    i.RetailPrice * $("#ItemName" + i.Rownum).val(),
                    "<button type='button' class='btn btn-sm btn-warning removepurchase' id=data" + i.Rownum + ">" +
                    "      <i class='material-icons'>remove_shopping_cart</i> Void " +
                    "</button> "

                ]).draw(false);
                total += i.RetailPrice * $("#ItemName" + i.Rownum).val()
            });
        }

        $('#SalesTable tbody').on('click', '.removepurchase', function () {
            var data = t.row($(this).parents('tr')).data();
            purchasedItems = purchasedItems.filter(function (obj) {
                return obj.Rownum !== data[1]
            })
            var total = 0;
            t.clear().draw();
            purchasedItems.forEach(function (i) {
                t.row.add([
                    i.ID,
                    i.Rownum,
                    i.Type,
                    i.Price,
                    i.RetailPrice,
                    i.ItemName,
                    $("#ItemName" + i.Rownum).val(),
                    i.RetailPrice * $("#ItemName" + i.Rownum).val(),
                    "<button type='button' class='btn btn-sm btn-warning removepurchase' id=data" + i.Rownum + ">" +
                    "      <i class='material-icons'>remove_shopping_cart</i> Void " +
                    "</button> "

                ]).draw(false);
                total += i.RetailPrice * $("#ItemName" + i.Rownum).val()
            });
            $("#TheTotal").text(total);
        });

        $("#TheTotal").text(total);
            
       
    });
   
}

function containsObject(obj, list) {
    var i;
    for (i = 0; i < list.length; i++) {
        if (list[i] === obj) {
            return true;
        }
    }
    return false;
}


function getReceipt() {
    var datanow = t.rows().data();
    var receiptlist = [];
    for (var x = 0; x < datanow.length; x++) {
        var receipt = {
                ItemID: datanow[x][0],
                Rownum: datanow[x][1],
            Type: datanow[x][2],
            Price: datanow[x][3],
            RetailPrice: datanow[x][4],
                ItemName: datanow[x][5],
                Amount: datanow[x][6],
        }
        receiptlist.push(receipt);
    }
    //console.log(receiptlist);
    $.ajax({
        url: '../POS/GetReceipt',
        data: JSON.stringify({
            receiptlist: receiptlist,
           
        }),
        type: 'POST',
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (returnData) {
            purchasedItems = [];
            swal("Purchased Successful");
            t.clear().draw();
            itemtable.clear().draw();
            $("#TheTotal").text("");
        }
    });
}