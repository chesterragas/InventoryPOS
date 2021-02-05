$(function () {
    Initializepage();
    Dropdown_select("ServiceCategory", "/Helper/GetDropdown_CategoryService");
    $('#ServiceForm').on('submit', function (e) {
        e.preventDefault();

        if ($('#ID').val() == "") {
            AddService($(this));
        }
        else {
            EditService($(this));
        }

    });

    $(".file-upload").on('change', function () {
        var files = new FormData();
        var file1 = document.getElementById("Photo").files[0];
        files.append('files[0]', file1);
        imageName = file1.name;
        $.ajax({
            type: 'POST',
            url: '/Service/UploadServicePhoto',
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
    $("#ServiceForm")[0].reset();
    $("#ID").val("");
    $('#ServiceTable').DataTable({
        ajax: {
            url: '../Service/GetServiceList',
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
                text: "Add Service",
                className: "btn btn-success",
                action: function () {

                    $("#ServiceModal").modal("show");
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
                        logO = "/PictureResources/ServicePhoto/" + x.Photo.replace(/\s/g, '');
                    }

                    return "<img class='card-img-top img-responsive' style='cursor: pointer;width:50px;height:30px;' src='" + logO + "' alt='Card image cap'>";

                }
            },
            { title: "Service Category", data: "ServiceCategorySer", name: "ServiceCategorySer" },
            { title: "Service Name", data: "ServiceName", name: "ServiceName" },
            { title: "Price", data: "Price", name: "Price" },
          
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

        $('#ServiceName').val(data.ServiceName);
        $('#ServiceCategory').val(data.ServiceCategory);
        $('#Price').val(data.Price);
        if (data.Photo != null) {
            $("#photohere").attr("src", "/PictureResources/ItemPhoto/" + data.Photo.replace(/\s/g, ''));
        }
        //$('select[name=selValue]').val(data.ServiceCategory);
        $('.selectpicker').selectpicker('refresh')
        $('#ID').val(data.ID);
        $("tr").removeClass("row_selected");
        $(this).parents('tr').addClass("row_selected");

        $("#ServiceModal").modal("show");
    });
    $('#ServiceTable tbody').on('click', '.btndelete', function () {
        var tabledata = $('#ServiceTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#ID').val(data.ID);
        Deletionheres('../Service/DeleteService', data.ID, data.ServiceName);

    });


}

function AddService(data) {
    var datanow = data.serialize() + "&Photo=" + imageName;
    $.ajax({
        url: '../Service/CreateService',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#ServiceModal").modal("hide");

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


function EditService(data) {
    var datanow = data.serialize() + "&Photo=" + imageName;
    $.ajax({
        url: '../Service/EditService',
        data: datanow,
        type: 'POST',
        datatype: "json",
        success: function (returnData) {
            if (returnData.msg == "Success") {
                Initializepage();
                $("#ServiceModal").modal("hide");
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


