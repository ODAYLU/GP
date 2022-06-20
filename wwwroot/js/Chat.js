var Connection = new signalR.HubConnectionBuilder()
    .withUrl('/chat').build();
Connection.start()
    .catch(error =>
        console.log(error));

Connection.on("connectedUsers", function (users) {

    var text = JSON.stringify(users);
    $("#usersActive").val(users);
    $.ajax({
        method: 'Get',
        url: `/ContactUser/GetUsersContact`,
        data: { text: text },
        processData: false,
        contentType: false,
        success: function (data) {

            var text = "";
            var img = "";
            var UnRead = "";
            var isactive = "";
            var NewMessages = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].user.profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].user.profilePicture}`));

                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].flag == false) {
                    UnRead = `<img class="Notification" src="/images/Notification.gif" style="width:50px;" />`
                    NewMessages =` <span style="position: absolute;height: 25px;width:25px;  top: 0px;right:5px; border-radius: 50%;background: red; padding:5px"></span>`;
                }
                else {
                    UnRead = "";
                }
                if (data[i].isActive == true) {

                    isactive = `<div class="user-status"></div>`
                }
                else {
                    isactive = "";
                }

                text += `<li class="messaging-member messaging-member--new messaging-member--online">
                        <input type="text" value="${data[i].user.id}" hidden />
                    <div class="messaging-member__wrapper">
                        <div class="messaging-member__avatar">
                            <img src="${img}" alt="Bessie Cooper" loading="lazy" />
                         ${isactive}
                     
                        </div>

                        <span class="messaging-member__name  textstart"> ${data[i].user.userName}${UnRead} </span>

                    </div>


                </li>`;

            }
            $("#btnChat").append(NewMessages);
            $("#Users").html(text).addClass("");

        }
    });
    $.ajax({
        method: 'Get',
        url: `/ContactUser/GetUsers`,
        data: { text: text },
        processData: false,
        contentType: false,
        success: function (data) {
           
            var text = "";
            var img = "";
            var UnRead = "";
           
            var NewMessages = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].user.profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].user.profilePicture}`));

                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].flag == false) {
                    UnRead = `<img class="Notification" src="/images/Notification.gif" style="width:50px;" />`
                    NewMessages = ` <span style="position: absolute;height: 25px;width:25px;  top: 0px;right:5px; border-radius: 50%;background: red; padding:5px"></span>`;
                }
                else {
                    UnRead = "";
                }
                
                

                text += `<li class="messaging-member messaging-member--new messaging-member--online">
                        <input type="text" value="${data[i].user.id}" hidden />
                    <div class="messaging-member__wrapper">
                        <div class="messaging-member__avatar">
                            <img src="${img}" alt="Bessie Cooper" loading="lazy" />
                         <div class="user-status"></div>
                         
                        </div>

                        <span class="messaging-member__name  textstart"> ${data[i].user.userName} ${UnRead} </span>

                    </div>


                </li>`;

            }
            $("#btnChat").append(NewMessages);
            $("#UsersOnline").html(text).addClass("");

        }
    });
});
$(document).ready(function () {
    $("#boxSend").attr("hidden", true);
    $("#Messages").attr("hidden", true);
    $("#UsersOnline").attr("hidden", false);
    $("#Users").attr("hidden", true);
    if (localStorage.getItem("ChatUserId").length > 0) {


        $("#boxSend").attr("hidden", false);
        $("#Messages").removeClass("d-none");
        $("#Header").removeClass("d-none");
        $("#Image").addClass("d-none");
        $("#Messages").attr("hidden", false);
        // $(this).children(".Notification").addClass("d-none");
        $("#btnSend").attr("disabled", false);
        $("#txtId").val(localStorage.getItem("ChatUserId"));
        $("#CurrentUserHeader").html(` <span class="messaging-member__name  textstart"> ${localStorage.getItem("ChatUserName")} </span>`);
        var IdR = localStorage.getItem("ChatUserId");
        $("#Header").html(` <span class="messaging-member__name  textstart"> ${localStorage.getItem("ChatUserName")} </span>`);
        $.ajax({
            method: 'Post',
            url: `/ContactUser/GetMessages?ReciverId=${IdR}`,
            data: { ReciverId: IdR },
            processData: false,
            contentType: false,
            success: function (data) {

                var text = "";
                var img = "";
                var time = "";
                for (var i = 0; i < data.length; i++) {
                    if (i % 10 == 0) {
                        time = `<div class="chat__time">${getLastSeen(data[i].time)}</div>`;
                    }
                    else {
                        time = `<div class="chat__time" hidden>${getLastSeen(data[i].time)}</div>`;
                    }
                    if (data[i].profilePicture != null) {

                        img = "data:image/jpeg;base64," + btoa(atob(`${data[i].profilePicture}`));

                    } else {
                        img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                    }
                    if (data[i].receiverId == IdR) {

                        text += `<li>
                        ${time}
                        <div class="chat__bubble chat__bubble--you">
                            ${data[i].text}
                        </div>
                    </li>`;
                    }
                    else {
                        text += `<li>
                        ${time}
                        <div class="chat__bubble chat__bubble--me">
                            ${data[i].text}
                        </div>
                    </li>`;

                    }

                }

                $("#Messages").html(text);
                $('.chat__content').scrollTop($(".chat__content")[0].scrollHeight);
            }
        });
        localStorage.setItem("ChatUserId", "");
    }
});
Connection.on("receiveMessage", function (msg, users) {

    var Id = $("#txtIdUserCurrent").val();
    var text = JSON.stringify(users);
    var txtMessage = "";
    var img = "";

    if (msg.userId == Id) {

        txtMessage = `<li>
                        <div class="chat__time">${getLastSeen(msg.time)}</div>
                        <div class="chat__bubble chat__bubble--you">
                            ${msg.text}
                        </div>
                    </li>`;
        $("#Messages").append(txtMessage);
    }
    else {
        txtMessage = `<li>
                        <div class="chat__time">${getLastSeen(msg.time)}</div>
                        <div class="chat__bubble chat__bubble--me">
                            ${msg.text}
                        </div>
                    </li>`;

        $("#Messages").append(txtMessage);
    }

    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetUsersContact`,
        data: { text: text },
        processData: false,
        contentType: false,
        success: function (data) {

            var text = "";
            var img = "";
            var UnRead = "";
            var isactive = ""
            for (var i = 0; i < data.length; i++) {
                if (data[i].user.profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].user.profilePicture}`));

                } else {
                    img = "https://randomuser.me/api/portraits/thumb/men/74.jpg";
                }
                if (data[i].flag == false) {
                    UnRead = `<img class="Notification" src="/images/Notification.gif" style="width:50px;" />`
                }
                else {
                    UnRead = "";
                }
                if (data[i].user.isActive == true) {

                    isactive = `<div class="user-status"></div>`
                }
                else {
                    isactive = "";
                }
                text += `<li class="messaging-member messaging-member--new messaging-member--online">	
                    <input type="text" value="${data[i].user.id}" hidden />
                    <div class="messaging-member__wrapper">
                        <div class="messaging-member__avatar">
                            <img src="${img}" alt="Bessie Cooper" loading="lazy" />
                                ${isactive}
                        </div>

                        <span class="messaging-member__name  textstart"> ${data[i].user.userName} </span>
                   
                    </div>
                    ${UnRead}
                </li>`;

            }
            $("#Users").html(text).addClass("");
        }
    });

});
//-----------------------------------------------------------
$("body").on("mouseenter", "#Messages li",function () {
   
    $(this).children(".chat__time").attr("hidden", false);
}).on("mouseleave", "#Messages li", function () {

    $(this).children(".chat__time").attr("hidden", true);
});
$("#btnUserOnline").on("click", function () {
    $(this).addClass("active");
    $("#btnUserContact").removeClass("active");
    $("#UsersOnline").attr("hidden", false);
    $("#Users").attr("hidden", true);
});
$("#btnUserContact").on("click", function () {
    $(this).addClass("active");
    $("#btnUserOnline").removeClass("active");
    $("#Users").attr("hidden", false);
    $("#UsersOnline").attr("hidden", true);
});

$("body").on("click", "#Users li", function (e) {
    e.preventDefault();
    $("#boxSend").attr("hidden", false);
    $("#Messages").removeClass("d-none");
    $("#Header").removeClass("d-none");
    $("#Image").addClass("d-none");
    $("#Messages").attr("hidden", false);
    $(this).children(".Notification").addClass("d-none");
    $("#btnSend").attr("disabled", false);
    $("#txtId").val($(this).children("input").val());
    $("#CurrentUserHeader").html($(this).html());
    var IdR = $("#txtId").val();
    $("#Header").html($(this).html());
    sessionStorage.setItem("IdUserChating", IdR);

    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetMessages?ReciverId=${IdR}`,
        data: { ReciverId: IdR },
        processData: false,
        contentType: false,
        success: function (data) {

            var text = "";
            var img = "";
            var time = "";
            for (var i = 0; i < data.length; i++) {
                if (i % 10 == 0) {
                    time = `<div class="chat__time">${getLastSeen(data[i].time)}</div>`;
                }
                else {
                    time = `<div class="chat__time" hidden>${getLastSeen(data[i].time)}</div>`;
                }
                if (data[i].profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].profilePicture}`));

                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].receiverId == IdR) {

                    text += `<li>
                        ${time}
                        <div class="chat__bubble chat__bubble--you">
                            ${data[i].text}
                        </div>
                    </li>`;
                }
                else {
                    text += `<li>
                        ${time}
                        <div class="chat__bubble chat__bubble--me">
                            ${data[i].text}
                        </div>
                    </li>`;

                }

            }

            $("#Messages").html(text);
            $('.chat__content').scrollTop($(".chat__content")[0].scrollHeight);
        }
    });

});
$("body").on("click", "#UsersOnline li", function (e) {
    e.preventDefault();
    $("#boxSend").attr("hidden", false);
    $("#Messages").removeClass("d-none");
    $("#Header").removeClass("d-none");
    $("#Image").addClass("d-none");
    $("#Messages").attr("hidden", false);
    $(this).children(".Notification").addClass("d-none");
    $("#btnSend").attr("disabled", false);
    $("#txtId").val($(this).children("input").val());
    $("#CurrentUserHeader").html($(this).html());
    var IdR = $("#txtId").val();
    $("#Header").html($(this).html());
    sessionStorage.setItem("IdUserChating", IdR);

    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetMessages?ReciverId=${IdR}`,
        data: { ReciverId: IdR },
        processData: false,
        contentType: false,
        success: function (data) {

            var text = "";
            var img = "";
            var time = "";
            for (var i = 0; i < data.length; i++) {
                if (i % 10 == 0) {
                    time = `<div class="chat__time">${getLastSeen(data[i].time)}</div>`;
                }
                else {
                    time = `<div class="chat__time" hidden>${getLastSeen(data[i].time)}</div>`;
                }
                if (data[i].profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].profilePicture}`));

                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].receiverId == IdR) {

                    text += `<li>
                        ${time}
                        <div class="chat__bubble chat__bubble--you">
                            ${data[i].text}
                        </div>
                    </li>`;
                }
                else {
                    text += `<li>
                        ${time}
                        <div class="chat__bubble chat__bubble--me">
                            ${data[i].text}
                        </div>
                    </li>`;

                }

            }

            $("#Messages").html(text);
            $('.chat__content').scrollTop($(".chat__content")[0].scrollHeight);
        }
    });

});
$("#btnSend").click(function () {
    debugger;

    var val = $("#txtInput").val().length;

    if (val > 0) {
        var IdSender = $("#txtId").val().trim();
        var thisUser = $("#txtIdUserCurrent").val().trim();
        var message = $("#txtInput").val();

        if (thisUser != IdSender) {

            var text = ` <li>
                <div class="chat__bubble chat__bubble--you">
                    ${message}
                </div>
            </li>`;
            $("#Messages").append(text);
            $('.chat__content').scrollTop($(".chat__content")[0].scrollHeight);
            $("#txtInput").val("");
        }
        Connection.invoke("SendMessage", message, IdSender);
    }




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

