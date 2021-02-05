$(function () {
  
    var str = location.href.toLowerCase();
    $("#themenu li a").each(function () {
        if (str.indexOf(this.href.toLowerCase()) > -1) {
            $(this).parent("li").addClass("active");

        }
    });
   
    

    var link = str.toLowerCase();
    var res = link.split("/");

    var page = "";
    switch (res[3]) {
        case "masters": page = "masters"
            break;
        case "transaction": page = "transaction"
            break;
        case "summary": page = "summary"
            break;
        case "forecast": page = "forecast"
            break;
    }

    //if (str.includes("masters")) {
    if (page == "masters") {
        $("li").removeClass("menu-open");
        $('#master').removeClass("collapse");
        $('#master').addClass("collapse show");

        $("#dashboardl").removeClass("active");
    }

    if (page == "transaction") {
        $("li").removeClass("menu-open");
        $('#transaction').removeClass("collapse");
        $('#transaction').addClass("collapse show");

        $("#dashboardl").removeClass("active");
    }
    
    $("#Logouthere").removeClass("active menu-open");

})