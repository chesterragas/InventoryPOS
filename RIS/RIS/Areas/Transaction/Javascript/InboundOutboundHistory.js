$(function () {
    Dropdown_select("ItemCategory", "/Helper/GetDropdown_CategoryItem");

    Initializepage();

    $(".filter").on("change", function(){
        Initializepage();
    })
});

function Initializepage() {
    $("#ID").val("");
    $('#HistoryStockTable').DataTable({
        ajax: {
            url: '../InboundOutboundHistory/GetInboundOutboundList',
            type: "GET",
            data: {
                Category: $("#ItemCategory").val(),
                Type: $("#Type").val()
            },
            datatype: "json"
        },
        lengthMenu: [[10, 50, 100], [10, 50, 100]],
        lengthChange: true,
        scrollCollapse: true,
        serverSide: "true",
        order: [0, "asc"],
        processing: "true",
        language: {
            "processing": "processing... please wait"
        },
        //dom: 'Bfrtip',
        destroy: true,
        dom: 'frtip',
       
        columns: [
            { title: "ID", data: "ID", visible: false },
            {
                title: "Photo", data: function (x) {
                    var logO = "";
                    if (x.Photo == '' || x.Photo == null) {
                        logO = '/Content/images/2014-09-16-Anoynmous-The-Rise-of-Personal-Networks.jpg';
                    }
                    else {
                        logO = "/PictureResources/ItemPhoto/" + x.Photo.replace(/\s/g, '');
                    }

                    return "<img class='card-img-top img-responsive' style='cursor: pointer;width:50px;height:30px;' src='" + logO + "' alt='Card image cap'>";

                }
            },
            { title: "Item Category", data: "CategoryName", name: "CategoryName" },
            { title: "Item Name", data: "ItemName", name: "ItemName" },
            {
                title: "Type", data: function (x) {
                    return (x.StockType == "In")?"Inbound" : "Outbound";
                }, name: "Type"
            },
            { title: "Quantity", data: "StockQty", name: "StockQty" },
            { title: "Remarks", data: "Remarks", name: "Remarks" },

        ],
        initComplete: function () {


        }
    });
  
}