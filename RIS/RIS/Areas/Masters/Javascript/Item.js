$(function () {
    Initializepage();
    Dropdown_select("ItemCategory", "/Helper/GetDropdown_CategoryItem");
    $('#ItemForm').on('submit', function (e) {
        e.preventDefault();

        if ($('#ID').val() == "") {
            AddItem($(this));
        }
        else {
            EditItem($(this));
        }

    });

    $('#StockForm').on('submit', function (e) {
        e.preventDefault();

        if ($('#ID').val() != "") {
            UpdateStock();
        }
        
    });

    $(".file-upload").on('change', function () {
        var files = new FormData();
        var file1 = document.getElementById("Photo").files[0];
        files.append('files[0]', file1);
        imageName = file1.name;
        $.ajax({
            type: 'POST',
            url: '/Item/UploadItemPhoto',
            data: files,
            dataType: 'json',
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                
            },
            error: function (error) {
                $('#uploadMsg').text('Error has occured. Upload is failed');
            }
        });

    });
});


var imageName;
function Initializepage() {
    $("#ItemForm")[0].reset();
    $("#ID").val("");
    $('#ItemTable').DataTable({
        ajax: {
            url: '../Item/GetItemList',
            type: "POST",
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
        dom: 'Bfrtip',
        buttons: [
            {
                text: "Add Item",
                className: "btn btn-success",
                action: function () {

                    $("#ItemModal").modal("show");
                }
            },
        ],
        columns: [
            { title: "ID", data: "ID", visible: false },
            {
                title: "Photo", data: function (x) {
                    var logO = "";
                    if (x.Photo == '' || x.Photo == null || x.Photo == "undefined") {
                        logO = '/PictureResources/no_item.gif';
                    }
                    else {
                        logO = "/PictureResources/ItemPhoto/" + x.Photo.replace(/\s/g,'');
                    }
                   
                    return "<img class='card-img-top img-responsive' style='cursor: pointer;width:50px;height:30px;' src='" + logO + "' alt='Card image cap'>";
                            
                }},
            { title: "Item Category", data: "ItemCategoryName", name: "ItemCategoryName" },
            { title: "Item Name", data: "ItemName", name: "ItemName" },
            { title: "Price", data: "Price", name: "Price" },
            { title: "Retail Price", data: "RetailPrice", name: "RetailPrice" },
            //{ title: "Stocks", data: "CurrentStock", name: "CurrentStock" },
            //{
            //    title: "Update Stocks", data: function (x) {
            //        return "<button type='button' class='btn btn-sm btn-info btnstock' id=Stockdata" + x.ID + ">" +
            //            "      <i class='material-icons'>shutter_speed</i> Stocks " +
            //            "</button> " 
                        
            //    }
            //},
            {
                title: "Action", data: function (x) {
                    return "<button type='button' class='btn btn-sm btn-info btnedit' id=data" + x.ID + ">" +
                        "      <i class='material-icons'>update</i> Modify " +
                        "</button> " +
                        "<button class='btn btn-sm btn-warning btndelete'>" +
                        "      <i class='material-icons'>delete</i> Delete " +
                        "</button>"
                }
            },
        ],
        initComplete: function () {


        }
    });
    $('#ItemTable tbody').off('click');
    $('#ItemTable tbody').on('click', '.btnstock', function () {

        var tabledata = $('#ItemTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();

      
        $('#ID').val(data.ID);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#StockModal").modal("show");
    });
    $('#ItemTable tbody').on('click', '.btnedit', function () {

        var tabledata = $('#ItemTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();

        $('#ItemName').val(data.ItemName);
        $('#ItemCategory').val(data.ItemCategory);
        $('#Price').val(data.Price);
        $('#RetailPrice').val(data.RetailPrice);
        if (data.Photo != null) {
            $("#photohere").attr("src", "/PictureResources/ItemPhoto/" + data.Photo.replace(/\s/g, ''));
        }
        
        //$('select[name=selValue]').val(data.ItemCategory);
        $('.selectpicker').selectpicker('refresh')
        $('#ID').val(data.ID);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#ItemModal").modal("show");
    });
    $('#ItemTable tbody').on('click', '.btndelete', function () {
        var tabledata = $('#ItemTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#ID').val(data.ID);
        Deletionheres('../Item/DeleteItem', data.ID, data.ItemName);

    });


}

function AddItem(data) {
    var datanow = data.serialize() + "&Photo=" + imageName;
    $.ajax({
        url: '../Item/CreateItem',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
               
                Initializepage();
                $("#ItemModal").modal("hide");
                
                swal({
                    title: 'Saved!',
                    text: 'Data Successfully Saved',
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false
                }).catch(swal.noop)
            }
            else {
                swal({
                    title: "Data already exist",
                    text: "Data failed to save",
                    type: 'warning',
                    timer: 2000,
                    showConfirmButton: false

                }).catch(swal.noop)
            }

        }
    });
  
}


function EditItem(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Item/EditItem',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#ItemModal").modal("hide");
                swal({
                    title: 'Saved!',
                    text: 'Data Successfully Saved',
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false
                }).catch(swal.noop)
            }
            else {
                swal({
                    title: "Data already exist",
                    text: "Data failed to save",
                    type: 'warning',
                    timer: 2000,
                    showConfirmButton: false

                }).catch(swal.noop)
            }

        }
    });
}


function UpdateStock() {
    $.ajax({
        url: '../Item/UpdateStocks',
        data: {
            ID: $("#ID").val(),
            StockType: $("#StockType").val(),
            Amount: $("#Amount").val(),

        },
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#StockModal").modal("hide");
                swal({
                    title: 'Saved!',
                    text: 'Data Successfully Saved',
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false
                }).catch(swal.noop)
            }
            else {
                swal({
                    title: "Data already exist",
                    text: "Data failed to save",
                    type: 'warning',
                    timer: 2000,
                    showConfirmButton: false

                }).catch(swal.noop)
            }

        }
    });
}