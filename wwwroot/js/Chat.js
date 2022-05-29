    var Connection = new signalR.HubConnectionBuilder()
        .withUrl('/chat').build();
    Connection.start()
        .catch(error =>
            console.log(error));

Connection.on("connectedUsers", function (users) {

    var text = JSON.stringify(users);
    $("#usersActive").val(users);
    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetUsers?text=${users}`,
        data: { text: text },
        processData: false,
        contentType: false,
        success: function (data) {
            
            var text = "";
            var img = "";
            var UnRead = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].user.profilePicture != null) {
                   
                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].user.profilePicture}`));
                   
                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].flag == false) {
                    UnRead = `<img class="Notification" src="/images/Notification.gif" style="width:50px;" />`
                }
                else {
                    UnRead = "";
                }
                text += `<a class="list-group-item py-3" >
                                <input type="text" value="${data[i].user.id}" hidden />
                                <div class="pull-right">
                                    <img src="${img}" alt="" class="img-avatar">
                                        <small class="list-group-item-heading">${data[i].user.userName} </small>
                                </div>
                                ${UnRead}
                            </a>`;

            }
            $("#Users").html(text).addClass("");

        }
    });
});

Connection.on("receiveMessage", function (msg,users) {
    
    var Id = $("#txtIdUserCurrent").val();
    var text = JSON.stringify(users);
    var txtMessage = "";
    var img = "";
    //if (msg.sender.profilePicture != null) { img = "data:image/jpeg;base64," + btoa(atob(`${data[i].profilePicture}`)); }
    //else { img = "https://bootdey.com/img/Content/avatar/avatar1.png"; }
    if (msg.userId == Id)
    {
        
        txtMessage = `<div class="message-feed media text-end">
                        <div class="media-body">
                            <div class="mf-content">
                                ${msg.text}
                            </div>
                            <small class="mf-date"><i class="bi bi-clock-o"></i>${getLastSeen(msg.time)}</small>
                        </div>
                    </div>`;
        $("#Messages").append(txtMessage);
    }
    else {
        txtMessage = ` <div class="message-feed media text-start">
                        <div class="media-body">
                            <div class="mf-content">
                                ${msg.text}
                            </div>
                            <small class="mf-date"><i class="bi bi-clock-o"></i>${getLastSeen(msg.time)}</small>
                        </div>
                    </div>`;
        
        $("#Messages").append(txtMessage);
    }
   
    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetUsers?text=${users}`,
        data: { text: text },
        processData: false,
        contentType: false,
        success: function (data) {

            var text = "";
            var img = "";
            var UnRead = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].user.profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].user.profilePicture}`));

                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].flag == false) {
                    UnRead = `<img class="Notification" src="/images/Notification.gif" style="width:50px;" />`
                }
                else {
                    UnRead = "";
                }
                text += `<a class="list-group-item py-3" >
                                <input type="text" value="${data[i].user.id}" hidden />
                                <div class="pull-right">
                                    <img src="${img}" alt="" class="img-avatar">
                                        <small class="list-group-item-heading">${data[i].user.userName} </small>
                                </div>
                                ${UnRead}
                            </a>`;

            }
            $("#Users").html(text).addClass("");
        }
    });
    
});


$("body").on("click", "#Users a", function (e) {
    e.preventDefault();

    $("#Messages").removeClass("d-none");
    $("#Header").removeClass("d-none");
    $("#Image").addClass("d-none");
    $(this).children(".Notification").addClass("d-none");
    $("#btnSend").attr("disabled",false);
    $("#txtId").val($(this).children("input").val());
    $("#CurrentUserHeader").html($(this).html());
    var IdR = $("#txtId").val();
    $("#Header").html($(this).html());
    sessionStorage.setItem("IdUserChating", IdR);

    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetMessages?ReciverId=${IdR}`,
        data: { ReciverId: IdR},
        processData: false,
        contentType: false,
        success: function (data) {
            
            var text = "";
            var img = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].profilePicture != null) {

                    img = "data:image/jpeg;base64," + btoa(atob(`${data[i].profilePicture}`));

                } else {
                    img = "https://bootdey.com/img/Content/avatar/avatar1.png";
                }
                if (data[i].receiverId == IdR) {
                    
                    text += `<div class="message-feed media text-end">
                        <div class="media-body">
                            <div class="mf-content">
                                ${data[i].text}
                            </div>
                            <small class="mf-date"><i class="bi bi-clock-o"></i>${getLastSeen(data[i].time)}</small>
                        </div>
                    </div>`;
                }
                else {
                    text += `<div class="message-feed right">
                        <div class="pull-left me-2">
                            <img src="${img}" alt="" class="img-avatar">
                        </div>
                        <div class="media-body">
                            <div class="mf-content">
                                ${data[i].text}
                            </div>
                            <small class="mf-date"><i class="bi bi-clock-o"></i> ${getLastSeen(data[i].time)}</small>
                        </div>
                    </div>`;
                    
                }
                
            }

            $("#Messages").html(text);
        }
    });
 
});

$("#btnSend").click(function () {
    

    var val = $("#txtInput").val().length;
  
    if (val > 0) {
        var IdSender = $("#txtId").val().trim();
        var thisUser = $("#txtIdUserCurrent").val().trim();
        var message = $("#txtInput").val();

        if (thisUser != IdSender) {
           
            var text = ` <div class="message-feed media text-end">
                        <div class="media-body">
                            <div class="mf-content">
                                ${message}
                            </div>
                          
                        </div>
                    </div>`;
            $("#Messages").append(text);
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

    