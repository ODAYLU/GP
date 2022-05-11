    var Connection = new signalR.HubConnectionBuilder()
        .withUrl('/chat').build();
    Connection.start()
        .catch(error =>
            console.log(error));

Connection.on("connectedUsers", function (users) {

    var text = JSON.stringify(users);
    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetUsers?text=${users}`,
        data: { text: text },
        processData: false,
        contentType: false,
        success: function (data) {
            
            var text = "";
            for (var i = 0; i < data.length; i++) {
                
                text += `<li   class="clearfix ">
                                 <input type="text" value="${data[i].id}" hidden/>
                            <img class="" src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="avatar">
                            <div class="about">
                                <div class="name">${data[i].userName}</div>
                                <div class="status"> <i class="fa fa-circle offline"></i> left 7 mins ago </div>
                            </div>
                          
                        </li>`;
            }
            $("#Users").html(text).addClass("");
        }
    });
});

Connection.on("receiveMessage", function (msg) {
    
    var Id = $("#txtIdUserCurrent").val();
    var txtMessage = "";
    if (msg.userId == Id)
    {
        txtMessage = `<li class="clearfix d-grid justify-content-start">
                    <div class="message-data text-right">
                        <span class="message-data-time">${msg.time}</span>
                        <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                    </div>
                    <div class="message other-message float-right"> ${msg.text}</div>
                </li>`
    }
    else {
        txtMessage = `<li class="clearfix d-grid justify-content-end" >
                    <div class="message-data text-right">
                        <span class="message-data-time">${msg.time}</span>
                        <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                    </div>
                    <div class="message other-message float-right"> ${msg.text}</div>
                </li>`
        
    }
   
    $("#Messages").append(txtMessage);
    
    });


$("body").on("click", "#Users li", function () {
    $("#txtId").val($(this).children("input").val());
    var IdR = $("#txtId").val();
    $.ajax({
        method: 'Post',
        url: `/ContactUser/GetMessages?ReciverId=${IdR}`,
        data: { ReciverId: IdR},
        processData: false,
        contentType: false,
        success: function (data) {
            
            var text = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].receiverId == IdR) {
                    
                    text += `<li class="clearfix d-grid justify-content-start">
                                <div class="message-data text-right">
                                    <span class="message-data-time">${data[i].time}</span>
                                    <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                                </div>
                                <div class="message other-message float-right"> ${data[i].text}</div>
                            </li>`;
                }
                else {
                    text += `<li class="clearfix d-grid justify-content-end">
                                <div class="message-data text-right">
                                    <span class="message-data-time">${data[i].time}</span>
                                    <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                                </div>
                                <div class="message other-message float-right"> ${data[i].text}</div>
                            </li>`;
                    
                }
                
            }

            $("#Messages").html(text);
        }
        });
});

$("#btnSend").click(function () {
    

    var val = $("#txtInput").val().length;
    alert(val);
    if (val > 0) {
        var IdSender = $("#txtId").val().trim();
        var thisUser = $("#txtIdUserCurrent").val().trim();
        var message = $("#txtInput").val();
        if (thisUser != IdSender) {
            alert("Hello");
            var text = `<li class="clearfix d-grid justify-content-start" >
                        <div class="message-data text-right">
                            <span class="message-data-time"></span>
                            <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                        </div>
                        <div class="message other-message float-right">${message}</div>
                    </li>`;
            $("#Messages").append(text);
            
        }
        Connection.invoke("SendMessage", message, IdSender);
    }
        
        
    
   
});

    