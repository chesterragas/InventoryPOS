$(function () {
    Initializepage();
    $('#EmployeeForm').on('submit', function (e) {
        e.preventDefault();

        if ($('#ID').val() == "") {
            AddEmployee($(this));
        }
        else {
            EditEmployee($(this));
        }

    });

   
    $(".file-upload").on('change', function () {
        var files = new FormData();
        var file1 = document.getElementById("Photo").files[0];
        files.append('files[0]', file1);
        imageName = file1.name;
        $.ajax({
            type: 'POST',
            url: '/Employee/UploadEmployeePhoto',
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
    $("#EmployeeForm")[0].reset();
    $("#ID").val("");
    $('#EmployeeTable').DataTable({
        ajax: {
            url: '../Employee/GetEmployeeList',
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
                text: "Add Employee",
                className: "btn btn-success",
                action: function () {

                    $("#EmployeeModal").modal("show");
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
                        logO = "/PictureResources/EmployeePhoto/" + x.EmployeePhoto.replace(/\s/g, '');
                    }

                    return "<img class='card-img-top img-responsive' style='cursor: pointer;width:50px;height:30px;' src='" + logO + "' alt='Card image cap'>";

                }
            },
            { title: "Family Name", data: "Family_Name", name: "Family_Name" },
            { title: "First Name", data: "First_Name", name: "First_Name" },
            { title: "DailyWage", data: "DailyWage", name: "DailyWage" },
         
           
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
    $('#EmployeeTable tbody').off('click');
   
    $('#EmployeeTable tbody').on('click', '.btnedit', function () {

        var tabledata = $('#EmployeeTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();

        $('#Family_Name').val(data.Family_Name);
        $('#First_Name').val(data.First_Name);
        $('#DailyWage').val(data.DailyWage);
       
        if (data.EmployeePhoto != null) {
            $("#photohere").attr("src", "/PictureResources/EmployeePhoto/" + data.Photo.replace(/\s/g, ''));
        }

        //$('select[name=selValue]').val(data.EmployeeCategory);
        $('.selectpicker').selectpicker('refresh')
        $('#ID').val(data.ID);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#EmployeeModal").modal("show");
    });
    $('#EmployeeTable tbody').on('click', '.btndelete', function () {
        var tabledata = $('#EmployeeTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#ID').val(data.ID);
        Deletionheres('../Employee/DeleteEmployee', data.ID, data.EmployeeName);

    });


}

function AddEmployee(data) {
    var datanow = data.serialize() + "&Photo=" + imageName;
    $.ajax({
        url: '../Employee/CreateEmployee',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {

                Initializepage();
                $("#EmployeeModal").modal("hide");

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


function EditEmployee(data) {
    var datanow = data.serialize();
    $.ajax({
        url: '../Employee/EditEmployee',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#EmployeeModal").modal("hide");
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

