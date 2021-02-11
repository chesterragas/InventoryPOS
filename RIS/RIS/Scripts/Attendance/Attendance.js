$(function () {
    Initializepage();



    $("#checkAttendance").on("click", function () {

        $.ajax({
            url: '../EmployeeAttendance/CreateEmployeeLogs',
            data: JSON.stringify({
                data: chosenitem,

            }),
            type: 'POST',
            datatype: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (returnData) {
                swal({
                    title: 'Saved!',
                    text: 'Data Successfully Saved',
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false
                }).catch(swal.noop)
                chosenitem = [];
            }
        });

    });
    $("#checkAttendanceClear").on("click", function(){
        $.ajax({
            url: '../EmployeeAttendance/ClearData',
            type: 'GET',
            data: {
                datechosen: $("#chosendate").val()
            },
            datatype: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (returnData) {
                swal({
                    title: 'Saved!',
                    text: 'Data Cleared',
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false
                }).catch(swal.noop)
                chosenitem = [];
                Initializepage();
            }
        });

    })
 
});
function getDayName(dateStr, locale) {
    var date = new Date(dateStr);
    return date.toLocaleDateString(locale, { weekday: 'long' });
}

function formatDate(date) {
    var monthNames = [
        "January", "February", "March",
        "April", "May", "June", "July",
        "August", "September", "October",
        "November", "December"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();

    return monthNames[monthIndex] + ' ' + day + ', ' + year;
}
var chosenitem = [];
function Initializepage() {

    $.ajax({
        url: '/EmployeeAttendance/GetServerDate',
        type: "GET",
        datatype: "json",
        success: function (returnData) {
            $("#chosendate").val(returnData.thed);
            date = moment(returnData.thed).format("MM/DD/YYYY");//returnData.thed;//new Date();
            date = new Date(date);
           
            var dateStr = date;
            var day = getDayName(dateStr, "en-US");
            $('#WeekDay').html(day)

            
            val = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
            $('#datess').html(formatDate(date));



            $.ajax({
                url: '../EmployeeAttendance/GetEmployeeDataList',
                type: 'GET',
                data: {
                    datechosen: $("#chosendate").val()
                },
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                    var emplist = "";

                    for (var x = 0; x < returnData.data.length; x++) {
                        var status = (returnData.data[x].DateAttendance == null) ? "" : "active show";
                        emplist +=
                            "<ul class='nav nav-pills nav-pills-warning nav-pills-icons justify-content-center' role='tablist'>" +
                            "<li class='nav-item '>" +
                            "   <a class='nav-link attendance " + status + "' data-toggle='tab' href='#itemcategory' role='tablist' id=" + returnData.data[x].ID + ">" +
                            "       <img style='border-radius: 50%; width:120px;height:100px' src='/PictureResources/EmployeePhoto/" + returnData.data[x].EmployeePhoto.replace(/\s/g, '') + "'" + "/>" +
                            "       <div class='col-md-12'>" + returnData.data[x].First_Name + ' ' + returnData.data[x].Family_Name + "</div>" +
                            "   </a>" +
                            "</li>";
                        "</ul>";

                    }

                    $("#emplist").html(emplist);

                    $('.attendance').each(function (i, obj) {
                        if ($(this).hasClass("active")) {
                            chosenitem.push(this.id);
                        }
                       
                    });

                    $(".attendance").on("click", function () {

                        if ($(this).hasClass("active")) {
                            $(this).removeClass("active show");
                            chosenitem = [];
                            //$(".nav-pills > li > nav-link").removeClass("active");
                            //GetEmployee();
                        }
                        else {
                            chosenitem.push(this.id);
                        }



                    })
                }
            });

        }
    });

   
   
    

}

