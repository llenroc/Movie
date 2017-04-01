(function () {
    'use strict';
    //var chatHub = $.connection.myChatHub; //获取 hub的引用
    //chatHub.client.getMessage = function (message) { //为即将到来的信息注册
    //    //$.notify.info(message);
    //};

    infrastructure.event.on('infrastructure.signalr.connected', function () { //为连接事件注册
        //chatHub.server.sendMessage("Hi everybody, I'm connected to the chat!"); //给服务器发送信息
    });
    infrastructure.event.on('infrastructure.notifications.received', function (notification) {
        console.log(notification);
    });
})();