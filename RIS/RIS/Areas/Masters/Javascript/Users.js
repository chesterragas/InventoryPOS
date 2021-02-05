$(function () {
    demo.showSwal();
    Initializepage();

        $('#UserForm').on('submit', function (e) {
            e.preventDefault();
           
            if ($('#ID').val() == "") {
                AddUser($(this));
            }
            else {
                EditUser($(this));
            }
            
        });

});



function Initializepage() {
    $("#UserForm")[0].reset();
    $("#ID").val("");
    $('#UserTable').DataTable({
        ajax: {
            url: '../Users/GetUsersList',
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
                text: "Add User",
                className:"btn btn-success",
                action: function () {
                    
                    $("#UserModal").modal("show");
                }
            },
        ],
        columns: [
            { title: "ID", data: "ID", visible: false },
            { title: "UserName", data: "UserName", name: "UserName" },
            { title: "First Name", data: "FirstName", name: "FirstName" },
            { title: "Last Name", data: "LastName", name: "LastName" },
            { title: "Email", data: "Email", name: "Email" },
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
    $('#UserTable tbody').off('click');
    $('#UserTable tbody').on('click', '.btnedit', function () {

        var tabledata = $('#UserTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        
        $('#UserName').val(data.UserName);
        $('#FirstName').val(data.FirstName);
        $('#LastName').val(data.LastName);
        $('#Email').val(data.Email);
    
        $('#ID').val(data.ID);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#UserModal").modal("show");
    });
    $('#UserTable tbody').on('click', '.btndelete', function () {
        var tabledata = $('#UserTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#ID').val(data.ID);
        Deletionheres('../Users/DeleteUser', data.ID, data.UserName);

    });

    
}

function AddUser(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Users/CreateUser',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#UserModal").modal("hide");
                
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


function EditUser(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Users/EditUser',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#UserModal").modal("hide");
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
                    timer: 2000,
                    showConfirmButton: false
                }).catch(swal.noop)
            }

        }
    });
}