$(function () {
    $("#DateFrom").val(new Date().toDateInputValue());
    $("#DateTo").val(new Date);

    $(".datepic").on("change", Initializepage);

    Initializepage();

    $(".filter").on("change", function () {
        Initializepage();
    })
  
   
});

Date.prototype.toDateInputValue = function () {

    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0.10);
}

function Initializepage() {
    $("#ID").val("");
    $('#HistoryReceiptTable').DataTable({
        ajax: {
            url: '../POSReceipt/GetPosList',
            type: "GET",
            data: {
                DateFrom: $("#DateFrom").val(),
                DateTo: $("#DateTo").val()
            },
            datatype: "json"
        },
        lengthMenu: [[10, 50, 100], [10, 50, 100]],
        lengthChange: true,
        scrollCollapse: true,
        ordering:false,
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
            
            
            { title: "Receipt No", data: "ReceiptRefNo", name: "ReceiptRefNo" },
            { title: "Transaction Date", data: "Date", name: "Date" },
          
        ],
        initComplete: function () {


        }
    });

    $('#HistoryReceiptTable tbody').on('click', 'tr', function () {
        var tabledata = $('#HistoryReceiptTable').DataTable();
        var data = tabledata.row(this).data();
        ShowDetails(data.ReceiptRefNo);

    });
}

function ShowDetails(reNo) {
    $('#ReceiptTable').DataTable({
        ajax: {
            url: '../POSReceipt/GetPosListDetails?ReceiptNo='+reNo,
            type: "GET",
            
            datatype: "json"
        },
        lengthMenu: [[10, 50, 100], [10, 50, 100]],
        lengthChange: true,
        scrollCollapse: true,
        ordering: false,
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


            { title: "Receipt No", data: "ReceiptRefNo", name: "ReceiptRefNo" },
            { title: "Category Name", data: "CategoryName", name: "CategoryName" },
            { title: "Name", data: "ItemName", name: "ItemName" },
            { title: "Amount", data: "Amount", name: "Amount" },
            { title: "Price", data: "RetailPrice", name: "RetailPrice" },
            { title: "Type", data: "Type", name: "Type" },
            
        ],
        initComplete: function () {
            $("#receiptdetailsModal").modal("show");

        }
    });
}