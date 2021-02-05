demo = {

    showSwal: function (type) {
        if (type == 'basic') {
            swal({
                title: "Here's a message!",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-success"
            }).catch(swal.noop)

        } else if (type == 'title-and-text') {
            swal({
                title: "Here's a message!",
                text: "It's pretty, isn't it?",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info"
            }).catch(swal.noop)

        } else if (type == 'success-message') {
            swal({
                title: "Good job!",
                text: "You clicked the button!",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-success",
                type: "success"
            }).catch(swal.noop)

        } else if (type == 'warning-message-and-confirmation') {
            swal({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: 'Yes, delete it!',
                buttonsStyling: false
            }).then(function () {
                swal({
                    title: 'Deleted!',
                    text: 'Your file has been deleted.',
                    type: 'success',
                    confirmButtonClass: "btn btn-success",
                    buttonsStyling: false
                })
            }).catch(swal.noop)
        } else if (type == 'warning-message-and-cancel') {
            swal({
                title: 'Are you sure?',
                text: 'You will not be able to recover this imaginary file!',
                type: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, keep it',
                confirmButtonClass: "btn btn-success",
                cancelButtonClass: "btn btn-danger",
                buttonsStyling: false
            }).then(function () {
                swal({
                    title: 'Deleted!',
                    text: 'Your imaginary file has been deleted.',
                    type: 'success',
                    confirmButtonClass: "btn btn-success",
                    buttonsStyling: false
                }).catch(swal.noop)
            }, function (dismiss) {
                // dismiss can be 'overlay', 'cancel', 'close', 'esc', 'timer'
                if (dismiss === 'cancel') {
                    swal({
                        title: 'Cancelled',
                        text: 'Your imaginary file is safe :)',
                        type: 'error',
                        confirmButtonClass: "btn btn-info",
                        buttonsStyling: false
                    }).catch(swal.noop)
                }
            })

        } else if (type == 'custom-html') {
            swal({
                title: 'HTML example',
                buttonsStyling: false,
                confirmButtonClass: "btn btn-success",
                html: 'You can use <b>bold text</b>, ' +
                    '<a href="http://github.com">links</a> ' +
                    'and other HTML tags'
            }).catch(swal.noop)

        } else if (type == 'auto-close') {
            swal({
                title: "Auto close alert!",
                text: "I will close in 2 seconds.",
                timer: 2000,
                showConfirmButton: false
            }).catch(swal.noop)
        } else if (type == 'input-field') {
            swal({
                title: 'Input something',
                html: '<div class="form-group">' +
                    '<input id="input-field" type="text" class="form-control" />' +
                    '</div>',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success',
                cancelButtonClass: 'btn btn-danger',
                buttonsStyling: false
            }).then(function (result) {
                swal({
                    type: 'success',
                    html: 'You entered: <strong>' +
                        $('#input-field').val() +
                        '</strong>',
                    confirmButtonClass: 'btn btn-success',
                    buttonsStyling: false

                })
            }).catch(swal.noop)
        }
    }


}


function Deletionheres(link, ID, Name) {


    swal({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: 'Yes, delete it!',
        buttonsStyling: false
    }).then(function () {
            $.ajax({
                url: link,
                data: { ID: ID },
                type: 'POST',
                datatype: "json",
                success: function (returnData) {
                    if (returnData.msg == "Success") {
                        swal({
                            title: "Data Deleted",
                            timer: 2000,
                            type: 'success',
                            showConfirmButton: false
                        }).catch(swal.noop)
                        Initializepage();

                    }
                    else {
                        swal({
                            title: "Data Deletion Failed",
                           
                            timer: 2000,
                            type: 'warning',
                            showConfirmButton: false
                        }).catch(swal.noop)
                    }

                }
            });
    }).catch(swal.noop)

    //swal({
    //    title: "Are you sure?",
    //    //text: "You will not be able to recover this imaginary file!",   
    //    type: "warning",
    //    showCancelButton: true,
    //    confirmButtonColor: "#DD6B55",
    //    confirmButtonText: "Yes",
    //    cancelButtonText: "No",
    //    closeOnConfirm: true,
    //    closeOnCancel: true
    //}, function (isConfirm) {
    //    if (isConfirm) {

    //        $.ajax({
    //            url: link,
    //            data: { ID: ID },
    //            type: 'POST',
    //            datatype: "json",
    //            success: function (returnData) {
    //                if (returnData.msg == "Success") {
    //                    swal({
    //                        title: "Data Successfully Deleted",
    //                        text: "I will close in 2 seconds.",
    //                        timer: 2000,
    //                        showConfirmButton: false
    //                    }).catch(swal.noop)
    //                    Initializepage();
                       
    //                }
    //                else {
    //                    swal({
    //                        title: "Data Deletion Failed",
    //                        text: "I will close in 2 seconds.",
    //                        timer: 2000,
    //                        showConfirmButton: false
    //                    }).catch(swal.noop)
    //                }

    //            }
    //        });


    //    } else {
    //        swal("Cancelled", "Deletion Cancelled", "error");
    //    }
    //});
}

function Dropdown_select(id, url) {
    var option = '<option value="">--SELECT--</option>';
    $('#' + id).html(option);
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'JSON',
    }).done(function (data, textStatus, xhr) {
        $.each(data.list, function (i, x) {
            option = '<option value="' + x.value + '">' + x.text + '</option>';
            $('#' + id).append(option);
        });

        $("#" + id).val('default');
        $("#" + id).selectpicker("refresh");
       
    }).fail(function (xhr, textStatus, errorThrown) {
        console.log(errorThrown, textStatus);
    });
}
