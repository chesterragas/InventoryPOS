$(function () {
    Initializepage();
    $('#CategoryForm').on('submit', function (e) {
        e.preventDefault();

        if ($('#ID').val() == "") {
            AddCategory($(this));
        }
        else {
            EditCategory($(this));
        }

    });

    $('#ServiceForm').on('submit', function (e) {
        e.preventDefault();

        if ($('#ID2').val() == "") {
            AddService($(this));
        }
        else {
            EditService($(this));
        }

    });
});



function Initializepage() {
    $("#CategoryForm")[0].reset();
    $("#ID").val("");
    $('#CategoryTable').DataTable({
        ajax: {
            url: '../Category/GetCategoryList',
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
                text: "Add Item Category",
                className: "btn btn-success",
                action: function () {

                    $("#CategoryModal").modal("show");
                }
            },
        ],
        columns: [
            { title: "ID", data: "ID", visible: false },
            { title: "Item Category", data: "CategoryName", name: "CategoryName" },
          
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
    $('#CategoryTable tbody').off('click');
    $('#CategoryTable tbody').on('click', '.btnedit', function () {

        var tabledata = $('#CategoryTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();

        $('#CategoryName').val(data.CategoryName);
        $('#FirstName').val(data.FirstName);
        $('#LastName').val(data.LastName);
        $('#Email').val(data.Email);

        $('#ID').val(data.ID);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#CategoryModal").modal("show");
    });
    $('#CategoryTable tbody').on('click', '.btndelete', function () {
        var tabledata = $('#CategoryTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#ID').val(data.ID);
        Deletionheres('../Category/DeleteCategory', data.ID, data.CategoryName);

    });


    $("#ServiceForm")[0].reset();
    $("#ID2").val("");
    $('#ServiceTable').DataTable({
        ajax: {
            url: '../Category/GetServiceList',
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
                text: "Add Service Category",
                className: "btn btn-success",
                action: function () {

                    $("#CategoryModal2").modal("show");
                }
            },
        ],
        columns: [
            { title: "ID2", data: "ID2", visible: false },
            { title: "Service Category", data: "CategoryNameSer", name: "CategoryNameSer" },

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
    $('#ServiceTable tbody').off('click');
    $('#ServiceTable tbody').on('click', '.btnedit', function () {

        var tabledata = $('#ServiceTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();

        $('#CategoryNameSer').val(data.CategoryNameSer);


        $('#ID2').val(data.ID2);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#CategoryModal2").modal("show");
    });
    $('#ServiceTable tbody').on('click', '.btndelete', function () {
        var tabledata = $('#ServiceTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#ID2').val(data.ID2);
        Deletionheres('../Category/DeleteService', data.ID2, data.ServiceName);

    });
}

function AddCategory(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Category/CreateCategory',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#CategoryModal").modal("hide");

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
                    text: "I will close in 2 seconds.",
                    type: 'warning',
                    timer: 2000,
                    showConfirmButton: false

                }).catch(swal.noop)
            }

        }
    });
}

function EditCategory(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Category/EditCategory',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#CategoryModal").modal("hide");
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



function AddService(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Category/CreateService',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#CategoryModal2").modal("hide");

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
                    text: "I will close in 2 seconds.",
                    type: 'warning',
                    timer: 2000,
                    showConfirmButton: false

                }).catch(swal.noop)
            }

        }
    });
}

function EditService(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Category/EditService',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#CategoryModal2").modal("hide");
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