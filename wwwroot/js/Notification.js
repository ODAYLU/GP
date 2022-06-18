var Connection = new signalR.HubConnectionBuilder()
    .withUrl('/notification').build();
Connection.start()
    .catch(error =>
        console.log(error));
Connection.on("receiveNotification", function (Not) {

    var text = "";
    
    for (var i = 0; i < data.length; i++) {
        if (data[i].type == "action") {
            text += `<li><a class="dropdown-item" href="/Estate/Index">${Not.text}</a></li>`;
        } else if (Not.type == "comment") {
            text += `<li>
                                    <a class="dropdown-item" href="/Commments/Commments">${Not.text}</a>
                                    <small>${getLastSeen(Not.time)}</small>
                                </li>`;
        } else if (Not.type == "Reply") {
            text += `<li>
                                    <a class="dropdown-item" href="/commments/EstateComment/${Not.idAction}">${Not.text}</a>
                                    <small>${getLastSeen(Not.time)}</small>
                                </li>`;
        }
    }
    var count = parseInt($("#btnNotification").children("span").text().trim());
    $("#btnNotification").children("span").text(`${++count}`);
    $("#NotificationUser").prepend(text);
});
$(document).ready(function () {
    $.ajax({
        method: 'GET',
        url: `/Home/GetNotification`,
        processData: false,
        contentType: false,
        success: function (data) {
           
            var text = "";
            if (data.length > 0) {
                $("#NotificationUser").html("");
                for (var i = 0; i < data.length; i++) {
                    if (data[i].type == "action") {
                        text += `<li><a class="dropdown-item" href="/Estate/Index">${data[i].text}</a></li>`;
                    } else if (data[i].type == "comment") {
                        text += `<li>
                                    <a class="dropdown-item" href="/Commments/Commments">${data[i].text}</a>
                                    <small>${getLastSeen(data[i].time)}</small>
                                </li>`;
                    } else if (data[i].type == "Reply") {
                        text += `<li>
                                    <a class="dropdown-item" href="/commments/EstateComment/${data[i].idAction}">${data[i].text}</a>
                                    <small>${getLastSeen(data[i].time)}</small>
                                </li>`;
                    }
                }
            } else {
                text = `<li><a class="dropdown-item disabled" href="#">لا يوجد اشعارات جديدة</a></li>`;
            }
           
            $("#btnNotification").children("span").text(`${data.length}`);
            $("#NotificationUser").prepend(text);
        }
    });
});
$("#btnNotification").click(function () {
    
    $.ajax({
        method: 'GET',
        url: `/Home/SetNotificationReaded`,
        processData: false,
        contentType: false,
        success: function () {

        }
    });
    $("#btnNotification").children("span").text(`0`);
    
});
function getLastSeen(date) {


    let milSec = Date.now() - Date.parse(date);

    if (milSec < 5) {
        return `الآن`
    }
    else if (Math.ceil(milSec / 1000) < 60) {
        return `منذ ${Math.ceil(milSec / 1000)} ثانية`
    } else if (Math.ceil(milSec / 1000 / 60) < 60) {
        return `منذ ${Math.ceil(milSec / 1000 / 60)} دقيقة`
    }
    else if (Math.ceil(milSec / 1000 / 60 / 60) < 24) {
        return `منذ ${Math.ceil(milSec / 1000 / 60 / 60)} ساعة`
    }
    else if (Math.ceil(milSec / 1000 / 60 / 60 / 24) < 30) {
        return `منذ ${Math.ceil(milSec / 1000 / 60 / 60 / 24)} يوم`
    }
    else if (Math.ceil(milSec / 1000 / 60 / 60 / 24 / 30) < 12) {
        return `منذ ${Math.ceil(milSec / 1000 / 60 / 60 / 24 / 30)} شهر`
    }
    else {
        return `منذ ${Math.ceil(milSec / 1000 / 60 / 60 / 24 / 30 / 12)} سنة`
    }
}