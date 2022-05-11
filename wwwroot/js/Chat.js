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
            console.log(data);
            var text = "";
            for (var i = 0; i < data.length; i++) {
                console.log(data[i].userName);
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
    console.log(msg);
    var txtMessage = "";
    txtMessage = `<li class="clearfix">
                    <div class="message-data text-right">
                        <span class="message-data-time">${msg.time}</span>
                        <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                    </div>
                    <div class="message other-message float-right"> ${msg.text}</div>
                </li>`
    $("#Messages").append(txtMessage);
    });


$("body").on("click","#Users li",function () {
    $("#txtId").val($(this).children("input").val());
});

$("#btnSend").click(function () {
    console.log("click");
    var IdSender = $("#txtId").val();
    var message = $("#txtInput").val();
    Connection.invoke("SendMessage", message, IdSender);
});
    